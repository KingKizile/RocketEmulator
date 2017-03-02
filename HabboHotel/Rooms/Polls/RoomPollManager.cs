using Rocket.Communication.Packets.Outgoing.Rooms.Polls;
using Rocket.HabboHotel.GameClients;
using Rocket.HabboHotel.Rooms.Chat.Commands.iBlue.USERS;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rocket.HabboHotel.Rooms.Polls
{
    public static class RoomPollManager
    {
        public static List<Poll> Polls;
        public static List<UserPollData> UsersCachedPolls;
        public static Dictionary<int, UserPollData> UsersAnsweringPolls;
        public static bool Initialized { get; private set; }
        public static List<UserPollDecline> UsersDeclinedPolls;

        public static int POLL_DECLINE_TIMEOUT = 1800;



        public static void Init()
        {
            if (Initialized)
                return;

            Polls = new List<Poll>();
            UsersAnsweringPolls = new Dictionary<int, UserPollData>();
            UsersCachedPolls = new List<UserPollData>();
            UsersDeclinedPolls = new List<UserPollDecline>();

            FlushData();      

            //RocketEmulador.GetDatabaseManager().OnClientDisconnect += RoomPollManager_OnClientDisconnect; //removido para manutenção

            //HotelSystemUpdater.RegisterUpdate(new DelegateUpdateCommand(Update), "command_update_polls", "polls"); // removido para manutenção
            Initialized = true; 
        }

        static void RoomPollManager_OnClientDisconnect(GameClient client)
        {
            var userid = client.GetHabbo().Id;

            if (UsersAnsweringPolls.ContainsKey(userid))
                UsersAnsweringPolls.Remove(userid);
        }


        #region Polls

        public static void TrySendPollInvinte(int roomid, GameClient Session)
        {
            var poll = GetPollByRoomId(roomid);

            if (poll == null)
                return;

            if (IsDeclined(Session.GetHabbo().Id, poll.Id))
                return;

            var useranswerdpoll = UsersCachedPolls.Count(c => c.PollId == poll.Id && c.UserId == Session.GetHabbo().Id);
            if (useranswerdpoll >= poll.Limit)
                return;

            Session.SendMessage(new SendPollInvinteComposer(poll));
        }

        public static void TrySendPollInvinte(string code, GameClient Session)
        {
            var poll = GetPollByCode(code);

            if (poll == null)
                return;

            if (IsDeclined(Session.GetHabbo().Id, poll.Id))
                return;

            var useranswerdpoll = UsersCachedPolls.Count(c => c.PollId == poll.Id && c.UserId == Session.GetHabbo().Id);
            if (useranswerdpoll >= poll.Limit)
                return;

            Session.SendMessage(new SendPollInvinteComposer(poll));
        }



        public static bool TryGetPoll(int Id, out Poll poll)
        {
            poll = GetPoll(Id);

            return poll != null;

        }

        public static void DeclinePoll(int userid, int pollid)
        {
            UsersDeclinedPolls.Add(new UserPollDecline(userid, pollid, (int)RocketEmulador.GetUnixTimestamp()));
        }

        public static bool IsDeclined(int userid, int pollid)
        {
            var p = UsersDeclinedPolls.FirstOrDefault(c => c.UserId == userid && c.PollId == pollid);
            if (p == null)
                return false;

            if (p.CanInvinteAgain)
            {
                UsersDeclinedPolls.Remove(p);
                return false;
            }

            return true;
        }

        public static Poll GetPoll(int id)
        {
            return Polls.FirstOrDefault(c => c.Id == id);
        }

        public static Poll GetPoll(string code)
        {
            return Polls.FirstOrDefault(c => c.CodeName == code);
        }

        public static Poll GetPollByRoomId(int roomid)
        {
            return Polls.FirstOrDefault(c => c.RoomId == roomid);
        }

        public static Poll GetPollByCode(string code)
        {
            return Polls.FirstOrDefault(c => c.CodeName == code);
        }


        public static void RemovePoll(Poll Poll)
        {
            Polls.Remove(Poll);
        }
        #endregion


        #region Users Answered Polls

        #endregion


        #region Users Answering Polls

        public static void StartUserAnsweringPoll(int userid, int pollid)
        {
            if (GetPoll(pollid) == null)
                return;
            RemoveUserAnsweringPoll(userid);
            UsersAnsweringPolls.Add(userid, new UserPollData(userid, pollid));
        }

        public static UserPollData GetUserAnsweringPoll(int userid, int pollid)
        {
            if (UsersAnsweringPolls.ContainsKey(userid))
            {
                var usera = UsersAnsweringPolls[userid];
                if (usera.PollId != pollid)
                {
                    UsersAnsweringPolls.Remove(userid);
                    return null;
                }

                return usera;
            }
            return null;
        }

        public static void RemoveUserAnsweringPoll(int userid)
        {
            if (UsersAnsweringPolls.ContainsKey(userid))
                UsersAnsweringPolls.Remove(userid);
        }

        public static void FinishuserAnsweringPoll(int userid)
        {
            if (!UsersAnsweringPolls.ContainsKey(userid))
                return;

            var userapoll = UsersAnsweringPolls[userid];

            userapoll.Finish();
            UsersAnsweringPolls.Remove(userid);
            UsersCachedPolls.Add(userapoll);
        }

        #endregion;

        public static bool Update(GameClient Session, string[] Params)
        {
            FlushData();
            Session.SendNotification("Polls Manager Reloaded");
            return true;
        }

        public static void FlushData()
        {
            Polls.Clear();
            DataTable Table1;
            DataTable Table2;
            using (var adap = RocketEmulador.GetDatabaseManager().GetQueryReactor())
            {
                adap.SetQuery("SELECT * FROM server_polls");
                Table1 = adap.getTable();
            }

            foreach (DataRow row in Table1.Rows)
            {
                var Poll = new Poll(row);
                Polls.Add(Poll);
            }

            using (var adap = RocketEmulador.GetDatabaseManager().GetQueryReactor())
            {
                adap.SetQuery("SELECT * FROM server_polls_users");
                Table1 = adap.getTable();
            }

            using (var adap = RocketEmulador.GetDatabaseManager().GetQueryReactor())
            {
                adap.SetQuery("SELECT * FROM server_polls_users_answers");
                Table2 = adap.getTable();
            }

            foreach (DataRow Row in Table1.Rows)
            {
                UsersCachedPolls.Add(new UserPollData(Row, Table2));
            }


        }
    }
}
