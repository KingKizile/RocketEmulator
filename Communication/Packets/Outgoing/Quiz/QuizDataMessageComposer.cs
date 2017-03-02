using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rocket.Communication.Packets.Outgoing.Quiz
{
    class QuizDataMessageComposer : ServerPacket
    {
        public QuizDataMessageComposer() : base(ServerPacketHeader.QuizDataMessageComposer)
        {
            Random rnd = new Random();
            int domanda1 = rnd.Next(0, 3);
            int domanda2 = rnd.Next(0, 3);
            int domanda3 = rnd.Next(0, 3);
            int domanda4 = rnd.Next(0, 3);
            int domanda5 = rnd.Next(0, 3);
            base.WriteString("HabboWay1");
            base.WriteInteger(5);
            base.WriteInteger(domanda1);
            base.WriteInteger(domanda2);
            base.WriteInteger(domanda3);
            base.WriteInteger(domanda4);
            base.WriteInteger(domanda5);
        }
    }
}
