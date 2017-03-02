using Rocket.Communication.Packets.Outgoing.Rooms.Polls;
using Rocket.Communication.Packets.Outgoing.Rooms.Polls.Quick;
using Rocket.HabboHotel.GameClients;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rocket.HabboHotel.Rooms.Polls.Quick
{
    public class QuickPoll
    {
        public int Id;
        public string Title { get; set; }
        public List<UserQuickPoll> UsersPolls { get; set; }
        public int TimeLen;
        public Room Room { get; set; }

        public int StartTime { get; set; }

        public int ReamigTime
        {
            get
            {
                return Math.Max(0, (StartTime) - (int)RocketEmulador.GetUnixTimestamp());
            }
        }

        public QuickPoll(int id, Room room, string Title, int length)
        {
            this.Title = Title;
            this.Id = id;
            TimeLen = length;
            this.Room = room;
            UsersPolls = new List<UserQuickPoll>();
        }

        public void Start()
        {
            StartTime = (int)RocketEmulador.GetUnixTimestamp() + TimeLen;
            this.PerformStart();
        }

        public void PerformStart()
        {
            Room.SendMessage(new MatchingPollComposer(this));
        }

        public void PerformStartTo(GameClient Session)
        {
            if (ReamigTime == 0 || Voted(Session.GetHabbo().Id))
                return;

            Session.SendMessage(new MatchingPollComposer(this));
        }

        public List<UserQuickPoll> PVotes
        {
            get
            {
                return UsersPolls.Where(c => c.Yes).ToList();
            }
        }

        public List<UserQuickPoll> NVotes
        {
            get
            {
                return UsersPolls.Where(c => c.No).ToList();
            }
        }

        public void Stop()
        {
            TimeLen = 0;
            StartTime = 0;



            /*var e = new Rocket.Communication.Packets.Outgoing.ServerPacket(1922);
            //e.WriteBoolean(true);
            e.WriteInteger(-2);
            e.WriteInteger(0);

            Room.SendMessage(e);*/
            //Room.SendMessage(new MatchingPollComposer(this));
        }

        public bool Voted(int Userid)
        {
            return UsersPolls.Any(c => c.UserId == Userid);
        }

        public void AddVote(int UserId, bool VoteYes)
        {
            UsersPolls.Add(new UserQuickPoll(UserId, VoteYes));
        }
    }
}
