using HarmonyLib;
using InventorySystem.Items.Firearms.Modules;
using InventorySystem.Items.Firearms;
using PlayerStatsSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PluginAPI.Core;
using System.Threading.Tasks;

namespace LurkBoisModded.Extensions
{
    public static class FirearmExtensions
    {
        public static Firearm GetFirearm(this AutomaticAmmoManager manager)
        {
            Firearm f = (Firearm)AccessTools.Field(typeof(AutomaticAmmoManager), "_firearm").GetValue(manager);
            if (f == null)
            {
                Log.Warning("Firearm is null!");
            }
            return f;
        }

        public static Firearm GetFirearm(this ClipLoadedInternalMagAmmoManager manager)
        {
            Firearm f = (Firearm)AccessTools.Field(typeof(ClipLoadedInternalMagAmmoManager), "_firearm").GetValue(manager);
            if (f == null)
            {
                Log.Warning("Firearm is null!");
            }
            return f;
        }

        public static Firearm GetFirearm(this DisruptorAction action)
        {
            Firearm f = (Firearm)AccessTools.Field(typeof(DisruptorAction), "_firearm").GetValue(action);
            if (f == null)
            {
                Log.Warning("Firearm is null!");
            }
            return f;
        }

        public static Firearm GetFirearm(this TubularMagazineAmmoManager manager)
        {
            Firearm f = (Firearm)AccessTools.Field(typeof(TubularMagazineAmmoManager), "_firearm").GetValue(manager);
            if (f == null)
            {
                Log.Warning("Firearm is null!");
            }
            return f;
        }

        public static Dictionary<HitboxType, float> GetHitboxForce(this FirearmDamageHandler handler)
        {
            return AccessTools.FieldRefAccess<FirearmDamageHandler, Dictionary<HitboxType, float>>("HitboxToForce").Invoke(handler);
        }

        public static Dictionary<ItemType, float> GetForceByAmmoType(this FirearmDamageHandler handler)
        {
            return AccessTools.FieldRefAccess<FirearmDamageHandler, Dictionary<ItemType, float>>("AmmoToForce").Invoke(handler);
        }
    }
}
