using HarmonyLib;
using InventorySystem.Items;
using InventorySystem.Items.Firearms;
using InventorySystem.Items.Firearms.Modules;
using LurkBoisModded.Base;
using MEC;
using PlayerStatsSystem;
using PluginAPI.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LurkBoisModded.CustomItems
{
    public class SniperE11 : CustomItem, ICustomFirearmItem
    {
        public override CustomItemType CustomItemType => CustomItemType.SniperE11;

        public override ItemType BaseItemType => ItemType.GunE11SR;

        public override void OnItemCreated(ReferenceHub owner, ushort serial)
        {
            base.OnItemCreated(owner, serial);
            Firearm firearm = ItemBase as Firearm;
            firearm.Status = new FirearmStatus(1, firearm.Status.Flags, firearm.Status.Attachments);
            AutomaticAmmoManager manager = firearm.AmmoManagerModule as AutomaticAmmoManager;
            AccessTools.Field(typeof(AutomaticAmmoManager), "_defaultMaxAmmo").SetValue(manager, 1);
        }

        public void OnDamageByItem(DamageHandlerBase damageHandlerBase, ReferenceHub target)
        {
            
        }

        public void OnReloadStart()
        {
            
        }

        public void OnShot()
        {
            
        }

        public void OnReloadFinish(IAmmoManagerModule module, Firearm firearm)
        {
            Log.Debug(firearm.Status.ToString());
            if (firearm.Status.Flags.HasFlag(FirearmStatusFlags.Chambered) && !firearm.Status.Flags.HasFlag(FirearmStatusFlags.MagazineInserted))
            {
                Log.Debug("Triggered 1.5s delay");
                Timing.CallDelayed(1.5f, () =>
                {
                    firearm.Status = new FirearmStatus(1, firearm.Status.Flags, firearm.Status.Attachments);
                });
            }
            else if(firearm.Status.Flags.HasFlag(FirearmStatusFlags.Chambered) && firearm.Status.Flags.HasFlag(FirearmStatusFlags.MagazineInserted))
            {
                Log.Debug("Triggered 2s delay");
                if (firearm.Status.Flags.HasFlag(FirearmStatusFlags.Chambered))
                {
                    Timing.CallDelayed(2f, () =>
                    {
                        firearm.Status = new FirearmStatus(1, firearm.Status.Flags, firearm.Status.Attachments);
                    });
                }
            }
            else
            {
                Log.Debug("triggered 2.5s delay");
                Timing.CallDelayed(2.25f, () =>
                {
                    firearm.Status = new FirearmStatus(1, firearm.Status.Flags, firearm.Status.Attachments);
                });
            }
        }
    }
}
