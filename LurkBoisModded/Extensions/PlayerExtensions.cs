﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using CentralAuth;
using Hints;
using Interactables.Interobjects;
using InventorySystem.Items;
using InventorySystem.Items.Firearms;
using InventorySystem.Items.Firearms.Attachments;
using MEC;
using Mirror;
using PlayerRoles;
using PlayerRoles.FirstPersonControl.NetworkMessages;
using PlayerRoles.FirstPersonControl;
using PluginAPI.Core;
using UnityEngine;
using RemoteAdmin;

namespace LurkBoisModded.Extensions
{
    public static class PlayerExtensions
    {
        public static List<Player> SelectByClass(this List<Player> players, RoleTypeId role)
        {
            List<Player> selectedPlayers = players.Where(x => x.Role == role && x.ReferenceHub.authManager.InstanceMode == ClientInstanceMode.ReadyClient).ToList();
            return selectedPlayers;
        }

        public static void SendHint(this Player target, string text, float duration = 3f)
        {
            target.ReferenceHub.hints.Show(new TextHint(text, new HintParameter[] { new StringHintParameter(string.Empty) }, null, duration));
        }

        public static bool AddFirearm(this Player target, ItemType type, byte ammo)
        {
            if (target.ReferenceHub.authManager.InstanceMode != ClientInstanceMode.ReadyClient)
            {
                return false;
            }
            if (!type.ToString().Contains("Gun"))
            {
                return false;
            }
            ItemBase item = target.AddItem(type);
            Timing.CallDelayed(0.25f, () =>
            {
                if (item is Firearm firearm)
                {
                    
                }
            });
            return true;
        }
        public static void ApplyAttachments(this Player ply)
        {
            var item = ply.Items.Where(i => i is Firearm);

            foreach (var fire in item)
            {
                if (fire is Firearm fireArm)
                {
                    if (AttachmentsServerHandler.PlayerPreferences.TryGetValue(ply.ReferenceHub, out var value) && value.TryGetValue(fireArm.ItemTypeId, out var value2))
                        fireArm.ApplyAttachmentsCode(value2, reValidate: true);
                }
            }
        }

        public static bool InElevator(this Player target)
        {
            List<ElevatorDoor> AllElevs = new List<ElevatorDoor>();
            if (ElevatorDoor.AllElevatorDoors.TryGetValue(ElevatorManager.ElevatorGroup.LczB01, out var list))
            {
                AllElevs.AddRange(list);
            }
            if (ElevatorDoor.AllElevatorDoors.TryGetValue(ElevatorManager.ElevatorGroup.LczA01, out var list1))
            {
                AllElevs.AddRange(list1);
            }
            if (ElevatorDoor.AllElevatorDoors.TryGetValue(ElevatorManager.ElevatorGroup.LczA02, out var list2))
            {
                AllElevs.AddRange(list2);
            }
            if (ElevatorDoor.AllElevatorDoors.TryGetValue(ElevatorManager.ElevatorGroup.LczB02, out var list3))
            {
                AllElevs.AddRange(list3);
            }
            if (ElevatorDoor.AllElevatorDoors.TryGetValue(ElevatorManager.ElevatorGroup.Scp049, out var list4))
            {
                AllElevs.AddRange(list4);
            }
            if (ElevatorDoor.AllElevatorDoors.TryGetValue(ElevatorManager.ElevatorGroup.Nuke, out var list5))
            {
                AllElevs.AddRange(list5);
            }
            if (ElevatorDoor.AllElevatorDoors.TryGetValue(ElevatorManager.ElevatorGroup.GateA, out var list6))
            {
                AllElevs.AddRange(list6);
            }
            if (ElevatorDoor.AllElevatorDoors.TryGetValue(ElevatorManager.ElevatorGroup.GateB, out var list7))
            {
                AllElevs.AddRange(list7);
            }
            foreach (var elevdoors in AllElevs)
            {
                ElevatorChamber chamber = elevdoors.TargetPanel.AssignedChamber;
                if (chamber.WorldspaceBounds.Contains(target.Position))
                {
                    return true;
                }
            }
            return false;
        }

        private static MethodInfo sendSpawnMessage;

        public static MethodInfo SendSpawnMessage => sendSpawnMessage = typeof(NetworkServer).GetMethod("SendSpawnMessage", BindingFlags.NonPublic | BindingFlags.Static);

        public static void SetScale(this Player target, Vector3 scale)
        {
            try
            {
                target.ReferenceHub.transform.localScale = scale;
                CharacterController controller = target.GameObject.GetComponent<CharacterController>();
                controller.stepOffset = scale.y + controller.radius * 2;
                foreach (ReferenceHub hub in ReferenceHub.AllHubs.Where(x => x.authManager.InstanceMode == ClientInstanceMode.ReadyClient))
                {
                    SendSpawnMessage?.Invoke(null, new object[] { target.ReferenceHub.networkIdentity, hub.connectionToClient });
                }
            }
            catch (Exception exception)
            {
                Log.Error($"Error: {exception}");
            }
        }

        public static void SetScale(this Player target, float scale)
        {
            try
            {
                target.ReferenceHub.transform.localScale = new Vector3(scale, scale, scale);
                CharacterController controller = target.GameObject.GetComponent<CharacterController>();
                controller.stepOffset = scale + controller.radius * 2;
                foreach (ReferenceHub hub in ReferenceHub.AllHubs.Where(x => x.authManager.InstanceMode == ClientInstanceMode.ReadyClient))
                    SendSpawnMessage?.Invoke(null, new object[] { target.ReferenceHub.networkIdentity, hub.connectionToClient });
            }
            catch (Exception exception)
            {
                Log.Error($"Error: {exception}");
            }
        }

        public static ValueTuple<ushort, ushort> ToClientUShorts(this Quaternion rotation)
        {
            float num = -rotation.eulerAngles.x;
            if (num < -90f)
            {
                num += 360f;
            }
            else if (num > 270f)
            {
                num -= 360f;
            }
            double num2 = (double)Mathf.Clamp(rotation.eulerAngles.y, 0f, 360f);
            float num3 = Mathf.Clamp(num, -88f, 88f) + 88f;
            return new ValueTuple<ushort, ushort>((ushort)Math.Round(num2 * (double)182.04167f), (ushort)Math.Round((double)(num3 * 372.35794f)));
        }

        public static void SetRotation(this Player player, Vector3 forward)
        {
            player.ReferenceHub.SetHubRotation(forward);
        }

        public static void SetHubRotation(this Player player, Quaternion rotation)
        {
            player.ReferenceHub.SetHubRotation(rotation);
        }

        public static void SetRotation(this Player player, ushort horizontal, ushort vertical)
        {
            player.ReferenceHub.SetHubRotation(horizontal, vertical);
        }
    }
}
