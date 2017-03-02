using Rocket.HabboHotel.Rooms;
using Rocket.HabboHotel.GameClients;
using Rocket.Communication.Packets.Outgoing.Rooms.Camera;
using Rocket.HabboHotel.Camera;
using Rocket.Communication.Packets.Outgoing.Inventory.Purse;
using Rocket.HabboHotel.Items;
using Rocket.Communication.Packets.Outgoing.Inventory.Furni;
using Rocket.Database.Interfaces;
using Rocket.Utilities;
using System;

namespace Rocket.Communication.Packets.Incoming.Rooms.Camera
{
    public class PurchasePhotoEvent : IPacketEvent
    {
        public void Parse(GameClient Session, ClientPacket Packet)
        {
            if (!Session.GetHabbo().InRoom || Session.GetHabbo().Credits<RocketEmulador.GetGame().GetCameraManager().PurchaseCoinsPrice || Session.GetHabbo().Duckets<RocketEmulador.GetGame().GetCameraManager().PurchaseDucketsPrice)
                return;

            Room Room = Session.GetHabbo().CurrentRoom;

            if (Room == null)
                return;

            RoomUser User = Room.GetRoomUserManager().GetRoomUserByHabbo(Session.GetHabbo().Id);

            if (User == null || User.LastPhotoPreview == null)
                return;

            CameraPhotoPreview preview = User.LastPhotoPreview;
            
            if (RocketEmulador.GetGame().GetCameraManager().PurchaseCoinsPrice > 0)
            {
                Session.GetHabbo().Credits -= RocketEmulador.GetGame().GetCameraManager().PurchaseCoinsPrice;
                Session.SendMessage(new CreditBalanceComposer(Session.GetHabbo().Credits));
            }

            if (RocketEmulador.GetGame().GetCameraManager().PurchaseDucketsPrice > 0)
            {
                Session.GetHabbo().Duckets -= RocketEmulador.GetGame().GetCameraManager().PurchaseDucketsPrice;
                Session.SendMessage(new HabboActivityPointNotificationComposer(Session.GetHabbo().Duckets, Session.GetHabbo().Duckets));
            }

            using (IQueryAdapter dbClient = RocketEmulador.GetDatabaseManager().GetQueryReactor())
            {
                dbClient.RunQuery("UPDATE `camera_photos` SET `file_state` = 'purchased' WHERE `id` = '" + preview.Id + "' LIMIT 1");
                Console.WriteLine(@"Oinhe wulles fez isso aq");
            }

            Item photoPoster = ItemFactory.CreateSingleItemNullable(RocketEmulador.GetGame().GetCameraManager().PhotoPoster, Session.GetHabbo(),
"{\"w\":\"" + StringCharFilter.EscapeJSONString(RocketEmulador.GetGame().GetCameraManager().GetPath(CameraPhotoType.PURCHASED, preview.Id, preview.CreatorId)) + "\", \"n\":\"" + StringCharFilter.EscapeJSONString(Session.GetHabbo().Username) + "\", \"s\":\"" + Session.GetHabbo().Id + "\", \"u\":\"" + preview.Id + "\", \"t\":\"" + (long)RocketEmulador.Now() + "\"}", "");

            if (photoPoster != null)
            {
                Session.GetHabbo().GetInventoryComponent().TryAddItem(photoPoster);

                Session.SendMessage(new FurniListAddComposer(photoPoster));
                Session.SendMessage(new FurniListUpdateComposer());
                Session.SendMessage(new FurniListNotificationComposer(photoPoster.Id, 1));

            }

            Session.SendMessage(new CameraPhotoPurchaseOkComposer());
        }
    }
}