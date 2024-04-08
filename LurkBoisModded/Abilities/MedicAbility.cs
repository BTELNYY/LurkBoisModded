using LurkBoisModded.Base.Ability;
using LurkBoisModded.Managers;
using Mirror;
using PlayerRoles;
using PluginAPI.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace LurkBoisModded.Abilities
{
    public class MedicAbility : CustomCooldownAbility
    {
        public override AbilityType AbilityType => AbilityType.MedicAbility;

        public override float Cooldown => Plugin.GetConfig().AbilityConfig.MedicAbilityConfig.Cooldown;

        public override void OnTrigger()
        {
            base.OnTrigger();
            if(!CooldownReady)
            {
                return;
            }
            if(CurrentHub.inventory.CurItem == null || CurrentHub.inventory.CurInstance ==  null) 
            {
                SetRemainingCooldown(1f);
                CurrentHub.SendHint(Plugin.GetConfig().AbilityConfig.MedicAbilityConfig.HealingItemRequired);
                return;
            }
            if(CurrentHub.inventory.CurInstance.Category != ItemCategory.Medical)
            {
                SetRemainingCooldown(1f);
                CurrentHub.SendHint(Plugin.GetConfig().AbilityConfig.MedicAbilityConfig.HealingItemRequired);
                return;
            }
            float healing = 0;
            if (!Plugin.GetConfig().AbilityConfig.MedicAbilityConfig.HealingDict.ContainsKey(CurrentHub.inventory.CurItem.TypeId))
            {
                SetRemainingCooldown(1f);
                CurrentHub.SendHint(Plugin.GetConfig().AbilityConfig.MedicAbilityConfig.HealingItemRequired);
                return;
            }
            else
            {
                healing = Plugin.GetConfig().AbilityConfig.MedicAbilityConfig.HealingDict[CurrentHub.inventory.CurItem.TypeId];
            }
            Transform cam = CurrentHub.PlayerCameraReference;
            Vector3 raycastFrom = cam.position;
            raycastFrom.x += 0.25f;
            if (!Physics.Raycast(raycastFrom, cam.forward, out RaycastHit hit, Plugin.GetConfig().AbilityConfig.MedicAbilityConfig.HealMaxDistance, LayerMask.GetMask("Default", "Player", "Hitbox")))
            {
                SetRemainingCooldown(1f);
                CurrentHub.SendHint(Plugin.GetConfig().AbilityConfig.MedicAbilityConfig.NotInRange);
                return;
            }
            if (hit.transform.GetComponent<NetworkIdentity>() != null && hit.transform.GetComponent<NetworkIdentity>().netId == CurrentHub.netId)
            {
                ResetCooldown();
                return;
            }
            Player hitPlayer = Player.Get(hit.transform.GetComponentInParent<ReferenceHub>());
            if(hitPlayer.ReferenceHub.Network_playerId == CurrentHub.Network_playerId)
            {
                SetRemainingCooldown(1f);
                return;
            }
            if (!AllowHealing(CurrentHub.GetTeam(), hitPlayer.ReferenceHub.GetTeam(), out string response))
            {
                ResetCooldown();
                CurrentHub.SendHint(response);
                return;
            }
            else
            {
                if(hitPlayer.MaxHealth <= hitPlayer.Health)
                {
                    SetRemainingCooldown(1f);
                    CurrentHub.SendHint(Plugin.GetConfig().AbilityConfig.MedicAbilityConfig.PlayerHealthFull);
                }
                string message = Plugin.GetConfig().AbilityConfig.MedicAbilityConfig.HealSuccess.Replace("{username}", hitPlayer.Nickname).Replace("{health}", ((int)healing).ToString());
                CurrentHub.SendHint(message);
                string messageToOther = Plugin.GetConfig().AbilityConfig.MedicAbilityConfig.HealSuccessOtherPlayer.Replace("{username}", CurrentHub.nicknameSync.MyNick).Replace("{health}", ((int)healing).ToString());
                hitPlayer.Heal(healing);
                hitPlayer.SendHint(messageToOther);
                CurrentHub.RemoveItemFromHub(CurrentHub.inventory.CurItem.TypeId);
            }
        }

        public static bool AllowHealing(Team healer, Team healed, out string errorMsg)
        {
            Team[] foundationTeam = { Team.Scientists, Team.FoundationForces };
            Team[] chaosTeam = { Team.ChaosInsurgency, Team.ClassD };
            Team[] escapeTeam = { Team.Scientists, Team.ClassD };
            //Same team
            if(healer == healed)
            {
                errorMsg = null;
                return true;
            }
            //No Healing SCPs, unless you can
            if (healed == Team.SCPs && healer != Team.SCPs && !Plugin.GetConfig().AbilityConfig.MedicAbilityConfig.AllowHealSCPs)
            {
                errorMsg = Plugin.GetConfig().AbilityConfig.MedicAbilityConfig.CantHealSCPs;
                return false;
            }
            if (healed == Team.SCPs && healer != Team.SCPs && Plugin.GetConfig().AbilityConfig.MedicAbilityConfig.AllowHealSCPs)
            {
                errorMsg = null;
                return true;
            }
            if (escapeTeam.Contains(healed) && escapeTeam.Contains(healer) && Plugin.GetConfig().AbilityConfig.MedicAbilityConfig.AllowEscapeRoleCrossTeamHealing)
            {
                errorMsg = null;
                return true;
            }
            if(foundationTeam.Contains(healer) && foundationTeam.Contains(healed)) 
            {
                errorMsg = null;
                return true;
            }
            if (chaosTeam.Contains(healer) && chaosTeam.Contains(healed))
            {
                errorMsg = null;
                return true;
            }
            if (Plugin.GetConfig().AbilityConfig.MedicAbilityConfig.AllowHealOtherHumanTeams)
            {
                errorMsg = null;
                return true;
            }
            errorMsg = Plugin.GetConfig().AbilityConfig.MedicAbilityConfig.CantHealGeneric;
            return false;
        }
    }
}
