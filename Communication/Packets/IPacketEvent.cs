using Rocket.Communication.Packets.Incoming;
using Rocket.HabboHotel.GameClients;

namespace Rocket.Communication.Packets
{
    public interface IPacketEvent
    {
        void Parse(GameClient Session, ClientPacket Packet);
    }
}