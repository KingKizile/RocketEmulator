using System;

namespace Rocket.Communication.Packets.Outgoing.Handshake
{
    internal class VideoOffersRewardsMessageComposer : ServerPacket
    {
        public VideoOffersRewardsMessageComposer(int Id, string Type, string Message) : base(22)
        {
            base.WriteString(Type);
            base.WriteInteger(Id);
            base.WriteString(Message);
            base.WriteString("");
        }
    }
}
