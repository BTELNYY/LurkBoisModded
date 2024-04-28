using HarmonyLib;
using Hints;
using LurkBoisModded.Extensions;
using InventorySystem.Disarming;
using PluginAPI.Core;
using LurkBoisModded.Managers;
using LurkBoisModded.Base.CustomItems;
using LurkBoisModded.EventHandlers.Item;

namespace LurkBoisModded.Patches
{
    [HarmonyPatch(typeof(DisarmedPlayers), nameof(DisarmedPlayers.CanStartDisarming))]
    public class DisarmPatch
    {
        public static bool Prefix(ReferenceHub disarmerHub, ReferenceHub targetHub, ref bool __result)
        {
            if (targetHub.inventory.IsDisarmed())
            {
                return true;
            }
            int currentDisarmedPlayers = disarmerHub.GetAllDisarmedPlayersByDisarmer().Count;
            ItemType currentItemType = disarmerHub.inventory.CurItem.TypeId;
            ushort currentItemSerial = disarmerHub.inventory.CurItem.SerialNumber;
            bool overriden = false;
            if(CustomItemManager.SerialToItem.TryGetValue(currentItemSerial, out var item)) 
            {
                if(item is ICustomFirearmItem firearmItem)
                {
                    overriden = firearmItem.OverrideMaxDetainAmount;
                }
            }
            bool result = CustomItemHandler.OnPlayerStartDetaining(disarmerHub, targetHub);
            if (!result)
            {
                return false;
            }
            if(!Plugin.GetConfig().MaxDisarmsPerWeapon.ContainsKey(currentItemType))
            {
                return true;
            }
            if (Plugin.GetConfig().MaxDisarmsPerWeapon[currentItemType] == -1)
            {
                return true;
            }
            if (Plugin.GetConfig().MaxDisarmsPerWeapon[currentItemType] <= currentDisarmedPlayers && !targetHub.inventory.IsDisarmed() && !overriden)
            {
                string message = Plugin.GetConfig().MaxDisarmsReached.Replace("{count}", Plugin.GetConfig().MaxDisarmsPerWeapon[currentItemType].ToString());
                disarmerHub.SendHint(message);
                __result = false;
                return false;
            }
            return true;
        }
    }
}
