using Footprinting;
using InventorySystem.Items.Pickups;
using InventorySystem.Items.Radio;
using LurkBoisModded.Base;
using LurkBoisModded.Base.CustomItems;
using LurkBoisModded.Extensions;
using Mirror;
using PluginAPI.Core.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Utils;

namespace LurkBoisModded.CustomItems
{
    public class C4Detonator : CustomItem, ICustomRadioItem
    {
        public override CustomItemType CustomItemType => CustomItemType.C4Detonator;

        public static Dictionary<int, List<ItemPickupBase>> PlayersToExplosivesSet = new Dictionary<int, List<ItemPickupBase>>();
        
        public override ItemType BaseItemType => ItemType.Radio;

        public RadioItem Radio 
        { 
            get 
            {
                if (ItemState == ItemState.Dropped) return null;
                return CurrentOwner.GetItemBySerial(TrackedSerial) as RadioItem;
            } 
        }

        public override void OnItemPickedUp(ReferenceHub newOwner)
        {
            base.OnItemPickedUp(newOwner);
            Radio.SetEnabled(false);
        }

        public override bool OnItemEquip()
        {
            base.OnItemEquip();
            Radio.SetEnabled(false);
            CurrentOwner.SendHint(Config.CurrentConfig.C4Config.DetonatorHeldMessage, 1f);
            return true;
        }

        public bool OnRangeChanged(ReferenceHub hub, RadioItem item, RadioMessages.RadioRangeLevel rangeLevel)
        {
            return true;
        }

        public bool OnToggled(ReferenceHub hub, RadioItem item, bool state)
        {
            List<ItemPickupBase> Items = new List<ItemPickupBase>();
            if (!PlayersToExplosivesSet.ContainsKey(hub.PlayerId))
            {
                hub.SendHint(Config.CurrentConfig.C4Config.DetonatorNoC4);
                return false;
            }
            if (PlayersToExplosivesSet[hub.PlayerId].IsEmpty())
            {
                hub.SendHint(Config.CurrentConfig.C4Config.DetonatorNoC4);
                return false;
            }
            Items = PlayersToExplosivesSet[hub.PlayerId];
            int counter = 0;
            foreach(ItemPickupBase pickup in Items)
            {
                if (item == null)
                {
                    continue;
                }
                counter++;
                Vector3 newPos = pickup.Position;
                newPos.y += 1f;
                NetworkServer.Destroy(pickup.gameObject);
                ExplosionUtils.ServerExplode(newPos, new Footprint(CurrentOwner), ExplosionType.Grenade);
            }
            PlayersToExplosivesSet[hub.PlayerId].Clear();
            hub.SendHint(Config.CurrentConfig.C4Config.DetonateSuccess.Replace("{counter}", counter.ToString()));
            return true;
        }

        public bool OnUse(ReferenceHub hub, RadioItem item, float drain)
        {
            hub.SendHint(Config.CurrentConfig.C4Config.DetonatorTryUseMessage);
            return false;
        }
    }
}
