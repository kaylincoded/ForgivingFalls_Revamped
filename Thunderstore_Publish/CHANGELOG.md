# Changelog

## 1.1.0
- Fixed the mod doing nothing on current PEAK builds. `CharacterClimbing.playerSlide` became a
  property backed by a private `_playerSlide` field, so the old `GetField("playerSlide")` lookup
  returned null and the patch bailed out before touching anything.
- Retuned around the out-of-stamina slide PEAK has since grown of its own. The patch no longer
  overwrites the slide with a flat value; it clamps it below the game's fall-damage threshold and
  caps the climb timer that ramps the slide, so the vanilla curves still shape everything below
  those limits.
- Dropped the old `outOfStaminaFor = 0.5` write. On current builds that field only gates climb
  jumping and the out-of-stamina screen shake, so setting it blocked climb jumps after a grab.
- Added config: `MaxAttachSlide`, `MaxOutOfStaminaClimbTime`, `ResetFallTimerOnAttach`.

## 1.0.1
- Updated Readme

## 1.0.0
- Initial Release
