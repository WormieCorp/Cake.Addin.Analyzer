# cake-addin-analyzer

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

## License

[MIT © 2020-2021 Kim J. Nordmo](LICENSE.txt)
