using HarmonyLib;
using LurkBoisModded.Base.Ability;
using LurkBoisModded.Managers;
using MapGeneration;
using MEC;
using PlayerRoles.PlayableScps.Scp079;
using PlayerRoles;
using PluginAPI.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LurkBoisModded.Extensions;
using System.Security.Policy;
using LurkBoisModded.Effects;
using Mirror;

namespace LurkBoisModded.Abilities
{
    public class Scp079GasAbility : CustomCooldownAbility
    {
        public override AbilityType AbilityType => AbilityType.Scp079GasAbility;

        public override float Cooldown => 60f;

        public RoomIdentifier CurrentRoom;

        public override void OnFinishSetup()
        {
            base.OnFinishSetup();
            CurrentOwner.SendHint($"Use .gas in console when you are Access Tier {Config.CurrentConfig.Scp079Config.GasUnlockAt} to use poison gas in whatever room you are looking at. (Cannot be combined with the lockdown ability)", 10f);
        }

        public override void OnTrigger()
        {
            base.OnTrigger();
            if (!CooldownReady)
            {
                return;
            }
            Player player = Player.Get(CurrentOwner);
            if (player.Role != RoleTypeId.Scp079)
            {
                CurrentOwner.SendHint("You must be 079 to execute this command!");
                return;
            }
            Scp079Role scp079role = player.RoleBase as Scp079Role;
            if (!Plugin.GetConfig().Scp079Config.GasEnabled)
            {
                CurrentOwner.SendHint("This ability is disabled.");
                return;
            }
            Scp079TierManager tierManager;
            if (!scp079role.SubroutineModule.TryGetSubroutine(out tierManager))
            {
                CurrentOwner.SendHint("Sorry, something went wrong. (Failed to get TierManager)");
                return;
            }
            Scp079AuxManager auxManager;
            if (!scp079role.SubroutineModule.TryGetSubroutine(out auxManager))
            {
                CurrentOwner.SendHint("Sorry, something went wrong. (Failed to get AuxManager)");
                return;
            }
            Scp079LockdownRoomAbility lockRoomAbility;
            if (!scp079role.SubroutineModule.TryGetSubroutine(out lockRoomAbility))
            {
                CurrentOwner.SendHint("Sorry, something went wrong. (Failed to get Scp079LockdownRoomAbility)");
                return;
            }
            if (tierManager.AccessTierLevel < Plugin.GetConfig().Scp079Config.GasUnlockAt)
            {
                CurrentOwner.SendHint("Not high enough access tier!");
                return;
            }
            if (auxManager.CurrentAux < Plugin.GetConfig().Scp079Config.GasCost)
            {
                CurrentOwner.SendHint("Not enough AP!");
                return;
            }
            float remainingDuration = (float)(AccessTools.Property(typeof(Scp079LockdownRoomAbility), "RemainingLockdownDuration").GetGetMethod(true).Invoke(lockRoomAbility, null));
            RoomIdentifier lastLockedRoom = (RoomIdentifier)AccessTools.Field(typeof(Scp079LockdownRoomAbility), "_lastLockedRoom").GetValue(lockRoomAbility);
            if (remainingDuration > 0 && lastLockedRoom == scp079role.CurrentCamera.Room)
            {
                CurrentOwner.SendHint("Cannot gas a locked room!");
                return;
            }
            if (CurrentRoom != null)
            {
                CurrentOwner.SendHint("Already gassing a room or another 079 used the ability!");
                return;
            }
            auxManager.CurrentAux -= Plugin.GetConfig().Scp079Config.GasCost;
            RoomIdentifier targetRoom = scp079role.CurrentCamera.Room;
            CurrentRoom = targetRoom;
            Timing.CallDelayed(Plugin.GetConfig().Scp079Config.GasDuration + 1, () =>
            {
                CurrentRoom = null;
            });
            Timing.CallPeriodically(Plugin.GetConfig().Scp079Config.GasDuration, 1f, () =>
            {
                List<ReferenceHub> list = ReferenceHub.AllHubs.Where(x => RoomIdUtils.RoomAtPositionRaycasts(x.transform.position) == targetRoom).ToList();
                foreach (ReferenceHub hub in list)
                {
                    if (hub.IsSCP())
                    {
                        continue;
                    }
                    hub.playerEffectsController.ChangeState<Suffocation>(Plugin.GetConfig().Scp079Config.GasIntensity, Config.CurrentConfig.Scp079Config.GasDuration, false);
                }
            });
            return;
        }
    }
}
