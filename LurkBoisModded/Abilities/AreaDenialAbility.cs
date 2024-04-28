using PluginAPI.Core;
using LurkBoisModded.Base.Ability;
using LurkBoisModded.Base.CustomItems;
using LurkBoisModded.Base;
using LurkBoisModded.Extensions;
using LurkBoisModded.Managers;
using MapGeneration;
using MEC;
using PlayerRoles;
using System.Collections.Generic;
using System.Linq;

namespace LurkBoisModded.Abilities
{
    public class AreaDenialAbility : CustomCooldownAbility, IRequiredItemAbility
    {
        public override AbilityType AbilityType => AbilityType.AreaDenialAbility;

        public override float Cooldown => Plugin.GetConfig().AbilityConfig.AreaDenialAbilityConfig.Cooldown;

        public ItemType RequiredItemType => ItemType.GrenadeFlash;

        public override void OnTrigger()
        {
            base.OnTrigger();
            if(!CooldownReady || CurrentOwner.inventory.CurItem.TypeId != RequiredItemType)
            {
                return;
            }
            RoomIdentifier targetRoom = RoomIdUtils.RoomAtPosition(CurrentOwner.transform.position);
            List<ReferenceHub> affectedPlayers = targetRoom.GetPlayersInRoom().Where(x => x.GetTeam() != CurrentOwner.GetTeam()).ToList();
            CurrentOwner.SendHint(Plugin.GetConfig().AbilityConfig.AreaDenialAbilityConfig.WindupMessage.Replace("{windup}", ((int)Plugin.GetConfig().AbilityConfig.AreaDenialAbilityConfig.Windup).ToString()), 5f);
            CurrentOwner.RemoveItemFromHub(RequiredItemType);
            foreach (ReferenceHub hub in affectedPlayers)
            {
                hub.SendHint(Plugin.GetConfig().AbilityConfig.AreaDenialAbilityConfig.WarningMessage.Replace("{windup}", ((int)Plugin.GetConfig().AbilityConfig.AreaDenialAbilityConfig.Windup).ToString()), 5f);
            }
            float timeLeft = Plugin.GetConfig().AbilityConfig.AreaDenialAbilityConfig.Windup;
            Timing.CallPeriodically(Plugin.GetConfig().AbilityConfig.AreaDenialAbilityConfig.Windup, 1f, () => 
            {
                foreach(ReferenceHub hub in targetRoom.GetPlayersInRoom().Where(x => x.GetTeam() != CurrentOwner.GetTeam()))
                {
                    hub.SendHint(Plugin.GetConfig().AbilityConfig.AreaDenialAbilityConfig.WarningMessage.Replace("{windup}", ((int)timeLeft).ToString()), timeLeft);
                }
                timeLeft -= 1f;
            });
            Timing.CallDelayed(Plugin.GetConfig().AbilityConfig.AreaDenialAbilityConfig.Windup + 0.1f, () =>
            {
                List<ReferenceHub> stillAffectedPlayers = targetRoom.GetPlayersInRoom().Where(x => x.GetTeam() != CurrentOwner.GetTeam()).ToList();
                foreach (ReferenceHub hub in stillAffectedPlayers)
                {
                    foreach (EffectDefinition statusEffect in Plugin.GetConfig().AbilityConfig.AreaDenialAbilityConfig.Effects)
                    {
                        var result = hub.playerEffectsController.ChangeState(statusEffect.Name, statusEffect.Intensity, statusEffect.Duration);
                        if (result == null)
                        {
                            Log.Warning("Status effect failed to apply. EffectName: " + statusEffect.Name);
                        }
                    }
                }
                CurrentOwner.SendHint(Plugin.GetConfig().AbilityConfig.AreaDenialAbilityConfig.SuccessMessage.Replace("{count}", stillAffectedPlayers.Count.ToString()));
            });
        }
    }
}
