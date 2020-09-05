#load nuget:https://ci.appveyor.com/nuget/cake-recipe?package=Cake.Recipe&version=2.0.0-alpha0417&prerelease
#tool nuget:?package=NuGet.CommandLine&version=5.6.0

Environment.SetVariableNames();

BuildParameters.SetParameters(
	buildSystem: BuildSystem,
	context: Context,
	nugetConfig: "./src/NuGet.Config",
	preferredBuildAgentOperatingSystem: PlatformFamily.Linux,
	preferredBuildProviderType: BuildProviderType.GitHubActions,
	repositoryName: "Cake.Addin.Analyzer",
	repositoryOwner: "WormieCorp",
	shouldRunCodecov: !EnvironmentVariable("SKIP_CODECOV", false),
	shouldRunCoveralls: false,
	shouldRunDotNetCorePack: true,
	shouldRunDupFinder: false,
	shouldRunInspectCode: false, // Too many false flags
	shouldUseDeterministicBuilds: true,
	shouldUseTargetFrameworkPath: false,
	solutionFilePath: "./src/Cake.Addin.Analyzer_core.sln",
	sourceDirectoryPath: "./src",
	title: "Cake.Addin.Analyzer"
);

var feedzUrl = EnvironmentVariable("FEEDZ_SOURCE");

if (!string.IsNullOrEmpty(feedzUrl))
{
	BuildParameters.PackageSources.Add(new PackageSourceData(Context, "FEEDZ", feedzUrl, FeedType.NuGet, false));
}

BuildParameters.PrintParameters(Context);

ToolSettings.SetToolSettings(
	context: Context,
	testCoverageExcludeByFile: "**/*Designer.cs;**/*.g.cs;**/*.g.i.cs",
	testCoverageFilter: "+[*]* -[nunit.framework*]* -[NUnit3.TestAdapter*]*"
);
ToolSettings.SetToolPreprocessorDirectives(
	// Workaround until Cake.Kudu can resolve .NET Core edition
	kuduSyncGlobalTool: "#tool nuget:?package=KuduSync.NET&version=1.5.3"
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
