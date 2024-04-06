using LurkBoisModded.Abilities;
using LurkBoisModded.Base;
using LurkBoisModded.EventHandlers;
using MEC;
using PlayerRoles.FirstPersonControl;
using PluginAPI.Core.Attributes;
using PluginAPI.Enums;
using PluginAPI.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace LurkBoisModded.Managers
{
    [EventHandler]
    public class AbilityManager
    {
        public static Dictionary<AbilityType, Type> AbilityToType = new Dictionary<AbilityType, Type>()
        {
            [AbilityType.ProximityChat] = typeof(ProximityChatAbility),
            [AbilityType.RemoteExplosive] = typeof(RemoteExplosiveAbility),
            [AbilityType.Inspire] = typeof(InspireAbility),
            [AbilityType.WarCry] = typeof(WarCryAbility),
            [AbilityType.Scout] = typeof(ScoutAbility),
            [AbilityType.AreaDenialAbility] = typeof(AreaDenialAbility),
            [AbilityType.MedicAbility] = typeof(MedicAbility)
        };

        [PluginEvent(ServerEventType.PlayerSpawn)]
        public void OnSpawn(PlayerSpawnEvent ev)
        {
            foreach (CustomAbility ability in ev.Player.GameObject.GetComponents<CustomAbility>())
            {
                GameObject.Destroy(ability);
            }
            Timing.CallDelayed(1f, () =>
            {
                if (Plugin.GetConfig().ProximityChatConfig.AllowedRoles.Contains(ev.Role))
                {
                    ProximityChatAbility.ToggledPlayers.Remove(ev.Player.ReferenceHub);
                    ev.Player.ReferenceHub.AddAbility(AbilityType.ProximityChat);
                }
            });
        }

        [PluginEvent(ServerEventType.RoundRestart)]
        public void OnRoundRestart(RoundRestartEvent ev)
        {
            ProximityChatAbility.ToggledPlayers.Clear();
        }

        public static bool OnPlayerTogglingNoClip(ReferenceHub player)
        {
            if (FpcNoclip.IsPermitted(player))
            {
                return true;
            }

            List<CustomAbility> abilities = player.gameObject.GetComponents<CustomAbility>().ToList();
            foreach(CustomAbility ability in abilities)
            {
                ability.OnTrigger();
            }
            return false;
        }
    }
    public enum AbilityType
    {
        ProximityChat,
        RemoteExplosive,
        Inspire,
        WarCry,
        Scout,
        AreaDenialAbility,
        MedicAbility,
    }
}
