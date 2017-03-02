using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rocket.HabboHotel.Rooms.Polls
{
    public class PollUserAnswer
    {
        public int Main;
        public int QuestionId;
        public int PollId;
        public string AnswerData;


        public PollUserAnswer(int main, int questid, int pollid, string answerdata)
        {
            Main = main;
            QuestionId = questid;
            AnswerData = answerdata;
            PollId = pollid;
        }

        public PollUserAnswer(int main, int pollid, DataRow Row)
        {
            Main = main;
            PollId = pollid;

            this.QuestionId = Convert.ToInt32(Row["question_id"]);
            this.AnswerData = Row["answer_data"].ToString();
        }

    }
}