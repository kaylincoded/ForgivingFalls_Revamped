using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;

namespace ForgivingFallsRevamped;

[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
public class Plugin : BaseUnityPlugin
{
    internal static new ManualLogSource Logger;

    internal static ConfigEntry<float> MaxAttachSlide;
    internal static ConfigEntry<float> MaxOutOfStaminaClimbTime;
    internal static ConfigEntry<bool> ResetFallTimerOnAttach;

    private void Awake()
    {
        Logger = base.Logger;

        MaxAttachSlide = Config.Bind("Falls", "MaxAttachSlide", 15f,
            "Largest downward slide the game may hand you for grabbing a wall. PEAK starts dealing "
            + "fall damage once the slide passes about 17.9, so 15 keeps a grab survivable while "
            + "leaving the vanilla slide curve untouched below that. Use 5 for the 1.0.x feel, or 0 "
            + "to remove the slide outright.");

        MaxOutOfStaminaClimbTime = Config.Bind("Falls", "MaxOutOfStaminaClimbTime", 0.5f,
            "Cap in seconds on the out-of-stamina climb timer after a grab. PEAK squares this timer "
            + "to ramp up the slide that peels you off the wall, so a lower cap buys a longer hang. "
            + "Raise it past the game's own limit to leave the timer alone.");

        ResetFallTimerOnAttach = Config.Bind("Falls", "ResetFallTimerOnAttach", true,
            "Count a grab as the end of the fall, so the landing afterwards is not charged for the "
            + "airtime before it. PEAK already does this on every climbing tick while you still have "
            + "stamina; this extends it to grabs made on empty.");

        var harmony = new HarmonyLib.Harmony(MyPluginInfo.PLUGIN_GUID);
        harmony.PatchAll();
    }
}
