﻿using PluginAPI.Events;
using PluginAPI.Enums;
using PluginAPI.Core.Attributes;
using PluginAPI.Core;
using MEC;
using UnityEngine;
using System.Collections.Generic;
using LurkBoisModded.Base;
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

namespace LurkBoisModded.EventHandlers.Item
{
    [EventHandler]
    public class CustomItemHandler
    {
        public static readonly Dictionary<CustomItemType, Type> CustomItemToType = new Dictionary<CustomItemType, Type>()
        {
            [CustomItemType.Test] = typeof(CustomTestItem),
            [CustomItemType.SniperE11] = typeof(SniperE11),
            [CustomItemType.Landmine] = typeof(Landmine),
        };

        public static Dictionary<ushort, CustomItem> SerialToItem = new Dictionary<ushort, CustomItem>();

        public static GameObject CreatedGameObject { get; private set; }

        public static void OnPlayerPickupItem(ItemPickupBase item, ReferenceHub hub)
        {
            if (SerialToItem.ContainsKey(item.Info.Serial))
            {
                SerialToItem[item.Info.Serial].OnItemPickedUp(hub);
            }
        }

        [PluginEvent(ServerEventType.PlayerDropItem)]
        public void OnPlayerDropItem(PlayerDropItemEvent ev)
        {

        }

        [PluginEvent(ServerEventType.PlayerChangeItem)]
        public void OnPlayerEquipItem(PlayerChangeItemEvent ev)
        {
            if (SerialToItem.ContainsKey(ev.NewItem))
            {
                SerialToItem[ev.NewItem].OnItemEquip();
            }
        }

        [PluginEvent(ServerEventType.RoundEnd)]
        public void OnRoundEnd(RoundEndEvent ev)
        {
            foreach(var item in SerialToItem.Values)
            {
                item.OnItemDestroyed();
            }
            SerialToItem.Clear();
        }

        [PluginEvent(ServerEventType.PlayerReloadWeapon)]
        public void OnFirearmReload(PlayerReloadWeaponEvent ev)
        {
            if (SerialToItem.ContainsKey(ev.Firearm.ItemSerial) && SerialToItem[ev.Firearm.ItemSerial] is ICustomFirearmItem firearm)
            {
                firearm.OnReloadStart();
            }
        }

        [PluginEvent(ServerEventType.PlayerShotWeapon)]
        public void OnFirearmShot(PlayerShotWeaponEvent ev)
        {
            if (SerialToItem.ContainsKey(ev.Firearm.ItemSerial) && SerialToItem[ev.Firearm.ItemSerial] is ICustomFirearmItem firearm)
            {
                firearm.OnShot();
            }
        }

        [PluginEvent(ServerEventType.PlayerDamage)]
        public void OnPlayerShotByWeapon(PlayerDamageEvent ev)
        {
            if(ev.DamageHandler is FirearmDamageHandler handler)
            {
                if(SerialToItem.TryGetValue(handler.Attacker.Hub.inventory.CurItem.SerialNumber, out CustomItem item) && item.IsHeldItem)
                 {
                    if(item is ICustomFirearmItem firearm)
                    {
                        firearm.OnDamageByItem(handler, ev.Target.ReferenceHub);
                    }
                }
            }
        }

        public static void OnReloadFinish(IAmmoManagerModule module, Firearm firearm)
        {
            if(SerialToItem.TryGetValue(firearm.ItemSerial, out CustomItem item) && item is ICustomFirearmItem firearmItem)
            {
                firearmItem.OnReloadFinish(module, firearm);
            }
            else
            {
                if (!SerialToItem.ContainsKey(firearm.ItemSerial))
                {
                    Log.Debug(firearm.ItemSerial.ToString());
                }
                Log.Debug("Item isnt in dict or it isnt a ICustomFirearmItem!");
            }
        }

        public static void OnItemDropped(ItemPickupBase itemPickupBase, ReferenceHub hub)
        {
            if (SerialToItem.ContainsKey(itemPickupBase.Info.Serial))
            {
                SerialToItem[itemPickupBase.Info.Serial].OnItemDropped(hub, itemPickupBase);
            }
        }

        public static void Init()
        {
            GameObject gameObject = new GameObject("CustomItems");
            GameObject.DontDestroyOnLoad(gameObject);
            CreatedGameObject = gameObject;
            AutoFirearmReloadFinishPath.OnReloadFinish += OnReloadFinish;
            SemiAutoFirearmFinishPatch.OnReloadFinish += OnReloadFinish;
            TubeMagReloadFinishPatch.OnReloadFinish += OnReloadFinish;
            GenericHandler.OnRoundRestart += OnRoundRestart;
            DropItemPatch.OnItemDropped += OnItemDropped;
            SearchItemCompletorPatch.OnItemPickedUp += OnPlayerPickupItem;
        }

        public static CustomItem AddItem(ReferenceHub target, CustomItemType type)
        {
            if(!CustomItemToType.TryGetValue(type, out Type itemType))
            {
                return null;
            }
            GameObject obj = new GameObject("Object");
            obj.transform.parent = CreatedGameObject.transform;
            CustomItem item = (CustomItem)obj.AddComponent(itemType);
            ItemBase givenItem = target.inventory.ServerAddItem(item.BaseItemType);
            if (SerialToItem.ContainsKey(givenItem.ItemSerial))
            {
                SerialToItem.Remove(givenItem.ItemSerial);
            }
            SerialToItem.Add(givenItem.ItemSerial, item);
            item.OnItemCreated(target, givenItem.ItemSerial);
            return item;
        }

        public static void OnRoundRestart()
        {
            foreach(CustomItem c in SerialToItem.Values)
            {
                c.OnRoundRestart();
            }
            SerialToItem.Clear();
        }
    }
}
