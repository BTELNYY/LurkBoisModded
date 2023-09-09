using CommandSystem;
using CommandSystem.Commands.RemoteAdmin.Cleanup;
using HarmonyLib;
using InventorySystem.Items.Pickups;
using LurkBoisModded.EventHandlers;
using Mirror;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LurkBoisModded.Patches
{
    //[HarmonyPatch(typeof(ItemsCommand), nameof(ItemsCommand.Execute))]
    public class OnItemRemovedViaCommand
    {
        public static bool Prefix(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!sender.CheckPermission(PlayerPermissions.FacilityManagement, out response))
            {
                return true;
            }
            ItemPickupBase[] array = UnityEngine.Object.FindObjectsOfType<ItemPickupBase>();
            int num = array.Length;
            int num2;
            if (arguments.Count > 0 && int.TryParse(arguments.At(0), out num2) && num2 < array.Length)
            {
                num = num2;
            }
            for (int i = 0; i < num; i++)
            {
                if (CustomItemHandler.SerialToItem.ContainsKey(array[i].Info.Serial))
                {
                    CustomItemHandler.SerialToItem[array[i].Info.Serial].OnItemDestroyed();
                }
                NetworkServer.Destroy(array[i].gameObject);
            }
            ServerLogs.AddLog(ServerLogs.Modules.Administrative, string.Format("{0} has force-cleaned up {1} items.", sender.LogName, num), ServerLogs.ServerLogType.RemoteAdminActivity_GameChanging, false);
            response = string.Format("{0} items have been deleted.", num);
            return false;
        }
    }
}
