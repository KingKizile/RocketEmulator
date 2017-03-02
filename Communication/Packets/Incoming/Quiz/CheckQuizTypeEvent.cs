using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rocket.HabboHotel.GameClients;
using Rocket.Communication.Packets.Outgoing.Quiz;

namespace Rocket.Communication.Packets.Incoming.Quiz
{
    class CheckQuizTypeEvent : IPacketEvent
    {
        public void Parse(GameClient Session, ClientPacket Packet)
        {
            Session.SendMessage(new QuizDataMessageComposer());
        }
    }
}
