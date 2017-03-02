using Rocket.Database.Interfaces;

namespace Rocket.HabboHotel.Rooms.Chat.Commands.iBlue.USERS.Extra
{
    class DesativarComando : IChatCommand
    {
        public string PermissionRequired
        {
            get { return "comando_users"; }
        }

        public string Parameters
        {
            get { return ""; }
        }

        public string Description
        {
            get { return "Ativar ou desativar os comandos customs."; }
        }

        public void Execute(GameClients.GameClient Session, Rooms.Room Room, string[] Params)
        {
            Session.GetHabbo().AllowCommands = !Session.GetHabbo().AllowCommands;
            Session.SendWhisper("Você " + (Session.GetHabbo().AllowCommands == true ? "ativou" : "desativou") + " os todos os comandos.");

            using (IQueryAdapter dbClient = RocketEmulador.GetDatabaseManager().GetQueryReactor())
            {
                dbClient.SetQuery("UPDATE `users` SET `allow_commands` = @AllowCommands WHERE `id` = '" + Session.GetHabbo().Id + "'");
                dbClient.AddParameter("AllowCommands", RocketEmulador.BoolToEnum(Session.GetHabbo().AllowCommands));
                dbClient.RunQuery();
            }
        }
    }
}