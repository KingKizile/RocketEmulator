using Rocket.HabboHotel.GameClients;
using Rocket.HabboHotel.Rooms.Polls.Quick;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rocket.Communication.Packets.Outgoing.Rooms.Polls.Quick
{
    class ConcludePollComposer : ServerPacket
    {
        public ConcludePollComposer(QuickPoll Poll, int UserId, int VoteType)
            : base(ServerPacketHeader.MatchingPollFinishMessageComposer)
        {
            base.WriteInteger(UserId);//User id
            base.WriteString(VoteType.ToString());//1 to right / otherwise wrong
            base.WriteInteger(2);//len

            base.WriteString("1");
            base.WriteInteger(Poll.PVotes.Count);

            base.WriteString("0");
            base.WriteInteger(Poll.NVotes.Count);
        }
    }
}
