using System.Linq;
using Rocket.HabboHotel.GameClients;

namespace Rocket.HabboHotel.Rooms.Chat.Commands.iBlue.MOD
{
    class DarEmblemaATodos : IChatCommand
    {
        public string PermissionRequired
        {
            get { return "comando_mod"; }
        }

        public string Parameters
        {
            get { return "%badge%"; }
        }

        public string Description
        {
            get { return "Give a badge to the entire hotel."; }
        }

        public void Execute(GameClients.GameClient Session, Rooms.Room Room, string[] Params)
        {
            if (Params.Length == 1)
            {
                Session.SendWhisper("Please enter the code of the badge you'd like to give to the entire hotel.");
                return;
            }

            foreach (GameClient Client in RocketEmulador.GetGame().GetClientManager().GetClients.ToList())
            {
                if (Client == null || Client.GetHabbo() == null || Client.GetHabbo().Username == Session.GetHabbo().Username)
                    continue;

                if (!Client.GetHabbo().GetBadgeComponent().HasBadge(Params[1]))
                {
                    Client.GetHabbo().GetBadgeComponent().GiveBadge(Params[1], true, Client);
                    Client.SendWhisper("You have just been given a badge!");
                }
                else
                    Client.SendWhisper(Session.GetHabbo().Username + " tried to give you a badge, but you already have it!");
            }

            Session.SendWhisper("You have successfully given every user in this hotel the " + Params[1] + " badge!");
        }
    }
}
