using Rocket.HabboHotel.Rooms;
using Rocket.Communication.Packets.Outgoing.Rooms.Engine;
using Rocket.Communication.Packets.Outgoing.Rooms.AI.Pets;
using Rocket.HabboHotel.Catalog.Utilities;
using Rocket.HabboHotel.Items;
using Rocket.Communication.Packets.Outgoing.Inventory.Furni;
using Rocket.Communication.Packets.Outgoing.Catalog;
using Rocket.Database.Interfaces;
using System.Drawing;
using System.Runtime.CompilerServices;


namespace Rocket.Communication.Packets.Incoming.Rooms.AI.Pets.Horse
{
    class RemoveSaddleFromHorseEvent : IPacketEvent
    {
        public void Parse(HabboHotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            if (!Session.GetHabbo().InRoom)
                return;


            Room Room = null;


            if (!RocketEmulador.GetGame().GetRoomManager().TryGetRoom(Session.GetHabbo().CurrentRoomId, out Room))
                return;


            RoomUser PetUser = null;


            if (!Room.GetRoomUserManager().TryGetPet(Packet.PopInt(), out PetUser))
                return;


            //Fetch the furniture Id for the pets current saddle.
            int SaddleId = ItemUtility.GetSaddleId(PetUser.PetData.Saddle);


            //Remove the saddle from the pet.
            PetUser.PetData.Saddle = 0;


            using (IQueryAdapter dbClient = RocketEmulador.GetDatabaseManager().GetQueryReactor())
            {
                dbClient.RunQuery("UPDATE `bots_petdata` SET `have_saddle` = 0 WHERE `id` = '" + PetUser.PetData.PetId + "' LIMIT 1");
            }




            //When removing Saddle From Horse the user gets down the horse
            if (PetUser.RidingHorse)
            {
                RoomUser UserRiding = Room.GetRoomUserManager().GetRoomUserByVirtualId(PetUser.HorseID);
                if (UserRiding != null)
                {
                    UserRiding.RidingHorse = false;
                    PetUser.RidingHorse = false;
                    UserRiding.ApplyEffect(-1);
                    UserRiding.MoveTo(new Point(UserRiding.X + 1, UserRiding.Y + 1));
                }
                else
                    PetUser.RidingHorse = false;
            }


            ItemData ItemData = null;


            if (!RocketEmulador.GetGame().GetItemManager().GetItem(SaddleId, out ItemData))
                return;


            //Creates the item for the user
            Item Item = ItemFactory.CreateSingleItemNullable(ItemData, Session.GetHabbo(), "", "", 0, 0, 0);


            if (Item != null)
            {
                Session.GetHabbo().GetInventoryComponent().TryAddItem(Item);
                Session.SendMessage(new FurniListNotificationComposer(Item.Id, 1));
                Session.SendMessage(new PurchaseOKComposer());
                Session.SendMessage(new FurniListAddComposer(Item));
                Session.SendMessage(new FurniListUpdateComposer());
            }


            Room.SendMessage(new UsersComposer(PetUser));
            Room.SendMessage(new PetHorseFigureInformationComposer(PetUser));
        }
    }
}