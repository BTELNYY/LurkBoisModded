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
        public static Firearm GetFirearm(this AutomaticAmmoManager manager)
        {
            Firearm f = (Firearm)AccessTools.Field(typeof(AutomaticAmmoManager), "_firearm").GetValue(manager);
            if (f == null)
            {
                Log.Warning("Firearm is null!");
            }
            return f;
        }

        public static void ApplyAttachments(this Firearm firearm)
        {
            if (AttachmentsServerHandler.PlayerPreferences.TryGetValue(firearm.Owner, out var value) && value.TryGetValue(firearm.ItemTypeId, out var value2))
                firearm.ApplyAttachmentsCode(value2, reValidate: true);
            var firearmStatusFlags = FirearmStatusFlags.MagazineInserted;
            if (firearm.HasAdvantageFlag(AttachmentDescriptiveAdvantages.Flashlight))
                firearmStatusFlags |= FirearmStatusFlags.FlashlightEnabled;

            firearm.Status = new FirearmStatus(firearm.Status.Ammo, firearmStatusFlags, firearm.GetCurrentAttachmentsCode());
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
