using System;
using System.Linq;
using System.Text;

using Rocket.Communication.Packets.Outgoing.Inventory.Badges;

namespace Rocket.Communication.Packets.Incoming.Inventory.Badges
{
    class GetBadgesEvent : IPacketEvent
    {
        public void Parse(HabboHotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            Session.SendMessage(new BadgesComposer(Session));
        }
    }
}
