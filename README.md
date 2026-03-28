# Prompt Forge

Prompt Forge is a native Windows desktop app for building high-quality image prompts through structured controls instead of manual prompt writing. It is designed as a visual prompt synthesizer: fast, clean, and focused on shaping prompt language through artistic controls instead of chat-style interaction.

## Start Here

If you are opening the repo for the first time:

1. Open [PromptForge.sln](PromptForge.sln) in Visual Studio 2022.
2. Run the app with Ctrl+F5.
3. If you need help finding the built executable, read [WHERE_TO_FIND_THE_APP.md](WHERE_TO_FIND_THE_APP.md).

If the app has already been built, the executable is usually here:

PromptForge.App\bin\Debug\net8.0-windows\PromptForge.App.exe

## What It Does

Prompt Forge helps build image prompts through structured controls for:

- subject, action, and scene framing
- art style, material, and artist influence
- composition, mood, lighting, and color
- negative constraints and cleaner negative prompt generation
- reusable presets saved locally as JSON

## Showcase

### Main Workspace

![Prompt Forge main workspace](docs/screenshots/Screenshot%202026-03-28%20125121.png)

The main workspace gives a clean split between left-side controls and a large live prompt preview, keeping the generation flow readable while you iterate.

### Prompt and Blend View

![Prompt Forge prompt blend view](docs/screenshots/Screenshot%202026-03-28%20125152.png)

Artist influence blending, live preview updates, and action buttons stay visible together so prompt iteration feels fast and desktop-native instead of crowded.

### Full Window Overview

![Prompt Forge full interface](docs/screenshots/Screenshot%202026-03-28%20125219.png)

The full layout is designed to stay light, spacious, and practical for longer prompt-building sessions.

## Core Features

- Light-themed WPF desktop UI
- Live prompt preview
- Dual artist influence blending
- Artist blend summary cards
- Negative constraints with guided exclusions
- Copy, reset, save, load, rename, and delete preset actions
- Local JSON preset storage in %AppData%\PromptForge\Presets

## Stack

- C#
- .NET 8
- WPF
- MVVM

## Quick Launch

1. Open [PromptForge.sln](PromptForge.sln).
2. Make sure PromptForge.App is the startup project.
3. Press F5 to run with debugging, or Ctrl+F5 to run without debugging.

## Repo Notes

- Generated in/ and obj/ output is no longer tracked in Git.
- The local scrape cache is intentionally kept out of the repo.
- If you are sharing the app with someone who is not comfortable with Git, sending the built output folder or a zip through Google Drive is completely fine.
