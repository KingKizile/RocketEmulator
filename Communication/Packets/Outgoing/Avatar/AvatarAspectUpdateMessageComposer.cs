using System;
using System.Linq;
using System.Text;


using Rocket.HabboHotel.Rooms;
using Rocket.HabboHotel.GameClients;


namespace Rocket.Communication.Packets.Outgoing.Rooms.Engine
{
    class AvatarAspectUpdateMessageComposer : ServerPacket
    {
        public AvatarAspectUpdateMessageComposer(string Figure, string Gender)
            : base(ServerPacketHeader.AvatarAspectUpdateMessageComposer)
        {
            base.WriteString(Figure);
            base.WriteString(Gender);


        }
    }
}