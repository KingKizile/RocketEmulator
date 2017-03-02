using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rocket.HabboHotel.Rooms.Polls
{
    public class Poll
    {
        public int Id;
        public int RoomId;
        public string Title;
        public string Desc;
        public string Greeting;
        public string CodeName;
        public int Limit;

        public bool SomeBool;

        public List<PollQuestion> Questions;

        public Poll(DataRow Row)
        {
            this.Id = Convert.ToInt32(Row["id"]);
            this.RoomId = Convert.ToInt32(Row["room_id"]);
            this.Limit = Convert.ToInt32(Row["limit"]);

            this.Title = Row["title"].ToString();
            this.Desc = Row["description"].ToString();
            this.Greeting = Row["greetings_text"].ToString();
            this.CodeName = Row["code_name"].ToString();

            this.Questions = PollQuestion.Generate(this);
            SomeBool = true;
        }

        public PollQuestion AddQuestion(int id, int order, string question, PollQuestionType type)
        {
            var q = new PollQuestion(id, order, question, type);
            Questions.Add(q);
            return q;
        }

        public PollQuestion GetQuestion(int id)
        {
            foreach (var quest in Questions)
            {
                if (quest.Id == id)
                    return quest;

                foreach (var subquest in quest.SubQuestions)
                {
                    if (subquest.Id == id)
                        return subquest;
                }
            }

            return null;
        }

        public int QuestionsCountAll
        {
            get
            {
                var i = 0;
                foreach (var q in Questions)
                {
                    i++;
                    i += q.SubQuestions.Count;
                }

                return i;
            }
        }

        public int QuestionsCount
        {
            get
            {
                return Questions.Count;
            }
        }

        /*public int GetQuestionCountForOcasion(UserPollData User)
        {
            var i = 0;
            List<PollQuestion> ListedQuestions = new List<PollQuestion>();
            foreach (var answer in User.Answers)
            {
                
            }
        }*/

        /*public Dictionary<int, PollQuestion> GetQuestionSteps()
        {
            var i = new Dictionary<int, PollQuestion>();
            for (var n = 1; n < Questions.Count; n++)
            {
                i.Add(n, Questions[n - 1]);
            }

            return i;
        }*/

        public bool UserFinishedPoll(UserPollData User)
        {
            int i = 0;
            foreach (var q in Questions)
            {
                if (User.Answers.Any(c => c.QuestionId == q.Id))
                    i++;

                if (q.SubQuestions.Count > 0)
                    if (!User.Answers.Any(c => q.SubQuestions.FirstOrDefault(d => d.Id == c.QuestionId) != null))
                        i--;
            }

            return i >= Questions.Count;
        }
    }
}
