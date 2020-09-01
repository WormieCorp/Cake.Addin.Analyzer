---
Order: 2
Title: Building on Linux
Author: Kim J. Nordmo
---

## Requirements

The following are needed to build the Analyzer on Windows

- .NET Core SDK 3.1 and 2.1
- Mono 6.0.0+ _(earlier versions may work, but are not supported)_

All other dependencies will be automatically downloaded when invoking the build script.

## Invoking the build itself

1. To build the Analyzer, just open any shell
   and navigate to the root of downloaded/cloned repository.
2. After that just type `sh build.sh` and everything will be automatically
   built and all unit tests will run as well as a nuget package will be created.

_NOTE: The VSIX extension will not be built, and there is no way to debug
the extension on Linux_
