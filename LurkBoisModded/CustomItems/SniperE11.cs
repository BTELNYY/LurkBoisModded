using HarmonyLib;
using InventorySystem.Items;
using InventorySystem.Items.Firearms;
using InventorySystem.Items.Firearms.Modules;
using LurkBoisModded.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LurkBoisModded.CustomItems
{
    public class SniperE11 : CustomItem
    {
        public override CustomItemType CustomItemType => CustomItemType.SniperE11;

        public override ItemType BaseItemType => ItemType.GunE11SR;

        public override void OnItemCreated(ReferenceHub owner, ushort serial)
        {
            base.OnItemCreated(owner, serial);
            ItemBase item = owner.inventory.UserInventory.Items[serial];
            if(item is AutomaticFirearm firearm)
            {
                AutomaticAmmoManager manager = firearm.AmmoManagerModule as AutomaticAmmoManager;
                object[] array = { 1 };
                AccessTools.PropertySetter(typeof(AutomaticAmmoManager), "MaxAmmo").Invoke(manager, array);
            }
        }
    }
}
