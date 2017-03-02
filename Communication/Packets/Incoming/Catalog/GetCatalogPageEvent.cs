using System;

using Rocket.Communication.Packets.Outgoing.Catalog;
using Rocket.HabboHotel.Catalog;
using Rocket.HabboHotel.GameClients;
using Rocket.Communication.Packets.Incoming;

namespace Rocket.Communication.Packets.Incoming.Catalog
{
    public class GetCatalogPageEvent : IPacketEvent
    {
        public void Parse(GameClient Session, ClientPacket Packet)
        {
            int PageId = Packet.PopInt();
            int Something = Packet.PopInt();
            string CataMode = Packet.PopString();

            CatalogPage Page = null;
            if (!RocketEmulador.GetGame().GetCatalog().TryGetPage(PageId, out Page))
                return;

            if (!Page.Enabled || !Page.Visible || Page.MinimumRank > Session.GetHabbo().Rank || (Page.MinimumVIP > Session.GetHabbo().VIPRank && Session.GetHabbo().Rank == 1))
                return;

           Session.SendMessage(new CatalogPageComposer(Page, CataMode));
        }
    }
}