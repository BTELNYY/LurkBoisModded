using InventorySystem.Items.Pickups;
using LurkBoisModded.Base.CustomItems;
using LurkBoisModded.Extensions;
using PluginAPI.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace LurkBoisModded.CustomItems
{
    public class C4Charge : CustomItem
    {
        public override CustomItemType CustomItemType => CustomItemType.C4Charge;

        public override ItemType BaseItemType => ItemType.Coin;

        public ReferenceHub LastOwner = null;

        public override bool OnItemEquip()
        {
            base.OnItemEquip();
            CurrentOwner.SendHint(Config.CurrentConfig.C4Config.C4ChargeMessage);
            return true;
        }

        public override void OnItemDropped(ReferenceHub lastOwner, ItemPickupBase pickupBase)
        {
            base.OnItemDropped(lastOwner, pickupBase);
            LastOwner = lastOwner;
            if(C4Detonator.PlayersToExplosivesSet.ContainsKey(lastOwner.PlayerId))
            {
                C4Detonator.PlayersToExplosivesSet[lastOwner.PlayerId].Add(pickupBase);
                Log.Debug("Added to existing player!");
            }
            else
            {
                C4Detonator.PlayersToExplosivesSet.Add(lastOwner.PlayerId, new List<ItemPickupBase>() { pickupBase });
                Log.Debug("Added to new player!");
            }
        }

        public override void OnItemPickedUp(ReferenceHub newOwner)
        {
            base.OnItemPickedUp(newOwner);
            if (C4Detonator.PlayersToExplosivesSet.ContainsKey(LastOwner.PlayerId))
            {
                C4Detonator.PlayersToExplosivesSet[LastOwner.PlayerId].Remove(C4Detonator.PlayersToExplosivesSet[LastOwner.PlayerId].Where(x => x.NetworkInfo.Serial == TrackedSerial).First());
                Log.Debug("removed item from dict");
            }
        }
    }
}
