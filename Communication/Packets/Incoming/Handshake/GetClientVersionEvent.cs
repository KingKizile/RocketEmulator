using Rocket.HabboHotel.GameClients;
using Rocket.Communication.Packets.Incoming;

namespace Rocket.Communication.Packets.Incoming.Handshake
{
    public class GetClientVersionEvent : IPacketEvent
    {
        public void Parse(GameClient Session, ClientPacket Packet)
        {
            string Build = Packet.PopString();

            if (RocketEmulador.SWFRevision != Build)
                RocketEmulador.SWFRevision = Build;
        }
    }
}