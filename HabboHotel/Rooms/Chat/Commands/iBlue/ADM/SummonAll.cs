using System;
using System.Linq;
using System.Text;
using System.Runtime.CompilerServices;
using Rocket.HabboHotel.GameClients;
namespace Rocket.HabboHotel.Rooms.Chat.Commands.iBlue.ADM
{
    class SummonAll : IChatCommand
    {
        public string PermissionRequired
        {
            get { return "comando_mod"; }
        }

        public string Parameters
        {
            get { return "(username)"; }
        }

        public string Description
        {
            get { return "Traga todos os usuários para o quarto."; }
        }

        public void Execute(GameClients.GameClient Session, Rooms.Room Room, string[] Params)
        {
            foreach (GameClient Client in RocketEmulador.GetGame().GetClientManager().GetClients.ToList())
            {
                if (Client == null || Client.GetHabbo() == null || Client.GetHabbo().Username == Session.GetHabbo().Username)
                    continue;

                {
                    Client.GetHabbo().PrepareRoom(Session.GetHabbo().CurrentRoomId, "");
                }
            }
            Session.SendWhisper("Todos os usuários do hotel foram trazidos para o seu quarto", 34);
        }
    }
}