using System;
using Rocket.Communication.Packets.Incoming;
using Rocket.HabboHotel.GameClients;
using Rocket.Communication.Packets.Outgoing.Users;

namespace Rocket.Communication.Packets.Incoming.Users
{
    class ScrGetUserInfoEvent : IPacketEvent
    {
        public void Parse(GameClient Session, ClientPacket Packet)
        {
            Session.SendMessage(new ScrSendUserInfoComposer());
        }
    }
}
