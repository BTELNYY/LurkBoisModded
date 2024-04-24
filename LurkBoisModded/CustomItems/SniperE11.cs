using HarmonyLib;
using InventorySystem.Items;
using InventorySystem.Items.Firearms;
using InventorySystem.Items.Firearms.Modules;
using LurkBoisModded.Extensions;
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
    public class SniperE11 : CustomFirearm, ICustomFirearmItem
    {
        public override CustomItemType CustomItemType => CustomItemType.SniperE11;

        public override ItemType BaseItemType => ItemType.GunE11SR;

        public bool OnCooldown = false;
        public override void OnItemCreated(ReferenceHub owner, ushort serial)
        {
            base.OnItemCreated(owner, serial);
            AutomaticAmmoManager manager = (AutomaticAmmoManager)Firearm.AmmoManagerModule;
            if(manager == null)
            {
                return;
            }
            SetMaxAmmo(1);
        }

        public FirearmDamageHandler lastHandler = null;

        public override bool OnPlayerShotByWeapon(FirearmDamageHandler damageHandlerBase, ReferenceHub target)
        {
            float curDamage = damageHandlerBase.Damage;
            float newDamage = curDamage * Config.CurrentConfig.SniperE11Config.DamageMultiplier;
            damageHandlerBase.SetDamage(newDamage);
            damageHandlerBase.SetKnockbackMultiplier(Config.CurrentConfig.SniperE11Config.KnockbackMultiplier);
            damageHandlerBase.SetKnockbackAddative(Config.CurrentConfig.SniperE11Config.KnockbackAdditive);
            return true;
        }

        public override bool OnItemEquip()
        {
            if(Firearm.Status.Ammo > 1)
            {
                ForceSetAmmo(1);
            }
            return true;
        }

        public override bool OnReloadStart()
        {
            if(Firearm.Status.Ammo == 0)
            {
                if (OnCooldown)
                {
                    CurrentOwner.SendHint(Config.CurrentConfig.SniperE11Config.CooldownMessage);
                    return false;
                }
            }
            else
            {
                return false;
            }
            return true;
        }

        public override bool OnShot()
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
            SetCurrentAmmo(0);
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
            SetCurrentAmmo(1);
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

        public override void OnReloadFinish(IAmmoManagerModule module, Firearm firearm)
        {
            VerifyHasOneBullet();
        }
    }
}
