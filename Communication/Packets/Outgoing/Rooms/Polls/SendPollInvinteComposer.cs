using Rocket.HabboHotel.Rooms.Polls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rocket.Communication.Packets.Outgoing.Rooms.Polls
{
    class SendPollInvinteComposer : ServerPacket
    {
        public SendPollInvinteComposer(Poll Poll)
            : base(ServerPacketHeader.SendPollInvinteMessageComposer)
        {
            base.WriteInteger(Poll.Id);
            base.WriteString("10000");
            base.WriteString("10000");
            base.WriteString(Poll.Title + " - " + Poll.Desc);
        }
    }
}
