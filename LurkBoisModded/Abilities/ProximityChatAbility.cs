using Hints;
using LurkBoisModded.Base;
using Mirror;
using System.Collections.Generic;
using VoiceChat;
using VoiceChat.Networking;

namespace LurkBoisModded.Abilities
{
    public class ProximityChatAbility : CustomAbilityBase
    {
        public static readonly HashSet<ReferenceHub> ToggledPlayers = new HashSet<ReferenceHub>();

        public override AbilityType AbilityType => AbilityType.ProximityChat;

        public override void OnTrigger()
        {
            base.OnTrigger();
            ReferenceHub player = CurrentHub;
            if (!Plugin.GetConfig().ProximityChatConfig.AllowedRoles.Contains(player.roleManager.CurrentRole.RoleTypeId))
            {
                return;
            }
            if (ToggledPlayers.Contains(player))
            {
                ToggledPlayers.Remove(player);
                player.hints.Show(new TextHint(Plugin.GetConfig().ProximityChatConfig.ProximityChatDisabled, new HintParameter[] { new StringHintParameter(string.Empty) }, null, 4));
                return;
            }
            ToggledPlayers.Add(player);
            player.hints.Show(new TextHint(Plugin.GetConfig().ProximityChatConfig.ProximityChatEnabled, new HintParameter[] { new StringHintParameter(string.Empty) }, null, 4));
            return;
        }

        public override void OnFinishSetup()
        {
            base.OnFinishSetup();
            CurrentHub.SendHint(Plugin.GetConfig().ProximityChatConfig.ProximityChatCanBeUsed, 5f);
        }

        public static bool OnPlayerUsingVoiceChat(NetworkConnection connection, VoiceMessage message)
        {
            if (message.Channel != VoiceChatChannel.ScpChat)
                return true;

            if (!ReferenceHub.TryGetHubNetID(connection.identity.netId, out ReferenceHub player))
                return true;

            if (!Plugin.GetConfig().ProximityChatConfig.AllowedRoles.Contains(player.roleManager.CurrentRole.RoleTypeId) || (Plugin.GetConfig().ProximityChatConfig.EnableCustomChat && !ToggledPlayers.Contains(player)))
            {
                return true;
            }
            message.SendProximityMessage();
            return !Plugin.GetConfig().ProximityChatConfig.EnableCustomChat;
        }
    }
}
