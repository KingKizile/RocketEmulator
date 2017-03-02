using Rocket.HabboHotel.GameClients;
namespace Rocket.Communication.Packets.Outgoing.Handshake
{
    public class UserRightsComposer : ServerPacket
    {
        public UserRightsComposer(int Rank)
            : base(ServerPacketHeader.UserRightsMessageComposer)
        {
            base.WriteInteger(2);//Club level
            base.WriteInteger(Rank);
            if (Rank >= RocketEmulador.AmbassadorMinRank)
            {
                base.WriteBoolean(true);
            }
            else
            {
                base.WriteBoolean(false);
            }
        }
    }
}
