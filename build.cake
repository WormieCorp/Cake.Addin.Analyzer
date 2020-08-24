var target = Argument("target", "Default");
var configuration = Argument("configuration", "Release");
const string solution = "./src/CakeContrib.Analyzer_core.sln";
const string vsixProject = "./src/CakeContrib.Analyzer.Vsix/CakeContrib.Analyzer.Vsix.csproj";

Setup((context) =>
{
	var version = GitVersion(new GitVersionSettings
	{
		UpdateAssemblyInfo = false
	});

	return version;
});

Task("Restore")
	.Does(() =>
{
	DotNetCoreRestore(solution);
	DotNetCoreRestore(vsixProject);
});

Task("Build")
	.IsDependentOn("Restore")
	.Does<GitVersion>((version) =>
{
	DotNetCoreBuild(solution, new DotNetCoreBuildSettings
	{
		Configuration = configuration,
		MSBuildSettings = new DotNetCoreMSBuildSettings()
			.SetVersionPrefix(version.MajorMinorPatch),
		NoIncremental = true,
		NoRestore     = true,
		VersionSuffix = version.PreReleaseTag,
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
	Warning("VSIX Extensions are not supported, and should only be used for debugging purposes");
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

Task("Publish-GitHub-Packages")
	.WithCriteria(BuildSystem.IsRunningOnGitHubActions)
	.Does(() =>
{
	var packages = GetFiles($"./src/**/{configuration}/**/*.nupkg");
	// TODO
});

Task("Default")
	.IsDependentOn("Create-VSIX-Package")
	.IsDependentOn("Tests");

RunTarget(target);
