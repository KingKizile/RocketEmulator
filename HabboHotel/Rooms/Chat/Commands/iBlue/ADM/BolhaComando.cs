using Rocket.HabboHotel.Rooms.Chat.Styles;

namespace Rocket.HabboHotel.Rooms.Chat.Commands.iBlue.ADM
{
    class BolhaComando : IChatCommand
    {
        public string PermissionRequired
        {
            get { return "comando_adm"; }
        }

        public string Parameters
        {
            get { return "%id%"; }
        }

        public string Description
        {
            get { return "Use a custom bubble to chat with."; }
        }

        public void Execute(GameClients.GameClient Session, Rooms.Room Room, string[] Params)
        {
            RoomUser User = Room.GetRoomUserManager().GetRoomUserByHabbo(Session.GetHabbo().Id);
            if (User == null)
                return;

            if (Params.Length == 1)
            {
                Session.SendWhisper("Escreva o ID da bolha, por favor.!",34);
                return;
            }

            int Bubble = 0;
            if (!int.TryParse(Params[1].ToString(), out Bubble))
            {
                Session.SendWhisper("Digite um número válido.",34);
                return;
            }

            ChatStyle Style = null;
            if (!RocketEmulador.GetGame().GetChatManager().GetChatStyles().TryGetStyle(Bubble, out Style) || (Style.RequiredRight.Length > 0 && !Session.GetHabbo().GetPermissions().HasRight(Style.RequiredRight)))
            {
                Session.SendWhisper("Opa você não tem acesso a essa bolha!",34);
                return;
            }

            User.LastBubble = Bubble;
            Session.GetHabbo().CustomBubbleId = Bubble;
            Session.SendWhisper("Bolha mudada para: " + Bubble);
        }
    }
}