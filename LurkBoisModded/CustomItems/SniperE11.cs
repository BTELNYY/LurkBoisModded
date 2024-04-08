using HarmonyLib;
using InventorySystem.Items;
using InventorySystem.Items.Firearms;
using InventorySystem.Items.Firearms.Modules;
using LurkBoisModded.Base;
using MEC;
using PlayerRoles;
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

        public bool OnCooldown = false;

        public Firearm Firearm 
        { 
            get
            {
                return (Firearm)ItemBase;
            } 
        }

        public override void OnItemCreated(ReferenceHub owner, ushort serial)
        {
            base.OnItemCreated(owner, serial);
            Firearm firearm = ItemBase as Firearm;
            firearm.Status = new FirearmStatus(1, firearm.Status.Flags, firearm.Status.Attachments);
            AutomaticAmmoManager manager = (AutomaticAmmoManager)firearm.AmmoManagerModule;
            if(manager == null)
            {
                return;
            }
            AccessTools.Field(typeof(AutomaticAmmoManager), "_defaultMaxAmmo").SetValue(manager, 1);
        }

        public FirearmDamageHandler lastHandler = null;

        public bool OnPlayerShotByWeapon(FirearmDamageHandler damageHandlerBase, ReferenceHub target)
        {
            if(damageHandlerBase == lastHandler)
            {
                return false;
            }
            ushort currItemSerial = damageHandlerBase.Attacker.Hub.inventory.NetworkCurItem.SerialNumber;
            Firearm firearm = (Firearm)damageHandlerBase.Attacker.Hub.inventory.GetItemBySerial(currItemSerial);
            if(firearm == null)
            {
                return true;
            }
            FirearmDamageHandler handler = new FirearmDamageHandler(firearm, damageHandlerBase.Damage * Config.CurrentConfig.SniperE11Config.DamageMultiplier, target.IsHuman());
            lastHandler = handler;
            target.playerStats.DealDamage(handler);
            return false;
        }

        public bool OnReloadStart()
        {
            if (OnCooldown)
            {
                return false;
            }
            return true;
        }

        void Update()
        {
            if (OnCooldown)
            {
                VerifyHasOneBullet();
            }
        }

        public bool OnShot()
        {
            if(OnCooldown)
            {
                CurrentOwner.SendHint(Config.CurrentConfig.SniperE11Config.CooldownMessage);
                return false;
            }
            OnCooldown = true;
            Timing.CallDelayed(Config.CurrentConfig.SniperE11Config.CooldownTime, () => 
            {
                OnCooldown = false;
            });
            return true; 
        }

        public void VerifyHasOneBullet()
        {
            if(Firearm == null)
            {
                return;
            }
            byte currentAmmo = Firearm.Status.Ammo;
            CurrentOwner.AddItem(ItemType.Ammo556x45, currentAmmo);
            Firearm.Status = new FirearmStatus(0, FirearmStatusFlags.None, Firearm.Status.Attachments);
        }

        public void OnReloadFinish(IAmmoManagerModule module, Firearm firearm)
        {

        }
    }
}
