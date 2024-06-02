using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
using Mirror;
using PlayerRoles.FirstPersonControl;
using UnityEngine;

namespace LurkBoisModded.Patches.ServerFix
{
    [HarmonyPatch(typeof(FpcMotor), "DesiredMove", MethodType.Getter)]
    public class FpcMotorPatch
    {
        static readonly PropertyInfo Position = AccessTools.Property(typeof(FpcMotor), "Position");

        static readonly FieldInfo MainModule = AccessTools.Field(typeof(FpcMotor), "MainModule");

        static readonly FieldInfo Hub = AccessTools.Field(typeof(FpcMotor), "Hub");

        static readonly FieldInfo _defaultStepOffset = AccessTools.Field(typeof(FpcMotor), "_defaultStepOffset");

        static readonly FieldInfo _defaultHeight = AccessTools.Field(typeof(FpcMotor), "_defaultHeight");

        static readonly FieldInfo _lastMaxSpeed = AccessTools.Field(typeof(FpcMotor), "_lastMaxSpeed");

        static readonly FieldInfo _lastOverrideTime = AccessTools.Field(typeof(FpcMotor), "_lastOverrideTime");

        public static bool Prefix(FpcMotor __instance, ref Vector3 __result)
        {
            Vector3 position = __instance.ReceivedPosition.Position;
            Vector3 instancePosition = (Vector3)Position.GetValue(__instance);
            Vector3 v = position - instancePosition;
            FirstPersonMovementModule mainModule = (FirstPersonMovementModule)MainModule.GetValue(__instance);
            ReferenceHub hub = (ReferenceHub)Hub.GetValue(__instance);
            if (NetworkServer.active)
            {
                
               mainModule.CharController.stepOffset = Mathf.Min((float)_defaultStepOffset.GetValue(__instance) + Mathf.Clamp(v.y * 1.6f, 0.0f, 0.35f * mainModule.JumpSpeed), (float)_defaultHeight.GetValue(__instance) * hub.gameObject.transform.localScale.y);
                //Log.Info($"step offset {__instance.Hub.nicknameSync.MyNick}: {__instance.MainModule.CharController.stepOffset}");
                if (mainModule.Noclip.RecentlyActive)
                {
                    Position.SetValue(__instance, position);
                    __result = Vector3.zero;
                    return false;
                }
            }
            float num = v.MagnitudeIgnoreY();
            if ((double)num < 0.029999999329447746)
            {
                __result = Vector3.zero;
                return false;
            }
            if ((double)(num - 0.5f) < (float)_lastMaxSpeed.GetValue(__instance) && (double)Mathf.Abs(v.y) < (double)Mathf.Max(mainModule.JumpSpeed, Mathf.Abs(__instance.MoveDirection.y)))
            {
                __result = !NetworkServer.active ? v : new Vector3(v.x, 0.0f, v.z);
                return false;
            }
            if (NetworkServer.active)
            {
                if (((Stopwatch)_lastOverrideTime.GetValue(__instance)).Elapsed.TotalSeconds > 0.40000000596046448)
                {
                    mainModule.ServerOverridePosition(instancePosition, Vector3.zero);
                }
            }
            else
            {
                Position.SetValue(__instance, position);
            }
            __result = Vector3.zero;
            return false;
        }
    }
}
