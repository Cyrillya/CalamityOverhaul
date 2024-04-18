﻿using CalamityOverhaul.Content.Events;
using System.IO;
using Terraria;
using Terraria.ModLoader;

namespace CalamityOverhaul
{
    public enum CWRMessageType : byte
    {
        DompBool,
        RecoilAcceleration,
        TungstenRiotSync,
    }

    public class CWRNetCode
    {
        public static void HandlePacket(Mod mod, BinaryReader reader, int whoAmI) {
            CWRMessageType type = (CWRMessageType)reader.ReadByte();
            if (type == CWRMessageType.DompBool) {
                Main.player[reader.ReadInt32()].CWR().HandleDomp(reader);
            } 
            else if (type == CWRMessageType.RecoilAcceleration) {
                Main.player[reader.ReadInt32()].CWR().HandleRecoilAcceleration(reader);
            }
            else if (type == CWRMessageType.TungstenRiotSync) {
                TungstenRiot.Instance.TungstenRiotIsOngoing = reader.ReadBoolean();
                TungstenRiot.Instance.EventKillPoints = reader.ReadInt32();
            }
        }
    }
}
