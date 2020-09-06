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

EndGroup "Restoring .NET Core Tools"

StartGroup "Bootstrapping Cake"

dotnet cake --bootstrap

EndGroup "Bootstrapping Cake"

StartGroup "Building project with cake"

dotnet cake $@

EndGroup "Bootstrapping Cake"
