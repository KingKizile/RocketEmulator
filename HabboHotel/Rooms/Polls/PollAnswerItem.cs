using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rocket.HabboHotel.Rooms.Polls
{
    public class PollAnswerItem
    {
        public int Id;
        public string AnswerText;
        public int TargetSubQuestionId;
        public bool IsCorrectAnswer;

        public PollQuestion ParentQuestion;


        public static List<PollAnswerItem> Generate(PollQuestion Quest)
        {
            DataTable Table1;
            using (var adap = RocketEmulador.GetDatabaseManager().GetQueryReactor())
            {
                adap.SetQuery("SELECT * FROM server_polls_questions_answers WHERE question_id = '" + Quest.Id + "'");

                Table1 = adap.getTable();
            }
            var list = new List<PollAnswerItem>();

            foreach (DataRow Row in Table1.Rows)
            {
                var a = new PollAnswerItem(Row, Quest);
                list.Add(a);
            }

            return list;
        }

        public PollAnswerItem(DataRow Row, PollQuestion Main)
        {
            this.ParentQuestion = Main;

            this.Id = Convert.ToInt32(Row["id"]);
            this.AnswerText = Row["value"].ToString();
            this.IsCorrectAnswer = Convert.ToInt32(Row["is_correct"]) == 1;
            this.TargetSubQuestionId = Convert.ToInt32(Row["target_subquestion"]);
        }


        public PollAnswerItem(int id, string answertext, PollQuestion parentquestion, int targetsubqid)
        {
            Id = id;
            AnswerText = answertext;
            ParentQuestion = parentquestion;
            TargetSubQuestionId = targetsubqid;
        }
    }
}
