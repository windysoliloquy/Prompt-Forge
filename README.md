# Prompt Forge

Prompt Forge is a native Windows desktop app for building and iterating image-generation prompts through structured visual controls instead of chat-style drafting. It is designed for creators who want faster iteration, cleaner prompt language, and more deliberate control over style, mood, composition, artist influence, and output.

Prompt Forge is built specifically for image-generation prompt workflows. It is not a general-purpose AI chat prompt tool.

## Why Prompt Forge

- Instant prompt iteration without waiting on a chat model
- Structured visual control instead of freeform prompt wrestling
- Cleaner, more repeatable prompt-building workflows
- Strong balance of guided direction and manual control
- Better suited to focused desktop sessions than chat-based drafting

## What It Does

- Builds image-generation prompts through structured visual controls
- Updates prompts live as you adjust controls
- Supports both guided starting points and full manual refinement
- Generates both positive and negative prompt output
- Saves reusable setups locally for repeat workflows

## Key Features

- Live prompt preview
- Structured subject, action, and relationship framing
- Manual control over style, composition, mood, lighting, and color
- Guided prompt direction for faster ideation
- Dual artist influence blending with independent strength controls
- Artist blend summaries across composition, palette, surface, and mood
- Guided negative prompt generation with common exclusions built in
- Local preset save, load, rename, and delete support
- One-click copy for positive and negative prompts
- Multiple visual skins with persistent theme selection
- Native Windows desktop UI built for longer sessions

## Creative Controls

Prompt Forge includes structured controls for:

- Stylization
- Realism
- Texture Depth
- Narrative Density
- Symbolism
- Surface Age
- Camera Distance
- Camera Angle
- Background Complexity
- Motion Energy
- Atmospheric Depth
- Chaos
- Whimsy
- Tension
- Awe
- Lighting
- Saturation
- Contrast
- Aspect Ratio
- Print-Ready Output
- Transparent Background Output

## Artist Influence System

Prompt Forge includes a built-in artist profile library that lets you apply one or two artist influences to a prompt with adjustable strength. The system is designed to create nuanced stylistic direction rather than simple name-dropping, helping shape:

- Composition
- Palette
- Surface Character
- Mood

Prompt Forge supports a very large built-in prompt space based on its current controls and artist library. With user-defined styles added, the practical range expands much further.

## Negative Prompt Tools

Prompt Forge can generate a separate negative prompt based on guided exclusions such as:

- clutter
- muddy lighting
- distorted anatomy
- extra limbs or fingers
- text or watermark artifacts
- oversaturation
- flat composition
- messy backgrounds
- weak material definition
- blurry detail

## Screenshots

### Main Workspace

![Prompt Forge main workspace](docs/screenshots/Screenshot%202026-03-28%20125121.png)

### Prompt and Blend View

![Prompt Forge prompt blend view](docs/screenshots/Screenshot%202026-03-28%20125152.png)

### Full Window Overview

![Prompt Forge full interface](docs/screenshots/Screenshot%202026-03-28%20125219.png)

## Tech Stack

- C#
- .NET 8
- WPF
- MVVM

## Run From Source

1. Open [PromptForge.sln](PromptForge.sln) in Visual Studio 2022.
2. Make sure `PromptForge.App` is the startup project.
3. Press `F5` to run with debugging, or `Ctrl+F5` to run without debugging.

If you need help locating local build output, read [WHERE_TO_FIND_THE_APP.md](WHERE_TO_FIND_THE_APP.md).

## Demo Release Packaging

Prompt Forge now includes a repeatable Windows demo packaging path:

- dedicated Release publish profile
- self-contained `win-x64` publish output
- Inno Setup installer generation
- release README copied beside the installer

Release workflow details are in [docs/DEMO_RELEASE.md](docs/DEMO_RELEASE.md).

## Repo Notes

- Generated `bin/`, `obj/`, `.vs/`, `.dotnet/`, and packaged `artifacts/` output are ignored.
- Demo release assets are produced locally and copied into the owner’s `ready` folder during packaging.
- The app stores presets, theme state, demo state, and local unlock state under `%AppData%\PromptForge`.
