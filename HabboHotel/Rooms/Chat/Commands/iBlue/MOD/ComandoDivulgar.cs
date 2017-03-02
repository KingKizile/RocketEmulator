using Rocket.Communication.Packets.Outgoing.Rooms.Notifications;

namespace Rocket.HabboHotel.Rooms.Chat.Commands.iBlue.MOD
{
    class ComandoDivulgar : IChatCommand
    {
        public string PermissionRequired
        {
            get { return "comando_mod"; }
        }

        public string Parameters
        {
            get { return ""; }
        }

        public string Description
        {
            get { return "Envie uma mensagem para todo o hotel."; }
        }

        public void Execute(GameClients.GameClient Session, Rooms.Room Room, string[] Params)

        {

            string nome = Room.Name;
            int online = RocketEmulador.GetGame().GetClientManager().Count;


            RocketEmulador.GetGame().GetClientManager().SendMessage(new RoomNotificationComposer("divulgar", "message", "Tudo bem pessoal? Temos exatamente " + online + " usuários online, vamos divulgar!"));
            return;
        }
    }
}

