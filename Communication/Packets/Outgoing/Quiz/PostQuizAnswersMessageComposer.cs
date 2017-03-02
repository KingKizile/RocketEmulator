using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rocket.Communication.Packets.Outgoing.Quiz
{
    class PostQuizAnswersMessageComposer : ServerPacket
    {
        public PostQuizAnswersMessageComposer() : base(ServerPacketHeader.PostQuizAnswersMessageComposer)
        {
            Random rnd = new Random();
            int risposta1 = rnd.Next(0, 3);
            int risposta2 = rnd.Next(0, 3);
            int risposta3 = rnd.Next(0, 3);
            int risposta4 = rnd.Next(0, 3);
            int risposta5 = rnd.Next(0, 3);
            base.WriteString("HabboWay1");
            base.WriteInteger(5);
            base.WriteInteger(risposta1);
            base.WriteInteger(risposta2);
            base.WriteInteger(risposta3);
            base.WriteInteger(risposta4);
            base.WriteInteger(risposta5);
        }
    }
}
