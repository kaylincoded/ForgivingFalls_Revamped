using HarmonyLib;
using UnityEngine;

namespace ForgivingFallsRevamped;

// PEAK grew its own version of this mod. CharacterClimbing.StartClimbRpc now slides you down the
// wall when you grab it: playerSlide.y takes -outOfStamAttachSlide scaled by how far you reached
// whenever you are out of stamina, plus -convertFallVelToSlideCurve.Evaluate(-avarageVelocity.y)
// for the fall you arrived with, and data.outOfStaminaClimbingFor is topped up so Climbing() can
// square it into a ramp that peels you off the wall. SampleWall -> CheckFallDamage then injures
// you once the slide passes roughly 17.9, and zeroes data.sinceGrounded on the way through.
//
// So rather than replacing that with a flat slide, this trims it: clamp the slide below the damage
// threshold, cap the ramp timer, and let the vanilla curves shape everything underneath.
[HarmonyPatch(typeof(CharacterClimbing), "StartClimbRpc")]
public class CharacterClimbing_StartClimbRpc
{
    // Still private on CharacterClimbing, so it needs a field ref. Everything else this patch
    // touches is public, which keeps the breakage surface down to this one lookup.
    private static readonly AccessTools.FieldRef<CharacterClimbing, Character> CharacterOf =
        AccessTools.FieldRefAccess<CharacterClimbing, Character>("character");

    public static void Postfix(CharacterClimbing __instance)
    {
        var character = CharacterOf(__instance);
        if (character == null)
        {
            Plugin.Logger.LogWarning("CharacterClimbing.character was null; leaving the grab alone.");
            return;
        }

        var data = character.data;
        if (data == null)
        {
            Plugin.Logger.LogWarning("Character.data was null; leaving the grab alone.");
            return;
        }

        var maxSlide = Mathf.Abs(Plugin.MaxAttachSlide.Value);
        var slide = __instance.playerSlide;
        if (slide.y < -maxSlide)
        {
            slide.y = -maxSlide;
            __instance.playerSlide = slide;
        }

        // The game only ever raises this timer on an out-of-stamina grab, so capping it
        // unconditionally needs no stamina check of its own.
        var maxClimbTime = Mathf.Max(0f, Plugin.MaxOutOfStaminaClimbTime.Value);
        if (data.outOfStaminaClimbingFor > maxClimbTime)
        {
            data.outOfStaminaClimbingFor = maxClimbTime;
        }

        if (Plugin.ResetFallTimerOnAttach.Value)
        {
            data.sinceGrounded = 0f;
        }
    }
}
