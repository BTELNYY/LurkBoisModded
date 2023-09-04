using CommandSystem;
using LurkBoisModded.Managers;
using Mirror;
using RemoteAdmin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Utils;

namespace LurkBoisModded.Commands.RemoteAdmin
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class DeleteDummy : ICommand
    {
        public string Command => "deletedummy";

        public string[] Aliases => new string[] { "deletedummy" };

        public string Description => "Delete a dummy!";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!sender.CheckPermission(PlayerPermissions.PlayersManagement))
            {
                response = "No permission!";
                return false;
            }
            List<ReferenceHub> list;
            string[] array = { };
            if ((arguments.Count == 0 || (arguments.Count == 1 && !arguments.At(0).Contains("@"))))
            {
                list = new List<ReferenceHub>();
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
            List<ReferenceHub> dummies = list.Where(x => DummyManager.Dummies.Select(y => y.Network_playerId).ToList().Contains(x.Network_playerId)).ToList();
            foreach(ReferenceHub dummy in dummies)
            {
                NetworkServer.Destroy(dummy.gameObject);
            }
            response = $"Destroyed {dummies.Count} dummies";
            return true;
        }
    }
}
