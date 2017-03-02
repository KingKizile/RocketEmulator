using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rocket.Communication.Packets.Outgoing.Rooms.Polls
{
    class PollErrorAlertComposer : ServerPacket
    {
        public PollErrorAlertComposer()
            : base(ServerPacketHeader.PollErrorAlertMessageComposer)
        {
        }
    }
}
