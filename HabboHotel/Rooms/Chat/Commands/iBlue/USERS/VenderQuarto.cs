using System.Linq;

namespace Rocket.HabboHotel.Rooms.Chat.Commands.iBlue.USERS
{
    class VenderQuarto : IChatCommand
    {
        public string PermissionRequired
        {
            get { return "comando_users"; }
        }

        public string Parameters
        {
            get { return "%valor%"; }
        }

        public string Description
        {
            get { return "Permite que você vender o seu quarto pelo o preço pedido [ Ex 100C ou 200D"; }
        }

        public void Execute(GameClients.GameClient Session, Rooms.Room CurrentRoom, string[] Params)
        {
            //Gets the current RoomUser
            RoomUser User = CurrentRoom.GetRoomUserManager().GetRoomUserByHabbo(Session.GetHabbo().Id);

            //If it grabs an invalid user somehow, it returns
            if (User == null)
                return;



            //If does not have any value or the length does not contain a number with letter
            if (Params.Length == 1 || Params[1].Length < 2)
            {
                Session.SendWhisper("Digite o valor do quarto[Ex. 145c OU 750d]", 3);
                return;
            }

            //Assigns the input to a variable
            string ActualInput = Params[1];

            //If they are not the current room owner
            if (CurrentRoom.RoomData.OwnerId != User.HabboId)
            {
                Session.SendWhisper("Você precisa ser o proprietário da sala!", 3);
                return;
            }
            string roomCostType = ActualInput.Substring(ActualInput.Length - 1);
            //Declares the variable to be assigned in the try statement if they entered a valid cost
            int roomCost;
            try
            {
                //Great! It's valid if it passes this try
                roomCost = int.Parse(ActualInput.Substring(0, ActualInput.Length - 1));
            }
            catch
            {
                //Nope, Invalid integer
                User.GetClient().SendWhisper("É necessário introduzir um custo quarto válida", 3);
                return;
            }

            //Forget it, no longer for sale
            if (roomCost == 0)
            {
                CurrentRoom.RoomData.roomForSale = false;
                CurrentRoom.RoomData.roomSaleCost = 0;
                CurrentRoom.RoomData.roomSaleType = "";
                Session.SendWhisper("Se o seu quarto estava à venda, é agora não mais a venda.", 3);
                User.RoomOfferPending = false;
                User.RoomOffer = "";
                return;
            }

            //Is the cost too low or too high?
            if (roomCost < 1 || roomCost > 10000000)
            {
                User.GetClient().SendWhisper("Inválida custo do quarto , muito baixa ou muito alta", 3);
                return;
            }

            //If the input is coins or diamonds, then it will set the room for sale.
            if (ActualInput.EndsWith("c") || ActualInput.EndsWith("d"))
            {
                CurrentRoom.RoomData.roomForSale = true;
                CurrentRoom.RoomData.roomSaleCost = roomCost;
                CurrentRoom.RoomData.roomSaleType = roomCostType;
            }
            else
            {
                Session.SendWhisper("Inválida custo [Ex. 600c OR 400d]", 3);
                return;
            }

            //Whispers to all of the room users that the room is for sale
            //RoomSellCommand Updated By Hamada Zipto
            foreach (RoomUser UserInRoom in CurrentRoom.GetRoomUserManager().GetRoomUsers().ToList())
            {
                if (UserInRoom != null)
                {
                    UserInRoom.GetClient().SendWhisper("Este quarto está à venda por " + roomCost + roomCostType + " diga :comprarquarto " + roomCost + roomCostType + " para adquirir o mesmo.",34);
                }
            }


        }
    }
}