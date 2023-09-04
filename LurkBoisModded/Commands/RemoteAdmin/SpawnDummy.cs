using CommandSystem;
using System;
using PluginAPI;
using PluginAPI.Core;
using LurkBoisModded.Managers;

namespace LurkBoisModded.Commands.RemoteAdmin
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class SpawnDummy : ICommand
    {
        public string Command => "spawndummy";

        public string[] Aliases => new string[] { "spawndummy", "dummy" };

        public string Description => "Spawn a fake player.";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!sender.CheckPermission(PlayerPermissions.PlayersManagement))
            {
                response = "No permission!";
                return false;
            }
            Player runner = Player.Get(sender);
            ReferenceHub hub = DummyManager.SpawnDummy(runner.Position);
            response = $"Success! Delete the bot by using deletedummy {hub.Network_playerId.Value}";
            return true;
        }
    }
}
