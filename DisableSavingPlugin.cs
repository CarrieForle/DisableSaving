using System;
using BepInEx;
using HarmonyLib;
using BepInEx.Configuration;
using System.Reflection;

namespace DisableSaving;

// TODO - adjust the plugin guid as needed
[BepInAutoPlugin(id: "io.github.carrieforle.disablesaving")]
public partial class DisableSavingPlugin : BaseUnityPlugin
{
    public static ConfigEntry<bool> disableSaving;

    private void Awake()
    {
        disableSaving = Config.Bind(
            "DisableSaving",
            "DisableSaving",
            true,
            "Disable saving game progress");

        Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), Id);
    }
}

[HarmonyPatch(typeof(CheatManager), nameof(CheatManager.AllowSaving), MethodType.Getter)]
class Patch
{
    static void Postfix(ref bool __result)
    {
        __result = !DisableSavingPlugin.disableSaving.Value;
    }
}