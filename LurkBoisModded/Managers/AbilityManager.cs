using LurkBoisModded.Abilities;
using LurkBoisModded.Base;
using LurkBoisModded.Base.CustomItems;
using LurkBoisModded.Base.Ability;
using LurkBoisModded.Extensions;
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
using CentralAuth;
using static UnityEngine.GraphicsBuffer;
using PluginAPI.Core;

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
            [AbilityType.MedicAbility] = typeof(MedicAbility),
            [AbilityType.Scp079GasAbility] = typeof(Scp079GasAbility),
        };

        public static Dictionary<ReferenceHub, List<CustomAbility>> PlayerToAbility = new Dictionary<ReferenceHub, List<CustomAbility>>();

        public static void RemoveAllAbilities(ReferenceHub target)
        {
            List<CustomAbility> abilities = target.gameObject.GetComponents<CustomAbility>().ToList();
            foreach (CustomAbility ability in abilities)
            {
                ability.OnRemoved();
                GameObject.Destroy(ability);
            }
            if (PlayerToAbility.ContainsKey(target))
            {
                PlayerToAbility[target].Clear();
            }
        }

        public static void RemoveAbility<T>(ReferenceHub target) where T : CustomAbility
        {
            if (!PlayerToAbility.ContainsKey(target))
            {
                return;
            }
            List<CustomAbility> abilitiesToRemove = PlayerToAbility[target].Where(x => x.GetType() == typeof(T)).ToList();
            foreach(CustomAbility ab in abilitiesToRemove)
            {
                ab.OnRemoved();
                GameObject.Destroy(ab);
                PlayerToAbility[target].Remove(ab);
            }
        }

        public static T GetAbility<T>(ReferenceHub target) where T : CustomAbility
        {
            if (!PlayerToAbility.ContainsKey(target))
            {
                return null;
            }
            List<CustomAbility> matchedAbilities = PlayerToAbility[target].Where(x => x.GetType() == typeof(T)).ToList();
            if (matchedAbilities.IsEmpty())
            {
                return null;
            }
            return matchedAbilities.First() as T;
        }

        public static void AddAbility(ReferenceHub target, AbilityType type)
        {
            if (target.authManager.InstanceMode != ClientInstanceMode.ReadyClient || target.nicknameSync.MyNick == "Dedicated Server")
            {
                Log.Error("Tried adding ability to Dedicated Server or Unready Client!");
                return;
            }
            if (!AbilityManager.AbilityToType.ContainsKey(type))
            {
                Log.Error("Failed to find ability by AbilityType!");
                return;
            }
            CustomAbility ability = (CustomAbility)target.gameObject.AddComponent(AbilityManager.AbilityToType[type]);
            ability.CurrentOwner = target;
            ability.OnFinishSetup();
            if (PlayerToAbility.ContainsKey(target))
            {
                PlayerToAbility[target].Add(ability);
            }
            else
            {
                PlayerToAbility.Add(target, new List<CustomAbility> { ability });
            }
        }

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
            PlayerToAbility.Clear();
        }

        public static bool OnPlayerTogglingNoClip(ReferenceHub player)
        {
            if(player == null) return false;
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
        Scp079GasAbility,
    }
}
