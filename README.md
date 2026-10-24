# ForgivingFalls Revamped

<img src="Thunderstore_Publish/icon.png" alt="ForgivingFalls Revamped" width="160" align="right">

A fork of [Pandaismyname1's PeakForgivingFalls](https://github.com/Pandaismyname1/PeakForgivingFalls),
updated for current PEAK and retuned to work with the game's climbing code instead of over it.
All credit for the original idea and implementation goes to Pandaismyname1; MIT, same as upstream.

Grab a wall on the way down and you get to keep it. Run out of stamina mid-climb and you slide
instead of being peeled straight off.

## Why a revamp

Since the original release, PEAK grew its own version of this mechanic. `StartClimbRpc` now slides
you down the wall when you grab it out of stamina, scaled by how far you reached and how fast you
were already falling, and it tops up a timer that `Climbing()` squares into a ramp that eventually
peels you off. Fall damage from a grab lands once that slide passes roughly 17.9.

Two things followed from that:

- The original mod stopped working outright. `playerSlide` became a property backed by a private
  field, so its `GetField("playerSlide")` lookup returned null and the patch quietly did nothing.
- Hard-setting the slide to a flat value, as the original did, now throws away tuning the game
  does for itself.

So this version trims rather than replaces. It clamps the slide below the fall-damage threshold
and caps the peel-off timer, and lets the game's own curves shape everything under those limits.

## Configuration

`BepInEx/config/ForgivingFalls_Revamped.cfg`, written on first launch.

| Setting | Default | What it does |
| --- | --- | --- |
| `MaxAttachSlide` | `15` | Largest downward slide a wall grab may hand you. Fall damage starts past roughly 17.9, so the default keeps a grab survivable while leaving the game's slide curve intact below it. `5` matches the original mod, `0` removes the slide. |
| `MaxOutOfStaminaClimbTime` | `0.5` | Cap in seconds on the out-of-stamina climb timer after a grab. PEAK squares that timer to ramp the slide that peels you off, so a lower cap buys a longer hang. |
| `ResetFallTimerOnAttach` | `true` | Counts a grab as the end of the fall, so the landing after it is not charged for the airtime before it. PEAK already does this while you have stamina; this extends it to grabs made on empty. |

## Installation

Install through a mod manager, or drop `ForgivingFalls_Revamped.dll` in `BepInEx/plugins/`.

## Compatibility

- Verified against PEAK build 26155610 (Unity 6000.3.15f1), the 3.0.0 crossplay line
- BepInEx 5.x
- Mods are disabled in crossplay lobbies, so this applies to Steam-only lobbies

Safe to install alongside the original, which uses a different plugin GUID — though the original
does nothing on current builds, so there is no reason to run both.

## License

MIT, inherited from the original. See LICENSE.

## Building from source

1. Point the `PeakDir` property in `ForgivingFalls_Revamped.csproj` at your PEAK installation
2. `dotnet build -c Release`

Requires the .NET SDK and, for the `netstandard2.1` target, the netstandard targeting pack.
