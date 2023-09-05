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

namespace LurkBoisModded.Managers
{
    public class DummyManager
    {
        public static List<ReferenceHub> Dummies = new List<ReferenceHub>();
        public static List<string> DummiesToDestroyOnDeath = new List<string>();

        public static ReferenceHub SpawnDummy(Vector3 pos)
        {
            //Log.Debug("Spawn dummy!");
            try
            {

                #region Create Dummy

                var newPlayer =
                    UnityEngine.Object.Instantiate(NetworkManager.singleton.playerPrefab);
                int id = Dummies.Count;
                var fakeConnection = new FakeConnection(id++);
                var hubPlayer = newPlayer.GetComponent<ReferenceHub>();
                if (hubPlayer is null)
                {
                    newPlayer.AddComponent<ReferenceHub>();
                }
                if (hubPlayer.characterClassManager is null)
                {
                    hubPlayer.characterClassManager = new CharacterClassManager();
                }
                #endregion

                //Log.Debug("Adding dummy to the list of dummies");
                Dummies.Add(hubPlayer);
                //Log.Debug("Spawning dummy");
                NetworkServer.AddPlayerForConnection(fakeConnection, newPlayer);

                try
                {
                    //Log.Debug("Setting the UserId of the dummy");
                    AccessTools.Field(typeof(CharacterClassManager), "_privUserId").SetValue(hubPlayer.characterClassManager, $"Dummy-{id}@server");
                    //hubPlayer.characterClassManager._privUserId = $"SCP-575-{id}@server";
                    AccessTools.Field(typeof(CharacterClassManager), "InstanceMode").SetValue(hubPlayer.characterClassManager, ClientInstanceMode.Unverified);
                    //hubPlayer.characterClassManager.InstanceMode = ClientInstanceMode.Unverified;
                }
                catch
                {
                    
                }


                try
                {
                    //Log.Debug("Applying nickname");
                    hubPlayer.nicknameSync.ShownPlayerInfo &= ~PlayerInfoArea.Role;
                    hubPlayer.nicknameSync.ViewRange = 100f;
                    // SetNick it will always give an error but will apply it anyway.
                    object[] arr = { "Dummy" };
                    AccessTools.Method(typeof(NicknameSync), "SetNick").Invoke(hubPlayer.nicknameSync, arr);
                    //hubPlayer.nicknameSync.SetNick("Dummy");
                }
                catch
                {
                    // ignored
                }


                try
                {
                    hubPlayer.roleManager.ServerSetRole(RoleTypeId.Spectator, RoleChangeReason.RemoteAdmin);
                }
                catch (Exception e)
                {
                    Log.Error($"Error on {nameof(DummyManager)}: Error on set dummy role {e}");
                }

                hubPlayer.characterClassManager.GodMode = false;

                Timing.CallDelayed(0.3f, () =>
                {
                    //var room = victim.Room;

                    //if (room.Name == RoomName.Lcz173)
                    //{
                    //    hubPlayer.TryOverridePosition(room.ApiRoom.Position + new Vector3(0f, 13.5f, 0f), Vector3.zero);
                    //}
                    //else if (room.Name == RoomName.HczTestroom)
                    //{
                    //    if (DoorVariant.DoorsByRoom.TryGetValue(room, out var hashSet))
                    //    {
                    //        var door = hashSet.FirstOrDefault();
                    //        if (door != null) hubPlayer.TryOverridePosition(door.transform.position, Vector3.zero);
                    //    }
                    //}
                    //else
                    //{
                    //    hubPlayer.TryOverridePosition(room.ApiRoom.Position + new Vector3(0f, 1.3f, 0f), Vector3.zero);
                    //}
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

        public static void SpawnDummy(Vector3 pos, bool destroyOnDeath)
        {
            //Log.Debug("Spawn dummy!");
            try
            {

                #region Create Dummy

                var newPlayer =
                    UnityEngine.Object.Instantiate(NetworkManager.singleton.playerPrefab);
                int id = Dummies.Count;
                var fakeConnection = new FakeConnection(id++);
                var hubPlayer = newPlayer.GetComponent<ReferenceHub>();
                if (hubPlayer is null)
                {
                    newPlayer.AddComponent<ReferenceHub>();
                }
                if (hubPlayer.characterClassManager is null)
                {
                    hubPlayer.characterClassManager = new CharacterClassManager();
                }
                #endregion

                //Log.Debug("Adding dummy to the list of dummies");
                Dummies.Add(hubPlayer);
                //Log.Debug("Spawning dummy");
                NetworkServer.AddPlayerForConnection(fakeConnection, newPlayer);

                try
                {
                    //Log.Debug("Setting the UserId of the dummy");
                    AccessTools.Field(typeof(CharacterClassManager), "_privUserId").SetValue(hubPlayer.characterClassManager, $"Dummy-{id}@server");
                    if (destroyOnDeath)
                    {
                        DummiesToDestroyOnDeath.Add($"Dummy-{id}@server)");
                    }
                    //hubPlayer.characterClassManager._privUserId = $"SCP-575-{id}@server";
                    AccessTools.Field(typeof(CharacterClassManager), "InstanceMode").SetValue(hubPlayer.characterClassManager, ClientInstanceMode.Unverified);
                    //hubPlayer.characterClassManager.InstanceMode = ClientInstanceMode.Unverified;
                }
                catch (Exception ex)
                {
                    Log.Error(ex.ToString());
                }


                try
                {
                    //Log.Debug("Applying nickname");
                    hubPlayer.nicknameSync.ShownPlayerInfo &= ~PlayerInfoArea.Role;
                    hubPlayer.nicknameSync.ViewRange = 100f;
                    // SetNick it will always give an error but will apply it anyway.
                    object[] arr = { "Dummy" };
                    AccessTools.Method(typeof(NicknameSync), "SetNick").Invoke(hubPlayer.nicknameSync, arr);
                    //hubPlayer.nicknameSync.SetNick("Dummy");
                }
                catch (Exception)
                {
                    // ignored
                }


                try
                {
                    hubPlayer.roleManager.ServerSetRole(RoleTypeId.ClassD, RoleChangeReason.RemoteAdmin);
                }
                catch (Exception e)
                {
                    Log.Error($"Error on {nameof(DummyManager)}: Error on set dummy role {e}");
                }

                hubPlayer.characterClassManager.GodMode = false;

                Timing.CallDelayed(0.3f, () =>
                {
                    //var room = victim.Room;

                    //if (room.Name == RoomName.Lcz173)
                    //{
                    //    hubPlayer.TryOverridePosition(room.ApiRoom.Position + new Vector3(0f, 13.5f, 0f), Vector3.zero);
                    //}
                    //else if (room.Name == RoomName.HczTestroom)
                    //{
                    //    if (DoorVariant.DoorsByRoom.TryGetValue(room, out var hashSet))
                    //    {
                    //        var door = hashSet.FirstOrDefault();
                    //        if (door != null) hubPlayer.TryOverridePosition(door.transform.position, Vector3.zero);
                    //    }
                    //}
                    //else
                    //{
                    //    hubPlayer.TryOverridePosition(room.ApiRoom.Position + new Vector3(0f, 1.3f, 0f), Vector3.zero);
                    //}
                    hubPlayer.TryOverridePosition(pos + new Vector3(0f, 1.3f, 0f), Vector3.zero);
                });
            }
            catch (Exception e)
            {
                Log.Error($"Error on {nameof(DummyManager)}: {e} -- {e.Message}");
            }
        }

        public static ReferenceHub SpawnDummy(Vector3 pos, bool destroyOnDeath, string name, RoleTypeId role)
        {
            //Log.Debug("Spawn dummy!");
            try
            {

                #region Create Dummy

                var newPlayer =
                    UnityEngine.Object.Instantiate(NetworkManager.singleton.playerPrefab);
                int id = Dummies.Count;
                var fakeConnection = new FakeConnection(id++);

                var hubPlayer = newPlayer.GetComponent<ReferenceHub>();
                if (hubPlayer is null)
                {
                    newPlayer.AddComponent<ReferenceHub>();
                }
                if (hubPlayer.characterClassManager is null)
                {
                    hubPlayer.characterClassManager = new CharacterClassManager();
                }
                #endregion

                //Log.Debug("Adding dummy to the list of dummies");
                Dummies.Add(hubPlayer);
                //Log.Debug("Spawning dummy");
                NetworkServer.AddPlayerForConnection(fakeConnection, newPlayer);

                try
                {
                    //Log.Debug("Setting the UserId of the dummy");
                    AccessTools.Field(typeof(CharacterClassManager), "_privUserId").SetValue(hubPlayer.characterClassManager, $"Dummy-{id}@server");
                    if (destroyOnDeath)
                    {
                        DummiesToDestroyOnDeath.Add($"Dummy-{id}@server");
                    }
                    //hubPlayer.characterClassManager._privUserId = $"SCP-575-{id}@server";
                    AccessTools.Field(typeof(CharacterClassManager), "InstanceMode").SetValue(hubPlayer.characterClassManager, ClientInstanceMode.Unverified);
                    //hubPlayer.characterClassManager.InstanceMode = ClientInstanceMode.Unverified;
                }
                catch (Exception)
                {

                }

                hubPlayer.nicknameSync.ShownPlayerInfo &= ~PlayerInfoArea.Role;
                hubPlayer.nicknameSync.ViewRange = 100f;
                // SetNick it will always give an error but will apply it anyway.
                hubPlayer.nicknameSync.Network_myNickSync = name;



                try
                {
                    hubPlayer.roleManager.ServerSetRole(role, RoleChangeReason.RemoteAdmin);
                }
                catch (Exception e)
                {
                    Log.Error($"Error on {nameof(DummyManager)}: Error on set dummy role {e}");
                    return null;
                }

                hubPlayer.characterClassManager.GodMode = false;

                Timing.CallDelayed(0.3f, () =>
                {
                    //var room = victim.Room;

                    //if (room.Name == RoomName.Lcz173)
                    //{
                    //    hubPlayer.TryOverridePosition(room.ApiRoom.Position + new Vector3(0f, 13.5f, 0f), Vector3.zero);
                    //}
                    //else if (room.Name == RoomName.HczTestroom)
                    //{
                    //    if (DoorVariant.DoorsByRoom.TryGetValue(room, out var hashSet))
                    //    {
                    //        var door = hashSet.FirstOrDefault();
                    //        if (door != null) hubPlayer.TryOverridePosition(door.transform.position, Vector3.zero);
                    //    }
                    //}
                    //else
                    //{
                    //    hubPlayer.TryOverridePosition(room.ApiRoom.Position + new Vector3(0f, 1.3f, 0f), Vector3.zero);
                    //}
                    hubPlayer.TryOverridePosition(pos + new Vector3(0f, 1.3f, 0f), Vector3.zero);
                    hubPlayer.gameObject.transform.position = pos + new Vector3(0f, 1.3f, 0f);
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
            CustomReasonDamageHandler un = new CustomReasonDamageHandler("Decayed in Pocket Dimension", -1f);
            dummy.inventory.UserInventory.ReserveAmmo.Clear();
            InventoryInfo userInventory = dummy.inventory.UserInventory;
            while (userInventory.Items.Count > 0)
            {
                dummy.inventory.ServerRemoveItem(userInventory.Items.ElementAt(0).Key, null);
            }
            Timing.CallDelayed(0.3f, () =>
            {
                dummy.playerStats.DealDamage(un);
                //NetworkServer.Destroy(dummy.gameObject);
                //NetworkServer.Destroy(ragdoll.gameObject);
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
