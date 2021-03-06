﻿using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;

using Rocket.Communication.Packets.Outgoing.Avatar;

namespace Rocket.Communication.Packets.Incoming.Avatar
{
    class GetWardrobeEvent : IPacketEvent
    {
        public void Parse(HabboHotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            Session.SendMessage(new WardrobeComposer(Session));
        }
    }
}
