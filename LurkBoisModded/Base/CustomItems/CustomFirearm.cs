using InventorySystem.Items.Firearms;
using InventorySystem.Items.Firearms.Modules;
using PlayerStatsSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
using LurkBoisModded.Patches.Firearm;
using LurkBoisModded.Extensions;

namespace LurkBoisModded.Base.CustomItems
{
    public abstract class CustomFirearm : CustomItem, ICustomFirearmItem
    {
        public Firearm Firearm
        {
            get
            {
                return (Firearm)ItemBase;
            }
        }

        public FirearmPickup FirearmPickup
        {
            get
            {
                return (FirearmPickup)ItemPickupBase;
            }
        }

        public void SetMaxAmmo(byte amount)
        {
            MaxAmmoPatcher.AddFirearm(TrackedSerial, amount);
        }

        public void SetCurrentAmmo(byte amount)
        {
            if(ItemState == ItemState.Inventory)
            {
                 Firearm.Status = new FirearmStatus(amount, Firearm.Status.Flags, Firearm.Status.Attachments);
            }
            if(ItemState == ItemState.Dropped)
            {
                FirearmPickup.NetworkStatus = new FirearmStatus(amount, Firearm.Status.Flags, Firearm.Status.Attachments);
            }
        }

        public void SetCurrentAmmo(byte amount, bool adjust)
        {
            int giveBackAmount = 0;
            if (ItemState == ItemState.Inventory)
            {
                giveBackAmount = Firearm.Status.Ammo - amount;
                Firearm.Status = new FirearmStatus(amount, Firearm.Status.Flags, Firearm.Status.Attachments);
            }
            if (ItemState == ItemState.Dropped)
            {
                giveBackAmount = FirearmPickup.NetworkStatus.Ammo - amount;
                FirearmPickup.NetworkStatus = new FirearmStatus(amount, Firearm.Status.Flags, Firearm.Status.Attachments);
            }
            if(adjust)
            {
                if(giveBackAmount < 0)
                {
                    CurrentOwner.RemoveItems(Firearm.AmmoType, Math.Abs(giveBackAmount));
                }
                else
                {
                    CurrentOwner.AddItem(Firearm.AmmoType, giveBackAmount);
                }
            }
        }

        public byte CurrentAmmo
        {
            get
            {
                if (ItemState == ItemState.Inventory)
                {
                    return Firearm.Status.Ammo;
                }
                if (ItemState == ItemState.Dropped)
                {
                    return FirearmPickup.NetworkStatus.Ammo;
                }
                return 0;
            }
            set
            {
                SetCurrentAmmo(value);
            }
        }

        public byte CurrentMaxAmmo
        {
            get
            {
                if (ItemState == ItemState.Inventory)
                {
                    return Firearm.AmmoManagerModule.MaxAmmo;
                }
                if (ItemState == ItemState.Dropped)
                {
                    return byte.MaxValue;
                }
                return 0;
            }
            set
            {
                SetMaxAmmo(value);
            }
        }

        public virtual bool OverrideMaxDetainAmount => false;

        public virtual bool OnPlayerShotByWeapon(FirearmDamageHandler damageHandlerBase, ReferenceHub target)
        {
            return true;
        }

        public virtual void OnReloadFinish(IAmmoManagerModule module, Firearm firearm)
        {
            return;
        }

        public virtual bool OnReloadStart()
        {
            return true;
        }

        public virtual bool OnShot()
        {
            return true;
        }

        public virtual bool OnPlayerStartDetaining(ReferenceHub other)
        {
            return true;
        }

        public virtual bool OnPlayerDetain(ReferenceHub other)
        {
            return true;
        }
    }
}
