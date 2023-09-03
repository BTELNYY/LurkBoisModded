using PluginAPI.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PluginAPI.Enums;
using PluginAPI.Core.Attributes;
using UnityEngine;
using PluginAPI.Core;
using MapGeneration;
using MEC;
using Mirror;
using PlayerStatsSystem;
using PlayerRoles.Ragdolls;
using LurkBoisModded.Scripts;

namespace LurkBoisModded.EventHandlers
{
    public class RagdollHandler
    {
        public static void OnRagdollSpawn(BasicRagdoll ragdoll)
        {
            Vector3 vec = ragdoll.NetworkInfo.OwnerHub.transform.localScale;
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
            float time = UnityEngine.Random.Range(Plugin.GetConfig().Scp106PdConfig.Scp106PdDropDelayMin, Plugin.GetConfig().Scp106PdConfig.Scp106PdDropDelayMax);
            dropperScript.StartTimer(time, ragdoll);
        }
    }
}
