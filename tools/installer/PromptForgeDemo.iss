#ifndef SourceDir
  #define SourceDir "..\..\artifacts\demo\publish"
#endif

#ifndef OutputDir
  #define OutputDir "..\..\artifacts\demo\installer"
#endif

#ifndef SetupBaseName
  #define SetupBaseName "PromptForge-Demo-Setup"
#endif

#ifndef AppVersion
  #define AppVersion "1.0.0-demo"
#endif

#ifndef AppExeName
  #define AppExeName "PromptForge.App.exe"
#endif

#ifndef SetupIconFile
  #define SetupIconFile "..\..\PromptForge.App\Assets\PromptForge.ico"
#endif

[Setup]
AppId={{D0A51F85-9574-4D85-9781-43D80F2749D8}
AppName=Prompt Forge Demo
AppVersion={#AppVersion}
AppPublisher=Prompt Forge
DefaultDirName={localappdata}\Programs\Prompt Forge Demo
DefaultGroupName=Prompt Forge Demo
DisableProgramGroupPage=yes
UninstallDisplayIcon={app}\{#AppExeName}
OutputDir={#OutputDir}
OutputBaseFilename={#SetupBaseName}
SetupIconFile={#SetupIconFile}
Compression=lzma2
SolidCompression=yes
WizardStyle=modern
PrivilegesRequired=lowest
ArchitecturesAllowed=x64compatible
ArchitecturesInstallIn64BitMode=x64compatible

[Languages]
Name: "english"; MessagesFile: "compiler:Default.isl"

[Tasks]
Name: "desktopicon"; Description: "Create a desktop shortcut"; GroupDescription: "Additional icons:"

[Files]
Source: "{#SourceDir}\*"; DestDir: "{app}"; Flags: ignoreversion recursesubdirs createallsubdirs

[Icons]
Name: "{autoprograms}\Prompt Forge Demo"; Filename: "{app}\{#AppExeName}"
Name: "{autodesktop}\Prompt Forge Demo"; Filename: "{app}\{#AppExeName}"; Tasks: desktopicon

[Run]
Filename: "{app}\{#AppExeName}"; Description: "Launch Prompt Forge Demo"; Flags: nowait postinstall skipifsilent
