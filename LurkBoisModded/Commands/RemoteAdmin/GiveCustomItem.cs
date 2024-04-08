using CommandSystem;
using LurkBoisModded.Base.CustomItems;
using RemoteAdmin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Utils;
using YamlDotNet.Core;

namespace LurkBoisModded.Commands.RemoteAdmin
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class GiveCustomItem : ICommand
    {
        public string Command => "givecustom";

        public string[] Aliases => new string[] { "gcustom" };

        public string Description => "Gives users a custom item from the list of defined custom items.";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!sender.CheckPermission(PlayerPermissions.GivingItems))
            {
                response = "No Permission!";
                return false;
            }
            if (arguments.Count < 1)
            {
                response = "Incorrect Syntax. Syntax: givecustom <playerIds> <itemId>";
                return false;
            }
            string[] array = { };
            PlayerCommandSender playerCommandSender = sender as PlayerCommandSender;
            List<ReferenceHub> list;
            if (playerCommandSender != null && (arguments.Count == 0 || (arguments.Count == 1 && !arguments.At(0).Contains(".") && !arguments.At(0).Contains("@"))))
            {
                list = new List<ReferenceHub>();
                list.Add(playerCommandSender.ReferenceHub);
                if (arguments.Count > 1)
                {
                    array[0] = arguments.At(1);
                }
                else
                {
                    array = null;
                }
            }
            else
            {
                list = RAUtils.ProcessPlayerIdOrNamesList(arguments, 0, out array, false);
            }
            string itemId = array.Last();
            if (!Enum.TryParse<CustomItemType>(itemId, out CustomItemType result))
            {
                response = $"Can't find custom item ID with Enum name '{itemId}'";
                return false;
            }
            int affected = list.Count();
            foreach(ReferenceHub hub in list)
            {
                CustomItem item = hub.AddCustomItem(result);
                if (item == null)
                {
                    affected -= 1;
                }
            }
            response = $"Done! That command affected {affected} out of {list.Count()} specified player(s)!";
            return true;
        }
    }
}
