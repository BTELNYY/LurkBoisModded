using InventorySystem.Items.Firearms;
using LurkBoisModded.Base.CustomItems;
using LurkBoisModded.Managers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LurkBoisModded.Base
{
    public class ItemDefinition
    {
        public ItemDefinition() { }

        public ItemDefinition(CustomItemType type) 
        {
            CustomItem item = CustomItemManager.GetCustomItemByTypeDummy(type) ?? throw new ArgumentException("Failed to get Vanilla Item type from custom item type " + type.ToString() + " Becuase it is not registered.");
            VanillaItemType = item.BaseItemType;
            CustomItemType = type;
            if (IsFirearm)
            {
                FirearmDefinition = new FirearmDefinition(0, 0, true);
            }
        }

        public ItemDefinition(ItemType type)
        {
            VanillaItemType = type;
            if (IsFirearm)
            {
                FirearmDefinition = new FirearmDefinition(0, 0, FirearmStatusFlags.None, true);
            }
        }

        public ItemDefinition(ItemType type, FirearmDefinition definition)
        {
            VanillaItemType = type;
            FirearmDefinition = definition;
        }

        public ItemDefinition(CustomItemType type, FirearmDefinition definition)
        {
            CustomItem item = CustomItemManager.GetCustomItemByTypeDummy(type) ?? throw new ArgumentException("Failed to get Vanilla Item type from custom item type " + type.ToString() + " Becuase it is not registered.");
            VanillaItemType = item.BaseItemType;
            CustomItemType = type;
            FirearmDefinition = definition;
        }

        public ItemType VanillaItemType { get; set; } = ItemType.None;

        public CustomItemType CustomItemType { get; set; } = CustomItemType.None;

        [YamlDotNet.Serialization.YamlIgnore]
        public bool IsCustomItem
        {
            get
            {
                return CustomItemType != CustomItemType.None;
            }
        }

        [YamlDotNet.Serialization.YamlIgnore]
        public bool IsFirearm
        {
            get
            {
                return VanillaItemType.ToString().Contains("Gun");
            }
        }

        [YamlDotNet.Serialization.YamlIgnore]
        public bool IsAmmo
        {
            get
            {
                return VanillaItemType.ToString().Contains("Ammo");
            }
        }

        [Description("Only applies if the ItemType is a firearm")]
        public FirearmDefinition FirearmDefinition { get; set; } = null;

        [Description("Only Applies if the ItemType is any Ammo box type")]
        public ushort AmmoAmount { get; set; } = 0;

        [Description("Only applies if the ItemType is the radio or the micro")]
        public byte Charge { get; set; } = 100;

    }
}
