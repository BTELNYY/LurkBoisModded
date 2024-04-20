using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using PluginAPI.Core;
using PlayerStatsSystem;
using LurkBoisModded.Effects;
using InventorySystem.Items.Pickups;
using AdminToys;
using LurkBoisModded.Extensions;
using Mirror;
using PlayerRoles;
using Footprinting;

namespace LurkBoisModded.EnvriomentalHazards.Hazards.Fire
{
    public class FireHazard : BasicTimedHazard
    {
        public override float Duration => Config.CurrentConfig.MolotovConfiguration.FireDuration;

        public override HazardType HazardType => HazardType.Fire;

        public LightSourceToy LightSource;

        public GameObject LightSourceObject;

        public Team OwnerTeam = Team.Dead;

        public Footprint Owner;

        public override void Create()
        {
            base.Create();
            LightSourceObject = Utility.CreateAdminToy(AdminToyType.LightSource);
            LightSourceObject.transform.position = gameObject.transform.position;
            LightSource = LightSourceObject.GetComponent<LightSourceToy>();
            if(LightSource == null)
            {
                Log.Error("Can't get light source toy from light source!");
            }
            else
            {
                LightSource.NetworkLightColor = Config.CurrentConfig.FireConfig.FireColor.ConvertToColor();
                LightSource.NetworkLightRange = Config.CurrentConfig.MolotovConfiguration.FireRadius;
            }
        }

        public override void Destroy()
        {
            NetworkServer.Destroy(LightSourceObject);
            base.Destroy();
        }

        public void FixedUpdate()
        {
            if(LightSourceObject != null)
            {
                LightSourceObject.transform.position = gameObject.transform.position;
            }
            if(LightSource != null)
            {
                LightSource.NetworkPosition = gameObject.transform.position;
            }
            List<ReferenceHub> inRange = ReferenceHub.AllHubs.Where(x => Vector3.Distance(x.gameObject.transform.position, gameObject.transform.position) <= Config.CurrentConfig.MolotovConfiguration.FireRadius).ToList();
            foreach(ReferenceHub hub in inRange)
            {
                if(hub.GetTeam() == OwnerTeam && Owner.Hub != hub)
                {
                    continue;
                }
                hub.playerEffectsController.ChangeState<OnFire>(Config.CurrentConfig.MolotovConfiguration.FireDamageIntensity, Config.CurrentConfig.MolotovConfiguration.FireDuration);
            }
        }
    }
}
