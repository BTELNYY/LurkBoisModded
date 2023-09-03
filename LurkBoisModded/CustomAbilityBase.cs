using PluginAPI.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace LurkBoisModded
{
    public abstract class CustomAbilityBase : MonoBehaviour
    {
        public ReferenceHub CurrentHub;
        public bool Ready = false;

        public abstract AbilityType AbilityType { get; }

        public virtual void OnTrigger()
        {
            if (!Ready)
            {
                return;
            }
        }

        public virtual void OnFinishSetup()
        {
            Ready = true;
        }
    }

    public enum AbilityType
    {
        ProximityChat,
        RemoteExplosive,
        Inspire,
    }
}
