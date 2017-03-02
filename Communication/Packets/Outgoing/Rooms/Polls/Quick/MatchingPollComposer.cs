using Rocket.HabboHotel.Rooms.Polls.Quick;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rocket.Communication.Packets.Outgoing.Rooms.Polls.Quick
{
    class MatchingPollComposer : ServerPacket
    {
        public MatchingPollComposer(QuickPoll Poll)
            : base(ServerPacketHeader.MatchingPollMessageComposer)
        {
            base.WriteString("1"); // May mPoll ID
            base.WriteInteger(Poll.Id); // May mPoll ID
            base.WriteInteger(3); // May mPoll Question ID
            base.WriteInteger(Poll.ReamigTime); //Duration

            base.WriteInteger(0); //Id
            base.WriteInteger(0); //Number
            base.WriteInteger(0); //Type

            base.WriteString(Poll.Title); //May title 2

            base.WriteInteger(-1);//selection_min

            base.WriteInteger(0);/*2);//len

            base.WriteString("1000");
            base.WriteString("1000");

            base.WriteString("1000");
            base.WriteString("10000");*/
        }
    }
}
