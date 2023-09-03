using CommandSystem;
using PluginAPI.Core;
using RemoteAdmin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Utils;

//Copy paste from Exiled Admin tools, ported to NWAPI plus extensions.

namespace LurkBoisModded.Commands.RemoteAdmin
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class SetScaleMultiplier : ICommand
    {
        public string Command => "setscale";

        public string[] Aliases => new string[] { };

        public string Description => "Set the scale of a player.";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!((CommandSender)sender).CheckPermission(PlayerPermissions.ForceclassWithoutRestrictions))
            {
                response = "You do not have permission to use this command";
                return false;
            }

            if (arguments.Count < 1)
            {
                response = "Usage:\nscale <id/name> or * <value>" +
                    "\nscale reset";
                return false;
            }

            switch (arguments.At(0))
            {
                case "reset":
                    if (arguments.Count != 1)
                    {
                        response = "Usage: scale reset";
                        return false;
                    }
                    foreach (ReferenceHub hub in ReferenceHub.AllHubs)
                    {
                        Player.Get(hub).SetScale(1f);
                    }
                    response = $"Scale reset!";
                    return true;
                case "*":
                case "all":
                    if (arguments.Count != 2)
                    {
                        response = "Usage: scale * <value>";
                        return false;
                    }

                    if (!float.TryParse(arguments.At(1), out float value))
                    {
                        response = $"Error parsing scale: {arguments.At(1)}";
                        return false;
                    }

                    foreach (ReferenceHub hub in ReferenceHub.AllHubs)
                    {
                        Player player = Player.Get(hub);
                        player.SetScale(value);
                    }
                    response = $"Set player(s) scale to: {value}";
                    return true;
                default:
                    if (arguments.Count != 2)
                    {
                        response = "Usage: scale (player id / name) (value)";
                        return false;
                    }


                    try
                    {
                        List<ReferenceHub> players = ReferenceHub.AllHubs.Where(x => x.PlayerId == int.Parse(arguments.At(0))).ToList();
                        if (!float.TryParse(arguments.At(1), out float val))
                        {
                            response = $"Invalid value for scale: {arguments.At(1)}";
                            return false;
                        }

                        foreach(ReferenceHub p in players)
                        {
                            Player.Get(p).SetScale(val);
                        }
                        response = $"Done! Set scale to: {val}";
                        return true;
                    }
                    catch
                    {
                        response = "Player not found!";
                        return false;
                    }
            }
        }
    }
}
