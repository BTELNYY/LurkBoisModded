using CommandSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PluginAPI;
using PluginAPI.Core;
using InventorySystem.Items.Firearms;
using PlayerStatsSystem;

namespace LurkBoisModded.Commands.GameConsole
{
    [CommandHandler(typeof(ClientCommandHandler))]
    public class Suicide : ICommand
    {
        public string Command => "suicide";

        public string[] Aliases => new string[] { "suicide", "kys", "kill", "kms" };

        public string Description => "Suicide";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            Player target = Player.Get(sender);
            Firearm firearm = target.ReferenceHub.inventory.GetFirearm(target.ReferenceHub.inventory.CurItem.TypeId);
            if (!target.IsAlive)
            {
                response = "You are already dead!";
                return false;
            }
            if(firearm == null)
            {
                response = "You must hold a firearm to use this command!";
                return false;
            }
            if(firearm.Status.Ammo < 1)
            {
                response = "Your firearm must be loaded to use this command!";
                return false;
            }
            FirearmStatus status = new FirearmStatus((byte)(firearm.Status.Ammo - 1), firearm.Status.Flags, firearm.Status.Attachments);
            firearm.Status = status;
            int counter = 0;
            byte selectedClip = 0;
            foreach(FirearmAudioClip clip in firearm.AudioClips)
            {
                if (clip.HasFlag(FirearmAudioFlags.IsGunshot))
                {
                    selectedClip = (byte)counter;
                }
                counter++;
            }
            firearm.ServerSendAudioMessage(selectedClip);
            response = "Done.";
            target.Kill(Plugin.GetConfig().SuicideDeathReason);
            return true;
        }
    }
}
