using CommandSystem;
using PluginAPI.Core;
using RemoteAdmin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utils;
using LurkBoisModded.Base;
using LurkBoisModded.Managers;

namespace LurkBoisModded.Commands.RemoteAdmin
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class SetSubclass : ICommand
    {
        public string Command => "setsubclass";

        public string[] Aliases => new string[] { "setsub" , "subclass" };

        public string Description => "Set a player to a subclass";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!sender.CheckPermission(PlayerPermissions.ForceclassWithoutRestrictions, out response))
            {
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
            string subclass = array.Last();
            int num = 0;
            sender.Respond("Setting subclass to: " + subclass, true);
            if (list != null)
            {
                foreach (ReferenceHub referenceHub in list)
                {
                    Player p = Player.Get(referenceHub);
                    Subclass baseSubClass = SubclassManager.GetSubclass(subclass);
                    if(baseSubClass == null)
                    {
                        response = "Failed to find subclass!";
                        return false;
                    }
                    referenceHub.SetSubclass(baseSubClass);
                    num++;
                }
            }
            response = string.Format("Done! The request affected {0} player{1}", num, (num == 1) ? "!" : "s!");
            return true;
        }
    }
}
