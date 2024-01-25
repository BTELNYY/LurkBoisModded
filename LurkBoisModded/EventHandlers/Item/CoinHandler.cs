using PluginAPI.Events;
using PluginAPI.Enums;
using PluginAPI.Core.Attributes;
using PluginAPI.Core;
using UnityEngine;
using Utils;

namespace LurkBoisModded.EventHandlers.Item
{
    [EventHandler]
    public class CoinHandler
    {
        [PluginEvent(ServerEventType.PlayerCoinFlip)]
        public void OnFlipCoin(PlayerCoinFlipEvent ev)
        {
            float random = Random.Range(0f, 100f);
            if(random <= (100f * Plugin.GetConfig().CoinExplodeChance))
            {
                ExplosionUtils.ServerExplode(ev.Player.ReferenceHub);
            }
        }
    }
}
