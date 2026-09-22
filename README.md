# Aeon

Aeon is an experimental, machine-intelligence .NET conversational-agent runtime. It combines configurable text normalization, pattern-based dialogue matching, participant-specific predicates, and file-backed personality data.

The project is under active development and is intended for research, demonstration, and controlled experimentation. It is not represented as a general-purpose or safety-critical autonomous system.

## Current status

The runtime loads a configured personality and supports terminal conversations. The checked-in Rhodo personality is the default demonstration configuration. Learning, emotional behavior, and broader embodiment features remain experimental.

## Repository layout

| Path | Purpose |
| --- | --- |
| `Aeon.Library` | Core interpreter, normalization, dialogue processing, and utilities. |
| `Aeon.Runtime` | .NET console host, configuration, personalities, and runtime assets. |
| `Aeon.Library.SmokeTests` | Dependency-free regression checks for core library contracts. |
| `docs` | Reference material, validation documents, patent material, and development notes. |
| `docs/user-manual` | Central LaTeX user, architecture, authoring, and extension manual. |
| `media` | Demonstration media. |

## Run Aeon

Aeon targets .NET 10. The checked-in Rhodo personality and configuration are copied beside the runtime during the build.

```bash
dotnet build Aeon.Runtime/Aeon.Runtime.csproj --configuration Release
dotnet run --project Aeon.Runtime/Aeon.Runtime.csproj --configuration Release --no-build
```

Enter a message and press Enter. Type `exit` to end the console session, or `quit` to leave terminal mode. Transcripts and diagnostic logs are written below the runtime output directory: `bin/<configuration>/net10.0/logs`.

## Linux speech output

Text output works without additional software. To enable speech on Linux without access to any private package, install eSpeak NG and enable the bundled fallback in `Aeon.Runtime/config/settings.xml`:

```bash
sudo apt install espeak-ng
```

```xml
<item name="voiceenabled" value="true"/>
<item name="voicebackend" value="espeak"/>
```

Then run Aeon normally. The default fallback uses `espeak-ng` and the `en-us` voice; customize `espeakcommand` or `espeakvoice` in the same settings file if needed. Set `voicebackend` to `auto` to prefer the optional AeonVoice backend when it has been included, while retaining eSpeak NG as the fallback.

### Optional AeonVoice profiles

Authorized Linux x64 users can include the private AeonVoice package for the Toptygin and Leena profiles. Copy `NuGet.Private.config.example` to `NuGet.Config`, authenticate to GitHub Packages using local credentials, and run:

```bash
dotnet run --project Aeon.Runtime/Aeon.Runtime.csproj --configuration Release -p:UsePrivateAeonVoice=true
```

This enhancement is optional: do not add the private feed or build property when using eSpeak NG alone.

## Verify the core

Run the dependency-free smoke tests after changes to the interpreter or participant state:

```bash
dotnet run --project Aeon.Library.SmokeTests/Aeon.Library.SmokeTests.csproj --configuration Release
```

The smoke tests currently cover participant-state isolation and request ownership. The command-line session above remains the end-to-end runtime check.

## Engineering notes

- Personality files are loaded in deterministic order, so category replacement is consistent across operating systems.
- Participant predicates are private to each conversation; one participant's state does not affect another's.
- Startup exits clearly when required configuration or personality categories cannot be loaded.
- The interpreter uses pattern-based rules; personality data should be reviewed and tested with the behavior it is intended to produce.

## User manual

The central guide for operation, theory, personality authoring, commands, state, extension code, testing, and responsible deployment is [the Aeon User Manual](docs/user-manual/Aeon-User-Manual.tex). Build it with `pdflatex` from `docs/user-manual`; that directory's [README](docs/user-manual/README.md) includes the exact command.

## Project history

Aeon originated as a conversational software project in 2003 and has been developed across several software and hardware experiments. Its current focus is a maintainable .NET runtime and a high-quality interactive experience. Historical reference material is retained in [`docs`](docs/).

## Support Aeon

Support helps sustain Aeon's research, maintenance, and demonstrations. Choose the option that works best for you:

- [Sponsor Cartheur on Patreon](https://www.patreon.com/cartheur)
- [Support Cartheur on Liberapay](https://liberapay.com/cartheur)

GitHub also surfaces these options from the repository's **Sponsor** menu.

## License

Aeon is provided under the [Aeon Proprietary Reference-Only License](LICENSE). It may be viewed, studied, and evaluated for personal or internal reference; other use requires prior written permission from Cartheur.
