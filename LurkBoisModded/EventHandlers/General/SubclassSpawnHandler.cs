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
                    case SpawnableTeamType.NineTailedFox:
                        HandleMtfSpawn(ev.Players);
                        break;
                    case SpawnableTeamType.ChaosInsurgency:
                        HandleCiSpawn(ev.Players);
                        break;
                }
            });
        }

        [PluginEvent(ServerEventType.RoundStart)]
        public void OnRoundStart(RoundStartEvent ev)
        {
            Timing.CallDelayed(0.15f, () => 
            {
                List<Player> alive = Player.GetPlayers().Where(x => !x.IsSCP && x.IsAlive).ToList();
                HandleScp3114Spawn(alive);
                List<Player> dclass = Utility.GetPlayersByRole(RoleTypeId.ClassD);
                HandleClassD(dclass);
                List<Player> guards = Utility.GetPlayersByRole(RoleTypeId.FacilityGuard);
                HandleGuards(guards);
                List<Player> scientists = Utility.GetPlayersByRole(RoleTypeId.Scientist);
                HandleScientists(scientists);
            });
        }

        private void HandleScp3114Spawn(List<Player> selectablePlayers)
        {
            float currentSpawnChance = Plugin.GetConfig().Scp3114Config.Scp3114SpawnChance;
            if(!(selectablePlayers.Count >= Plugin.GetConfig().Scp3114Config.MinimumPlayers))
            {
                currentSpawnChance = 0f;
                return;
            }
            if(!(Utility.GetPlayersByRole(RoleTypeId.Scp079).Count == 0 && Plugin.GetConfig().Scp3114Config.NoScp079Means100PercentSpawn))
            {
                currentSpawnChance = 0f;
                return;
            }
            else
            {
                currentSpawnChance = 1f;
            }
            float randomValue = Random.Range(0f, 1f);
            if(!(randomValue <= currentSpawnChance))
            {
                return;
            }
            Scp3114Spawned = true;
            selectablePlayers.RandomItem().SetRole(RoleTypeId.Scp3114, RoleChangeReason.RoundStart);
        }

        private void HandleMtfSpawn(List<Player> players)
        {
            List<Player> handledPlayers = players;
            foreach (string subclass in Plugin.GetConfig().SubclassSpawnConfig.MtfSpawnSubclasses.Keys)
            {
                Subclass subclassbase = SubclassManager.GetSubclass(subclass);
                if (subclassbase == null)
                {
                    Log.Warning("Can't find subclass by name! Name: " + subclass);
                    continue;
                }
                for (int i = 0; i < Plugin.GetConfig().SubclassSpawnConfig.MtfSpawnSubclasses[subclass]; i++)
                {
                    Player selectedPlayer = handledPlayers.RandomItem();
                    selectedPlayer.ReferenceHub.SetSubclass(subclassbase);
                    handledPlayers.Remove(selectedPlayer);
                }
            }
        }

        private void HandleCiSpawn(List<Player> players)
        {
            List<Player> handledPlayers = players;
            foreach (string subclass in Plugin.GetConfig().SubclassSpawnConfig.CiSpawnSubclasses.Keys)
            {
                Subclass subclassbase = SubclassManager.GetSubclass(subclass);
                if (subclassbase == null)
                {
                    Log.Warning("Can't find subclass by name! Name: " + subclass);
                    continue;
                }
                for (int i = 0; i < Plugin.GetConfig().SubclassSpawnConfig.CiSpawnSubclasses[subclass]; i++)
                {
                    Player selectedPlayer = handledPlayers.RandomItem();
                    selectedPlayer.ReferenceHub.SetSubclass(subclassbase);
                    handledPlayers.Remove(selectedPlayer);
                }
            }
        }

        private void HandleClassD(List<Player> players)
        {
            List<Player> handledPlayers = players;
            List<string> shuffledRoles = Plugin.GetConfig().SubclassSpawnConfig.ClassDSubclasses.Keys.ToList();
            shuffledRoles.ShuffleList();
            if (Scp3114Spawned)
            {
                SubclassManager.TempDisallowedRooms.Add(MapGeneration.RoomName.Lcz173);
            }
            foreach (string subclass in shuffledRoles)
            {
                Subclass subclassbase = SubclassManager.GetSubclass(subclass);
                if (subclassbase == null)
                {
                    Log.Warning("Can't find subclass by name! Name: " + subclass);
                    continue;
                }
                for (int i = 0; i < Plugin.GetConfig().SubclassSpawnConfig.ClassDSubclasses[subclass]; i++)
                {
                    Player selectedPlayer = handledPlayers.RandomItem();
                    selectedPlayer.ReferenceHub.SetSubclass(subclassbase);
                    handledPlayers.Remove(selectedPlayer);
                }
            }
        }

        private void HandleGuards(List<Player> players)
        {
            List<Player> handledPlayers = players;
            List<string> shuffledRoles = Plugin.GetConfig().SubclassSpawnConfig.GuardSubclasses.Keys.ToList();
            shuffledRoles.ShuffleList();
            foreach (string subclass in shuffledRoles)
            {
                Subclass subclassbase = SubclassManager.GetSubclass(subclass);
                if (subclassbase == null)
                {
                    Log.Warning("Can't find subclass by name! Name: " + subclass);
                    continue;
                }
                for (int i = 0; i < Plugin.GetConfig().SubclassSpawnConfig.GuardSubclasses[subclass]; i++)
                {
                    Player selectedPlayer = handledPlayers.RandomItem();
                    selectedPlayer.ReferenceHub.SetSubclass(subclassbase);
                    handledPlayers.Remove(selectedPlayer);
                }
            }
        }

        private void HandleScientists(List<Player> players)
        {
            List<Player> handledPlayers = players;
            List<string> shuffledRoles = Plugin.GetConfig().SubclassSpawnConfig.ScientistSubclasses.Keys.ToList();
            shuffledRoles.ShuffleList();
            foreach (string subclass in shuffledRoles)
            {
                Subclass subclassbase = SubclassManager.GetSubclass(subclass);
                if (subclassbase == null)
                {
                    Log.Warning("Can't find subclass by name! Name: " + subclass);
                    continue;
                }
                for (int i = 0; i < Plugin.GetConfig().SubclassSpawnConfig.ScientistSubclasses[subclass]; i++)
                {
                    Player selectedPlayer = handledPlayers.RandomItem();
                    selectedPlayer.ReferenceHub.SetSubclass(subclassbase);
                    handledPlayers.Remove(selectedPlayer);
                }
            }
        }
    }
}
