using CommandSystem;
using System;
using PluginAPI.Events;
using PluginAPI.Enums;
using PluginAPI.Core.Attributes;
using PluginAPI.Core;
using PlayerRoles;
using ICommand = CommandSystem.ICommand;
using PlayerRoles.PlayableScps.Scp079;
using HarmonyLib;
using MapGeneration;
using MEC;
using System.Collections.Generic;
using System.Linq;
using LurkBoisModded.Effects;
using LurkBoisModded.Abilities;
using LurkBoisModded.Extensions;

namespace LurkBoisModded.Commands.GameConsole
{
    [CommandHandler(typeof(ClientCommandHandler))]
    public class CommandGas : ICommand
    {
        public string Command => "gas";

        public string[] Aliases => new string[] { "gas", "nt", "toxin" };

        public string Description => "Deploys poison gas in the current room. Can't be run in the same room as a lockdown";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            Player player = Player.Get(sender);
            Scp079GasAbility ability = player.ReferenceHub.GetAbility<Scp079GasAbility>();
            if(ability == null)
            {
                response = "Can't find that ability!";
                return false;
            }
            ability.OnTrigger();
            response = "Watch you screen!";
            return true;
        }
    }
}
