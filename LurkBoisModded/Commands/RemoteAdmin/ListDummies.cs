using CommandSystem;
using LurkBoisModded.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LurkBoisModded.Commands.RemoteAdmin
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class ListDummies : ICommand
    {
        public string Command => "listdummies";

        public string[] Aliases => new string[] { "listdummy", "dummies", "dumlist" };

        public string Description => "Lists all current dummies and their player IDs.";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if(!sender.CheckPermission(PlayerPermissions.PlayersManagement))
            {
                response = "No Permission!";
                return false;
            }
            string responseTemp = string.Empty;
            foreach(ReferenceHub dummy in DummyManager.Dummies)
            {
                responseTemp += $"{dummy.nicknameSync.DisplayName}: {dummy.PlayerId}";
            }
            response = responseTemp.TrimEnd('\n');
            return true;
        }
    }
}
