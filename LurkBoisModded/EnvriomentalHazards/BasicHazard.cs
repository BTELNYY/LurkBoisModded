using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Text;
using System.Threading.Tasks;

namespace LurkBoisModded.EnvriomentalHazards
{
    public abstract class BasicHazard : MonoBehaviour
    {
        public abstract HazardType HazardType { get; }

        public ushort ItemSerial { get; private set; } = 0;

        void Start()
        {
            ItemSerial = HazardManager.GetNextSerial();
        }

        public virtual void Create()
        {

        }

        public virtual void Destroy()
        {
            GameObject.Destroy(gameObject);
        }
    }
}
