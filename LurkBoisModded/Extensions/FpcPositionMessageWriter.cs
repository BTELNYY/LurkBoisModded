using Mirror;
using PlayerRoles.FirstPersonControl.NetworkMessages;
using PlayerRoles.FirstPersonControl;
using RelativePositioning;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;

namespace LurkBoisModded.Extensions
{
    internal static class FpcPositionMessageWriter
    {
        static FpcPositionMessageWriter()
        {
            Writer<FpcPositionMessage>.write = new Action<NetworkWriter, FpcPositionMessage>(FpcPositionMessageWriter.WriteFpcPositionMessage);
        }

        private static void WriteFpcPositionMessage(NetworkWriter writer, FpcPositionMessage message)
        {
            if (FpcPositionMessageWriter.valuesToApply > (FpcPositionMessageWriter.AppliedValues)0)
            {
                FpcPositionMessageWriter.WriteCustomFpcPositionMessage(writer, message, FpcPositionMessageWriter.valuesToApply);
                FpcPositionMessageWriter.valuesToApply = (FpcPositionMessageWriter.AppliedValues)0;
                return;
            }
            ReferenceHub refhub = (ReferenceHub)AccessTools.Field(typeof(FpcPositionMessage), "_receiver").GetValue(message);
            FpcServerPositionDistributor.WriteAll(refhub, writer);
        }

        private static void WriteCustomFpcPositionMessage(NetworkWriter writer, FpcPositionMessage message, FpcPositionMessageWriter.AppliedValues appliedValues)
        {
            ReferenceHub receiver = (ReferenceHub)AccessTools.Field(typeof(FpcPositionMessage), "_receiver").GetValue(message);
            FirstPersonMovementModule fpcModule = ((IFpcRole)receiver.roleManager.CurrentRole).FpcModule;
            bool flag = (appliedValues & FpcPositionMessageWriter.AppliedValues.ApplyMouseLook) > (FpcPositionMessageWriter.AppliedValues)0;
            bool flag2 = (appliedValues & FpcPositionMessageWriter.AppliedValues.ApplyPosition) > (FpcPositionMessageWriter.AppliedValues)0;
            bool flag3 = (appliedValues & FpcPositionMessageWriter.AppliedValues.ApplyMovementState) > (FpcPositionMessageWriter.AppliedValues)0;
            ushort num;
            ushort num2;
            if (!flag)
            {
                num = 0;
                num2 = 0;
            }
            else
            {
                ValueTuple<ushort, ushort> valueTuple = FpcPositionMessageWriter.appliedMouseLook;
                num = valueTuple.Item1;
                num2 = valueTuple.Item2;
            }
            byte b = ((byte)(flag3 ? (FpcPositionMessageWriter.appliedMovementState & (PlayerMovementState)3) : fpcModule.CurrentMovementState));
            RelativePosition relativePosition = (flag2 ? FpcPositionMessageWriter.appliedPosition : default(RelativePosition));
            byte b2 = b;
            if (flag)
            {
                b2 |= 32;
            }
            if (flag2)
            {
                b2 |= 64;
            }
            if (fpcModule.IsGrounded)
            {
                b2 |= 128;
            }
            writer.Write<ushort>(2);
            writer.Write<RecyclablePlayerId>(receiver.Network_playerId);
            writer.Write<byte>(b2);
            if (flag2)
            {
                RelativePositionSerialization.WriteRelativePosition(writer, relativePosition);
            }
            if (flag)
            {
                NetworkWriterExtensions.WriteUShort(writer, num += 1);
                NetworkWriterExtensions.WriteUShort(writer, num2 += 1);
            }
            writer.Write<RecyclablePlayerId>(receiver.Network_playerId);
            writer.Write<byte>(b2);
            if (flag2)
            {
                RelativePositionSerialization.WriteRelativePosition(writer, relativePosition);
            }
            if (flag)
            {
                NetworkWriterExtensions.WriteUShort(writer, (ushort)(num - 1));
                NetworkWriterExtensions.WriteUShort(writer, (ushort)(num2 - 1));
            }
        }

        internal static FpcPositionMessageWriter.AppliedValues valuesToApply;

        internal static ValueTuple<ushort, ushort> appliedMouseLook;

        internal static RelativePosition appliedPosition;

        internal static PlayerMovementState appliedMovementState;

        [Flags]
        public enum FpcSyncDataByteFlags : byte
        {
            Crouching = 0,
            Sneaking = 1,
            Walking = 2,
            Sprinting = 3,
            BitCustom = 128,
            BitPosition = 64,
            BitMouseLook = 32
        }

        [Flags]
        internal enum AppliedValues : byte
        {
            ApplyMouseLook = 1,
            ApplyPosition = 2,
            ApplyMovementState = 4
        }
    }
}
