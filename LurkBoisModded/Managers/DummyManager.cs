using HarmonyLib;
using InventorySystem;
using MEC;
using Mirror;
using PlayerRoles;
using PlayerStatsSystem;
using PluginAPI.Core.Attributes;
using PluginAPI.Core;
using PluginAPI.Enums;
using UnityEngine;
using LurkBoisModded.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PlayerRoles.FirstPersonControl;
using CentralAuth;
using PlayerRoles.Ragdolls;

namespace LurkBoisModded.Managers
{
    public class DummyManager
    {
        public static List<ReferenceHub> Dummies = new List<ReferenceHub>();
        public static List<string> DummiesToDestroyOnDeath = new List<string>();

        private static ReferenceHub SpawnDummyInternal()
        {
            try
            {
                var clone = GameObject.Instantiate(NetworkManager.singleton.playerPrefab);
                var hub = clone.GetComponent<ReferenceHub>();
                int hubPlayerId = hub.PlayerId;
                NetworkServer.AddPlayerForConnection(new FakeConnection(hubPlayerId++), clone);
                AccessTools.PropertySetter(typeof(NicknameSync), nameof(NicknameSync.MyNick)).Invoke(hub.nicknameSync, new object[] {"Dummy"});
                PlayerAuthenticationManager authManager = hub.authManager;
                AccessTools.Field(typeof(PlayerAuthenticationManager), "_privUserId").SetValue(authManager, $"Dummy-{ReferenceHub.AllHubs.Count + 1}");
                authManager.NetworkSyncedUserId = $"Dummy-{ReferenceHub.AllHubs.Count + 1}";
                AccessTools.Field(typeof(PlayerAuthenticationManager), "_targetInstanceMode").SetValue(authManager, ClientInstanceMode.DedicatedServer);
                hub.roleManager.ServerSetRole(PlayerRoles.RoleTypeId.Spectator, PlayerRoles.RoleChangeReason.RemoteAdmin);
                Dummies.Add(hub);
                return hub;
            }
            catch (Exception ex)
            {
                Log.Error("SpawnDummyPlayer encountered an exception: " + ex.Message);
                return null;
            }
        }

        public static ReferenceHub SpawnDummy(Vector3 pos, RoleTypeId role)
        {
            //Log.Debug("Spawn dummy!");
            try
            {
                ReferenceHub hubPlayer = SpawnDummyInternal();
                hubPlayer.roleManager.ServerSetRole(role, RoleChangeReason.RemoteAdmin);
                Timing.CallDelayed(0.3f, () =>
                {
                    hubPlayer.TryOverridePosition(pos, Vector3.zero);
                });
                return hubPlayer;
            }
            catch (Exception e)
            {
                Log.Error($"Error on {nameof(DummyManager)}: {e} -- {e.Message}");
                return null;
            }
        }

        public static ReferenceHub SpawnDummy(Vector3 pos, bool destroyOnDeath, RoleTypeId role)
        {
            //Log.Debug("Spawn dummy!");
            try
            {
                ReferenceHub hubPlayer = SpawnDummyInternal();
                //Log.Debug("Adding dummy to the list of dummies");
                if (destroyOnDeath)
                {
                    DummiesToDestroyOnDeath.Add(hubPlayer.authManager.UserId);
                }
                hubPlayer.roleManager.ServerSetRole(role, RoleChangeReason.RemoteAdmin);
                Timing.CallDelayed(0.3f, () =>
                {
                    hubPlayer.TryOverridePosition(pos + new Vector3(0f, 1.3f, 0f), Vector3.zero);
                });
                return hubPlayer;
            }
            catch (Exception e)
            {
                Log.Error($"Error on {nameof(DummyManager)}: {e} -- {e.Message}");
                return null;
            }
        }

        public static ReferenceHub SpawnDummy(Vector3 pos, bool destroyOnDeath, string name, RoleTypeId role)
        {
            //Log.Debug("Spawn dummy!");
            try
            {
                ReferenceHub hubPlayer = SpawnDummyInternal();
                //Log.Debug("Spawning dummy");
                if(destroyOnDeath)
                {
                    DummiesToDestroyOnDeath.Add(hubPlayer.authManager.UserId);
                }
                hubPlayer.nicknameSync.Network_myNickSync = name;
                hubPlayer.roleManager.ServerSetRole(role, RoleChangeReason.RemoteAdmin);
                Timing.CallDelayed(0.3f, () =>
                {
                    hubPlayer.TryOverridePosition(pos + new Vector3(0f, 1.3f, 0f), Vector3.zero);
                });
                return hubPlayer;
            }
            catch (Exception e)
            {
                Log.Error($"Error on {nameof(DummyManager)}: {e} -- {e.Message}");
                return null;
            }
        }

        public static void CloneRagdoll(BasicRagdoll ragdoll, Vector3 spawnPosition)
        {
            var dummy = SpawnDummy(spawnPosition, true, ragdoll.Info.Nickname, ragdoll.Info.RoleType);
            DamageHandlerBase damageHandlerBase = ragdoll.Info.Handler;
            dummy.inventory.UserInventory.ReserveAmmo.Clear();
            InventoryInfo userInventory = dummy.inventory.UserInventory;
            while (userInventory.Items.Count > 0)
            {
                dummy.inventory.ServerRemoveItem(userInventory.Items.ElementAt(0).Key, null);
            }
            Timing.CallDelayed(0.3f, () =>
            {
                dummy.playerStats.DealDamage(ragdoll.Info.Handler);
            });
        }

        public static void SpawnRagdoll(string name, Vector3 position, RoleTypeId role, DeathTranslation translation)
        {
            ReferenceHub dummy = SpawnDummy(position, false, name, role);
            UniversalDamageHandler un = new UniversalDamageHandler(-1, translation);
            Timing.CallDelayed(0.4f, () =>
            {
                dummy.playerStats.DealDamage(un);
            });
            NetworkServer.Destroy(dummy.gameObject);
        }

        public static void SpawnRagdoll(string name, Vector3 position, RoleTypeId role, string reason)
        {
            ReferenceHub dummy = SpawnDummy(position, false, name, role);
            Timing.CallDelayed(0.4f, () =>
            {
                Player.Get(dummy).Kill(reason);
            });
            NetworkServer.Destroy(dummy.gameObject);
        }

        [PluginEvent(ServerEventType.PlayerDeath)]
        public void OnPlayerDeath(Player player, Player attacker, DamageHandlerBase damageHandler)
        {
            if (DummiesToDestroyOnDeath.Contains(player.UserId))
            {
                NetworkServer.Destroy(player.GameObject);
            }
        }
    }
}
