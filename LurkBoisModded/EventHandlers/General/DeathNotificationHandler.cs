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
    public class DeathNotificationHandler
    {
        [PluginEvent(ServerEventType.PlayerDeath)]
        public void OnPlayerDeath(PlayerDeathEvent @event)
        {
            if(@event.Attacker == null)
            {
                return;
            }
            if(Config.CurrentConfig.DoKillMessages)
            {
                string proc = Config.CurrentConfig.KillMessage.Replace("{playername}", @event.Player.Nickname).Replace("{color}", @event.Player.RoleBase.RoleColor.ToHex());
                @event.Attacker.SendHint(proc);
            }
        }
    }
}
