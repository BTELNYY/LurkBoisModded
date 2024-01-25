using AdminToys;
using Footprinting;
using Mirror;
using PlayerRoles;
using PluginAPI.Core.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Utils;

namespace LurkBoisModded.Scripts
{
    public class LandmineScript : MonoBehaviour
    {
        public GameObject PrimitiveTarget;

        private AdminToyBase _adminToyBase;

        public ReferenceHub OwnerHub;

        private Footprint _playerFootprint;

        public bool IsReady = false;

        public void Ready()
        {
            _adminToyBase = PrimitiveTarget.GetComponent<AdminToyBase>();
            _playerFootprint = new Footprint(OwnerHub);
            IsReady = true;
        }

        void Update()
        {
            if (!IsReady)
            {
                return;
            }
            _adminToyBase.NetworkPosition = gameObject.transform.position + new Vector3(0, 0.25f, 0f);
       }

        void FixedUpdate()
        {
            if (!IsReady)
            {
                return;
            }
            List<ReferenceHub> inRange = ReferenceHub.AllHubs.Where(x => Vector3.Distance(x.gameObject.transform.position, PrimitiveTarget.transform.position) <= Plugin.GetConfig().LandmineConfiguration.TriggerDistance).ToList();
            if(inRange.Count == 0)
            {
                return;
            }
            bool detonate = inRange.Any(x => x.GetTeam() != _playerFootprint.Hub.GetTeam());
            Vector3 newPos = gameObject.transform.position;
            newPos.y += 1f;
            NetworkServer.Destroy(PrimitiveTarget);
            ExplosionUtils.ServerExplode(newPos, _playerFootprint);
            NetworkServer.Destroy(gameObject);
        }
    }
}
