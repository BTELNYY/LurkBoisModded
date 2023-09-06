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

namespace LurkBoisModded.EventHandlers
{
    public class SubclassSpawnHandler
    {
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
                List<Player> dclass = Utility.GetPlayersByRole(RoleTypeId.ClassD);
                HandleClassD(dclass);
                List<Player> guards = Utility.GetPlayersByRole(RoleTypeId.FacilityGuard);
                HandleGuards(guards);
                List<Player> scientists = Utility.GetPlayersByRole(RoleTypeId.Scientist);
                HandleScientists(scientists);
            });
        }


        private void HandleMtfSpawn(List<Player> players)
        {
            List<Player> handledPlayers = players;
            foreach (string subclass in Plugin.GetConfig().SubclassSpawnConfig.MtfSpawnSubclasses.Keys)
            {
                SubclassBase subclassbase = SubclassManager.GetSubclass(subclass);
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
                SubclassBase subclassbase = SubclassManager.GetSubclass(subclass);
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
            foreach (string subclass in Plugin.GetConfig().SubclassSpawnConfig.ClassDSubclasses.Keys)
            {
                SubclassBase subclassbase = SubclassManager.GetSubclass(subclass);
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
            foreach (string subclass in Plugin.GetConfig().SubclassSpawnConfig.GuardSubclasses.Keys)
            {
                SubclassBase subclassbase = SubclassManager.GetSubclass(subclass);
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
            foreach (string subclass in Plugin.GetConfig().SubclassSpawnConfig.ScientistSubclasses.Keys)
            {
                SubclassBase subclassbase = SubclassManager.GetSubclass(subclass);
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
