using Rocket.HabboHotel.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rocket.HabboHotel.Rooms.Polls.Quick
{
    public class UserQuickPoll
    {
        public int UserId;
        public bool Yes;
        public bool No
        {
            get
            {
                return !Yes;
            }
        }

        public UserQuickPoll(int uid, bool yes)
        {
            UserId = uid;
            Yes = yes;
        }

        public Habbo GetHabbo()
        {
            return RocketEmulador.GetHabboById(UserId);
        }
    }
}
