using PluginAPI.Events;
using PluginAPI.Enums;
using PluginAPI.Core.Attributes;
using PluginAPI.Core;
using MEC;
using UnityEngine;
using System.Collections.Generic;
using LurkBoisModded.Base.CustomItems;
using System;
using InventorySystem.Items;
using InventorySystem;
using LurkBoisModded.CustomItems;
using LurkBoisModded.EventHandlers.General;
using LurkBoisModded.Patches;
using PlayerStatsSystem;
using InventorySystem.Items.Firearms.Modules;
using InventorySystem.Items.Firearms;
using InventorySystem.Items.Pickups;
using InventorySystem.Items.ThrowableProjectiles;
using InventorySystem.Items.Usables;
using LurkBoisModded.Managers;
using System.Reflection;
using System.Linq;
using PluginAPI.Core.Items;
using LurkBoisModded.Base;

namespace LurkBoisModded.EventHandlers.Item
{
    [EventHandler]
    public class CustomItemHandler
    {
        public static void Init()
        {
            GenericHandler.OnRoundRestart += OnRoundRestart;
            DropItemPatch.OnItemDropped += OnItemDropped;
            SearchItemCompletorPatch.OnItemPickedUp += OnPlayerPickupItem;
        }

        public static void OnPlayerPickupItem(ItemPickupBase item, ReferenceHub hub)
        {
            if (CustomItemManager.SerialToItem.ContainsKey(item.Info.Serial))
            {
                CustomItemManager.SerialToItem[item.Info.Serial].OnItemPickedUp(hub);
            }
        }

        [PluginEvent(ServerEventType.PlayerSearchedPickup)]
        public bool OnPlayerSearchedPickup(PlayerSearchedPickupEvent ev)
        {
            if (CustomItemManager.SerialToItem.ContainsKey(ev.Item.Info.Serial))
            {
                return CustomItemManager.SerialToItem[ev.Item.Info.Serial].OnItemSearched(ev.Player.ReferenceHub, ev.Item);
            }
            return true;
        }

        [PluginEvent(ServerEventType.PlayerSearchPickup)]
        public bool OnPlayerSearchPickup(PlayerSearchPickupEvent ev)
        {
            if (CustomItemManager.SerialToItem.ContainsKey(ev.Item.Info.Serial))
            {
                return CustomItemManager.SerialToItem[ev.Item.Info.Serial].OnItemSearch(ev.Player.ReferenceHub, ev.Item);
            }
            return true;
        }

        [PluginEvent(ServerEventType.PlayerDropItem)]
        public bool OnPlayerDropItem(PlayerDropItemEvent ev)
        {
            if (CustomItemManager.SerialToItem.ContainsKey(ev.Item.ItemSerial))
            {
                return CustomItemManager.SerialToItem[ev.Item.ItemSerial];
            }
            return true;
        }

        [PluginEvent(ServerEventType.PlayerChangeItem)]
        public bool OnPlayerEquipItem(PlayerChangeItemEvent ev)
        {
            if (CustomItemManager.SerialToItem.ContainsKey(ev.NewItem))
            {
                return CustomItemManager.SerialToItem[ev.NewItem].OnItemEquip();
            }
            return true;
        }

        [PluginEvent(ServerEventType.PlayerUseItem)]
        public bool OnPlayerUseItem(PlayerUseItemEvent ev)
        {
            if (CustomItemManager.SerialToItem.ContainsKey(ev.Item.ItemSerial))
            {
                return CustomItemManager.SerialToItem[ev.Item.ItemSerial].OnItemUse(ev.Player.ReferenceHub, (UsableItem)ev.Item);
            }
            return true;
        }

        [PluginEvent(ServerEventType.PlayerUsedItem)]
        public bool OnPlayerUsedItem(PlayerUsedItemEvent ev)
        {
            if (CustomItemManager.SerialToItem.ContainsKey(ev.Item.ItemSerial))
            {
                return CustomItemManager.SerialToItem[ev.Item.ItemSerial].OnItemUsed(ev.Player.ReferenceHub, (UsableItem)ev.Item);
            }
            return true;
        }

        [PluginEvent(ServerEventType.PlayerCancelUsingItem)]
        public bool OnPlayerCancelUseItem(PlayerCancelUsingItemEvent ev)
        {
            if (CustomItemManager.SerialToItem.ContainsKey(ev.Item.ItemSerial))
            {
                return CustomItemManager.SerialToItem[ev.Item.ItemSerial].OnItemUseCancelled(ev.Player.ReferenceHub, ev.Item);
            }
            return true;
        }

        public static void OnItemDropped(ItemPickupBase itemPickupBase, ReferenceHub hub)
        {
            if (CustomItemManager.SerialToItem.ContainsKey(itemPickupBase.Info.Serial))
            {
                CustomItemManager.SerialToItem[itemPickupBase.Info.Serial].OnItemDropped(hub, itemPickupBase);
            }
        }

        [PluginEvent(ServerEventType.RoundEnd)]
        public void OnRoundEnd(RoundEndEvent ev)
        {
            List<CustomItem> items = CustomItemManager.SerialToItem.Values.ToList();
            foreach(var item in items)
            {
                item.OnItemDestroyed();
            }
            CustomItemManager.SerialToItem.Clear();
        }

        [PluginEvent(ServerEventType.PlayerReloadWeapon)]
        public bool OnFirearmReload(PlayerReloadWeaponEvent ev)
        {
            if (CustomItemManager.SerialToItem.ContainsKey(ev.Firearm.ItemSerial) && CustomItemManager.SerialToItem[ev.Firearm.ItemSerial] is ICustomFirearmItem firearm)
            {
                return firearm.OnReloadStart();
            }
            return true;
        }

        [PluginEvent(ServerEventType.PlayerShotWeapon)]
        public bool OnFirearmShot(PlayerShotWeaponEvent ev)
        {
            if (CustomItemManager.SerialToItem.ContainsKey(ev.Firearm.ItemSerial) && CustomItemManager.SerialToItem[ev.Firearm.ItemSerial] is ICustomFirearmItem firearm)
            {
                return firearm.OnShot();
            }
            return true;
        }

        [PluginEvent(ServerEventType.PlayerDamage)]
        public bool OnPlayerShotByWeapon(PlayerDamageEvent ev)
        {
            if(ev.DamageHandler is FirearmDamageHandler handler)
            {
                if(CustomItemManager.SerialToItem.TryGetValue(handler.Attacker.Hub.inventory.CurItem.SerialNumber, out CustomItem item) && item.IsHeldItem)
                 {
                    if(item is ICustomFirearmItem firearm)
                    {
                        return firearm.OnPlayerShotByWeapon(handler, ev.Target.ReferenceHub);
                    }
                }
            }
            return true;
        }

        [PluginEvent(ServerEventType.PlayerHandcuff)]
        public bool OnPlayerDetained(PlayerHandcuffEvent ev)
        {
            ushort serial = ev.Player.CurrentItem.ItemSerial;
            if(CustomItemManager.SerialToItem.TryGetValue(serial, out CustomItem item))
            {
                if(item is ICustomFirearmItem firearm)
                {
                    return firearm.OnPlayerDetain(ev.Target.ReferenceHub);
                }
            }
            return true;
        }

        public static bool OnPlayerStartDetaining(ReferenceHub detainer, ReferenceHub target)
        {
            ushort currentItemSerial = detainer.inventory.CurItem.SerialNumber;
            if (CustomItemManager.SerialToItem.TryGetValue(currentItemSerial, out var item))
            {
                if (item is ICustomFirearmItem firearmItem)
                {
                    return firearmItem.OnPlayerStartDetaining(target);
                }
            }
            return true;
        }

        public static bool OnGrenadeFuseEnd(EffectGrenade grenade)
        {
            if(CustomItemManager.SerialToItem.TryGetValue(grenade.Info.Serial, out CustomItem item) && item is ICustomGrenadeItem grenadeItem)
            {
                return grenadeItem.OnFuseEnd(grenade);
            }
            return true;
        }

        [PluginEvent(ServerEventType.PlayerRadioToggle)]
        public bool OnPlayerToggleRadio(PlayerRadioToggleEvent ev)
        {
            if(CustomItemManager.SerialToItem.TryGetValue(ev.Radio.ItemSerial, out CustomItem item) && item is ICustomRadioItem radioItem)
            {
                return radioItem.OnToggled(ev.Player.ReferenceHub, ev.Radio, ev.NewState);
            }
            return true;
        }

        //[PluginEvent(ServerEventType.PlayerUsingRadio)]
        public bool OnPlayerUseingRadio(PlayerUsingRadioEvent ev)
        {
            if (CustomItemManager.SerialToItem.TryGetValue(ev.Radio.ItemSerial, out CustomItem item) && item is ICustomRadioItem radioItem)
            {
                return radioItem.OnUse(ev.Player.ReferenceHub, ev.Radio, ev.Drain);
            }
            return true;
        }

        [PluginEvent(ServerEventType.PlayerChangeRadioRange)]
        public bool OnPlayerChangeRangeSetting(PlayerChangeRadioRangeEvent ev)
        {
            if (CustomItemManager.SerialToItem.TryGetValue(ev.Radio.ItemSerial, out CustomItem item) && item is ICustomRadioItem radioItem)
            {
                return radioItem.OnRangeChanged(ev.Player.ReferenceHub, ev.Radio, ev.Range);
            }
            return true;
        }

        public static void OnRoundRestart()
        {
            foreach(CustomItem c in CustomItemManager.SerialToItem.Values)
            {
                c.OnRoundRestart();
            }
            CustomItemManager.SerialToItem.Clear();
        }
    }
}
