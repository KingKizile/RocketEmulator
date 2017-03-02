using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using Rocket.Database.Interfaces;


namespace Rocket.HabboHotel.Rooms.Chat.Commands.iBlue.USERS.Extra
{
    class MutarBots : IChatCommand
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
            get { return "Ignorar o bate-papo bot ou ative-a novamente."; }
        }

        public void Execute(GameClients.GameClient Session, Rooms.Room Room, string[] Params)
        {
            Session.GetHabbo().AllowBotSpeech = !Session.GetHabbo().AllowBotSpeech;
            using (IQueryAdapter dbClient = RocketEmulador.GetDatabaseManager().GetQueryReactor())
            {
                dbClient.RunQuery("UPDATE `users` SET `bots_muted` = '" + ((Session.GetHabbo().AllowBotSpeech) ? 1 : 0) + "' WHERE `id` = '" + Session.GetHabbo().Id + "' LIMIT 1");
            }

            if (Session.GetHabbo().AllowBotSpeech)
                Session.SendNotification("Mudança bem sucedida, você já não pode ver discurso de bots.");
            else
                Session.SendNotification("Mudança bem sucedida, agora você pode ver o discurso de bots.");
        }
    }
}
