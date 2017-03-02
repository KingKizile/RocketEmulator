using Rocket.Communication.Packets.Outgoing.Moderation;

namespace Rocket.HabboHotel.Rooms.Chat.Commands.iBlue.ADM
{
    class ManutencaoComando : IChatCommand
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
            get { return "Alerta de Hotel em Manutenção"; }
        }

        public void Execute(GameClients.GameClient Session, Rooms.Room Room, string[] Params)
        {
            if (Params.Length == 1)
            {
                Session.SendWhisper("Algo deu errado, tente novamente!",34);
                return;
            }

            string Message = CommandManager.MergeParams(Params, 1);
            RocketEmulador.GetGame().GetClientManager().SendMessage(new BroadcastMessageAlertComposer("<b><font color=\"#C50505\" size=\"15\">Hotel em manutenção</font><br></b>O Hotel vai fechar em 3 minutos. Para evitar confusão, aquisição de Mobis, HC e trocas estão desativadas. Agradecemos sua visita e estaremos de volta em 15 minutos."));

            return;
        }
    }
}
