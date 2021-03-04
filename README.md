# Cake Addin Analyzer (Cake.Addin.Analyzer)

<!-- ALL-CONTRIBUTORS-BADGE:START - Do not remove or modify this section -->
[![All Contributors](https://img.shields.io/badge/all_contributors-2-orange.svg?style=flat-square)](#contributors-)
<!-- ALL-CONTRIBUTORS-BADGE:END -->
[![standard-readme compliant](https://img.shields.io/badge/standard--readme-OK-green.svg?style=flat-square)](https://github.com/RichardLitt/standard-readme)
![GitHub Workflow Status](https://img.shields.io/github/workflow/status/WormieCorp/Cake.Addin.Analyzer/Build?logo=github&style=flat-square)
![Nuget](https://img.shields.io/nuget/v/Cake.Addin.Analyzer?logo=nuget&style=flat-square)
![CodeQL](https://github.com/WormieCorp/Cake.Addin.Analyzer/workflows/CodeQL/badge.svg)

Analyzer for cake addins to detect if there are any recommended or required changes that can be done.

## Table of Contents

- [Background](#background)
- [Install](#install)
- [Usage](#usage)
- [Maintainers](#maintainers)
- [Contributing](#contributing)
- [License](#license)

## Background

This analyzer came into the created because I wanted to get some conformaty with Cake addins by reporting recommended/required changes that are expected for every Cake addin.

## Install

You can simply make use of this analyzer by running the following command in the directory of each project you wish the analyzer to run on.

```shell
dotnet add package Cake.Addin.Analyzer
```

## Usage

There is nothing specifically that needs to be done to use the analyzer.
It will automatically run when calling `dotnet build`, or when opening a file in Visual Studio.

You can configure the severity of each rule using a editorconfig file (see each rule for details on how here: <https://wormiecorp.github.io/Cake.Addin.Analyzer/rules>).

## Maintainers

[@AdmiringWorm](https://github.com/AdmiringWorm)

## Contributing

See [the contributing file](CONTRIBUTING.md)!

PRs accepted.

Small note: If editing the README, please conform to the [standard-readme](https://github.com/RichardLitt/standard-readme) specification.

### Contributors ✨

Thanks goes to these wonderful people ([emoji key](https://allcontributors.org/docs/en/emoji-key)):

<!-- ALL-CONTRIBUTORS-LIST:START - Do not remove or modify this section -->
<!-- prettier-ignore-start -->
<!-- markdownlint-disable -->
<table>
  <tr>
    <td align="center"><a href="https://github.com/AdmiringWorm"><img src="https://avatars.githubusercontent.com/u/1474648?v=4?s=100" width="100px;" alt=""/><br /><sub><b>Kim J. Nordmo</b></sub></a><br /><a href="#maintenance-AdmiringWorm" title="Maintenance">🚧</a></td>
    <td align="center"><a href="https://github.com/pascalberger"><img src="https://avatars.githubusercontent.com/u/2190718?v=4?s=100" width="100px;" alt=""/><br /><sub><b>Pascal Berger</b></sub></a><br /><a href="https://github.com/WormieCorp/Cake.Addin.Analyzer/issues?q=author%3Apascalberger" title="Bug reports">🐛</a></td>
  </tr>
</table>

<!-- markdownlint-restore -->
<!-- prettier-ignore-end -->

<!-- ALL-CONTRIBUTORS-LIST:END -->

This project follows the [all-contributors](https://github.com/all-contributors/all-contributors) specification. Contributions of any kind welcome!

## License

[MIT © 2020-2021 Kim J. Nordmo](LICENSE.txt)
