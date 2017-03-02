using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using Rocket.Communication.Packets.Outgoing.Sound;

namespace Rocket.Communication.Packets.Incoming.Sound
{
    class GetSongInfoEvent : IPacketEvent
    {
        public void Parse(HabboHotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            Session.SendMessage(new TraxSongInfoComposer());
        }
    }
}
