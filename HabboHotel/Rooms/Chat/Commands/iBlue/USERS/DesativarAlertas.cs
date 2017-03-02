using Rocket.Database.Interfaces;

namespace Rocket.HabboHotel.Rooms.Chat.Commands.iBlue.USERS.Extra
{
    class DesativarAlertas : IChatCommand
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
            get { return "Ativar ou desativar mensagens de eventos."; }
        }

        public void Execute(GameClients.GameClient Session, Rooms.Room Room, string[] Params)
        {
            Session.GetHabbo().AllowEvents = !Session.GetHabbo().AllowEvents;
            Session.SendWhisper("Você " + (Session.GetHabbo().AllowEvents == true ? "ativou" : "desativou") + " os alertas de eventos com sucesso.");

            using (IQueryAdapter dbClient = RocketEmulador.GetDatabaseManager().GetQueryReactor())
            {
                dbClient.SetQuery("UPDATE `users` SET `allow_events` = @AllowEvents WHERE `id` = '" + Session.GetHabbo().Id + "'");
                dbClient.AddParameter("AllowEvents", RocketEmulador.BoolToEnum(Session.GetHabbo().AllowEvents));
                dbClient.RunQuery();
            }
        }
    }
}