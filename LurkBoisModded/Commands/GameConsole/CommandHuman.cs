using CommandSystem;
using LurkBoisModded.Managers;
using PluginAPI.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LurkBoisModded.Commands.GameConsole
{
    [CommandHandler(typeof(ClientCommandHandler))]
    public class CommandHuman : ICommand
    {
        public string Command => "human";

        public string[] Aliases => new string[] { };

        public string Description => "Request to swap to become a human.";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            Player player = Player.Get(sender);
            if (player == null)
            {
                response = "You can only run this command as a player!";
                return false;
            }
            if (!player.IsSCP)
            {
                response = "You must be an SCP to run this command!";
                return false;
            }
            if (!RoleSwapManager.CanSwap)
            {
                response = "Cannot swap now, times up!";
                return false;
            }
            RoleSwapManager.SendSCPSwapRequest(player.ReferenceHub);
            response = "Success!";
            return true;
        }
    }
}
