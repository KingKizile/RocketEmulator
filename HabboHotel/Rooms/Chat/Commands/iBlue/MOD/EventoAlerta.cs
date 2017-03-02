using Rocket.Communication.Packets.Outgoing.Rooms.Notifications;
using Rocket.HabboHotel.GameClients;
using System.Linq;

namespace Rocket.HabboHotel.Rooms.Chat.Commands.iBlue.MOD
{
    internal class EventoAlerta : IChatCommand
    {
        public string PermissionRequired
        {
            get
            {
                return "comando_mod";
            }
        }
        public string Parameters
        {
            get
            {
                return "%message%";
            }
        }
        public string Description
        {
            get
            {
                return "Enviar um alerta de evento";
            }
        }
        public void Execute(GameClient Session, Room Room, string[] Params)
        {
            if (Session != null)
            {
                if (Room != null)
                {
                    if (Params.Length == 1)
                    {
                        Session.SendWhisper("Por favor, digite uma mensagem para enviar.");
                        return;
                    }
                    foreach (GameClient client in RocketEmulador.GetGame().GetClientManager().GetClients.ToList())
                        if (client.GetHabbo().AllowEvents == true)
                        {
                            string Message = CommandManager.MergeParams(Params, 1);

                            client.SendMessage(new RoomNotificationComposer("Está acontecendo um evento!",
                                 "Está acontecendo um novo jogo realizado pela equipe Staff! <br><br>Este, tem o intuito de proporcionar um entretenimento a mais para os usuários!<br><br>Evento: <b>" + Message +
                                 "</b><br>Por: <b>" + Session.GetHabbo().Username +
                                 "</b> <br><br>Caso deseje participar, clique no botão abaixo! Para desativar os alertas de eventos digite: :alertas",

                                 "/fig/" + Session.GetHabbo().Look + "", "Participar do Evento", "event:navigator/goto/" + Session.GetHabbo().CurrentRoomId));
                        }
                        else
                            client.SendWhisper("Parece que está havendo um novo evento em nosso hotel. Para reativar as mensagens de eventos digite :alertas", 1);

                }
            }
        }
    }
}