using Footprinting;
using InventorySystem.Items.Radio;
using PluginAPI.Events;
using PluginAPI.Core;
using PluginAPI.Enums;
using PluginAPI.Core.Attributes;
using System.Collections.Generic;
using Utils;
using InventorySystem.Items.Pickups;
using PlayerRoles.Spectating;
using Mirror;
using HarmonyLib;
using System;
using LurkBoisModded.Base;
using UnityEngine;

namespace LurkBoisModded.Abilities
{
    public class RemoteExplosiveAbility : CustomAbilityBase, IRequiredItemAbility
    {
        public List<RadioPickup> TrackedRadios = new List<RadioPickup>();
        public bool Used = false;

        public override AbilityType AbilityType => AbilityType.RemoteExplosive;

        public ItemType RequiredItemType => ItemType.Radio;

        public override void OnTrigger()
        {
            base.OnTrigger();
            if(CurrentHub.inventory.CurItem.TypeId != RequiredItemType)
            {
                return;
            }
            if(TrackedRadios.Count == 0)
            {
                CurrentHub.SendHint("You must drop a radio to detonate your explosives!");
                return;
            }
            RadioItem radio = CurrentHub.inventory.CurInstance as RadioItem;
            if(radio.BatteryPercent == 0)
            {
                CurrentHub.SendHint("You must use a charged radio!");
                return;
            }
            if (Used && Plugin.GetConfig().AbilityConfig.RemoteExplosiveAbilityConfig.IsSingleUse)
            {
                CurrentHub.SendHint("This is a one time use ability!");
                return;
            }
            byte finalPenalty = (byte)Math.Max(0, radio.BatteryPercent - Plugin.GetConfig().AbilityConfig.RemoteExplosiveAbilityConfig.BatteryPercentPenaltyPerUse);
            radio.BatteryPercent = finalPenalty;
            AccessTools.Method(typeof(RadioItem), "SendStatusMessage").Invoke(radio, null);
            Footprint footprint = new Footprint(CurrentHub);
            int counter = 0;
            foreach(RadioPickup item in TrackedRadios)
            {
                if(item == null)
                {
                    TrackedRadios.Remove(item);
                    continue;
                }
                if(counter >= Plugin.GetConfig().AbilityConfig.RemoteExplosiveAbilityConfig.MaxDetonations)
                {
                    TrackedRadios.Remove(item);
                    continue;
                }
                counter++;
                Vector3 newPos = item.Position;
                newPos.y += 1f;
                NetworkServer.Destroy(item.gameObject);
                ExplosionUtils.ServerExplode(newPos, footprint);
            }
            CurrentHub.SendHint($"Detonated {counter} Explosive(s)");
            TrackedRadios.Clear();
            Used = true;
        }

        public override void OnFinishSetup()
        {
            ItemPickupBase.OnPickupAdded += OnItemDropped;
            ItemPickupBase.OnPickupDestroyed += OnItemPickup;
            base.OnFinishSetup();
        }

        public void OnItemPickup(PlayerSearchedPickupEvent ev)
        {
            if(!(ev.Item is RadioPickup))
            {
                return;
            }
            if (TrackedRadios.Contains(ev.Item as RadioPickup))
            {
                if(ev.Player.ReferenceHub.Network_playerId == CurrentHub.Network_playerId)
                {
                    ev.Player.SendHint("You disarmed your own explosive, drop the radio again to arm it.");
                    return;
                }
                ev.Player.SendHint("You disarmed a remote explosive. Careful!");
            }
        }

        public void OnItemDropped(ItemPickupBase pickup)
        {
            if(pickup.PreviousOwner.Hub.Network_playerId != CurrentHub.Network_playerId)
            {
                return;
            }
            if(pickup is RadioPickup)
            {
                TrackedRadios.Add(pickup as RadioPickup);
            }
        }

        public void OnItemPickup(ItemPickupBase pickup)
        {
            if (pickup.PreviousOwner.Hub.Network_playerId != CurrentHub.Network_playerId)
            {
                return;
            }
            if (pickup is RadioPickup)
            {
                TrackedRadios.Remove(pickup as RadioPickup);
            }
        }
    }
}
