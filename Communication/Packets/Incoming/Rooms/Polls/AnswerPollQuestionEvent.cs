using Rocket.Communication.Packets.Outgoing.Rooms.Polls.Quick;
using Rocket.HabboHotel.GameClients;
using Rocket.HabboHotel.Rooms.Polls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rocket.Communication.Packets.Incoming.Rooms.Polls
{
    class AnswerPollQuestionEvent : IPacketEvent
    {
        public void Parse(GameClient Session, ClientPacket Packet)
        {
            var PollId = Packet.PopInt();
            var QuestId = Packet.PopInt();

            var AnswerLenght = Packet.PopInt();
            var Answers = new List<string>();
            while (AnswerLenght > 0)
            {
                Answers.Add(Packet.PopString());
                AnswerLenght--;
            }


            if (PollId == -2)
            {
                //QuickPoll
                var Room = Session.GetHabbo().CurrentRoom;
                if (Room == null)
                    return;
                
                if (Room.QuickPoll == null)
                    return;

                bool Voted = Answers.FirstOrDefault() == "1";
                Room.QuickPoll.AddVote(Session.GetHabbo().Id, Voted);
                foreach (var user in Room.GetRoomUserManager().GetUserList().Where(c => !c.IsBot))
                {
                    var msg = new ConcludePollComposer(Room.QuickPoll, Session.GetHabbo().Id, Voted ? 1 : 0);
                    //if (!Room.QuickPoll.Voted(user.GetClient().GetHabbo().Id))
                    Room.SendMessage(msg);

                    //Session.SendMessage(msg);
                }
                return;
            }



            var Poll = RoomPollManager.GetPoll(PollId);
            if (Poll == null)
                return;

            var userAnsweringPoll = RoomPollManager.GetUserAnsweringPoll(Session.GetHabbo().Id, PollId);
            if (userAnsweringPoll == null)
                return;

            // var quest = Poll.GetQuestion(



            userAnsweringPoll.Answer(QuestId, string.Join("\t", Answers.ToArray()));

            if (userAnsweringPoll.CompletedAllQuestions)
                RoomPollManager.FinishuserAnsweringPoll(userAnsweringPoll.UserId);

        }
    }
}
