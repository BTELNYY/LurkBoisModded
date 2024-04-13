using Mirror;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using PluginAPI.Core;
using UnityEngine;

namespace LurkBoisModded.Extensions
{
    public static class MirrorExtensions
    {
        private static MethodInfo sendSpawnMessage;

        public static MethodInfo SendSpawnMessage => sendSpawnMessage = typeof(NetworkServer).GetMethod("SendSpawnMessage", BindingFlags.NonPublic | BindingFlags.Static);

        public static void SetScale(this NetworkIdentity netId, float scale)
        {
            try
            {
                netId.gameObject.transform.localScale = new Vector3(scale, scale, scale);
                foreach (ReferenceHub hub in ReferenceHub.AllHubs)
                {
                    SendSpawnMessage?.Invoke(null, new object[] { netId, hub.connectionToClient });
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
            }
        }

        public static void SetScale(this NetworkIdentity netId, Vector3 scale)
        {
            try
            {
                netId.gameObject.transform.localScale = scale;
                foreach (ReferenceHub hub in ReferenceHub.AllHubs)
                {
                    SendSpawnMessage?.Invoke(null, new object[] { netId, hub.connectionToClient });
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
            }
        }

        public static void Respawn(this NetworkIdentity netId)
        {
            try
            {
                foreach (ReferenceHub hub in ReferenceHub.AllHubs)
                {
                    SendSpawnMessage?.Invoke(null, new object[] { netId, hub.connectionToClient });
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
            }
        }
    }
}
