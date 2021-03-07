DtronixModel [![Build Status](https://github.com/Dtronix/DtronixModel/actions/workflows/dotnet-core-desktop.yml/badge.svg)](https://github.com/Dtronix/DtronixModel/actions/workflows/dotnet-core-desktop.yml)
============
DtronixModel is a compact .net tool to aid in database modeling.

#### Nuget Packages
| Package | Status |
| ------- | ------ |
|DtronixModel | [![NuGet](https://img.shields.io/nuget/v/DtronixModel.svg?maxAge=60)](https://www.nuget.org/packages/DtronixModel) |
|DtronixModel.Tools | [![NuGet](https://img.shields.io/nuget/v/DtronixModel.Tools.svg?maxAge=60)](https://www.nuget.org/packages/DtronixModel.Tools) |

#### Supported Databases
- MySql
- SQLite

### Build Requirements

The Ddl schema is generated from an XSD  file with the program "xsd2code community edition 3.4". https://xsd2code.codeplex.com/

### Installation

On Windows, run the "install.bat" in PowerShell and this will restore the NuGet packages, create the installer and silently install the DtronixModeler application.

### License
Released under [MIT license](http://opensource.org/licenses/MIT).
