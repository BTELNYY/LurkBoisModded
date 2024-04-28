using InventorySystem.Items.Firearms;
using InventorySystem.Items.Firearms.Modules;
using LurkBoisModded.Base.CustomItems;
using LurkBoisModded.Extensions;
using PlayerStatsSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace LurkBoisModded.CustomItems
{
    public class NotScaryCOM15 : CustomFirearm
    {
        public override CustomItemType CustomItemType => CustomItemType.NotScaryCOM15;

        public override ItemType BaseItemType => ItemType.GunCOM15;

        public override bool OnPlayerStartDetaining(ReferenceHub other)
        {
            CurrentOwner.SendHint("You cannot detain players with this item!");
            return false;
        }
    }
}
