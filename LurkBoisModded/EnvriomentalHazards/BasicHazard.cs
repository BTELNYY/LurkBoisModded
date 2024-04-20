using Mirror;
using UnityEngine;

namespace LurkBoisModded.EnvriomentalHazards
{
    public abstract class BasicHazard : MonoBehaviour
    {
        public abstract HazardType HazardType { get; }

        public ushort HazardSerial { get; private set; } = 0;

        void Start()
        {
            HazardSerial = HazardManager.GetNextSerial();
        }

        public virtual void Create()
        {

        }

        public virtual void Destroy()
        {
            NetworkServer.Destroy(gameObject);
            Destroy(gameObject);
        }
    }
}
