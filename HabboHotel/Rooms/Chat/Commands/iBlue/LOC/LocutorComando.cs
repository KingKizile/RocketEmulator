using Rocket.Communication.Packets.Outgoing.Rooms.Notifications;

namespace Rocket.HabboHotel.Rooms.Chat.Commands.iBlue.LOC
{
    class LocutorComando : IChatCommand
    {
        public string PermissionRequired
        {
            get { return "comando_loc"; }
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

            string Mensagem = CommandManager.MergeParams(Params, 1);

            if (Params.Length == 1)

            {
                Session.SendWhisper("Por favor digite uma mensagem para enviar.",34);
                return;
            }

            RocketEmulador.GetGame().GetClientManager().SendMessage(new RoomNotificationComposer("locutor", "message", "O Locutor " + Session.GetHabbo().Username + " está na radio com a Programaação " + Mensagem ));
            return;
        }
    }
}

