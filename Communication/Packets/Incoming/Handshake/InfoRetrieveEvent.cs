using System;

using Rocket.Communication.Packets.Incoming;
using Rocket.HabboHotel.Groups;
using Rocket.HabboHotel.GameClients;
using Rocket.Communication.Packets.Outgoing.Handshake;

namespace Rocket.Communication.Packets.Incoming.Handshake
{
    public class InfoRetrieveEvent : IPacketEvent
    {
        public void Parse(GameClient Session, ClientPacket Packet)
        {
            Session.SendMessage(new UserObjectComposer(Session.GetHabbo()));
            Session.SendMessage(new UserPerksComposer(Session.GetHabbo()));
        }
    }
}