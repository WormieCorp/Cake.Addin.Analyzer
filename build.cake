#load nuget:https://ci.appveyor.com/nuget/cake-recipe?package=Cake.Recipe&version=2.0.0-alpha0363&prerelease
#tool nuget:?package=NuGet.CommandLine&version=5.6.0

Environment.SetVariableNames();

BuildParameters.SetParameters(
	context: Context,
	buildSystem: BuildSystem,
	sourceDirectoryPath: "./src",
	solutionFilePath: "./src/CakeContrib.Analyzer_core.sln",
	title: "CakeContrib.Analyzer",
	repositoryOwner: "AdmiringWorm",
	repositoryName: "CakeContrib.Analyzer",
	//shouldRunDupFinder: false,
	shouldUseDeterministicBuilds: true,
	shouldUseTargetFrameworkPath: false,
	shouldRunCodecov: true,
	nugetConfig: "./src/NuGet.Config");

BuildParameters.PrintParameters(Context);

ToolSettings.SetToolSettings(
	context: Context,
	testCoverageExcludeByFile: "**/*Designer.cs;**/*.g.cs;**/*.g.i.cs",
	testCoverageFilter: "+[*]* -[nunit.framework*]* -[NUnit3.TestAdapter*]*");

Task("Transform-Text-Templates")
	.IsDependeeOf("DotNetCore-Build")
	.Does(() =>
{
	var ttFile = File("./src/CakeContrib.Analyzer.Tests/TestFiles/TestTemplates.tt");
	var outputFile = File("./src/CakeContrib.Analyzer.Tests/TestFiles/TestTemplates");

	var exitCode = StartProcess("dotnet",
		new ProcessSettings {
			Arguments = new ProcessArgumentBuilder()
				.Append("t4")
				.AppendSwitchQuoted("--out","=", outputFile.ToString())
				.AppendQuoted(ttFile.ToString())
		});

	if (exitCode != 0)
	{
		throw new Exception("Text Template transformation failed");
	}
});


Build.RunDotNetCore();
