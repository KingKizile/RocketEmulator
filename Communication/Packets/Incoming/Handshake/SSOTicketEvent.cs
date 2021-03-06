﻿using System;

using Rocket.Communication.Packets.Incoming;
using Rocket.HabboHotel.GameClients;
using Rocket.Communication.Packets.Outgoing.Handshake;

namespace Rocket.Communication.Packets.Incoming.Handshake
{
    public class SSOTicketEvent : IPacketEvent
    {
        public void Parse(GameClient Session, ClientPacket Packet)
        {
            if (Session == null || Session.RC4Client == null || Session.GetHabbo() != null)
                return;

            Session.TryAuthenticate(Packet.PopString());
        }
    }
}