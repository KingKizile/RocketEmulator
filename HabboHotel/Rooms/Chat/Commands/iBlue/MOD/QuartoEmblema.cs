using System.Linq;

namespace Rocket.HabboHotel.Rooms.Chat.Commands.iBlue.MOD
{
    class QuartoEmblema : IChatCommand
    {
        public string PermissionRequired
        {
            get { return "comando_mod"; }
        }

        public string Parameters
        {
            get { return "%badge%"; }
        }

        public string Description
        {
            get { return "De Emblema Para o Quarto"; }
        }

        public void Execute(GameClients.GameClient Session, Rooms.Room Room, string[] Params)
        {
            if (Params.Length == 1)
            {
                Session.SendWhisper("Por favor, digite o nome do Emblema que você gostaria de dar ao quarto.");
                return;
            }

            foreach (RoomUser User in Room.GetRoomUserManager().GetUserList().ToList())
            {
                if (User == null || User.GetClient() == null || User.GetClient().GetHabbo() == null)
                    continue;

                if (!User.GetClient().GetHabbo().GetBadgeComponent().HasBadge(Params[1]))
                {
                    User.GetClient().GetHabbo().GetBadgeComponent().GiveBadge(Params[1], true, User.GetClient());
                    User.GetClient().SendWhisper("Você Ganhou um Emblema!");
                }
                else
                    User.GetClient().SendWhisper(Session.GetHabbo().Username + " Tentei te dar um Emblema, mas você já tem!");
            }

            Session.SendWhisper("Vocês deram  Emblema com êxito a cada usuário nesta sala  " + Params[2] + " badge!");
        }
    }
}
