using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;

namespace Rocket.HabboHotel.Rooms.Chat.Commands.iBlue.USERS.Extra
{
    class StatusComando : IChatCommand
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
            get { return "Ver suas estatísticas atuais."; }
        }

        public void Execute(GameClients.GameClient Session, Rooms.Room Room, string[] Params)
        {
            double Minutes = Session.GetHabbo().GetStats().OnlineTime / 60;
            double Hours = Minutes / 60;
            int OnlineTime = Convert.ToInt32(Hours);
            string s = OnlineTime == 1 ? "" : "s";

            StringBuilder HabboInfo = new StringBuilder();
            HabboInfo.Append("Estatísticas da sua conta:\r\r");

            HabboInfo.Append("Informação de moeda:\r");
            HabboInfo.Append("Créditos: " + Session.GetHabbo().Credits + "\r");
            HabboInfo.Append("Duckets: " + Session.GetHabbo().Duckets + "\r");
            HabboInfo.Append("Diamantes: " + Session.GetHabbo().Diamonds + "\r");
            HabboInfo.Append("Tempo Online: " + OnlineTime + " Hour" + s + "\r");
            HabboInfo.Append("Respeitos: " + Session.GetHabbo().GetStats().Respect + "\r");
            HabboInfo.Append("Pontos: " + Session.GetHabbo().GOTWPoints + "\r\r");


            Session.SendNotification(HabboInfo.ToString());
        }
    }
}
