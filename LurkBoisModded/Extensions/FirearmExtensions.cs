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
using InventorySystem.Items.Firearms.Attachments;

namespace LurkBoisModded.Extensions
{
    public static class FirearmExtensions
    {
        public static Dictionary<HitboxType, float> GetHitboxForce(this FirearmDamageHandler handler)
        {
            return AccessTools.FieldRefAccess<FirearmDamageHandler, Dictionary<HitboxType, float>>("HitboxToForce").Invoke(null);
        }


        public static Dictionary<ItemType, float> GetForceByAmmoType(this FirearmDamageHandler handler)
        {
            return AccessTools.FieldRefAccess<FirearmDamageHandler, Dictionary<ItemType, float>>("AmmoToForce").Invoke(null);
        }
    }
}
