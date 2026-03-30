# Prompt Forge Demo Release

This workflow packages the existing demo build without changing Prompt Forge's prompt-generation logic or unlock behavior.

## Publish Strategy

- `Release` configuration
- `win-x64`
- self-contained
- normal published folder layout

This keeps the demo build reliable for WPF and avoids single-file packaging surprises around desktop resources and runtime behavior.

## Files Added For Packaging

- `PromptForge.App/Properties/PublishProfiles/PromptForgeDemo.pubxml`
- `tools/Build-DemoRelease.ps1`
- `tools/installer/PromptForgeDemo.iss`
- `tools/New-PromptForgeIcon.ps1`

## Release Steps

1. Open PowerShell in the repo root.
2. Run:

```powershell
powershell -ExecutionPolicy Bypass -File .\tools\Build-DemoRelease.ps1
```

3. If Inno Setup 6 is installed, the script generates:

`C:\Users\windy\OneDrive\Desktop\ready\PromptForge-Demo-Setup.exe`

4. The script also copies the publish folder to:

`C:\Users\windy\OneDrive\Desktop\ready\PromptForge-Demo`

5. For a visible click target even before an installer is built, the script also creates:

`C:\Users\windy\OneDrive\Desktop\ready\PromptForge Demo.lnk`

6. The script also places a release README next to the installer:

`C:\Users\windy\OneDrive\Desktop\ready\README.txt`

## External Prerequisite For Installer Build

Install Inno Setup 6 so `ISCC.exe` is available at one of these standard paths:

- `C:\Users\windy\AppData\Local\Programs\Inno Setup 6\ISCC.exe`
- `C:\Program Files (x86)\Inno Setup 6\ISCC.exe`
- `C:\Program Files\Inno Setup 6\ISCC.exe`

If Inno Setup is not installed yet, the script still produces the cleaned publish folder plus shortcut and warns that the installer step was skipped.
