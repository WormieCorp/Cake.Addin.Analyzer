#load nuget:https://ci.appveyor.com/nuget/cake-recipe?package=Cake.Recipe&version=2.0.0-alpha0363&prerelease
#tool nuget:?package=NuGet.CommandLine&version=5.6.0

Environment.SetVariableNames();

BuildParameters.SetParameters(
	buildSystem: BuildSystem,
	context: Context,
	nugetConfig: "./src/NuGet.Config",
	preferredBuildAgentOperatingSystem: PlatformFamily.Windows,
	preferredBuildProviderType: BuildProviderType.GitHubActions,
	repositoryName: "Cake.Addin.Analyzer",
	repositoryOwner: "AdmiringWorm",
	shouldRunCodecov: true,
	shouldRunCoveralls: false,
	shouldRunDotNetCorePack: true,
	shouldRunDupFinder: false,
	shouldRunInspectCode: false, // Too many false flags
	shouldUseDeterministicBuilds: true,
	shouldUseTargetFrameworkPath: false,
	solutionFilePath: "./src/Cake.Addin.Analyzer_core.sln",
	sourceDirectoryPath: "./src",
	title: "Cake Addin Analyzer"
);

BuildParameters.PrintParameters(Context);

ToolSettings.SetToolSettings(
	context: Context,
	testCoverageExcludeByFile: "**/*Designer.cs;**/*.g.cs;**/*.g.i.cs",
	testCoverageFilter: "+[*]* -[nunit.framework*]* -[NUnit3.TestAdapter*]*"
);

Task("Transform-Text-Templates")
	.IsDependeeOf("DotNetCore-Build")
	.Does(() =>
{
	var ttFiles = GetFiles("src/**/*.tt");

	int exitCode = 0;

	foreach (var file in ttFiles)
	{
		Information("Transforming Text Template: '{0}'", file.GetFilename());
		int newexitcode = StartProcess("dotnet",
			new ProcessSettings {
				Arguments = new ProcessArgumentBuilder()
					.Append("t4")
					.AppendQuoted(file.ToString())
			});
		if (exitCode == 0)
			exitCode = newexitcode;
	}

	if (exitCode != 0)
	{
		throw new Exception("Text Template transformation failed");
	}
});


Build.RunDotNetCore();
