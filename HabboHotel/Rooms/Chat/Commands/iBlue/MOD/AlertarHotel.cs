using Rocket.Communication.Packets.Outgoing.Rooms.Notifications;

namespace Rocket.HabboHotel.Rooms.Chat.Commands.iBlue.MOD
{
    class AlertarHotel : IChatCommand
    {
        public string PermissionRequired
        {
            get { return "comando_mod"; }
        }

        public string Parameters
        {
            get { return "%mensagem%"; }
        }

        public string Description
        {
            get { return "Envie uma mensagem para todo o hotel."; }
        }

        public void Execute(GameClients.GameClient Session, Rooms.Room Room, string[] Params)

        {

            string nome = Room.Name;
            string Mensagem = CommandManager.MergeParams(Params, 1);


            RocketEmulador.GetGame().GetClientManager().SendMessage(new RoomNotificationComposer("alerta", "message", Mensagem ));
            return;
        }
    }
}

