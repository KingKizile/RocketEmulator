using System;
using Rocket.Communication.Packets.Incoming;

using Rocket.HabboHotel.GameClients;
using Rocket.Communication.Packets.Outgoing.Catalog;
using Rocket.Communication.Packets.Outgoing.BuildersClub;

namespace Rocket.Communication.Packets.Incoming.Catalog
{
    public class GetCatalogIndexEvent : IPacketEvent
    {
        public void Parse(GameClient Session, ClientPacket Packet)
        {
            /*int Sub = 0;

            if (Session.GetHabbo().GetSubscriptionManager().HasSubscription)
            {
                Sub = Session.GetHabbo().GetSubscriptionManager().GetSubscription().SubscriptionId;
            }*/

            Session.SendMessage(new CatalogIndexComposer(Session, RocketEmulador.GetGame().GetCatalog().GetPages()));//, Sub));
            Session.SendMessage(new CatalogItemDiscountComposer());
            Session.SendMessage(new BCBorrowedItemsComposer());
        }
    }
}