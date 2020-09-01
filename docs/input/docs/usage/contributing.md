---
Order: 20
Title: Contributing
---

This is very much an active project so any and all contributions are welcome,
even just finding issues!

## Reporting Issues

All issues should be tracked at [GitHub](https://github.com/WormieCorp/Cake.Addin.Analyzer),
with enough information to reproduce the issue.

## Code Contributions

This repository is based around the Git Flow workflow, using feature/hotfix/bugfix
branches and pull requests to manage incoming changes and fixes.
Generally speaking you can follow a similar guidance as Cake itself with a few changes
(found [here](https://cakebuild.net/docs/contributing/contribution-guidelines)),
which can be summarised as follows:

- Find a change or fix you want to implement
- Fork the repo
- Workflow for new features
  - Create a new branch named `feature/<your-feature-name>` and make your changes
  - Open a PR from your feature branch against the `develop` branch
    (include the GitHub issue number)
- Workflow for bug fixes in the latest stable version
  - Create a new branch named `hotfix/<your-bugfix-name>` and make your changes
  - Open a PR from your hotfix branch against the `master` branch
    (include the GitHub issue number)  
    _(This will be re-targeted to a different branch when accepted)_
- Workflow for bug fixes in an unpublished version of Cake.Addin.Analyzer
  - Create a new branch named `bugfix/<your-bugfix-name>` and make your changes
  - Open a PR from your bugfix branch against the `develop` branch
    (include the GitHub issue number)
- Success! I will provide feedback if needed, or just accept the changes directly
  and they should appear in the next release

Additionally to the Cake guidelines, we als follow a convention regarding
commit messages and github pull request titles.
The commits and pull request titles are expected to follow the
[Conventional Commits](https://www.conventionalcommits.org/en/v1.0.0/) convention,
with the following types supported:

- `feat:` - Adds a new feature to the analyzer. This is generally a new Rule,
  or a new Code Fix.
- `fix:` - Fixes an existing issue/bug in the library
- `docs:` - Changes only related to documentation. This can be both xml comments,
  the wyam documentation, the readme or even just updating the year in the License.
- `style:` - Changes that do not affect the meaning of any code
  (white-space, formatting, missing semi-colons, etc)
- `refactor:` - A code change that neither fixes a bug nor adds a feature
- `perf:` - A code change that improves performance
- `test:` - Adding missing unit tests or correcting existing tests
- `build:` - Changes that affect the build system or external dependencies
- `ci:` - Changes to our CI configuration (also requires a scope to be used,
  these can be `ga` or `codeql`)
- `chore:` - Other changes that don't modify src or test files
- `revert:` - A commit that reverts a previous commit (should only be used if
  the code is already in develop/master)

## License

Note that this project (and all contributions) fall under the [MIT License terms](https://github.com/WormieCorp/Cake.Addin.Analyzer/blob/master/LICENSE.txt).
