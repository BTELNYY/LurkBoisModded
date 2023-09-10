using HarmonyLib;
using InventorySystem.Items.Firearms;
using InventorySystem.Items.Firearms.Modules;
using PluginAPI.Core;
using System;

namespace LurkBoisModded.Patches.Firearm.Reload
{
    [HarmonyPatch(typeof(AutomaticAmmoManager), nameof(AutomaticAmmoManager.ServerTryReload))]
    public class AutoFirearmReloadFinishPath
    {
        public static Action<IAmmoManagerModule, InventorySystem.Items.Firearms.Firearm> OnReloadFinish;

        public static void Postfix(AutomaticAmmoManager __instance, ref bool __result)
        {
            if (__result)
            {
                InventorySystem.Items.Firearms.Firearm firearm = (InventorySystem.Items.Firearms.Firearm)AccessTools.Field(typeof(AutomaticAmmoManager), "_firearm").GetValue(__instance);
                OnReloadFinish?.Invoke(__instance, firearm);
            }
        }
    }
}
