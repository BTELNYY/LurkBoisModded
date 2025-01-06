using CommandSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PluginAPI;
using PluginAPI.Core;
using InventorySystem.Items.Firearms;
using LurkBoisModded.Extensions;
using PlayerStatsSystem;

namespace LurkBoisModded.Commands.GameConsole
{
    [CommandHandler(typeof(ClientCommandHandler))]
    public class Suicide : ICommand
    {
        public string Command => "suicide";

        public string[] Aliases => new string[] { "suicide", "kys", "kill", "kms" };

        public string Description => "Suicide";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            Player target = Player.Get(sender);
            Firearm firearm = target.ReferenceHub.GetFirearm(target.ReferenceHub.inventory.CurItem.TypeId);
            if (!target.IsAlive)
            {
                response = "You are already dead!";
                return false;
            }
            if(firearm == null)
            {
                response = "You must hold a firearm to use this command!";
                return false;
            }
            response = "Done.";
            target.Kill(Plugin.GetConfig().SuicideDeathReason);
            return true;
        }
    }
}
