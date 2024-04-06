using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using PluginAPI.Core;
using PlayerStatsSystem;
using LurkBoisModded.Effects;

namespace LurkBoisModded.EnvriomentalHazards.Hazards.Fire
{
    public class FireHazard : BasicTimedHazard
    {
        public override float Duration => Config.CurrentConfig.MolotovConfiguration.FireDuration;

        public override HazardType HazardType => HazardType.Fire;

        private SphereCollider collider;

        public override void Create()
        {
            base.Create();
            collider = gameObject.AddComponent<SphereCollider>();
            collider.isTrigger = true;
            collider.radius = Config.CurrentConfig.MolotovConfiguration.FireRadius;
        }

        public override void Destroy()
        {
            Destroy(collider);
            base.Destroy();
        }

        public void OnCollisionEnter(Collision collision)
        {
            Log.Debug(collision.gameObject.name);
            Player player = Player.Get(collision.gameObject);
            if(player == null)
            {
                return;
            }
            CustomReasonDamageHandler handler = new CustomReasonDamageHandler(Config.CurrentConfig.FireConfig.DeathReason, Config.CurrentConfig.MolotovConfiguration.FireFirstTickDamage);
            player.Damage(handler);
            player.ReferenceHub.playerEffectsController.ChangeState<OnFire>(Config.CurrentConfig.MolotovConfiguration.FireDamageIntensity, Config.CurrentConfig.MolotovConfiguration.FireDuration);
        }

        public void OnCollisionStay(Collision collision)
        {
            Log.Debug(collision.gameObject.name);
            Player player = Player.Get(collision.gameObject);
            if (player == null)
            {
                return;
            }
            player.ReferenceHub.playerEffectsController.ChangeState<OnFire>(Config.CurrentConfig.MolotovConfiguration.FireDamageIntensity, Config.CurrentConfig.MolotovConfiguration.FireDuration);
        }
    }
}
