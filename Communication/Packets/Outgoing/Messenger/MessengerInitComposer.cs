using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;

using Rocket.HabboHotel.Users;
using Rocket.HabboHotel.Users.Messenger;
using Rocket.HabboHotel.Users.Relationships;

namespace Rocket.Communication.Packets.Outgoing.Messenger
{
    class MessengerInitComposer : ServerPacket
    {
        public MessengerInitComposer()
            : base(ServerPacketHeader.MessengerInitMessageComposer)
        {
            base.WriteInteger(RocketGame.MessengerFriendLimit);//Friends max.
            base.WriteInteger(300);
            base.WriteInteger(800);
            //base.WriteInteger(1100);
            base.WriteInteger(0); // category count
            base.WriteBoolean(true);
        }
    }
}
