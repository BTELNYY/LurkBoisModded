using LurkBoisModded.Base.CustomItems;
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
using LurkBoisModded.Extensions;
using System.Security.Policy;
using InventorySystem.Disarming;

namespace LurkBoisModded.CustomItems
{
    public class Landmine : CustomItem
    {
        public override CustomItemType CustomItemType => CustomItemType.Landmine;

        public override ItemType BaseItemType => ItemType.Radio;

        public LandmineScript landmineScript = null;

        public override bool OnItemEquip()
        {
            base.OnItemEquip();
            CurrentOwner.SendHint("You are holding a landmine, drop it to set it up!");
            return true;
        }

        public override void OnItemDropped(ReferenceHub lastOwner, ItemPickupBase pickupBase)
        {
            base.OnItemDropped(lastOwner, ItemPickupBase);
            if (lastOwner.inventory.IsDisarmed())
            {
                return;
            }
            AdminToyBase @base = Utility.GetAdminToy(AdminToyType.LightSource);
            if (@base == null)
            {
                lastOwner.SendHint("Something went wrong when placing landmine.");
                return;
            }
            GameObject newObject = GameObject.Instantiate(@base.gameObject);
            NetworkServer.Spawn(newObject);
            LandmineScript landMine = pickupBase.gameObject.AddComponent<LandmineScript>();
            landMine.OwnerHub = lastOwner;
            landMine.PrimitiveTarget = newObject;
            landMine.Ready();
            landmineScript = landMine;
            lastOwner.SendHint("Landmine set.");
        }

        public override void OnItemPickedUp(ReferenceHub newOwner)
        {
            base.OnItemPickedUp(newOwner);
            if(landmineScript != null)
            {
                landmineScript.DestroySelf();
                landmineScript = null;
            }
        }
    }
}
