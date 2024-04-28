using Hints;
using LurkBoisModded.Base.Ability;
using LurkBoisModded.Managers;
using Mirror;
using System.Collections.Generic;
using VoiceChat;
using VoiceChat.Networking;
using UnityEngine;
using PlayerRoles.FirstPersonControl;
using LurkBoisModded.Extensions;

namespace LurkBoisModded.Abilities
{
    public class ProximityChatAbility : CustomAbility
    {
        public static readonly HashSet<ReferenceHub> ToggledPlayers = new HashSet<ReferenceHub>();

        public ReferenceHub MySpeaker;

        public override AbilityType AbilityType => AbilityType.ProximityChat;

        public override void OnTrigger()
        {
            base.OnTrigger();
            if (RoundSummary.singleton.RoundEnded())
            {
                return;
            }
            ReferenceHub player = CurrentOwner;
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

        void Update()
        {
            //MySpeaker.TryOverridePosition(CurrentHub.transform.position, Vector3.forward);
        }

        public override void OnFinishSetup()
        {
            base.OnFinishSetup();
            //MySpeaker = DummyManager.SpawnDummy(CurrentHub.transform.position);
            //MySpeaker.roleManager.ServerSetRole(PlayerRoles.RoleTypeId.Tutorial, PlayerRoles.RoleChangeReason.RemoteAdmin);
            //MySpeaker.netIdentity.SetScale(Vector3.zero);
            //MySpeaker.nicknameSync.Network_displayName = "ProximityChatSpeaker (DO NOT KILL)";
            CurrentOwner.SendHint(Plugin.GetConfig().ProximityChatConfig.ProximityChatCanBeUsed, 5f);
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
            bool result = Plugin.GetConfig().ProximityChatConfig.EnableCustomChat;
            if (result)
            {
                message.SendProximityMessage();
                return false;
            }
            return true;
        }
    }
}
