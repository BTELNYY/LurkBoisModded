using HarmonyLib;
using LurkBoisModded.Commands.GameConsole;
using PlayerRoles.PlayableScps.Scp079;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LurkBoisModded.Patches.Scp079
{
    [HarmonyPatch(typeof(Scp079LockdownRoomAbility), "ServerProcessCmd")]
    public class Scp079LockdownPatch
    {
        public static bool Prefix(Scp079LockdownRoomAbility __instance)
        {
            if(CommandGas.CurrentRoom == __instance.ScpRole.CurrentCamera.Room)
            {
                return false;
            }
            return true;
        }
    }
}
