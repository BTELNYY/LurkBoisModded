using CommandSystem;
using LurkBoisModded.Managers;
using PlayerRoles;
using PluginAPI.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LurkBoisModded.Commands.GameConsole
{
    [CommandHandler(typeof(ClientCommandHandler))]
    public class CommandScpClaim : ICommand
    {
        public string Command => "scp";

        public string[] Aliases => new string[] { "scpclaim" };

        public string Description => "Claim an SCP's role who wants to swap. Usage: .scp <scpnumber>";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            Player player = Player.Get(sender);
            if (player == null)
            {
                response = "You must be a player to use this command!";
                return false;
            }
            ReferenceHub playerHub = player.ReferenceHub;
            if(!player.IsHuman)
            {
                response = "You must be a human to use this command!";
                return false;
            }
            if(arguments.Count < 1)
            {
                response = "Incorrect amount of arguments!";
                return false;
            }
            string scpNumber = arguments.At(0);
            string scpRole = "Scp" + scpNumber;
            RoleTypeId role = RoleTypeId.None;
            try
            {
                role = (RoleTypeId)Enum.Parse(typeof(RoleTypeId), scpRole);
            }
            catch
            {
                
            }
            if(role == RoleTypeId.None)
            {
                response = "Can't find SCP by Number: " + scpNumber;
                return false;
            }
            ReferenceHub[] scps = RoleSwapManager.SCPSwapRequests.Where(x => x.roleManager.CurrentRole.RoleTypeId == role).ToArray();
            if(scps.Length == 0)
            {
                response = "No SCP with that number wanted to swap.";
                return false;
            }
            ReferenceHub scp = scps[0];
            RoleSwapManager.SwapScpAndHuman(playerHub, scp);
            response = "Success";
            return true;
        }
    }
}
