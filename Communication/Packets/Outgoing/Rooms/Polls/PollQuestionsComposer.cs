using Rocket.HabboHotel.Rooms.Polls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rocket.Communication.Packets.Outgoing.Rooms.Polls
{
    class PollQuestionsComposer : ServerPacket
    {
        public PollQuestionsComposer(Poll Poll)
            : base(ServerPacketHeader.PollQuestionsMessageComposer)
        {
            /*goto nrom;
            base.WriteInteger(1);
            base.WriteString("Title");//Title
            base.WriteString("Vlw"); //Greeting

            base.WriteInteger(1); //while quests
            
            base.WriteInteger(1);//quest id
            base.WriteInteger(1);// quest order
            base.WriteInteger(1); //quest type
            base.WriteString("Eu Sou gay?");

            base.WriteInteger(0); //Unknown
            base.WriteInteger(0); //Unknown

            base.WriteInteger(2);// Answers amount

            base.WriteString("1");
            base.WriteString("Sim");
            base.WriteInteger(3);

            base.WriteString("2");
            base.WriteString("Não");
            base.WriteInteger(3);

            base.WriteInteger(1);

            base.WriteInteger(3);
            base.WriteInteger(3);
            base.WriteInteger(3);
            base.WriteString("Serio?");

            base.WriteInteger(3);
            base.WriteInteger(0);

            base.WriteInteger(0);

            base.WriteBoolean(true);




            return;
            nrom:*/
            base.WriteInteger(Poll.Id);
            base.WriteString(Poll.Title);
            base.WriteString(Poll.Greeting);

            base.WriteInteger(Poll.Questions.Count);

            foreach (var Quest in Poll.Questions)
            {
                base.WriteInteger(Quest.Id);
                base.WriteInteger(Quest.Id);
                base.WriteInteger((int)Quest.Type);
                base.WriteString(Quest.Question);

                base.WriteInteger(Quest.Id);
                base.WriteInteger(-1);

                base.WriteInteger((int)Quest.Type < 3 ? Quest.Answers.Count : 0);
                if ((int)Quest.Type < 3)
                {
                    foreach (var Answer in Quest.Answers)
                    {
                        base.WriteString(Answer.Id.ToString());
                        base.WriteString(Answer.AnswerText);
                        base.WriteInteger(Answer.TargetSubQuestionId); //Maybe Parent  Question ID or Actual Question ID
                    }
                }

                base.WriteInteger(Quest.SubQuestions.Count);
                foreach (var SubQuest in Quest.SubQuestions)
                {
                    base.WriteInteger(SubQuest.Id);
                    base.WriteInteger(SubQuest.Id);
                    base.WriteInteger((int)SubQuest.Type);
                    base.WriteString(SubQuest.Question);

                    base.WriteInteger(SubQuest.Id);
                    base.WriteInteger(-1);

                    base.WriteInteger((int)SubQuest.Type < 3 ? SubQuest.Answers.Count : 0);
                    if ((int)SubQuest.Type < 3)
                    {
                        foreach (var Answer in SubQuest.Answers)
                        {
                            base.WriteString(Answer.Id.ToString());
                            base.WriteString(Answer.AnswerText);
                            base.WriteInteger(Answer.TargetSubQuestionId); //Maybe Parent  Question ID or Actual Question ID
                        }
                    }
                }
            }

            base.WriteBoolean(Poll.SomeBool);
        }
        public void WriteQuestion(PollQuestion Quest)
        {
            base.WriteInteger(Quest.Id);
            base.WriteInteger(Quest.Id);
            base.WriteInteger((int)Quest.Type);
            base.WriteString(Quest.Question);

            base.WriteInteger(0);
            base.WriteInteger(0);

            base.WriteInteger((int)Quest.Type < 3 ? Quest.Answers.Count : 0);
            if ((int)Quest.Type < 3)
            {
                foreach (var Answer in Quest.Answers)
                {
                    base.WriteString(Answer.Id.ToString());
                    base.WriteString(Answer.AnswerText);
                    base.WriteInteger(Answer.TargetSubQuestionId); //Maybe Parent  Question ID or Actual Question ID
                }
            }
        }
    }
}
