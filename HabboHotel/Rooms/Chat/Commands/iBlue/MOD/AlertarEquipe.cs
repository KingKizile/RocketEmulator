using Rocket.Communication.Packets.Outgoing.Moderation;

namespace Rocket.HabboHotel.Rooms.Chat.Commands.iBlue.MOD
{
    class AlertarEquipe : IChatCommand
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
            get { return "Envia uma mensagem digitada por você para membros do pessoal on-line atual."; }
        }

        public void Execute(GameClients.GameClient Session, Rooms.Room Room, string[] Params)
        {
            if (Params.Length == 1)
            {
                Session.SendWhisper("Escreva uma mensagem para enviar.");
                return;
            }

            string Message = CommandManager.MergeParams(Params, 1);
            RocketEmulador.GetGame().GetClientManager().StaffAlert(new BroadcastMessageAlertComposer("Mensagem da Equipe:\r\r" + Message + "\r\n" + "- " + Session.GetHabbo().Username));
            return;
        }
    }
}
