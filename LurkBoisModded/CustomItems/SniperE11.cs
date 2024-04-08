using HarmonyLib;
using InventorySystem.Items;
using InventorySystem.Items.Firearms;
using InventorySystem.Items.Firearms.Modules;
using LurkBoisModded.Base.CustomItems;
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
            AutomaticAmmoManager manager = (AutomaticAmmoManager)Firearm.AmmoManagerModule;
            if(manager == null)
            {
                return;
            }
            AccessTools.Field(typeof(AutomaticAmmoManager), "_defaultMaxAmmo").SetValue(manager, (byte)1);
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

        public override bool OnItemEquip()
        {
            if(Firearm.Status.Ammo > 1)
            {
                ForceSetAmmo(1);
            }
            return true;
        }

        public bool OnReloadStart()
        {
            if(Firearm.Status.Ammo == 0)
            {
                if (OnCooldown)
                {
                    CurrentOwner.SendHint(Config.CurrentConfig.SniperE11Config.CooldownMessage);
                    return false;
                }
                return true;
            }
            if(Firearm.Status.Ammo > 1)
            {
                VerifyHasOneBullet();
                return false;
            }
            return true;
        }

        void Update()
        {
            if (OnCooldown)
            {
                CurrentOwner.SendHint(Config.CurrentConfig.SniperE11Config.CooldownMessage);
                ForceSetAmmo(0);
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
            ForceSetAmmo(0);
            return true; 
        }

        public void VerifyHasOneBullet()
        {
            if(Firearm == null)
            {
                return;
            }
            byte currentAmmo = Firearm.Status.Ammo;
            if(currentAmmo <= 1)
            {
                return;
            }
            ForceSetAmmo(1);
        }

        public void ForceSetAmmo(byte amount)
        {
            if (Firearm == null)
            {
                return;
            }
            byte currentAmmo = Firearm.Status.Ammo;
            byte properAmount = Math.Min(currentAmmo, amount);
            CurrentOwner.AddItem(ItemType.Ammo556x45, (byte)(properAmount - amount));
            Firearm.Status = new FirearmStatus(properAmount, FirearmStatusFlags.None, Firearm.Status.Attachments);
        }

        public void OnReloadFinish(IAmmoManagerModule module, Firearm firearm)
        {
            VerifyHasOneBullet();
        }
    }
}
