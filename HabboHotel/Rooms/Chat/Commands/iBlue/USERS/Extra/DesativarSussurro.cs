using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;

namespace Rocket.HabboHotel.Rooms.Chat.Commands.iBlue.USERS.Extra
{
    class DesativarSussurro : IChatCommand
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
            get { return "Permite habilitar ou desabilitar a capacidade de receber sussurros."; }
        }

        public void Execute(GameClients.GameClient Session, Rooms.Room Room, string[] Params)
        {
            Session.GetHabbo().ReceiveWhispers = !Session.GetHabbo().ReceiveWhispers;
            Session.SendNotification("You're " + (Session.GetHabbo().ReceiveWhispers ? "Agora" : "Já não") + "pode receber sussurros!");
        }
    }
}
