using Rocket.HabboHotel.GameClients;
using Rocket.HabboHotel.Groups.Forums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rocket.Communication.Packets.Outgoing.Groups
{
    class ForumsListDataComposer : ServerPacket
    {
        public ForumsListDataComposer(ICollection<GroupForum> Forums, GameClient Session, int ViewOrder = 0, int StartIndex = 0, int MaxLength = 20)
            : base(ServerPacketHeader.ForumsListDataMessageComposer)
        {
            base.WriteInteger(ViewOrder);
            base.WriteInteger(StartIndex);
            base.WriteInteger(StartIndex);

            base.WriteInteger(Forums.Count); // Liste Compte Forums

            foreach (var Forum in Forums)
            {
                var lastpost = Forum.GetLastPost();
                var isn = lastpost == null;
                base.WriteInteger(Forum.Id);
                base.WriteString(Forum.Name);
                base.WriteString(Forum.Description);
                base.WriteString(Forum.Group.Badge);
                base.WriteInteger(0);
                base.WriteInteger(0);
                base.WriteInteger(Forum.MessagesCount);
                base.WriteInteger(Forum.UnreadMessages(Session.GetHabbo().Id));
                base.WriteInteger(0);
                base.WriteInteger(!isn ? lastpost.GetAuthor().Id : 0);
                base.WriteString(!isn ? lastpost.GetAuthor().Username : "");
                base.WriteInteger(!isn ? (int)RocketEmulador.GetUnixTimestamp() - lastpost.Timestamp : 0);
            }
        }
    }
}