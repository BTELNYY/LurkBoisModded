using LurkBoisModded.Base;
using LurkBoisModded.EventHandlers;
using LurkBoisModded.Extensions;
using MEC;
using PlayerRoles;
using PluginAPI.Core.Attributes;
using PluginAPI.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LurkBoisModded.Managers
{
    [EventHandler]
    public class RoleSwapManager
    {
        public static List<ReferenceHub> SCPSwapRequests = new List<ReferenceHub>();


        public static void SendSCPSwapRequest(ReferenceHub sender)
        {
            if(sender.GetTeam() != Team.SCPs)
            {
                return;
            }
            string scpNumber = sender.roleManager.CurrentRole.RoleTypeId.ToString().Replace("Scp", "");
            foreach (ReferenceHub hub in ReferenceHub.AllHubs.Where(x => x.IsAlive()))
            {
                Broadcast.Singleton.RpcAddElement(Config.CurrentConfig.RoleSwapConfig.ScpHumanSwapRequestMessage.Replace("{scp}", scpNumber), 10, Broadcast.BroadcastFlags.Normal);
            }
            SCPSwapRequests.Add(sender);
        }

        public static void SwapScpAndHuman(ReferenceHub human, ReferenceHub scp)
        {
            if(!human.IsAlive() || human.GetTeam() == Team.SCPs)
            {
                return;
            }
            if(scp.GetTeam() != Team.SCPs)
            {
                return;
            }
            RoleTypeId humanRole = human.roleManager.CurrentRole.RoleTypeId;
            RoleTypeId scpRole = scp.roleManager.CurrentRole.RoleTypeId;
            Subclass humanSubclass = human.GetSubclass();
            Subclass scpSubclass = scp.GetSubclass();
            if(humanSubclass != null)
            {
                scp.SetSubclass(humanSubclass);
            }
            else
            {
                scp.roleManager.ServerSetRole(humanRole, RoleChangeReason.RoundStart);
            }
            if(scpSubclass != null)
            {
                human.SetSubclass(scpSubclass);
            }
            else
            {
                human.roleManager.ServerSetRole(scpRole, RoleChangeReason.RoundStart);
            }
            if (SCPSwapRequests.Contains(scp))
            {
                SCPSwapRequests.Remove(scp);
            }
        }

        public static bool CanSwap
        {
            get
            {
                return _canSwap;
            }
        }

        private static bool _canSwap = true;

        [PluginEvent(PluginAPI.Enums.ServerEventType.RoundStart)]
        public void RoundStartEvent(RoundStartEvent ev)
        {
            _canSwap = true;
            Timing.CallDelayed(Config.CurrentConfig.RoleSwapConfig.SwapTime, () => 
            {
                _canSwap = false;
            });
        }

    }
}
