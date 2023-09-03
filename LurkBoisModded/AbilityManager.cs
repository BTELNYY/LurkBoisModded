using PlayerRoles.FirstPersonControl;
using System.Collections.Generic;
using System.Linq;
using PluginAPI.Events;
using PluginAPI.Core;
using PluginAPI.Enums;
using PluginAPI.Core.Attributes;
using System;
using LurkBoisModded.Abilities;
using YamlDotNet.Serialization.NamingConventions;
using YamlDotNet.Serialization;
using UnityEngine;

namespace LurkBoisModded
{
    public class AbilityManager
    {
        public static Dictionary<AbilityType, Type> AbilityToType = new Dictionary<AbilityType, Type>()
        {
            [AbilityType.ProximityChat] = typeof(ProximityChatAbility),
            [AbilityType.RemoteExplosive] = typeof(RemoteExplosiveAbility),
            [AbilityType.Inspire] = typeof(InspireAbility)
        };

        [PluginEvent(ServerEventType.PlayerSpawn)]
        public void OnSpawn(PlayerSpawnEvent ev)
        {
            foreach (CustomAbilityBase ability in ev.Player.GameObject.GetComponents<CustomAbilityBase>())
            {
                GameObject.Destroy(ability);
            }
            if (Plugin.GetConfig().ProximityChatConfig.AllowedRoles.Contains(ev.Role))
            {
                ProximityChatAbility.ToggledPlayers.Remove(ev.Player.ReferenceHub);
                ev.Player.ReferenceHub.AddAbility(AbilityType.ProximityChat);
            }
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

            List<CustomAbilityBase> abilities = player.gameObject.GetComponents<CustomAbilityBase>().ToList();
            foreach(CustomAbilityBase ability in abilities)
            {
                ability.OnTrigger();
            }
            return false;
        }
    }
}
