using Rocket.Database.Interfaces;
using Rocket.HabboHotel.Users;
namespace Rocket.HabboHotel.Rooms.Chat.Commands.iBlue.MOD
{
    class MutarComando : IChatCommand
    {
        public string PermissionRequired
        {
            get { return "comando_mod"; }
        }

        public string Parameters
        {
            get { return "%username% %time%"; }
        }

        public string Description
        {
            get { return "Silencie o outro usuário para um determinado período de tempo."; }
        }

        public void Execute(GameClients.GameClient Session, Rooms.Room Room, string[] Params)
        {
            if (Params.Length == 1)
            {
                Session.SendWhisper("Por favor, digite um nome de usuário e uma hora válida em segundos (máximo 600, nada mais serão definido volta a 600).", 34);
                return;
            }

            Habbo Habbo = RocketEmulador.GetHabboByUsername(Params[1]);
            if (Habbo == null)
            {
                Session.SendWhisper("Não achei esse user na Database.", 34);
                return;
            }

            if (Habbo.GetPermissions().HasRight("mod_tool") && !Session.GetHabbo().GetPermissions().HasRight("mod_mute_any"))
            {
                Session.SendWhisper("Opa, você não pode silenciar aquele usuário.", 34);
                return;
            }

            double Time;
            if (double.TryParse(Params[2], out Time))
            {
                if (Time > 600 && !Session.GetHabbo().GetPermissions().HasRight("mod_mute_limit_override"))
                    Time = 600;

                using (IQueryAdapter dbClient = RocketEmulador.GetDatabaseManager().GetQueryReactor())
                {
                    dbClient.RunQuery("UPDATE `users` SET `time_muted` = '" + Time + "' WHERE `id` = '" + Habbo.Id + "' LIMIT 1");
                }

                if (Habbo.GetClient() != null)
                {
                    Habbo.TimeMuted = Time;
                    Habbo.GetClient().SendWhisper("Foi Mutado  " + Time + " segundos!");
                }

                Session.SendWhisper("Com sucesso tenha silenciado " + Habbo.Username + " for " + Time + " segundos.", 34);
            }
            else
                Session.SendWhisper("Por favor insira um inteiro válido.", 34);
        }
    }
}