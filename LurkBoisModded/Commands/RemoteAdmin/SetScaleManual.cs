using CommandSystem;
using PluginAPI.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Text;
using System.Threading.Tasks;

namespace LurkBoisModded.Commands.RemoteAdmin
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class SetScaleManual : ICommand
    {
        public string Command => "setscalespecific";

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
                response = "Usage:\nscale <id/name> or * <x,y,z>" +
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
                        response = "Usage: scale * <x,y,z>";
                        return false;
                    }

                    Vector3 vec = Vector3.zero;

                    string[] array = arguments.At(1).Split(',');
                    if (array.Length > 3)
                    {
                        response = "Invalid Size. Format: x,y,z";
                        return false;
                    }
                    int counter = 0;
                    foreach (string s in array)
                    {
                        if (!float.TryParse(s, out float result))
                        {
                            response = "Invalid Size. Format: x,y,z";
                            return false;
                        }
                        switch (counter)
                        {
                            case 0: vec.x = result; break;
                            case 1: vec.y = result; break;
                            case 2: vec.z = result; break;
                        }
                        counter++;
                    }

                    foreach (ReferenceHub hub in ReferenceHub.AllHubs)
                    {
                        Player player = Player.Get(hub);
                        player.SetScale(vec);
                    }
                    response = $"Set player(s) scale to: {vec}";
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

                        Vector3 vec1 = Vector3.zero;

                        string[] array1 = arguments.At(1).Split(',');
                        if(array1.Length > 3)
                        {
                            response = "Invalid Size. Format: x,y,z";
                            return false;
                        }
                        int counter1 = 0;
                        foreach(string s in array1)
                        {
                            if(!float.TryParse(s, out float result))
                            {
                                response = "Invalid Size. Format: x,y,z";
                                return false;
                            }
                            switch (counter1)
                            {
                                case 0: vec1.x = result; break;
                                case 1: vec1.y = result; break;
                                case 2: vec1.z = result; break;
                            }
                            counter1++;
                        }

                        foreach (ReferenceHub p in players)
                        {
                            Player.Get(p).SetScale(vec1);
                        }
                        response = $"Done! Set scale to: {vec1}";
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
