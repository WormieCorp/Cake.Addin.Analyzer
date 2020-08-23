var target = Argument("target", "Default");
var configuration = Argument("configuration", "Release");
const string solution = "./src/CakeContrib.Analyzer_core.sln";
const string vsixProject = "./src/CakeContrib.Analyzer.Vsix/CakeContrib.Analyzer.Vsix.csproj";

Task("Restore")
	.Does(() =>
{
	DotNetCoreRestore(solution);
	DotNetCoreRestore(vsixProject);
});

Task("Build")
	.IsDependentOn("Restore")
	.Does(() =>
{
	DotNetCoreBuild(solution, new DotNetCoreBuildSettings
	{
		Configuration = configuration,
		NoIncremental = true,
		NoRestore     = true
	});
});

Task("Tests")
	.IsDependentOn("Build")
	.Does(() =>
{
	DotNetCoreTest(solution, new DotNetCoreTestSettings
	{
		Configuration = configuration,
		NoBuild = true
	});
});

Task("Create-VSix-Package")
	.WithCriteria(IsRunningOnWindows)
	.Does(() =>
{
	var latestInstallation = VSWhereLatest();
	var expectedPath = latestInstallation + "/MSBuild/Current/Bin/MSBuild.exe";
	if (FileExists(expectedPath))
	{
		MSBuild(vsixProject, new MSBuildSettings
		{
			Configuration = configuration,
			ToolPath = expectedPath
		}.SetVerbosity(Verbosity.Minimal));
	}
});

Task("Default")
	.IsDependentOn("Tests")
	.IsDependentOn("Create-VSIX-Package");

RunTarget(target);
