
using System.Collections.Generic;
using Rocket.HabboHotel.Users.Messenger;
using Rocket.HabboHotel.Cache;

namespace Rocket.Communication.Packets.Outgoing.Messenger
{
    class BuddyRequestsComposer : ServerPacket
    {
        public BuddyRequestsComposer(ICollection<MessengerRequest> requests) : base(ServerPacketHeader.BuddyRequestsMessageComposer)
        {
            base.WriteInteger(requests.Count);
            base.WriteInteger(requests.Count);

            foreach (MessengerRequest Request in requests)
            {
                base.WriteInteger(Request.From);
                base.WriteString(Request.Username);

                UserCache User = RocketEmulador.GetGame().GetCacheManager().GenerateUser(Request.From);
                base.WriteString(User != null ? User.Look : "");
            }
        }
    }
}
