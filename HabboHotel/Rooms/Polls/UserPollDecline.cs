using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rocket.HabboHotel.Rooms.Polls
{
    public class UserPollDecline
    {
        public int UserId;
        public int PollId;
        public int Timestamp;

        public UserPollDecline(int userid, int poll, int time)
        {
            UserId = userid;
            PollId = poll;
            Timestamp = time;
        }

        public bool CanInvinteAgain
        {
            get
            {
                var i = ((int)RocketEmulador.GetUnixTimestamp()) - Timestamp;
                return i >= RoomPollManager.POLL_DECLINE_TIMEOUT;
            }
        }
    }
}
