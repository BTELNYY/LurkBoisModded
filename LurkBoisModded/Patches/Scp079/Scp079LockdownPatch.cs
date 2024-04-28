using HarmonyLib;
using LurkBoisModded.Abilities;
using LurkBoisModded.Commands.GameConsole;
using LurkBoisModded.Extensions;
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
            Scp079GasAbility ability = __instance.Owner.GetAbility<Scp079GasAbility>();
            if(ability == null)
            {
                return true;
            }
            if(ability.CurrentRoom == __instance.CastRole.CurrentCamera)
            {
                return false;
            }
            return true;
        }
    }
}
