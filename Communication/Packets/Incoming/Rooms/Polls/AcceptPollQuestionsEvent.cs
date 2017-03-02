using Rocket.Communication.Packets.Outgoing.Rooms.Polls;
using Rocket.HabboHotel.GameClients;
using Rocket.HabboHotel.Rooms.Polls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rocket.Communication.Packets.Incoming.Rooms.Polls
{
    class AcceptPollQuestionsEvent : IPacketEvent
    {
        public void Parse(GameClient Session, ClientPacket Packet)
        {
            var PollId = Packet.PopInt();
            Poll Poll;
            if (!RoomPollManager.TryGetPoll(PollId, out Poll))
            {
                Session.SendMessage(new PollErrorAlertComposer());
                return;
            }

            RoomPollManager.StartUserAnsweringPoll(Session.GetHabbo().Id, PollId);
            Session.SendMessage(new PollQuestionsComposer(Poll));
        }
    }
}
