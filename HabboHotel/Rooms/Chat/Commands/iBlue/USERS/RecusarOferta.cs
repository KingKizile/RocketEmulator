namespace Rocket.HabboHotel.Rooms.Chat.Commands.iBlue.USERS
{
    class RecusarOferta : IChatCommand
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
            get { return "Recusar a atual oferta de sala de pé"; }
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
                        OfferingUser.GetClient().SendWhisper("Este usuário recusou sua oferta.");
                        RoomOwner.RoomOfferPending = false;
                        RoomOwner.RoomOfferUser = 0;
                        RoomOwner.RoomOffer = "";
                    }
                }
            }
        }
    }
}