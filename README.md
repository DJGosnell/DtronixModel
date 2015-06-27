DtronixModel
============
DtronixModel is a small .net tool to aid in database creation and data modeling for SQLite and MySQL databases.

### Build Requirements
NuGet Package System.Data.SQLite.Core.1.0.97.0

NuGet Package MySql.ConnectorNET.Data.6.8.3.2

To generate the installer, InnoSetup 5.5.5 is required.
http://www.jrsoftware.org/isdl.php

The Ddl schema is geenrated from an XSD  file with the program "xsd2code community edition 3.4". https://xsd2code.codeplex.com/

### Installation

On Windows, run the "install.ps1" in PowerShell and this will restore the NuGet packages, create the installer and silently install the DtronixModeler application.

### License
Released under [MIT license](http://opensource.org/licenses/MIT).
