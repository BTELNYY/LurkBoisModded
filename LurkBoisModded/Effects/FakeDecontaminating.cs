using CustomPlayerEffects;
using Mirror;
using PlayerRoles;
using PlayerStatsSystem;
using Subtitles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LurkBoisModded.Effects
{
    public class FakeDecontaminating : TickingEffectBase
    {
        protected override void OnTick()
        {
            if (NetworkServer.active && base.Hub.roleManager.CurrentRole is IHealthbarRole healthbarRole)
            {
                float damage = healthbarRole.MaxHealth / 10f;
                DamageHandlerBase.CassieAnnouncement cassieAnnouncement = new DamageHandlerBase.CassieAnnouncement();
                cassieAnnouncement.Announcement = "LOST IN DECONTAMINATION SEQUENCE";
                cassieAnnouncement.SubtitleParts = new SubtitlePart[1]
                {
                new SubtitlePart(SubtitleType.LostInDecontamination, (string[])null)
                };
                DamageHandlerBase.CassieAnnouncement cassieAnnouncement2 = cassieAnnouncement;
                base.Hub.playerStats.DealDamage(new UniversalDamageHandler(damage, DeathTranslations.Decontamination, cassieAnnouncement2));
            }
        }
    }
}
