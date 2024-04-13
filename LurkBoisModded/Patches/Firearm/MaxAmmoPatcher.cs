using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HarmonyLib;
using System.Threading.Tasks;
using LurkBoisModded.Extensions;
using InventorySystem.Items.Firearms.Modules;

namespace LurkBoisModded.Patches.Firearm
{
    [HarmonyPatch]
    public class MaxAmmoPatcher
    {
        static Dictionary<ushort, byte> AmmoManager = new Dictionary<ushort, byte>();

        public static void Clear()
        {
            AmmoManager.Clear();
        }

        public static void AddFirearm(ushort id, byte value)
        {
            if(AmmoManager.ContainsKey(id))
            {
                AmmoManager[id] = value;
                return;
            }
            AmmoManager.Add(id, value);
        }

        public static void RemoveFirearm(ushort id)
        {
            if (!AmmoManager.ContainsKey(id))
            {
                AmmoManager.Remove(id);
            }
        }

        public static void UpdateFirearm(ushort id, byte value)
        {
            if (AmmoManager.ContainsKey(id))
            {
                AmmoManager[id] = value;
            }
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(AutomaticAmmoManager), nameof(AutomaticAmmoManager.MaxAmmo), MethodType.Getter)]
        public static void AutoFirearmPatch(AutomaticAmmoManager __instance, ref byte __result)
        {
            if (AmmoManager.ContainsKey(__instance.GetFirearm().ItemSerial))
            {
                __result = AmmoManager[__instance.GetFirearm().ItemSerial];
            }
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(TubularMagazineAmmoManager), nameof(TubularMagazineAmmoManager.MaxAmmo), MethodType.Getter)]
        public static void TubeFirearmPatch(TubularMagazineAmmoManager __instance, ref byte __result)
        {
            if (AmmoManager.ContainsKey(__instance.GetFirearm().ItemSerial))
            {
                __result = AmmoManager[__instance.GetFirearm().ItemSerial];
            }
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(ClipLoadedInternalMagAmmoManager), nameof(ClipLoadedInternalMagAmmoManager.MaxAmmo), MethodType.Getter)]
        public static void ClipLoadedFirearmPatch(ClipLoadedInternalMagAmmoManager __instance, ref byte __result)
        {
            if (AmmoManager.ContainsKey(__instance.GetFirearm().ItemSerial))
            {
                __result = AmmoManager[__instance.GetFirearm().ItemSerial];
            }
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(DisruptorAction), nameof(DisruptorAction.MaxAmmo), MethodType.Getter)]
        public static void DisruptorFirearmPatch(DisruptorAction __instance, ref byte __result)
        {
            if (AmmoManager.ContainsKey(__instance.GetFirearm().ItemSerial))
            {
                __result = AmmoManager[__instance.GetFirearm().ItemSerial];
            }
        }
    }
}
