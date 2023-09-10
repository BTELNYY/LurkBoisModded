using InventorySystem.Items.Firearms;
using LurkBoisModded.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LurkBoisModded.CustomItems
{
    public class CustomTestItem : CustomItem
    {
        public override CustomItemType CustomItemType => CustomItemType.Test;

        public override ItemType BaseItemType => ItemType.Medkit;

        public override void OnItemDropped(ReferenceHub lastOwner)
        {
            base.OnItemDropped(lastOwner);
            lastOwner.SendHint("Custom Item Dropped!");
        }

        public override void OnItemEquip()
        {
            base.OnItemEquip();
            CurrentOwner.SendHint("Custom Item Equip!");
        }

        public override void OnItemPickedUp(ReferenceHub newOwner)
        {
            base.OnItemPickedUp(newOwner);
            CurrentOwner.SendHint("Custom Item Picked up!");
        }
    }
}
