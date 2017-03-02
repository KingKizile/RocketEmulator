using Rocket.Communication.Packets.Outgoing.Catalog;
using Rocket.HabboHotel.GameClients;
using Rocket.Communication.Packets.Incoming;

namespace Rocket.Communication.Packets.Incoming.Catalog
{
    public class GetGiftWrappingConfigurationEvent : IPacketEvent
    {
        public void Parse(GameClient Session, ClientPacket Packet)
        {
            Session.SendMessage(new GiftWrappingConfigurationComposer());
        }
    }
}