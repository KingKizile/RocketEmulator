namespace Rocket.HabboHotel.Rooms.Chat.Commands.iBlue.MOD
{
    class AlertarQuarto : IChatCommand
    {
        public string PermissionRequired
        {
            get { return "comando_mod"; }
        }

        public string Parameters
        {
            get { return "%message%"; }
        }

        public string Description
        {
            get { return "Envie uma mensagem para os usuários nesta sala."; }
        }

        public void Execute(GameClients.GameClient Session, Rooms.Room Room, string[] Params)
        {
            if (Params.Length == 1)
            {
                Session.SendWhisper("Por favor digite uma mensagem que você gostaria de enviar para a sala.");
                return;
            }

            if (!Session.GetHabbo().GetPermissions().HasRight("mod_alert") && Room.OwnerId != Session.GetHabbo().Id)
            {
                Session.SendWhisper("Você pode apenas mandar alerta em seu próprio quarto!");
                return;
            }

            string Message = CommandManager.MergeParams(Params, 1);
            foreach (RoomUser RoomUser in Room.GetRoomUserManager().GetRoomUsers())
            {
                if (RoomUser == null || RoomUser.GetClient() == null || Session.GetHabbo().Id == RoomUser.UserId)
                    continue;

                RoomUser.GetClient().SendWhisper(Session.GetHabbo().Username + " alertou o quarto com a seguinte mensagem:\n\n" + Message);
            }
            Session.SendWhisper("Mensagem enviada com sucesso para o quarto.");
        }
    }
}
