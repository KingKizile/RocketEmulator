using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;

using Rocket.Communication.Packets.Incoming;
using Rocket.HabboHotel.Users;
using Rocket.HabboHotel.Navigator;
using Rocket.HabboHotel.GameClients;
using Rocket.Communication.Packets.Outgoing.Navigator;


namespace Rocket.Communication.Packets.Incoming.Navigator
{
    public class GetUserFlatCatsEvent : IPacketEvent
    {
        public void Parse(GameClient Session, ClientPacket Packet)
        {
            if (Session == null)
                return;

            ICollection<SearchResultList> Categories = RocketEmulador.GetGame().GetNavigator().GetFlatCategories();

            Session.SendMessage(new UserFlatCatsComposer(Categories, Session.GetHabbo().Rank));
        }
    }
}