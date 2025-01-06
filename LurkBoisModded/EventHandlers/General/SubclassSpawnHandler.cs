using PluginAPI.Events;
using PluginAPI.Enums;
using PluginAPI.Core.Attributes;
using PluginAPI.Core;
using Respawning;
using MEC;
using UnityEngine;
using System.Collections.Generic;
using PlayerRoles;
using LurkBoisModded.Base;
using LurkBoisModded.Managers;
using System.Linq;
using LurkBoisModded.Extensions;

namespace LurkBoisModded.EventHandlers.General
{
    [EventHandler]
    public class SubclassSpawnHandler
    {
        static bool Scp3114Spawned = false;

        [PluginEvent(ServerEventType.TeamRespawn)]
        public void OnTeamRespawn(TeamRespawnEvent ev)
        {
            Timing.CallDelayed(0.15f, () => 
            {
                switch (ev.Team)
                {
                    case Faction.FoundationStaff:
                        HandleMtfSpawn(ev.Players);
                        break;
                    case Faction.FoundationEnemy:
                        HandleCiSpawn(ev.Players);
                        break;
                }
            });
        }

        [PluginEvent(ServerEventType.RoundStart)]
        public void Handle3114(RoundStartEvent ev)
        {
            Timing.CallDelayed(0.1f, () => 
            {
                List<Player> alive = Player.GetPlayers().Where(x => x.IsSCP).ToList();
                HandleScp3114Spawn(alive);
            });
        }

        [PluginEvent(ServerEventType.RoundStart)]
        public void HandleClassD(RoundStartEvent ev)
        {
            Timing.CallDelayed(0.3f, () => 
            {
                List<Player> dclass = Utility.GetPlayersByRole(RoleTypeId.ClassD);
                HandleClassD(dclass);
            });
        }

        [PluginEvent(ServerEventType.RoundStart)]
        public void HandleGuards(RoundStartEvent ev)
        {
            Timing.CallDelayed(0.5f, () =>
            {
                List<Player> guards = Utility.GetPlayersByRole(RoleTypeId.FacilityGuard);
                HandleGuards(guards);
            });
        }

        [PluginEvent(ServerEventType.RoundStart)]
        public void HandleScientists(RoundStartEvent ev)
        {
            Timing.CallDelayed(0.3f, () =>
            {
                List<Player> scientists = Utility.GetPlayersByRole(RoleTypeId.Scientist);
                HandleScientists(scientists);
            });
        }

        public void HandleSpawn(List<Player> players, Dictionary<string, int> subclass)
        {
            foreach (string s in subclass.Keys)
            {
                Subclass subclassbase = SubclassManager.GetSubclass(s);
                if (subclassbase == null)
                {
                    Log.Warning("Can't find subclass by name! Name: " + s);
                    continue;
                }
                for (int i = 0; i < subclass[s]; i++)
                {
                    if (players.IsEmpty())
                    {
                        continue;
                    }
                    if(subclassbase.TargetRoleUsed && players.Where(x => x.Role == subclassbase.TargetRole).Count() == 0)
                    {
                        Log.Info($"Unable to find any player to be assigned TargetRole subclass {subclassbase.FileName}. Target Role: {subclassbase.TargetRole}");
                        break;
                    }
                    Player selectedPlayer = players.ToArray().RandomItem();
                    selectedPlayer.ReferenceHub.SetSubclass(subclassbase);
                    players.Remove(selectedPlayer);
                }
            }
        }

        private void HandleScp3114Spawn(List<Player> selectablePlayers)
        {
            float currentSpawnChance = Plugin.GetConfig().Scp3114Config.Scp3114SpawnChance;
            if(!(Player.GetPlayers().Where(x => !x.IsSCP && x.IsAlive).Count() >= Plugin.GetConfig().Scp3114Config.MinimumPlayers))
            {
                currentSpawnChance = 0f;
                return;
            }
            if((Utility.GetPlayersByRole(RoleTypeId.Scp079).Count == 0 && Plugin.GetConfig().Scp3114Config.NoScp079Means100PercentSpawn))
            {
                currentSpawnChance = 1f;
            }
            float randomValue = Random.Range(0f, 1f);
            if(!(randomValue <= currentSpawnChance))
            {
                return;
            }
            Scp3114Spawned = true;
            selectablePlayers.GetRandomItem().SetRole(RoleTypeId.Scp3114, RoleChangeReason.RoundStart);
        }

        private void HandleMtfSpawn(List<Player> players)
        {
            List<Player> handledPlayers = players;
            HandleSpawn(handledPlayers, Config.CurrentConfig.SubclassSpawnConfig.MtfSpawnSubclasses);
        }

        private void HandleCiSpawn(List<Player> players)
        {
            List<Player> handledPlayers = players;
            HandleSpawn(handledPlayers, Config.CurrentConfig.SubclassSpawnConfig.CiSpawnSubclasses);
        }

        private void HandleClassD(List<Player> players)
        {
            List<Player> handledPlayers = players;
            if (Scp3114Spawned)
            {
                SubclassManager.TempDisallowedRooms.Add(MapGeneration.RoomName.Lcz173);
            }
            HandleSpawn(handledPlayers, Config.CurrentConfig.SubclassSpawnConfig.ClassDSubclasses);
        }

        private void HandleGuards(List<Player> players)
        {
            List<Player> handledPlayers = players;
            List<string> shuffledRoles = Plugin.GetConfig().SubclassSpawnConfig.GuardSubclasses.Keys.ToList();
            shuffledRoles.ShuffleList();
            HandleSpawn(handledPlayers, Config.CurrentConfig.SubclassSpawnConfig.GuardSubclasses);
        }

        private void HandleScientists(List<Player> players)
        {
            List<Player> handledPlayers = players;
            List<string> shuffledRoles = Plugin.GetConfig().SubclassSpawnConfig.ScientistSubclasses.Keys.ToList();
            shuffledRoles.ShuffleList();
            HandleSpawn(handledPlayers, Config.CurrentConfig.SubclassSpawnConfig.ScientistSubclasses);
        }
    }
}
