using Rocket.Communication.Packets.Outgoing.Rooms.Notifications;

namespace Rocket.HabboHotel.Rooms.Chat.Commands.iBlue.ADM
{
    class AlertaComLink : IChatCommand
    {
        public string PermissionRequired
        {
            get { return "comando_adm"; }
        }

        public string Parameters
        {
            get { return "%message%"; }
        }

        public string Description
        {
            get { return "Envie uma mensagem para todo o hotel, com um link."; }
        }

        public void Execute(GameClients.GameClient Session, Rooms.Room Room, string[] Params)
        {
            if (Params.Length == 2)
            {
                Session.SendWhisper("Por favor digite uma mensagem e uma URL para enviar...",34);
                return;
            }

            string URL = Params[1];

            string Message = CommandManager.MergeParams(Params, 2);
            RocketEmulador.GetGame().GetClientManager().SendMessage(new RoomNotificationComposer("Mensagem da equipe", Message + "\r\n" + "- " + Session.GetHabbo().Username, "CLIQUE AQUI", URL, URL));
            return;
        }
    }
}
