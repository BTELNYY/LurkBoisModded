using PluginAPI.Core.Attributes;
using PluginAPI.Events;
using PluginAPI.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PlayerStatsSystem;
using LurkBoisModded.Extensions;
using Utils.NonAllocLINQ;
using PluginAPI.Core;
using Mono.CompilerServices.SymbolWriter;

namespace LurkBoisModded.EventHandlers.General
{
    [EventHandler]
    public class KillCountHandler
    {
        public Dictionary<uint, int> Kills = new Dictionary<uint, int>();

        [PluginEvent(ServerEventType.PlayerDeath)]
        public void OnPlayerDeath(PlayerDeathEvent @event) 
        {
            if(@event.Attacker == null)
            {
                return;
            }
            if(@event.Attacker.IsSCP && Config.CurrentConfig.KillCountConfig.IgnoreSCPKills)
            {
                return;
            }
            if (Config.CurrentConfig.KillCountConfig.ResetOnDeath)
            {
                if (Kills.ContainsKey(@event.Player.ReferenceHub.netId))
                {
                    Kills.Remove(@event.Player.ReferenceHub.netId);
                }
            }
            if (Kills.ContainsKey(@event.Attacker.ReferenceHub.netId))
            {
                Kills[@event.Attacker.ReferenceHub.netId]++;
            }
            else
            {
                Kills.Add(@event.Attacker.ReferenceHub.netId, 1);
            }
        }

        [PluginEvent(ServerEventType.RoundEnd)]
        public void OnRoundEnd(RoundEndEvent @event) 
        {
            string msg = Config.CurrentConfig.KillCountConfig.Message;
            if (Kills.IsEmpty())
            {
                return;
            }
            int counter = 0;
            foreach (KeyValuePair<uint, int> author in Kills.OrderBy(key => key.Key))
            {
                if(counter >= Config.CurrentConfig.KillCountConfig.MaxAmountDisplayed)
                {
                    return;
                }
                ReferenceHub hub = ReferenceHub.AllHubs.Where(x => x.netId == author.Key).FirstOrDefault();
                if(hub == null)
                {
                    continue;
                }
                msg += $"<color={hub.roleManager.CurrentRole.RoleColor.ToHex()}>{hub.nicknameSync.MyNick}</color>: {author.Value}\n";
                counter++;
            }
            foreach(ReferenceHub hub in ReferenceHub.AllHubs)
            {
                Player p = Player.Get(hub);
                p.SendBroadcast(msg, Config.CurrentConfig.KillCountConfig.MessageDuration, shouldClearPrevious: true);
            }
        }
    }
}
