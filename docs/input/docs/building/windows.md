---
Order: 1
Title: Building on Windows
Author: Kim J. Nordmo
---

## Requirements

The following are needed to build the Analyzer on Windows:

- Visual Studio 2019
- .NET Core SDK 3.1 and 2.1
- .NET Framework 4.6.1

All other dependencies will be automatically downloaded when invoking the build script.

## Invoking the build itself

1. To build the Analyzer, just open powershell
   and navigate to the root of downloaded/cloned repository.
2. After that just type `.\build.ps1` and everything will be automatically
   built and all unit tests will run as well as a nuget package will be created.

_NOTE: If you wish to build and debug the VSIX extension, you need
to launch Visual Studio 2019 and build the project through that_
