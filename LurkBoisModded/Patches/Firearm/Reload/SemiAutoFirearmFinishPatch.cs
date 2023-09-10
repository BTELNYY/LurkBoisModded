using HarmonyLib;
using InventorySystem.Items.Firearms.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LurkBoisModded.Patches.Firearm.Reload
{
    [HarmonyPatch(typeof(ClipLoadedInternalMagAmmoManager), nameof(ClipLoadedInternalMagAmmoManager.ServerTryReload))]
    public class SemiAutoFirearmFinishPatch
    {
        public static Action<IAmmoManagerModule, InventorySystem.Items.Firearms.Firearm> OnReloadFinish;

        public static void Postfix(ClipLoadedInternalMagAmmoManager __instance, ref bool __result)
        {
            if (__result)
            {
                InventorySystem.Items.Firearms.Firearm firearm = (InventorySystem.Items.Firearms.Firearm)AccessTools.Field(typeof(ClipLoadedInternalMagAmmoManager), "_firearm").GetValue(__instance);
                OnReloadFinish?.Invoke(__instance, firearm);
            }
        }
    }
}
