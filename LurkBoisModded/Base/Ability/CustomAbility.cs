using UnityEngine;
using LurkBoisModded.Managers;
using PlayerRoles.Spectating;

namespace LurkBoisModded.Base.Ability
{
    public abstract class CustomAbility : MonoBehaviour
    {
        public ReferenceHub CurrentOwner { get; set; }

        public bool Ready { get; set; } = false;

        public abstract AbilityType AbilityType { get; }

        void Update()
        {
            
        }

        public virtual void OnStartSpectating(ReferenceHub spectator)
        {

        }

        public virtual void OnStopSpectating(ReferenceHub spectator)
        {

        }

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
