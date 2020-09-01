function StartGroup([string]$message) {
	if (Test-Path Env:\GITHUB_ACTION) {
		"::group::$message"
	}
 else {
		"##################################################"
		"# {0,-46} #" -f $message
		"##################################################"
		""
	}
}

function EndGroup([string]$message) {
	if (Test-Path Env:\GITHUB_ACTION) {
		"::endgroup::$message"
	}
 else {
		"##################################################"
		""
	}
}

StartGroup "Restoring .NET Core Tools"

dotnet tool restore

EndGroup "Restoring .NET Core Tools"

StartGroup "Bootstrapping Cake"

dotnet cake --bootstrap

EndGroup "Bootstrapping Cake"

StartGroup "Building project with cake"

dotnet cake @args

EndGroup "Bootstrapping Cake"
