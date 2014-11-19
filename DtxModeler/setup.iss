; Script generated by the Inno Setup Script Wizard.
; SEE THE DOCUMENTATION FOR DETAILS ON CREATING INNO SETUP SCRIPT FILES!

#dim Version[4]
#define MainBinaryName "DtxModeler.exe"
#expr ParseVersion(MainBinaryName, Version[0], Version[1], Version[2], Version[3])
#define AppVersion Str(Version[0]) + "." + Str(Version[1]) + "." + Str(Version[2]) + "." + Str(Version[3])
#define ShortAppVersion Str(Version[0]) + "." + Str(Version[1]) + "." + Str(Version[2])

[Setup]
; NOTE: The value of AppId uniquely identifies this application.
; Do not use the same AppId value in installers for other applications.
; (To generate a new GUID, click Tools | Generate GUID inside the IDE.)
AppId={{D516E71D-8297-41AC-9CB4-123FC50072AD}
AppName=DtronixModel
AppVersion={#ShortAppVersion}
AppPublisher=Dtronix
AppPublisherURL=http://dtronix.com
AppSupportURL=http://dtronix.com
AppUpdatesURL=http://dtronix.com
DefaultDirName={pf}\DtronixModel
DefaultGroupName=DtronixModel
OutputDir=/
OutputBaseFilename=DtronixModeler-{#ShortAppVersion}
SetupIconFile=C:\Program Files (x86)\Inno Setup 5\Examples\Setup.ico
Compression=lzma
SolidCompression=yes
ArchitecturesInstallIn64BitMode=x64

[Languages]
Name: "english"; MessagesFile: "compiler:Default.isl"

[Files]
Source: {#MainBinaryName}; DestDir: "{app}"; Flags: ignoreversion
; NOTE: Don't use "Flags: ignoreversion" on any shared system files
Source: "DtxModel.dll"; DestDir: "{app}"
Source: "System.Data.SQLite.dll"; DestDir: "{app}"
Source: "x86\SQLite.Interop.dll"; DestDir: "{app}\x86"
Source: "x64\SQLite.Interop.dll"; DestDir: "{app}\x64"

[Icons]
Name: "{group}\DtronixModel"; Filename: "{app}\{#MainBinaryName}"
Name: "{group}\{cm:UninstallProgram,DtronixModel}"; Filename: "{uninstallexe}"

[Run]
Filename: "{app}\DtxModeler.exe"; Description: "{cm:LaunchProgram,DtronixModel}"; Flags: nowait postinstall skipifsilent
