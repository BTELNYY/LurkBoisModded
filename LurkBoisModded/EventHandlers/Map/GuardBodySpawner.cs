using Interactables.Interobjects.DoorUtils;
using InventorySystem.Items.Firearms;
using InventorySystem.Items;
using InventorySystem;
using LurkBoisModded.Managers;
using MapGeneration;
using MEC;
using Mirror;
using PlayerRoles.FirstPersonControl;
using PlayerRoles;
using PluginAPI.Core.Attributes;
using PluginAPI.Core;
using PluginAPI.Enums;
using PluginAPI.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using LurkBoisModded.Extensions;

namespace LurkBoisModded.EventHandlers.Map
{
    [EventHandler]
    public class GuardBodySpawner
    {
        [PluginEvent(ServerEventType.RoundStart)]
        public void SpawnRagdollsEvent(RoundStartEvent ev)
        {
            Timing.CallDelayed(0.5f, () =>
            {
                SpawnRagdolls();
            });
        }

        public static void SpawnRagdolls()
        {
            if (!Config.CurrentConfig.FacilityConfig.GuardBodyConfig.ShouldGuardCorpsesSpawn)
            {
                return;
            }
            Log.Info("Spawning guard bodies!");
            RoundSummary.RoundLock = true;
            List<RoomName> allowedRooms = Config.CurrentConfig.FacilityConfig.GuardBodyConfig.GuardCorpseSpawnableRooms;
            List<string> npcNames = Config.CurrentConfig.NPCConfig.Names;
            List<string> npcDeathReasons = Config.CurrentConfig.NPCConfig.MemeDeathReasons;
            for (int i = 0; i < Config.CurrentConfig.FacilityConfig.GuardBodyConfig.GuardCorpseAmount; i++)
            {
                try
                {
                    RoomName chosenRoom = allowedRooms.GetRandomItem();
                    if (allowedRooms.Count == 0)
                    {
                        continue;
                    }
                    allowedRooms.Remove(chosenRoom);
                    HashSet<RoomIdentifier> rooms = RoomIdUtils.FindRooms(chosenRoom, FacilityZone.None, RoomShape.Undefined);
                    if (rooms != null)
                    {
                        RoomIdentifier room = rooms.GetRandomItem();
                        Log.Debug("Chosen room: " + room.Name.ToString());
                        HashSet<DoorVariant> doors = DoorVariant.DoorsByRoom[room];
                        DoorVariant door = doors.GetRandomItem();
                        Log.Debug("Chosen door: " + door.name);
                        Vector3 doorPosition = door.transform.position;
                        doorPosition.y += 1f;
                        string name = npcNames.GetRandomItem();
                        if (npcNames.Count == 0)
                        {
                            Log.Warning("Ran out of names for NPCs!");
                            name = "Ran out of names!";
                        }
                        else
                        {
                            npcNames.Remove(name);
                        }
                        name += " (NPC)";
                        ReferenceHub dummyHub = DummyManager.SpawnDummy(doorPosition, false, name, RoleTypeId.FacilityGuard);
                        dummyHub.inventory.UserInventory.ReserveAmmo.Clear();
                        InventoryInfo userInventory = dummyHub.inventory.UserInventory;
                        while (userInventory.Items.Count > 0)
                        {
                            dummyHub.inventory.ServerRemoveItem(userInventory.Items.ElementAt(0).Key, null);
                        }
                        //Log.Debug("Dummy Spawned");
                        if (room.Zone == FacilityZone.HeavyContainment)
                        {
                            //Log.Debug("Adding Items! (HCZ)");
                            foreach (ItemType item in Config.CurrentConfig.FacilityConfig.GuardBodyConfig.HczGuardCorpseContents.Keys)
                            {
                                int Amount = Config.CurrentConfig.FacilityConfig.GuardBodyConfig.HczGuardCorpseContents[item];
                                if (item.ToString().Contains("Gun"))
                                {
                                    ItemBase itemBase = dummyHub.inventory.ServerAddItem(item);
                                    Firearm firearm = (Firearm)itemBase;
                                    FirearmStatus status = new FirearmStatus((byte)Amount, firearm.Status.Flags, firearm.Status.Attachments);
                                    firearm.Status = status;
                                    continue;
                                }
                                if (item.ToString().Contains("Ammo"))
                                {
                                    dummyHub.inventory.ServerAddAmmo(item, Amount);
                                    continue;
                                }
                                for (int y = 0; y < Amount; y++)
                                {
                                    dummyHub.inventory.ServerAddItem(item);
                                }
                            }
                        }
                        else
                        {
                            //Log.Debug("Adding items! (EZ)");
                            foreach (ItemType item in Config.CurrentConfig.FacilityConfig.GuardBodyConfig.EzGuardCorpseContents.Keys)
                            {
                                int Amount = Config.CurrentConfig.FacilityConfig.GuardBodyConfig.EzGuardCorpseContents[item];
                                if (item.ToString().Contains("Gun"))
                                {
                                    ItemBase itemBase = dummyHub.inventory.ServerAddItem(item);
                                    Firearm firearm = (Firearm)itemBase;
                                    FirearmStatus status = new FirearmStatus((byte)Amount, firearm.Status.Flags, firearm.Status.Attachments);
                                    firearm.Status = status;
                                    //Log.Debug("Added firearm");
                                    continue;
                                }
                                if (item.ToString().Contains("Ammo"))
                                {
                                    //Log.Debug("Added Ammo");
                                    dummyHub.inventory.ServerAddAmmo(item, Amount);
                                    continue;
                                }
                                for (int y = 0; y < Amount; y++)
                                {
                                    //Log.Debug("Added item");
                                    dummyHub.inventory.ServerAddItem(item);
                                }
                            }
                        }
                        string deathReason = npcDeathReasons.GetRandomItem();
                        if (npcDeathReasons.Count == 0)
                        {
                            Log.Warning("Ran out of NPC death reasons!");
                            deathReason = "Ran out of death reasons.";
                        }
                        else
                        {
                            npcDeathReasons.Remove(deathReason);
                        }
                        dummyHub.TryOverridePosition(doorPosition, Vector3.up);
                        //Log.Debug("Killing NPC!");
                        Player npc = Player.Get(dummyHub);
                        try
                        {
                            npc.Kill(deathReason);
                        }
                        catch (Exception)
                        {
                            //Log.Error(ex.ToString());
                        }
                        //Log.Debug("Despawning NPC!");
                        //npc.Kick(null, "Despawn");
                        NetworkServer.Destroy(dummyHub.gameObject);
                        NetworkServer.Destroy(npc.GameObject);
                    }
                    else
                    {
                        Log.Warning("Can't find room! Room: " + chosenRoom.ToString());
                    }
                }
                catch (Exception ex)
                {
                    Log.Error(ex.ToString());
                    RoundSummary.RoundLock = false;
                }
            }
            RoundSummary.RoundLock = false;
        }
    }
}
