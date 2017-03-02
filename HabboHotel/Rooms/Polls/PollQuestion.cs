using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rocket.HabboHotel.Rooms.Polls
{
    public class PollQuestion
    {
        public Poll Poll;
        public int Id;

        public PollQuestion ParentQuestion;
        public int ParentQuestionId;

        public int OrderNum;

        public string Question;

        public List<PollAnswerItem> Answers;

        public PollQuestionType Type;
        public List<PollQuestion> SubQuestions;

        public static List<PollQuestion> Generate(Poll Poll)
        {
            DataTable Table1;
            using (var adap = RocketEmulador.GetDatabaseManager().GetQueryReactor())
            {
                adap.SetQuery("SELECT * FROM server_polls_questions WHERE poll_id = '" + Poll.Id + "'");
                Table1 = adap.getTable();
            }

            var list = new List<PollQuestion>();
            var children = new List<PollQuestion>();

            foreach (DataRow Row in Table1.Rows)
            {
                var q = new PollQuestion(Row, Poll);
                if (q.ParentQuestionId > 0)
                    children.Add(q);
                else
                    list.Add(q);
            }

            foreach (var item in list)
            {
                foreach (var child in children)
                {
                    if (child.ParentQuestionId == item.Id)
                    {
                        child.ParentQuestion = item;
                        item.SubQuestions.Add(child);
                    }
                }
            }

            return list;
        }

        public PollQuestion(DataRow Row, Poll Main)
        {
            this.Poll = Main;
            this.Id = Convert.ToInt32(Row["id"]);
            this.OrderNum = Convert.ToInt32(Row["order_num"]);
            this.ParentQuestionId = Convert.ToInt32(Row["parent_id"]);
            this.Question = Row["question"].ToString();
            this.Type = (PollQuestionType)Convert.ToInt32(Row["type"]);

            this.Answers = PollAnswerItem.Generate(this);
            this.SubQuestions = new List<PollQuestion>();


        }



        //Normal Quest
        public PollQuestion(int id, Poll poll, PollQuestion parentquest, int order, string question, List<PollAnswerItem> answers, PollQuestionType type, List<PollQuestion> subquests)
        {
            Id = id;
            Poll = poll;
            ParentQuestion = parentquest;
            OrderNum = order;
            Question = question;
            Answers = answers;
            Type = type;
            SubQuestions = subquests;
        }

        //Question Parent
        public PollQuestion(int id, Poll poll, int order, string question, List<PollAnswerItem> answers, PollQuestionType type)
        {
            Id = id;
            Poll = poll;
            ParentQuestion = null;
            OrderNum = order;
            Question = question;
            Answers = answers;
            Type = type;
            SubQuestions = null;
        }

        public PollQuestion(int id, int order, string question, PollQuestionType type)
        {
            Id = id;
            OrderNum = order;
            Question = question;
            Answers = new List<PollAnswerItem>();
            SubQuestions = new List<PollQuestion>();
            ParentQuestion = null;
            Type = type;
        }

        private PollQuestion(int id, int order, PollQuestion parentquest, string question, PollQuestionType type)
        {
            Id = id;
            OrderNum = order;
            Question = question;
            Answers = new List<PollAnswerItem>();
            SubQuestions = new List<PollQuestion>();
            ParentQuestion = parentquest;
            Type = type;
        }

        public PollAnswerItem AddAnswer(int id, string answertext, string answerkey, int answerkeyint, int targetsub = 0)
        {
            var i = new PollAnswerItem(id, answertext, this, targetsub);
            this.Answers.Add(i);
            return i;
        }

        public PollQuestion AddSubQuestion(int id, int order, string Question, PollQuestionType type)
        {
            var q = new PollQuestion(id, order, this, Question, type);
            SubQuestions.Add(q);
            return q;
        }

        public PollQuestion GetSubQuestion(int id)
        {
            return SubQuestions.FirstOrDefault(c => c.Id == id);
        }

    }

    public enum PollQuestionType
    {
        RADIO = 1,
        CHECKBOX = 2,
        TEXTBOX = 3,
        TYPE_4 = 4
    }
}
