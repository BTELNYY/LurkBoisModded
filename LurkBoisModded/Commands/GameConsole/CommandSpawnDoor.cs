using CommandSystem;
using System;
using PluginAPI.Core;
using UnityEngine;
using Interactables.Interobjects.DoorUtils;

namespace LurkBoisModded.Commands.GameConsole
{
    public class CommandSpawnDoor : ICommand
    {
        public string Command => "spawndoor";

        public string[] Aliases => new string[] { "spdoor", "spawndoor" };

        public string Description => "Debug Command";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            Player p = Player.Get(sender);
            Utility.CreateDoor(p.Position, p.Rotation, new Vector3(1f, 1f, 1f), DoorType.EZ, new KeycardPermissions[] {KeycardPermissions.Checkpoints, KeycardPermissions.ExitGates});
            response = "Done!";
            return true;
        }
    }
}
