﻿using LurkBoisModded.Scripts;
using MapGeneration;
using PlayerRoles.Ragdolls;
using PlayerStatsSystem;
using UnityEngine;
using LurkBoisModded.Extensions;

namespace LurkBoisModded.EventHandlers.Map
{
    [EventHandler]
    public class RagdollHandler
    {
        public static void OnRagdollSpawn(BasicRagdoll ragdoll)
        {
            Vector3 vec = ragdoll.NetworkInfo.OwnerHub.transform.localScale;
            if(ragdoll.NetworkInfo.RoleType == PlayerRoles.RoleTypeId.Scp049)
            {
                return;
            }
            ragdoll.netIdentity.SetScale(vec);
        }

        public static void PocketRagdollHandle(BasicRagdoll ragdoll)
        {
            //Log.Debug("Ragdoll handler called!");
            DamageHandlerBase damageHandler = ragdoll.Info.Handler;
            UniversalDamageHandler un = damageHandler as UniversalDamageHandler;
            //Log.Debug("Ready to call delayed body handler!");
            if (un.TranslationId != 7)
            {
                //Log.Debug("Invalid translation ID!");
                return;
            }
            //Log.Debug("Adding to ragdoll queue!");
            if (RoomIdUtils.RoomAtPositionRaycasts(ragdoll.CenterPoint.position).Name != RoomName.Pocket)
            {
                //Log.Debug("Not in the pocket dimension!");
                return;
            }
            RagdollDropperScript dropperScript = ragdoll.gameObject.AddComponent<RagdollDropperScript>();
            float time = Random.Range(Plugin.GetConfig().Scp106PdConfig.Scp106PdDropDelayMin, Plugin.GetConfig().Scp106PdConfig.Scp106PdDropDelayMax);
            dropperScript.StartTimer(time, ragdoll);
        }
    }
}
