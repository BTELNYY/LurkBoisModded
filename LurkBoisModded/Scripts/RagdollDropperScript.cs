using UnityEngine;
using System;
using System.Collections;
using MEC;
using PluginAPI.Core;
using System.Collections.Generic;
using LurkBoisModded.EventHandlers;
using System.Linq;
using MapGeneration;
using PlayerRoles.Ragdolls;

namespace LurkBoisModded.Scripts
{
    public class RagdollDropperScript : MonoBehaviour
    {
        public float Time;
        public BasicRagdoll Target;

        public void StartTimer(float time, BasicRagdoll target)
        {
            Log.Debug($"Starting Timer! Delay: {time}, Target: {target.NetworkInfo.Nickname}");
            Time = time;
            Target = target;
            StartCoroutine(BodyDrop());
        }

        IEnumerator BodyDrop()
        {
            yield return new WaitForSeconds(Time);
            BasicRagdoll targetRagdoll = Target;
            List<Player> players = Player.GetPlayers();
            List<Player> checkedPlayers = players.Where(p => !p.IsSCP && p.IsAlive && !p.InElevator() && RoomIdUtils.RoomAtPositionRaycasts(p.Position) != null && RoomIdUtils.RoomAtPositionRaycasts(p.Position).Name != RoomName.Pocket).ToList();
            if (checkedPlayers.Count <= 0)
            {
                yield break;
            }
            Player chosenPlayer = checkedPlayers.RandomItem();
            Vector3 raycastPos = chosenPlayer.ReferenceHub.transform.position;
            Vector3 teleportPos = chosenPlayer.ReferenceHub.transform.position;
            teleportPos.y += 10f;
            Timing.CallDelayed(1f, () =>
            {
                targetRagdoll.gameObject.transform.position = teleportPos;
                targetRagdoll.netIdentity.Respawn();
            });
        }
    }
}