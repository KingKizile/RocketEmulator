using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rocket.Communication.Packets.Outgoing.Inventory.Achievements;
using Rocket.HabboHotel.GameClients;

namespace Rocket.HabboHotel.Rooms.Chat.Commands.iBlue.USERS.Extra
{
    class OnlineComando : IChatCommand
    {
        public string PermissionRequired
        {
            get { return "comando_users"; }
        }

        public string Parameters
        {
            get { return "%online%"; }
        }

        public string Description
        {
            get { return "Veja quantos usuários tem online"; }
        }
        string hotelName = RocketEmulador.RocketData().data["hotelname"];
        public void Execute(GameClients.GameClient Session, Rooms.Room Room, string[] Params)
        {
            int OnlineUsers = RocketEmulador.GetGame().GetClientManager().Count;

            Session.SendWhisper("Temos agora " + OnlineUsers + " usuários onlines, no " + hotelName + ".");
        }
    }
}
