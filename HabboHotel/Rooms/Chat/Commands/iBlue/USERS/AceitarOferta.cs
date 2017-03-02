using System.Linq;
using System.Collections.Generic;
using Rocket.Database.Interfaces;
using Rocket.HabboHotel.Items;
using Rocket.Communication.Packets.Outgoing.Inventory.Purse;
using Rocket.Communication.Packets.Outgoing.Rooms.Session;
namespace Rocket.HabboHotel.Rooms.Chat.Commands.iBlue.USERS
{
    class AceitarOferta : IChatCommand
    {
        public string PermissionRequired
        {
            get { return "comando_users"; }
        }

        public string Parameters
        {
            get { return ""; }
        }

        public string Description
        {
            get { return "Aceitar a atual oferta de sala de pé."; }
        }

        public void Execute(GameClients.GameClient Session, Rooms.Room CurrentRoom, string[] Params)
        {
            RoomUser RoomOwner = CurrentRoom.GetRoomUserManager().GetRoomUserByHabbo(Session.GetHabbo().Id);
            if (RoomOwner.RoomOfferPending)
            {
                if (RoomOwner.GetClient().GetHabbo().CurrentRoom.RoomData.roomForSale)
                {
                    if (RoomOwner.GetClient().GetHabbo().CurrentRoom.RoomData.OwnerId == RoomOwner.GetClient().GetHabbo().Id)
                    {
                        RoomUser OfferingUser = CurrentRoom.GetRoomUserManager().GetRoomUserByHabbo(RoomOwner.RoomOfferUser);
                        OfferingUser.GetClient().SendWhisper("Este utilizador aceita sua oferta, vendendo o quarto");
                        NewRoomOwner(RoomOwner.GetClient().GetHabbo().CurrentRoom, OfferingUser, RoomOwner);
                        RoomOwner.RoomOfferPending = false;
                        RoomOwner.RoomOfferUser = 0;
                        RoomOwner.RoomOffer = "";
                    }
                }
            }

        }

        public void NewRoomOwner(Room RoomForSale, RoomUser BoughtRoomUser, RoomUser SoldRoomUser)
        {
            //Pre-Emptive Things
            using (IQueryAdapter Adapter = RocketEmulador.GetDatabaseManager().GetQueryReactor())
            {
                Adapter.SetQuery("UPDATE rooms SET owner = @newowner WHERE id = @roomid");
                Adapter.AddParameter("newowner", BoughtRoomUser.HabboId);
                Adapter.AddParameter("roomid", RoomForSale.RoomData.Id);
                Adapter.RunQuery();

                Adapter.SetQuery("UPDATE items SET user_id = @newowner WHERE room_id = @roomid");
                Adapter.AddParameter("newowner", BoughtRoomUser.HabboId);
                Adapter.AddParameter("roomid", RoomForSale.RoomData.Id);
                Adapter.RunQuery();

                Adapter.SetQuery("DELETE FROM room_rights WHERE room_id = @roomid");
                Adapter.AddParameter("roomid", RoomForSale.RoomData.Id);
                Adapter.RunQuery();

                if (RoomForSale.Group != null)
                {
                    Adapter.SetQuery("SELECT id FROM groups WHERE room_id = @roomid");
                    Adapter.AddParameter("roomid", RoomForSale.RoomData.Id);

                    int GroupId = Adapter.getInteger();

                    if (GroupId > 0)
                    {
                        RoomForSale.Group.ClearRequests();

                        foreach (int MemberId in RoomForSale.Group.GetAllMembers)
                        {
                            RoomForSale.Group.DeleteMember(MemberId);

                            GameClients.GameClient MemberClient = RocketEmulador.GetGame().GetClientManager().GetClientByUserID(MemberId);

                            if (MemberClient == null)
                                continue;

                            if (MemberClient.GetHabbo().GetStats().FavouriteGroupId == GroupId)
                            {
                                MemberClient.GetHabbo().GetStats().FavouriteGroupId = 0;
                            }
                        }

                        Adapter.RunQuery("DELETE FROM `groups` WHERE `id` = '" + RoomForSale.Group.Id + "'");
                        Adapter.RunQuery("DELETE FROM `group_memberships` WHERE `group_id` = '" + RoomForSale.Group.Id + "'");
                        Adapter.RunQuery("DELETE FROM `group_requests` WHERE `group_id` = '" + RoomForSale.Group.Id + "'");
                        Adapter.RunQuery("UPDATE `rooms` SET `group_id` = '0' WHERE `group_id` = '" + RoomForSale.Group.Id + "' LIMIT 1");
                        Adapter.RunQuery("UPDATE `user_stats` SET `groupid` = '0' WHERE `groupid` = '" + RoomForSale.Group.Id + "' LIMIT 1");
                        Adapter.RunQuery("DELETE FROM `items_groups` WHERE `group_id` = '" + RoomForSale.Group.Id + "'");
                    }
                    RocketEmulador.GetGame().GetGroupManager().DeleteGroup(RoomForSale.Group.Id);
                    RoomForSale.RoomData.Group = null;
                    RoomForSale.Group = null;
                }
            }
            RoomForSale.RoomData.OwnerId = BoughtRoomUser.HabboId;
            RoomForSale.RoomData.OwnerName = BoughtRoomUser.GetClient().GetHabbo().Username;
            foreach (Item CurrentItem in RoomForSale.GetRoomItemHandler().GetWallAndFloor)
            {
                CurrentItem.UserID = BoughtRoomUser.HabboId;
                CurrentItem.Username = BoughtRoomUser.GetClient().GetHabbo().Username;
            }
            if (RoomForSale.RoomData.roomSaleType == "c")
            {
                BoughtRoomUser.GetClient().GetHabbo().Credits -= RoomForSale.RoomData.roomSaleCost;
                BoughtRoomUser.GetClient().SendMessage(new CreditBalanceComposer(BoughtRoomUser.GetClient().GetHabbo().Credits));
            }
            else if (RoomForSale.RoomData.roomSaleType == "d")
            {
                BoughtRoomUser.GetClient().GetHabbo().Diamonds -= RoomForSale.RoomData.roomSaleCost;
                BoughtRoomUser.GetClient().SendMessage(new HabboActivityPointNotificationComposer(BoughtRoomUser.GetClient().GetHabbo().Diamonds, 0, 5));
            }
            if (RoomForSale.RoomData.roomSaleType == "c")
            {
                SoldRoomUser.GetClient().GetHabbo().Credits += RoomForSale.RoomData.roomSaleCost;
                SoldRoomUser.GetClient().SendMessage(new CreditBalanceComposer(SoldRoomUser.GetClient().GetHabbo().Credits));
            }
            else if (RoomForSale.RoomData.roomSaleType == "d")
            {
                SoldRoomUser.GetClient().GetHabbo().Diamonds += RoomForSale.RoomData.roomSaleCost;
                SoldRoomUser.GetClient().SendMessage(new HabboActivityPointNotificationComposer(SoldRoomUser.GetClient().GetHabbo().Diamonds, 0, 5));
            }

            RoomForSale.RoomData.roomForSale = false;
            RoomForSale.RoomData.roomSaleCost = 0;
            RoomForSale.RoomData.roomSaleType = "";

            Room R = null;
            if (!RocketEmulador.GetGame().GetRoomManager().TryGetRoom(RoomForSale.Id, out R))
                return;
            List<RoomUser> UsersToReturn = RoomForSale.GetRoomUserManager().GetRoomUsers().ToList();
            RocketEmulador.GetGame().GetNavigator().Init();
            RocketEmulador.GetGame().GetRoomManager().UnloadRoom(R, true);
            foreach (RoomUser User in UsersToReturn)
            {
                if (User == null || User.GetClient() == null)
                    continue;

                User.GetClient().SendMessage(new RoomForwardComposer(RoomForSale.Id));
                User.GetClient().SendWhisper("<b> Alertar quarto </b>\r\rO quarto acaba de ser sido comprado por\r\r<b>" + BoughtRoomUser.GetClient().GetHabbo().Username + "</b>!");

            }
        }


    }
}