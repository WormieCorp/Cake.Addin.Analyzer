#!/bin/sh
StartGroup() {
	if [ ! -z "$GITHUB_ACTION" ]; then
		echo "::group::$1"
	else
		echo "##################################################"
		printf '# %-46s #' "$1"
		echo ""
		echo "##################################################"
	fi
}

EndGroup() {
	if [ ! -z "$GITHUB_ACTION" ]; then
		echo "::endgroup::$1"
	else
		echo "##################################################"
	fi
}

StartGroup "Restoring .NET Core Tools"

dotnet tool restore

exitCode=$?

EndGroup "Restoring .NET Core Tools"

if [ "$exitCode" -ne "0" ]; then
  exit $exitCode
fi

StartGroup "Bootstrapping Cake"

dotnet cake --bootstrap

exitCode=$?

EndGroup "Bootstrapping Cake"

if [ "$exitCode" -ne "0" ]; then
  exit $exitCode
fi

StartGroup "Building project with cake"

dotnet cake $@

exitCode=$?

EndGroup "Bootstrapping Cake"

if [ "$exitCode" -ne "0" ]; then
  exit $exitCode
fi
