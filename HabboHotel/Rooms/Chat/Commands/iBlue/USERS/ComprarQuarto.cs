using System.Linq;
using System.Collections.Generic;
using Rocket.Database.Interfaces;
using Rocket.HabboHotel.Items;
using Rocket.Communication.Packets.Outgoing.Inventory.Purse;
using Rocket.Communication.Packets.Outgoing.Rooms.Session;
namespace Rocket.HabboHotel.Rooms.Chat.Commands.iBlue.USERS
{
    class ComprarQuarto : IChatCommand
    {
        public string PermissionRequired
        {
            get { return "comando_users"; }
        }

        public string Parameters
        {
            get { return "%offer%"; }
        }

        public string Description
        {
            get { return "Permite - lhe fazer um acordo , dizendo que o preço pedido vai comprar a sala[Ex. 1000c OU 200d]"; }
        }

        public void Execute(GameClients.GameClient Session, Rooms.Room CurrentRoom, string[] Params)
        {
      
            RoomUser User = CurrentRoom.GetRoomUserManager().GetRoomUserByHabbo(Session.GetHabbo().Id);
            RoomUser RoomOwner = CurrentRoom.GetRoomUserManager().GetRoomUserByHabbo(CurrentRoom.RoomData.OwnerId);
       
            if (User == null)
                return;

            if (RoomOwner == null)
            {
                User.GetClient().SendWhisper("Proprietário quarto não encontrado", 3);
                return;
            }
   
            if (!CurrentRoom.RoomData.roomForSale)
            {
                User.GetClient().SendWhisper("Você não pode comprar um quarto que o proprietário não quer vender", 3);
                return;
            }

        
            if (Params.Length == 1 || Params[1].Length < 2)
            {
                Session.SendWhisper("Por favor, indique uma oferta válida para o quarto [ Ex . 145000c OU 75000d ]", 3);
                return;
            }

            string ActualInput = Params[1];

            if (CurrentRoom.RoomData.OwnerId == User.HabboId)
            {
                Session.SendWhisper("Você não pode comprar o seu próprio quarto, apenas dizer :venderquarto 0c", 3);
                return;
            }

            string roomCostType = ActualInput.Substring(ActualInput.Length - 1);
            int roomCost;
            try
            {
                roomCost = int.Parse(ActualInput.Substring(0, ActualInput.Length - 1));
            }
            catch
            {
 
                User.GetClient().SendWhisper("É necessário introduzir uma oferta de sala válido", 3);
                return;
            }

            if (roomCost < 1 || roomCost > 10000000)
            {
                User.GetClient().SendWhisper("Oferta inválida, muito baixa ou muito alta", 3);
                return;
            }

            if (roomCost == CurrentRoom.RoomData.roomSaleCost && roomCostType == CurrentRoom.RoomData.roomSaleType)
            {
                if (roomCostType == "c")
                {
                    if (User.GetClient().GetHabbo().Credits >= roomCost)
                    {
                        NewRoomOwner(CurrentRoom, User, RoomOwner);
                    }
                    else
                    {
                        User.GetClient().SendWhisper("Você não tem créditos suficientes", 3);
                        return;
                    }
                }
                else if (roomCostType == "d")
                {
                    if (User.GetClient().GetHabbo().Diamonds >= roomCost)
                    {
                        NewRoomOwner(CurrentRoom, User, RoomOwner);
                    }
                    else
                    {
                        User.GetClient().SendWhisper("Você não tem diamantes suficientes", 3);
                        return;
                    }
                }
                else
                {
                    Session.SendWhisper("Você nunca deveria ter visto esse erro, entre em contato com a equipe.", 3);
                    return;
                }
            }
            else
            {
                MakeOffer(CurrentRoom, User, RoomOwner, roomCost, roomCostType);
            }

        }

        public void NewRoomOwner(Room RoomForSale, RoomUser BoughtRoomUser, RoomUser SoldRoomUser)
        {
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
                User.GetClient().SendNotification("<b> Alertar quarto </b>\r\rO quarto acaba de ser sido comprado por\r\r<b>" + BoughtRoomUser.GetClient().GetHabbo().Username + "</b>!");

            }
        }

        public void MakeOffer(Room RoomForSale, RoomUser OfferingUser, RoomUser RoomOwner, int OfferCost, string OfferType)
        {
            if (RoomOwner.RoomOfferPending)
            {
                OfferingUser.GetClient().SendWhisper("Este usuário tem uma oferta quarto pendente , por favor espere", 3);
                return;
            }
            if (OfferType == "c")
            {
                if (OfferingUser.GetClient().GetHabbo().Credits < OfferCost)
                {
                    OfferingUser.GetClient().SendWhisper("Você não tem que mesmo isso muitas créditos", 3);
                    return;
                }
            }
            else if (OfferType == "d")
            {
                if (OfferingUser.GetClient().GetHabbo().Diamonds < OfferCost)
                {
                    OfferingUser.GetClient().SendWhisper("Você não tem que mesmo isso muitas diamantes", 3);
                    return;
                }
            }
            string TheOffer = OfferCost + "" + OfferType;

            if (RoomOwner.RoomId == RoomForSale.RoomData.Id)
            {
                RoomOwner.GetClient().SendWhisper("Nova oferta de " + OfferingUser.GetUsername() + " para " + TheOffer + " :aceitar ou :rejeitar");
                RoomOwner.RoomOfferPending = true;
                RoomOwner.RoomOffer = TheOffer;
                RoomOwner.RoomOfferUser = OfferingUser.HabboId;
                OfferingUser.GetClient().SendWhisper("Oferta enviada!");
            }
            else
            {
                OfferingUser.GetClient().SendWhisper("Dono do quarto não foi encontrado.", 3);
                return;
            }
        }

    }
}