using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;

using Rocket.HabboHotel.Catalog;
using Rocket.Communication.Packets.Outgoing.Catalog;
using Rocket.HabboHotel.Catalog.Utilities;

namespace Rocket.Communication.Packets.Incoming.Catalog
{
    class GetCatalogOfferEvent : IPacketEvent
    {
        public void Parse(HabboHotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            int OfferId = Packet.PopInt();
            if (!RocketEmulador.GetGame().GetCatalog().ItemOffers.ContainsKey(OfferId))
                return;

            int PageId = RocketEmulador.GetGame().GetCatalog().ItemOffers[OfferId];


            CatalogPage Page;
            if (!RocketEmulador.GetGame().GetCatalog().TryGetPage(PageId, out Page))
                return;

            if (!Page.Enabled || !Page.Visible || Page.MinimumRank > Session.GetHabbo().Rank || (Page.MinimumVIP > Session.GetHabbo().VIPRank && Session.GetHabbo().Rank == 1))
                return;

            CatalogItem Item = null;
            if (!Page.ItemOffers.ContainsKey(OfferId))
                return;

            Item = (CatalogItem)Page.ItemOffers[OfferId];
            if (Item != null && ItemUtility.CanSelectAmount(Item))
                Session.SendMessage(new CatalogOfferComposer(Item));
        }
    }
}
