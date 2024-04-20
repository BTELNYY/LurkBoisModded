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
using LurkBoisModded.Patches.Firearm.Reload;
using InventorySystem.Items.Pickups;
using InventorySystem.Items.ThrowableProjectiles;
using InventorySystem.Items.Usables;
using LurkBoisModded.Managers;
using System.Reflection;
using System.Linq;
using PluginAPI.Core.Items;

namespace LurkBoisModded.EventHandlers.Item
{
    [EventHandler]
    public class CustomItemHandler
    {
        public static void Init()
        {
            AutoFirearmReloadFinishPath.OnReloadFinish += OnReloadFinish;
            SemiAutoFirearmFinishPatch.OnReloadFinish += OnReloadFinish;
            TubeMagReloadFinishPatch.OnReloadFinish += OnReloadFinish;
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

        public static bool OnGrenadeFuseEnd(EffectGrenade grenade)
        {
            if(CustomItemManager.SerialToItem.TryGetValue(grenade.Info.Serial, out CustomItem item) && item is ICustomGrenadeItem grenadeItem)
            {
                return grenadeItem.OnFuseEnd(grenade);
            }
            return true;
        }

        public static void OnReloadFinish(IAmmoManagerModule module, Firearm firearm)
        {
            if(CustomItemManager.SerialToItem.TryGetValue(firearm.ItemSerial, out CustomItem item) && item is ICustomFirearmItem firearmItem)
            {
                firearmItem.OnReloadFinish(module, firearm);
            }
            else
            {
                if (!CustomItemManager.SerialToItem.ContainsKey(firearm.ItemSerial))
                {
                    Log.Debug(firearm.ItemSerial.ToString());
                }
                Log.Debug("Item isnt in dict or it isnt a ICustomFirearmItem!");
            }
        }

        public static void OnItemDropped(ItemPickupBase itemPickupBase, ReferenceHub hub)
        {
            if (CustomItemManager.SerialToItem.ContainsKey(itemPickupBase.Info.Serial))
            {
                CustomItemManager.SerialToItem[itemPickupBase.Info.Serial].OnItemDropped(hub, itemPickupBase);
            }
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
