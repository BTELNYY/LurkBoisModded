using LurkBoisModded.Base.CustomItems;
using LurkBoisModded.EventHandlers.General;
using LurkBoisModded.Patches.Firearm.Reload;
using LurkBoisModded.Patches;
using RoundRestarting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using InventorySystem.Items;
using PluginAPI.Core;
using InventorySystem;

namespace LurkBoisModded.Managers
{
    public class CustomItemManager
    {
        public static Dictionary<CustomItemType, Type> CustomItemToType = new Dictionary<CustomItemType, Type>();

        public static Dictionary<ushort, CustomItem> SerialToItem = new Dictionary<ushort, CustomItem>();

        public static Dictionary<CustomItemType, CustomItem> TypeToItemDummy = new Dictionary<CustomItemType, CustomItem>();

        public static CustomItem GetCustomItemByTypeDummy(CustomItemType type)
        {
            if (TypeToItemDummy.ContainsKey(type))
            {
                return TypeToItemDummy[type];
            }
            return null;
        }

        public static GameObject CreatedGameObject { get; private set; }

        public static void Init()
        {
            GameObject gameObject = new GameObject("CustomItems");
            GameObject.DontDestroyOnLoad(gameObject);
            CreatedGameObject = gameObject;
            GameObject tempObj = new GameObject("CustomItemHolder");
            Type[] types = Assembly.GetExecutingAssembly().GetTypes().Where(x => x.IsSubclassOf(typeof(CustomItem))).ToArray();
            foreach (var item in types)
            {
                if (item.IsAbstract)
                {
                    continue;
                }
                try
                {
                    CustomItem createdItem = (CustomItem)tempObj.AddComponent(item);
                    if (createdItem == null)
                    {
                        Log.Error("Failed to create new instance of custom item with type " + item.FullName);
                        continue;
                    }
                    if (CustomItemToType.ContainsKey(createdItem.CustomItemType))
                    {
                        Log.Error("Tried to register item which does already is registered. Type: " + item.FullName);
                        continue;
                    }
                    CustomItemToType.Add(createdItem.CustomItemType, item);
                    Log.Info($"Added custom item, CustomItemType: {createdItem.CustomItemType}, Full Type: {item.FullName}");
                    if (TypeToItemDummy.ContainsKey(createdItem.CustomItemType))
                    {
                        Log.Warning("Tried to register custom item type which is already registred!");
                    }
                    else
                    {
                        TypeToItemDummy.Add(createdItem.CustomItemType, createdItem);
                    }
                }
                catch (Exception ex)
                {
                    Log.Error($"Failed to register custom item {item.FullName}. Error: {ex}");
                    continue;
                }
            }
        }

        public static CustomItem AddItem(ReferenceHub target, CustomItemType type)
        {
            if(type == CustomItemType.None)
            {
                return null;
            }
            if (!CustomItemManager.CustomItemToType.TryGetValue(type, out Type itemType))
            {
                return null;
            }
            GameObject obj = new GameObject("Object");
            obj.transform.parent = CustomItemManager.CreatedGameObject.transform;
            CustomItem item = (CustomItem)obj.AddComponent(itemType);
            if (item == null)
            {
                Log.Error("Custom item could not be created!");
                return null;
            }
            ItemBase givenItem = target.inventory.ServerAddItem(item.BaseItemType);
            if (givenItem == null)
            {
                GameObject.Destroy(item);
                GameObject.Destroy(obj);
                return null;
            }
            if (CustomItemManager.SerialToItem.ContainsKey(givenItem.ItemSerial))
            {
                CustomItemManager.SerialToItem.Remove(givenItem.ItemSerial);
            }
            CustomItemManager.SerialToItem.Add(givenItem.ItemSerial, item);
            item.OnItemCreated(target, givenItem.ItemSerial);
            return item;
        }
    }
}
