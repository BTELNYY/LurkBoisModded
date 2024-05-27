using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PluginAPI.Enums;
using PluginAPI.Events;
using PluginAPI.Core.Attributes;
using MEC;
using PluginAPI.Core;
using Interactables.Interobjects.DoorUtils;
using LurkBoisModded.Extensions;
using CustomPlayerEffects;
using Interactables.Interobjects;
using System.Security.Policy;
using UnityEngine;
using LurkBoisModded.Scripts;

namespace LurkBoisModded.EventHandlers.Map
{
    [EventHandler]
    public class HCZDeconHandler
    {
        GameObject decontamObject;

        [PluginEvent(ServerEventType.RoundStart)]
        public void RoundStart(RoundStartEvent ev)
        {
            decontamObject = new GameObject("HCZ Decontam Object");
            decontamObject.AddComponent<HCZDeconTimedEventHandler>();
        }

        [PluginEvent(ServerEventType.RoundEnd)]
        public void RoundEnd(RoundEndEvent ev)
        {
            if(decontamObject != null)
            {
                GameObject.Destroy(decontamObject);
            }
        }
    }
}
