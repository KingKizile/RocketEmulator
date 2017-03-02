using System.Linq;
using System.Collections.Generic;
using MoreLinq;
using Rocket.HabboHotel.Items;
using Rocket.Communication.Packets.Outgoing.Inventory.Furni;



namespace Rocket.Communication.Packets.Incoming.Inventory.Furni
{
    class RequestFurniInventoryEvent : IPacketEvent
    {
        public void Parse(HabboHotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            IEnumerable<Item> Items = Session.GetHabbo().GetInventoryComponent().GetWallAndFloor;

            int page = 0;
            int pages = ((Items.Count() - 1) / 700) + 1;

            if (Items.Count() == 0)
            {
                Session.SendMessage(new FurniListComposer(Items.ToList(), 1, 0));
            }
            else
            {
                foreach (ICollection<Item> batch in Items.Batch(700))
                {
                    Session.SendMessage(new FurniListComposer(batch.ToList(), pages, page));

                    page++;
                }
            }
        }
    }
}
