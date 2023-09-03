using UnityEngine;

namespace LurkBoisModded.Base
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
