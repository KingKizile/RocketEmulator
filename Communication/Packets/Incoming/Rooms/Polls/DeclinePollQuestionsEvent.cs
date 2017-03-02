using Rocket.HabboHotel.GameClients;
using Rocket.HabboHotel.Rooms.Polls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rocket.Communication.Packets.Incoming.Rooms.Polls
{
    class DeclinePollQuestionsEvent : IPacketEvent
    {
        public void Parse(GameClient Session, ClientPacket Packet)
        {
            var PollId = Packet.PopInt();
            RoomPollManager.DeclinePoll(Session.GetHabbo().Id, PollId);
        }
    }
}
