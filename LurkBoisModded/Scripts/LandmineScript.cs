using AdminToys;
using Footprinting;
using InventorySystem.Disarming;
using LurkBoisModded.Extensions;
using MEC;
using Mirror;
using PlayerRoles;
using PluginAPI.Core;
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

        private LightSourceToy _adminToyBase;

        public ReferenceHub OwnerHub;

        private Footprint _playerFootprint;

        private Team _placedTeam = Team.Dead;

        public bool IsReady = false;

        public bool DoFriendlyFire
        {
            get
            {
                return Server.FriendlyFire;
            }
        }

        public void DestroySelf()
        {
            NetworkServer.Destroy(PrimitiveTarget);
            IsReady = false;
            Destroy(this);
        }

        public void Ready()
        {
            _adminToyBase = PrimitiveTarget.GetComponent<LightSourceToy>();
            _adminToyBase.NetworkScale = new Vector3(1, 1, 1);
            _adminToyBase.NetworkLightRange = Config.CurrentConfig.LandmineConfiguration.LightRange;
            _adminToyBase.NetworkLightIntensity = Config.CurrentConfig.LandmineConfiguration.LightIntensity;
            _adminToyBase.NetworkLightColor = Config.CurrentConfig.LandmineConfiguration.UnarmedColor.ConvertToColor();
            _playerFootprint = new Footprint(OwnerHub);
            _placedTeam = OwnerHub.GetTeam();
            Timing.CallDelayed(Config.CurrentConfig.LandmineConfiguration.ArmTime, () => 
            {
                _adminToyBase.NetworkLightColor = Config.CurrentConfig.LandmineConfiguration.ArmedColor.ConvertToColor();
                IsReady = true;
            });
        }

        void FixedUpdate()
        {
            PrimitiveTarget.transform.position = gameObject.transform.position;
            _adminToyBase.NetworkPosition = gameObject.transform.position;
        }

        void Update()
        {
            if (!IsReady)
            {
                return;
            }
            List<ReferenceHub> inRange = ReferenceHub.AllHubs.Where(x => Vector3.Distance(x.gameObject.transform.position, gameObject.transform.position) <= Plugin.GetConfig().LandmineConfiguration.TriggerDistance).ToList();
            if (inRange.Count == 0)
            {
                return;
            }
            bool detonate = false;
            if (Server.FriendlyFire)
            {
                detonate = true;
            }
            else
            {
                detonate = inRange.Any(x => x.GetTeam() != _placedTeam);
            }
            if(!detonate)
            {
                return;
            }
            _adminToyBase.NetworkLightColor = Color.white;
            Timing.CallDelayed(Config.CurrentConfig.LandmineConfiguration.TimeUntilExplosion, () => 
            {
                Vector3 newPos = gameObject.transform.position;
                newPos.y += 0.25f;
                NetworkServer.Destroy(PrimitiveTarget);
                ExplosionUtils.ServerExplode(newPos, _playerFootprint);
                NetworkServer.Destroy(gameObject);
            });
        }
    }
}
