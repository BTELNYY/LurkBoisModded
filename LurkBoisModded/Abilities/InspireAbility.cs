using PlayerRoles;
using PlayerStatsSystem;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using LurkBoisModded.Extensions;
using LurkBoisModded.Base;
using LurkBoisModded.Base.Ability;
using LurkBoisModded.Managers;

namespace LurkBoisModded.Abilities
{
    public class InspireAbility : CustomCooldownAbility
    {
        public override AbilityType AbilityType => AbilityType.Inspire;

        public override float Cooldown => Plugin.GetConfig().AbilityConfig.InspireAbilityConfig.Cooldown;

        public override void OnTrigger()
        {
            base.OnTrigger();
            if (!CooldownReady)
            {
                return;
            }
            List<ReferenceHub> sameTeamHubs = ReferenceHub.AllHubs.Where(x => x.GetTeam() == CurrentOwner.GetTeam()).ToList();
            List<ReferenceHub> affectedHubs = sameTeamHubs.Where(x => Vector3.Distance(x.transform.position, CurrentOwner.transform.position) <= Plugin.GetConfig().AbilityConfig.InspireAbilityConfig.Range).ToList();
            if(affectedHubs.Count <= 1)
            {
                ResetCooldown();
                CurrentOwner.SendHint(Plugin.GetConfig().AbilityConfig.InspireAbilityConfig.NoTargetsMessage);
                return;
            }
            foreach(ReferenceHub hub in affectedHubs)
            {
                if(!hub.playerStats.TryGetModule<AhpStat>(out AhpStat stat))
                {
                    continue;
                }
                stat.ServerAddProcess(Plugin.GetConfig().AbilityConfig.InspireAbilityConfig.AhpGranted);
                hub.SendHint(Plugin.GetConfig().AbilityConfig.InspireAbilityConfig.Inspired.Replace("{playername}", CurrentOwner.nicknameSync.MyNick));
            }
            CurrentOwner.SendHint(Plugin.GetConfig().AbilityConfig.InspireAbilityConfig.YouInspired.Replace("{count}", affectedHubs.Count.ToString()));
        }
    }
}
