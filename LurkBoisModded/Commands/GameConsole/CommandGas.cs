using CommandSystem;
using System;
using PluginAPI.Events;
using PluginAPI.Enums;
using PluginAPI.Core.Attributes;
using PluginAPI.Core;
using PlayerRoles;
using ICommand = CommandSystem.ICommand;
using PlayerRoles.PlayableScps.Scp079;
using HarmonyLib;
using MapGeneration;
using MEC;
using System.Collections.Generic;
using System.Linq;
using LurkBoisModded.Effects;

namespace LurkBoisModded.Commands.GameConsole
{
    [CommandHandler(typeof(ClientCommandHandler))]
    public class CommandGas : ICommand
    {
        public static RoomIdentifier CurrentRoom;
        public static bool Cooldown;

        public string Command => "gas";

        public string[] Aliases => new string[] { "gas", "nt", "toxin" };

        public string Description => "Deploys poison gas in the current room. Can't be run in the same room as a lockdown";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            Player player = Player.Get(sender);
            if(player.Role != RoleTypeId.Scp079)
            {
                response = "You must be 079 to execute this command!";
                return false;
            }
            Scp079Role scp079role = player.RoleBase as Scp079Role;
            if (!Plugin.GetConfig().Scp079Config.GasEnabled)
            {
                response = "This command is disabled.";
                return false;
            }
            Scp079TierManager tierManager;
            if(!scp079role.SubroutineModule.TryGetSubroutine(out tierManager))
            {
                response = "Sorry, something went wrong. (Failed to get TierManager)";
                return false;
            }
            Scp079AuxManager auxManager;
            if(!scp079role.SubroutineModule.TryGetSubroutine(out auxManager))
            {
                response = "Sorry, something went wrong. (Failed to get AuxManager)";
                return false;
            }
            Scp079LockdownRoomAbility lockRoomAbility;
            if(!scp079role.SubroutineModule.TryGetSubroutine(out lockRoomAbility))
            {
                response = "Sorry, something went wrong. (Failed to get Scp079LockdownRoomAbility)";
                return false;
            }
            if(tierManager.AccessTierLevel < Plugin.GetConfig().Scp079Config.GasUnlockAt)
            {
                response = "Not high enough access tier!";
                return false;
            }
            if(auxManager.CurrentAux < Plugin.GetConfig().Scp079Config.GasCost)
            {
                response = "Not enough AP!";
                return false;
            }
            float remainingDuration = (float)(AccessTools.Property(typeof(Scp079LockdownRoomAbility), "RemainingLockdownDuration").GetGetMethod(true).Invoke(lockRoomAbility, new object[] { }));
            RoomIdentifier lastLockedRoom = (RoomIdentifier)AccessTools.Field(typeof(Scp079LockdownRoomAbility), "_lastLockedRoom").GetValue(lockRoomAbility);
            if(remainingDuration > 0 && lastLockedRoom == scp079role.CurrentCamera.Room)
            {
                response = "Cannot gas a locked room!";
                return false;
            }
            if(CurrentRoom != null)
            {
                response = "Already gassing a room or another 079 used the command!";
                return false;
            }
            if (Cooldown)
            {
                response = "On cooldown!";
                return false;
            }
            Cooldown = true;
            auxManager.CurrentAux -= Plugin.GetConfig().Scp079Config.GasCost;
            response = "Done.";
            RoomIdentifier targetRoom = scp079role.CurrentCamera.Room;
            CurrentRoom = targetRoom;
            Timing.CallDelayed(Plugin.GetConfig().Scp079Config.GasDuration + 1, () =>
            {
                CurrentRoom = null;
            });
            Timing.CallDelayed(Plugin.GetConfig().Scp079Config.GasCooldown + Plugin.GetConfig().Scp079Config.GasDuration, () =>
            {
                Cooldown = false;
            });
            Timing.CallPeriodically(Plugin.GetConfig().Scp079Config.GasDuration, 1f, () =>
            {
                List<ReferenceHub> list = ReferenceHub.AllHubs.Where(x => RoomIdUtils.RoomAtPositionRaycasts(x.transform.position) == targetRoom).ToList();
                foreach(ReferenceHub hub in list)
                {
                    if (hub.IsSCP())
                    {
                        continue;
                    }
                    hub.playerEffectsController.ChangeState<Suffocation>(Plugin.GetConfig().Scp079Config.GasIntensity, 3f, false);
                }
            });
            return true;
        }
    }
}
