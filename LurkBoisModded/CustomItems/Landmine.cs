using LurkBoisModded.Base;
using AdminToys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PluginAPI.Core;
using UnityEngine;
using Mirror;
using LurkBoisModded.Scripts;
using InventorySystem.Items;
using InventorySystem.Items.Pickups;

namespace LurkBoisModded.CustomItems
{
    public class Landmine : CustomItem
    {
        public override CustomItemType CustomItemType => CustomItemType.Landmine;

        public override ItemType BaseItemType => ItemType.Radio;

        public override void OnItemEquip()
        {
            base.OnItemEquip();
            CurrentOwner.SendHint("You are holding a landmine, drop it to set it up!");
        }

        public override void OnItemDropped(ReferenceHub lastOwner, ItemPickupBase pickupBase)
        {
            base.OnItemDropped(lastOwner, ItemPickupBase);
            bool success = Utility.TryGetAdminToyByName("PrimitiveObject", out AdminToyBase @base);
            if (!success)
            {
                Log.Error("Can't find admin toy by name! Name: " + "PrimitiveObject");
                lastOwner.SendHint("Something went wrong when placing landmine.");
                return;
            }
            GameObject newObject = GameObject.Instantiate(@base.gameObject);
            NetworkServer.Spawn(newObject);
            LandmineScript landMine = pickupBase.gameObject.AddComponent<LandmineScript>();
            landMine.OwnerHub = lastOwner;
            landMine.PrimitiveTarget = newObject;
            landMine.Ready();
            lastOwner.SendHint("Landmine set.");
        }
    }
}
