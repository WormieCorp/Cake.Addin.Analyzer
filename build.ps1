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

$exitCode = $LASTEXITCODE

EndGroup "Restoring .NET Core Tools"

if ($exitCode -ne 0) {
	exit $exitCode
}

StartGroup "Bootstrapping Cake"

dotnet cake --bootstrap

EndGroup "Bootstrapping Cake"

if ($exitCode -ne 0) {
	exit $exitCode
}

StartGroup "Building project with cake"

dotnet cake @args

EndGroup "Bootstrapping Cake"

if ($exitCode -ne 0) {
	exit $exitCode
}
