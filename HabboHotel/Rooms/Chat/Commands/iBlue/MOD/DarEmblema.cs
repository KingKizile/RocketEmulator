using Rocket.HabboHotel.GameClients;

namespace Rocket.HabboHotel.Rooms.Chat.Commands.iBlue.MOD
{
    class DarEmblema : IChatCommand
    {
        public string PermissionRequired
        {
            get { return "comando_mod"; }
        }

        public string Parameters
        {
            get { return "%username% %badge%"; }
        }

        public string Description
        {
            get { return "Dê um Emblema para outro usuário."; }
        }

        public void Execute(GameClients.GameClient Session, Rooms.Room Room, string[] Params)
        {
            if (Params.Length != 3)
            {
                Session.SendWhisper("Por favor digite um nome de usuário e o código do Emblema que você gostaria de dar!");
                return;
            }

            GameClient TargetClient = RocketEmulador.GetGame().GetClientManager().GetClientByUsername(Params[1]);
            if (TargetClient != null)
            {
                if (!TargetClient.GetHabbo().GetBadgeComponent().HasBadge(Params[2]))
                {
                    TargetClient.GetHabbo().GetBadgeComponent().GiveBadge(Params[2], true, TargetClient);
                    if (TargetClient.GetHabbo().Id != Session.GetHabbo().Id)
                        TargetClient.SendWhisper("Você Recebeu Um Emblema");
                    else
                        Session.SendWhisper("Você Deu com êxito o Emblema " + Params[2] + "!");
                }
                else
                    Session.SendWhisper("Opa, o usuário já tem este Emblema (" + Params[2] + ") !");
                return;
            }
            else
            {
                Session.SendWhisper("Opa, não encontramos esse usuário alvo!");
                return;
            }
        }
    }
}
