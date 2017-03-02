namespace Rocket.HabboHotel.Rooms.Chat.Commands.iBlue.MOD
{
    class IgnorarSussurro : IChatCommand
    {
        public string PermissionRequired
        {
            get { return "comando_mod"; }
        }

        public string Parameters
        {
            get { return ""; }
        }

        public string Description
        {
            get { return "Allows you to ignore all of the whispers in the room, except from your own."; }
        }

        public void Execute(GameClients.GameClient Session, Rooms.Room Room, string[] Params)
        {
            Session.GetHabbo().IgnorePublicWhispers = !Session.GetHabbo().IgnorePublicWhispers;
            Session.SendWhisper("You're " + (Session.GetHabbo().IgnorePublicWhispers ? "now" : "no longer") + " ignoring public whispers!");
        }
    }
}
