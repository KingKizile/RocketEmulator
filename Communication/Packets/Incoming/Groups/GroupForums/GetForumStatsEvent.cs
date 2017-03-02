using Rocket.Communication.Packets.Outgoing;
using Rocket.Communication.Packets.Outgoing.Groups;
using Rocket.HabboHotel.GameClients;
using Rocket.HabboHotel.Groups.Forums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rocket.Communication.Packets.Incoming.Groups
{
    class GetForumStatsEvent : IPacketEvent
    {
        public void Parse(GameClient Session, ClientPacket Packet)
        {
            var GroupForumId = Packet.PopInt();

            GroupForum Forum;
            if (!RocketEmulador.GetGame().GetGroupForumManager().TryGetForum(GroupForumId, out Forum))
            {
                Session.SendNotification("Oops, There was a problem. Contact Staff to resolve this.");
                return;
            }

            Session.SendMessage(new ForumDataComposer(Forum, Session));

        }
    }
}