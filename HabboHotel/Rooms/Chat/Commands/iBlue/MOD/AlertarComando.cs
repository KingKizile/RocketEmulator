using Rocket.HabboHotel.GameClients;

namespace Rocket.HabboHotel.Rooms.Chat.Commands.iBlue.MOD
{
    class AlertarComando : IChatCommand
    {
        public string PermissionRequired
        {
            get { return "comando_mod"; }
        }

        public string Parameters
        {
            get { return "%username% %Messages%"; }
        }

        public string Description
        {
            get { return "Alerte um usuário com uma mensagem de sua escolha."; }
        }

        public void Execute(GameClients.GameClient Session, Rooms.Room Room, string[] Params)
        {
            if (Params.Length == 1)
            {
                Session.SendWhisper("Por favor, digite o nome de usuário do usuário que deseja alertar.");
                return;
            }

            GameClient TargetClient = RocketEmulador.GetGame().GetClientManager().GetClientByUsername(Params[1]);
            if (TargetClient == null)
            {
                Session.SendWhisper("An error occoured whilst finding that user, maybe they're not online.");
                return;
            }

            if (TargetClient.GetHabbo() == null)
            {
                Session.SendWhisper("An error occoured whilst finding that user, maybe they're not online.");
                return;
            }

            if (TargetClient.GetHabbo().Username == Session.GetHabbo().Username)
            {
                Session.SendWhisper("Há Você não.");
                return;
            }

            string Message = CommandManager.MergeParams(Params, 2);

            TargetClient.SendWhisper(Session.GetHabbo().Username + " Você alertou com a seguinte mensagem:\n\n" + Message);
            Session.SendNotification("Alerta enviado com êxito para " + TargetClient.GetHabbo().Username);
           
        }
    }
}
