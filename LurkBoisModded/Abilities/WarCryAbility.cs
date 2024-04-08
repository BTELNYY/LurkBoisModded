using LurkBoisModded.Base.Ability;
using LurkBoisModded.Base;
using PlayerRoles;
using PlayerStatsSystem;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using LurkBoisModded.Managers;
using System.Text;
using System.Threading.Tasks;

namespace LurkBoisModded.Abilities
{
    public class WarCryAbility : CustomCooldownAbility
    {
        public override AbilityType AbilityType => AbilityType.WarCry;

        public override float Cooldown => Plugin.GetConfig().AbilityConfig.InspireAbilityConfig.Cooldown;

        public override void OnTrigger()
        {
            base.OnTrigger();
            if (!CooldownReady)
            {
                return;
            }
            List<ReferenceHub> sameTeamHubs = ReferenceHub.AllHubs.Where(x => x.GetTeam() == CurrentHub.GetTeam()).ToList();
            List<ReferenceHub> affectedHubs = sameTeamHubs.Where(x => Vector3.Distance(x.transform.position, CurrentHub.transform.position) <= Plugin.GetConfig().AbilityConfig.InspireAbilityConfig.Range).ToList();
            if (affectedHubs.Count <= 1)
            {
                ResetCooldown();
                CurrentHub.SendHint(Plugin.GetConfig().AbilityConfig.WarCryAbilityConfig.NoTargetsMessage);
                return;
            }
            foreach (ReferenceHub hub in affectedHubs)
            {
                foreach(EffectDefinition def in Plugin.GetConfig().AbilityConfig.WarCryAbilityConfig.Effects)
                {
                    if(!hub.playerEffectsController.TryGetEffect(def.Name, out var effect))
                    {
                        Debug.LogWarning("Failed to find effect. Name: " + def.Name);
                    }
                    else
                    {
                        hub.playerEffectsController.ChangeState(def.Name, def.Intensity, def.Duration);
                    }
                }
                hub.SendHint(Plugin.GetConfig().AbilityConfig.WarCryAbilityConfig.WarCryEffectYou.Replace("{playername}", CurrentHub.nicknameSync.MyNick));
            }
            CurrentHub.SendHint(Plugin.GetConfig().AbilityConfig.WarCryAbilityConfig.WarCryHeard.Replace("{count}", affectedHubs.Count.ToString()));
        }
    }
}
