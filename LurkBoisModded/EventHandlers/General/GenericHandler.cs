using PluginAPI.Events;
using PluginAPI.Enums;
using PluginAPI.Core.Attributes;
using MEC;
using PlayerRoles;
using UnityEngine;
using LurkBoisModded.Commands.GameConsole;
using LurkBoisModded.Managers;
using LurkBoisModded.Abilities;
using LurkBoisModded.Base;
using System;

namespace LurkBoisModded.EventHandlers.General
{
    [EventHandler]
    public class GenericHandler
    {
        [PluginEvent(ServerEventType.PlayerSpawn)]
        public void OnSpawn(PlayerSpawnEvent ev)
        {
            Timing.CallDelayed(0.1f, () =>
            {
                //Fix SCP 049 cloak?
                if(ev.Player.Role == RoleTypeId.Scp049)
                {
                    return;
                }
                //Reset scale if you are alive
                if (ev.Player.IsAlive)
                {
                    ev.Player.SetScale(1f);
                }
                //Randomize human
                if(Plugin.GetConfig().RandomizeHumanHeight && !ev.Player.IsSCP && ev.Player.IsAlive)
                {
                    float randomValue = UnityEngine.Random.Range(Plugin.GetConfig().MinHeight, Plugin.GetConfig().MaxHeight);
                    Vector3 vec = new Vector3(1, randomValue, 1);
                    ev.Player.SetScale(vec);
                }
                //Randomize SCP 939
                if(ev.Player.Role == RoleTypeId.Scp939 && Plugin.GetConfig().Scp939Config.ModifyHeight)
                {
                    float randomValue = UnityEngine.Random.Range(Plugin.GetConfig().Scp939Config.MinHeight, Plugin.GetConfig().Scp939Config.MaxHeight);
                    Vector3 scale = new Vector3(1, randomValue, 1);
                    ev.Player.SetScale(scale);
                }
            });
        }

        [PluginEvent(ServerEventType.PlayerDeath)]
        public void OnPlayerDeath(PlayerDeathEvent ev)
        {
            ev.Player.CustomInfo = string.Empty;
            ev.Player.PlayerInfo.IsRoleHidden = false;
            ev.Player.ReferenceHub.RemoveAllAbilities();
            ev.Player.ReferenceHub.SetMaxHealth(-1f);
        }

        [PluginEvent(ServerEventType.PlayerChangeRole)]
        public void OnClassChange(PlayerChangeRoleEvent ev)
        {
            if(ev.ChangeReason == RoleChangeReason.Destroyed)
            {
                return;
            }
            ev.Player.CustomInfo = string.Empty;
            ev.Player.PlayerInfo.IsRoleHidden = false;
            ev.Player.ReferenceHub.RemoveAllAbilities();
            ev.Player.ReferenceHub.SetMaxHealth(-1f);
        }

        [PluginEvent(ServerEventType.RoundEnd)]
        public void OnRoundEnd(RoundEndEvent ev)
        {
            CommandGas.Cooldown = false;
            CommandGas.CurrentRoom = null;
            SubclassManager.TempDisallowedRooms.Clear();
            ProximityChatAbility.ToggledPlayers.Clear();
        }

        public static event Action OnRoundRestart;

        [PluginEvent(ServerEventType.RoundRestart)]
        public void OnRoundRestarted(RoundRestartEvent ev)
        {
            OnRoundRestart?.Invoke();
        }
    }
}
