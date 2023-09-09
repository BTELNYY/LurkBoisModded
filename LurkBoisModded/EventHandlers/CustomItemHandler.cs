using PluginAPI.Events;
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

namespace LurkBoisModded.EventHandlers
{
    public class CustomItemHandler
    {
        public static readonly Dictionary<CustomItemType, Type> CustomItemToType = new Dictionary<CustomItemType, Type>()
        {
            [CustomItemType.Test] = typeof(CustomTestItem),
            [CustomItemType.SniperE11] = typeof(SniperE11),
        };

        public static Dictionary<ushort, CustomItem> SerialToItem = new Dictionary<ushort, CustomItem>();

        public static GameObject CreatedGameObject { get; private set; }

        [PluginEvent(ServerEventType.PlayerSearchedPickup)]
        public void OnPlayerPickupItem(PlayerSearchedPickupEvent ev)
        {
            if (SerialToItem.ContainsKey(ev.Item.Info.Serial))
            {
                SerialToItem[ev.Item.Info.Serial].OnItemPickedUp(ev.Player.ReferenceHub);
            }
        }

        [PluginEvent(ServerEventType.PlayerDropItem)]
        public void OnPlayerDropItem(PlayerDropItemEvent ev)
        {
            if (SerialToItem.ContainsKey(ev.Item.ItemSerial))
            {
                SerialToItem[ev.Item.ItemSerial].OnItemDropped(ev.Player.ReferenceHub);
            }
        }

        [PluginEvent(ServerEventType.PlayerChangeItem)]
        public void OnPlayerEquipItem(PlayerChangeItemEvent ev)
        {
            if (SerialToItem.ContainsKey(ev.NewItem))
            {
                SerialToItem[ev.NewItem].OnItemEquip();
            }
        }

        [PluginEvent(ServerEventType.RoundRestart)]
        public void OnRoundEnd(RoundRestartEvent ev)
        {
            foreach(var item in SerialToItem.Values)
            {
                item.OnItemDestroyed();
            }
            SerialToItem.Clear();
        }

        public static void Init()
        {
            GameObject gameObject = new GameObject("CustomItems");
            GameObject.DontDestroyOnLoad(gameObject);
            CreatedGameObject = gameObject;
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
            item.OnItemCreated(target, givenItem.ItemSerial);
            SerialToItem.Add(givenItem.ItemSerial, item);
            return item;
        }
    }
}
