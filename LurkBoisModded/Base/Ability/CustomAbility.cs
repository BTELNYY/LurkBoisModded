using UnityEngine;
using LurkBoisModded.Managers;

namespace LurkBoisModded.Base.Ability
{
    public abstract class CustomAbility : MonoBehaviour
    {
        public ReferenceHub CurrentHub;
        public bool Ready { get; set; } = false;

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

        public virtual void OnRemoved()
        {
            Ready = false;
        }
    }
}
