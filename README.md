# Prompt Forge
Prompt Forge is a native Windows desktop app for building high-quality image prompts through structured controls instead of manual prompt writing.
## What New Users Should Open
If someone just wants to try the app after building it, the Windows executable is usually here:
PromptForge.App\\bin\\Debug\\net8.0-windows\\PromptForge.App.exe
There is also a dedicated beginner guide at the top of the repo:
- [WHERE_TO_FIND_THE_APP.md](C:/Users/windy/OneDrive/Desktop/Prompt%20Forge/WHERE_TO_FIND_THE_APP.md)
## Stack
- C#
- .NET 8
- WPF
- MVVM
## Open in Visual Studio
Open [PromptForge.sln](C:/Users/windy/OneDrive/Desktop/Prompt%20Forge/PromptForge.sln) in Visual Studio 2022 or later with the .NET desktop development workload installed.
## Build and Run in Visual Studio
1. Open [PromptForge.sln](C:/Users/windy/OneDrive/Desktop/Prompt%20Forge/PromptForge.sln).
2. Make sure PromptForge.App is the startup project.
3. Press F5 to run with debugging, or Ctrl+F5 to run without debugging.
4. Visual Studio will build the app and place the executable in PromptForge.App\\bin\\Debug\\net8.0-windows\\.
## Run the Built App Directly
If the solution has already been built, you can launch the app directly from:
PromptForge.App\\bin\\Debug\\net8.0-windows\\PromptForge.App.exe
If you do not see that file yet, build the solution once in Visual Studio first.
## Presets
Presets are saved as JSON in %AppData%\\PromptForge\\Presets.
## Notes
- This repository does not need the local scrape cache or generated build folders tracked in Git.
- If you are sharing the app with someone who is not comfortable with Git, sending the built output folder or a zip from Google Drive is completely fine.
