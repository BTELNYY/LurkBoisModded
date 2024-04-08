using CommandSystem;
using LurkBoisModded.Base.CustomItems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LurkBoisModded.Commands.RemoteAdmin
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class ListCustomItems : ICommand
    {
        public string Command => "customitems";

        public string[] Aliases => new string[] { "listcustomitems", "cilist", "customitemlist" };

        public string Description => "Lists all custom items of the plugin.";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!sender.CheckPermission(PlayerPermissions.GivingItems))
            {
                response = "No Permission!";
                return false;
            }
            response = string.Join("\n", Enum.GetNames(typeof(CustomItemType)));
            return true;
        }
    }
}
