using System;
using System.Collections.Generic;
using System.Linq;
using PluginAPI.Enums;
using PluginAPI.Events;
using PluginAPI.Core;
using PluginAPI.Core.Attributes;
using System.Text;
using System.Threading.Tasks;
using LurkBoisModded.Gamemodes;

namespace LurkBoisModded.EventHandlers.General
{
    [EventHandler]
    public class GamemodeInitHandler
    {
        [PluginEvent(ServerEventType.WaitingForPlayers)]
        public void OnLobbyReady()
        {
            GamemodeManager.TriggerNextRoundGamemode();
        }

        [PluginEvent(ServerEventType.RoundEnd)]
        public void OnRoundEnd()
        {
            GamemodeManager.StopGamemode();
        }
    }
}
