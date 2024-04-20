using HarmonyLib;
using MEC;
using Mirror;
using PluginAPI.Core;
using RoundRestarting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace LurkBoisModded.Patches.ServerFix
{
    [HarmonyPatch(typeof(CustomLiteNetLib4MirrorTransport), nameof(CustomLiteNetLib4MirrorTransport.DelayConnections), MethodType.Setter)]
    public class DelayConnectionsPatch
    {
        static bool killProccess = false;

        public static void Prefix(bool value)
        {
            if(value)
            {
                Timing.CallDelayed(7f, () => 
                {
                    if (!killProccess)
                    {
                        return;
                    }
                    Log.Debug("Server is out of time. Killing.");
                    NetworkServer.SendToAll<RoundRestartMessage>(new RoundRestartMessage(RoundRestartType.FullRestart, 0f, 0, false, true), 0, false);
                    Application.Quit(0);
                });
                Log.Info("Frozen server killer started, server has a few seconds to allow connections or it will be killed.");
                killProccess = true;
            }
            else
            {
                Log.Info("Server is now allowing connections, aborting termination.");
                killProccess = false;
            }
        }
    }
}
