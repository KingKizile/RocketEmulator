using Rocket.Database;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rocket.HabboHotel.Rooms.Polls
{
    public class UserPollData
    {
        public int Id;
        public int UserId;
        public int PollId;
        public int Timestamp;
        public bool Finished;

        public List<PollUserAnswer> Answers;

        public Poll GetPoll()
        {
            return RoomPollManager.GetPoll(PollId);
        }

        public UserPollData(int userid, int pollid)
        {
            UserId = userid;
            PollId = pollid;
            Timestamp = (int)RocketEmulador.GetUnixTimestamp();
            Answers = new List<PollUserAnswer>();
        }

        public UserPollData(DataRow MainRow, DataTable Table)
        {
            Finished = true;
            Answers = new List<PollUserAnswer>();
            this.Id = Convert.ToInt32(MainRow["id"]);
            this.UserId = Convert.ToInt32(MainRow["user_id"]);
            this.PollId = Convert.ToInt32(MainRow["poll_id"]);
            this.Timestamp = Convert.ToInt32(MainRow["timestamp"]);

            foreach (DataRow Row in Table.Rows)
            {
                var pollid = Convert.ToInt32(Row["main_id"]);
                if (pollid != PollId)
                    continue;

                Answers.Add(new PollUserAnswer(this.Id, this.PollId, Row));
            }
        }

        public bool CompletedAllQuestions
        {
            get
            {
                var poll = GetPoll();
                if (poll == null)
                    return false;
                return poll.UserFinishedPoll(this);
            }
        }

        public void Answer(int questId, string answer)
        {
            var poll = this.GetPoll();
            if (poll == null)
                return;

            var question = poll.GetQuestion(questId);
            if (question == null)
                return;

            this.Answers.Add(new PollUserAnswer(this.Id, questId, this.PollId, answer));
        }

        public void Finish()
        {
            if (Finished)
                return;
            int Insert = 0;
            using (var adap = RocketEmulador.GetDatabaseManager().GetQueryReactor())
            {
                adap.SetQuery(string.Concat("INSERT INTO server_polls_users (user_id, poll_id, timestamp) VALUES ('", UserId, "', '", PollId, "', '", Timestamp, "')"));
                Insert = (int)(adap.InsertQuery());
            }

            this.Id = Insert;

            var str = new StringBuilder();
            str.Append("INSERT INTO server_polls_users_answers (main_id, question_id, answer_data) VALUES ");
            foreach (var answ in Answers)
            {
                answ.Main = this.Id;
                str.AppendLine("(");

                str.Append(string.Concat("'", this.Id, "', '", answ.QuestionId, "', '", answ.AnswerData, "'"));

                str.Append(")");
                if (answ == Answers.Last())
                    str.Append(";");
                else
                    str.Append(",");
            }

            using (var adap = RocketEmulador.GetDatabaseManager().GetQueryReactor())
            {
                adap.RunQuery(str.ToString());
            }

            Finished = true;
        }
    }
}
