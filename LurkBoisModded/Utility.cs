using Interactables.Interobjects.DoorUtils;
using Mirror;
using PlayerRoles;
using PlayerStatsSystem;
using PluginAPI.Roles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using PluginAPI;
using PluginAPI.Core;
using AdminToys;

namespace LurkBoisModded
{
    public class Utility
    {
        public static List<DoorVariant> SpawnedDoors = new List<DoorVariant>();

        public static BasicRagdoll MoveRagdoll(BasicRagdoll oldRagdoll, Vector3 position)
        {
            try
            {
                if (!NetworkServer.active || oldRagdoll.NetworkInfo.OwnerHub == null)
                {
                    return null;
                }

                GameObject gameObject = UnityEngine.Object.Instantiate(oldRagdoll.gameObject);
                if (gameObject.TryGetComponent<BasicRagdoll>(out var component))
                {
                    Transform transform = oldRagdoll.gameObject.transform;
                    component.NetworkInfo = new RagdollData(oldRagdoll.NetworkInfo.OwnerHub, oldRagdoll.NetworkInfo.Handler, position, transform.localRotation);
                }
                else
                {
                    component = null;
                }

                NetworkServer.Spawn(gameObject);
                return component;
            }
            catch (Exception ex)
            {
                Debug.LogError(ex);
                return null;
            }
        }

        public static DoorVariant CreateDoor(Vector3 position, Vector3 rotation, Vector3 scale, DoorType doorType = DoorType.LCZ, KeycardPermissions[] keycardPermissions = null)
        {
            MapGeneration.DoorSpawnpoint prefab = null;
            switch (doorType)
            {
                case DoorType.LCZ: prefab = UnityEngine.Object.FindObjectsOfType<MapGeneration.DoorSpawnpoint>().First(x => x.TargetPrefab.name.Contains("LCZ BreakableDoor")); break;
                case DoorType.HCZ: prefab = UnityEngine.Object.FindObjectsOfType<MapGeneration.DoorSpawnpoint>().First(x => x.TargetPrefab.name.Contains("HCZ BreakableDoor")); break;
                case DoorType.EZ: prefab = UnityEngine.Object.FindObjectsOfType<MapGeneration.DoorSpawnpoint>().First(x => x.TargetPrefab.name.Contains("EZ BreakableDoor")); break;
            }
            try
            {
                DoorPermissions perms = new DoorPermissions();
                perms.RequiredPermissions.SetAll(false);
                if (keycardPermissions == null)
                {
                    perms.RequiredPermissions.SetFlag(KeycardPermissions.None, true);
                }
                else
                {
                    foreach (KeycardPermissions perm in keycardPermissions)
                    {
                        perms.RequiredPermissions |= perm;
                    }
                }
                var door = UnityEngine.Object.Instantiate(prefab.TargetPrefab, position, Quaternion.Euler(rotation));
                door.transform.localScale = scale;
                door.RequiredPermissions = perms;
                SpawnedDoors.Add(door);
                NetworkServer.Spawn(door.gameObject);
                return door;
            }
            catch(Exception ex)
            {
                Log.Error(ex.ToString());
                return null;
            }
        }
        public static DoorVariant CreateDoor(Vector3 position, Quaternion rotation, Vector3 scale, DoorType doorType = DoorType.LCZ, KeycardPermissions[] keycardPermissions = null)
        {
            MapGeneration.DoorSpawnpoint prefab = null;
            switch (doorType)
            {
                case DoorType.LCZ: prefab = UnityEngine.Object.FindObjectsOfType<MapGeneration.DoorSpawnpoint>().First(x => x.TargetPrefab.name.Contains("LCZ BreakableDoor")); break;
                case DoorType.HCZ: prefab = UnityEngine.Object.FindObjectsOfType<MapGeneration.DoorSpawnpoint>().First(x => x.TargetPrefab.name.Contains("HCZ BreakableDoor")); break;
                case DoorType.EZ: prefab = UnityEngine.Object.FindObjectsOfType<MapGeneration.DoorSpawnpoint>().First(x => x.TargetPrefab.name.Contains("EZ BreakableDoor")); break;
            }
            try
            {
                DoorPermissions perms = new DoorPermissions();
                perms.RequiredPermissions.SetAll(false);
                if (keycardPermissions == null)
                {
                    perms.RequiredPermissions.SetFlag(KeycardPermissions.None, true);
                }
                else
                {
                    foreach (KeycardPermissions perm in keycardPermissions)
                    {
                        perms.RequiredPermissions |= perm;
                    }
                }
                var door = UnityEngine.Object.Instantiate(prefab.TargetPrefab, position, rotation);
                door.transform.localScale = scale;
                door.RequiredPermissions = perms;
                SpawnedDoors.Add(door);
                NetworkServer.Spawn(door.gameObject);
                return door;
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
                return null;
            }
        }
        
        public static bool TryGetDoorByName(string name, out DoorVariant door)
        {
            foreach(DoorVariant d in DoorVariant.AllDoors)
            {
                if(d.gameObject.TryGetComponent(out DoorNametagExtension extension))
                {
                    if(extension.GetName == name)
                    {
                        door = d;
                        return true;
                    }
                }
            }
            door = null;
            return false;
        }

        public static bool TryGetAdminToyByName(string name, out AdminToyBase adminToyBase)
        {
            List<GameObject> adminToys = NetworkClient.prefabs.Values.Where(x => x.TryGetComponent<AdminToyBase>(out AdminToyBase _)).ToList();
            foreach (AdminToyBase toy in adminToys.Select(x => x.GetComponent<AdminToyBase>()))
            {
                if(toy.CommandName == name)
                {
                    adminToyBase = toy;
                    return true;
                }
            }
            adminToyBase = null;
            return false;
        }

        public static List<Player> GetPlayersByRole(RoleTypeId role)
        {
            List<Player> result = new List<Player>();
            foreach(ReferenceHub hub in ReferenceHub.AllHubs)
            {
                if(hub.roleManager.CurrentRole.RoleTypeId == role && hub.characterClassManager.InstanceMode == ClientInstanceMode.ReadyClient)
                {
                    result.Add(Player.Get(hub));
                }
            }
            return result;
        }
    }

    public enum DoorType
    {
        LCZ,
        HCZ,
        EZ
    }
}
