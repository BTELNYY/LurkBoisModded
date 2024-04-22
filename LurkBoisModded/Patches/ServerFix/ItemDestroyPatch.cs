using HarmonyLib;
using InventorySystem.Items.Pickups;
using LurkBoisModded.Base;
using LurkBoisModded.Managers;
using LurkBoisModded.Base.CustomItems;
using LurkBoisModded.EventHandlers.Item;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mirror;
using UnityEngine;
using PluginAPI.Core;

namespace LurkBoisModded.Patches.ServerFix
{
    [HarmonyPatch(typeof(ItemPickupBase), "DestroySelf")]
    public class ItemDestroyPatch
    {
        public static bool Prefix(ItemPickupBase __instance)
        {
            if (!NetworkServer.active)
            {
                Debug.LogWarning("[Server] function 'System.Void InventorySystem.Items.Pickups.ItemPickupBase::DestroySelf()' called when server was not active");
            }
            else
            {
                if(__instance.gameObject == null)
                {
                    Log.Warning("Tried to destroy a itempickupbase with a null gameobject, this is not allowed.");
                    return false;
                }
                NetworkServer.Destroy(__instance.gameObject);
            }
            return false;
        }
    }
}
