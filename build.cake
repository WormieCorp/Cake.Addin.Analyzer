#load nuget:https://ci.appveyor.com/nuget/cake-recipe?package=Cake.Recipe&version=2.0.0-alpha0433&prerelease
#tool nuget:?package=NuGet.CommandLine&version=5.6.0
#tool dotnet:?package=dotnet-t4&version=2.0.5

Environment.SetVariableNames();

var platform = PlatformFamily.Linux;
var provider = BuildProviderType.GitHubActions;

if (HasEnvironmentVariable("APPVEYOR") && EnvironmentVariable("APPVEYOR_REPO_TAG", false))
{
	platform = PlatformFamily.Windows;
	provider = BuildProviderType.AppVeyor;
}

BuildParameters.SetParameters(
	buildSystem: BuildSystem,
	context: Context,
	nugetConfig: "./src/NuGet.Config",
	preferredBuildAgentOperatingSystem: platform,
	preferredBuildProviderType: provider,
	repositoryName: "Cake.Addin.Analyzer",
	repositoryOwner: "WormieCorp",
	shouldRunCodecov: !EnvironmentVariable("SKIP_CODECOV", false),
	shouldRunCoveralls: false,
	shouldRunDotNetCorePack: true,
	shouldRunDupFinder: true,
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
	.DoesForEach(GetFiles("src/**/*.tt"), (file) =>
{
	TransformTemplate(file);
});


Build.RunDotNetCore();
