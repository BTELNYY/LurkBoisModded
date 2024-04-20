using PluginAPI.Events;
using PluginAPI.Enums;
using PluginAPI.Core.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LurkBoisModded.Extensions;
using System.Threading.Tasks;
using PlayerRoles;

namespace LurkBoisModded.EventHandlers.General
{
    [EventHandler]
    public class DeathHandler
    {
        public Dictionary<uint, PlayerRoleBase> deadToOldRoles = new Dictionary<uint, PlayerRoleBase>();

        public static DeathHandler Instance { get; private set; }

        public DeathHandler() 
        {
            Instance = this;
        }

        [PluginEvent(ServerEventType.PlayerChangeRole)]
        public void OnPlayerChangeRole(PlayerChangeRoleEvent @event)
        {
            if(@event.ChangeReason != RoleChangeReason.Died)
            {
                return;
            }
            deadToOldRoles.Remove(@event.Player.NetworkId);
            deadToOldRoles.Add(@event.Player.NetworkId, @event.OldRole);
        }

        [PluginEvent(ServerEventType.PlayerDeath)]
        public void OnPlayerDeath(PlayerDeathEvent @event)
        {
            if(@event.Attacker == null)
            {
                return;
            }
            if (!deadToOldRoles.ContainsKey(@event.Player.NetworkId))
            {
                return;
            }
            if(Config.CurrentConfig.DoKillMessages)
            {
                string proc = Config.CurrentConfig.KillMessage.Replace("{playername}", @event.Player.Nickname).Replace("{color}", deadToOldRoles[@event.Player.NetworkId].RoleColor.ToHex());
                @event.Attacker.SendHint(proc);
            }
        }
    }
}
