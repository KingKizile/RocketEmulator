﻿using Rocket.HabboHotel.GameClients;

namespace Rocket.HabboHotel.Rooms.Chat.Commands.iBlue.MOD.Extra
{
    class CongelarComando : IChatCommand
    {
        public string PermissionRequired
        {
            get { return "comando_mod"; }
        }

        public string Parameters
        {
            get { return "%username%"; }
        }

        public string Description
        {
            get { return "Prevent another user from walking."; }
        }

        public void Execute(GameClients.GameClient Session, Rooms.Room Room, string[] Params)
        {
            if (Params.Length == 1)
            {
                Session.SendWhisper("Please enter the username of the user you wish to freeze.");
                return;
            }

            GameClient TargetClient = RocketEmulador.GetGame().GetClientManager().GetClientByUsername(Params[1]);
            if (TargetClient == null)
            {
                Session.SendWhisper("An error occoured whilst finding that user, maybe they're not online.");
                return;
            }

            RoomUser TargetUser = Session.GetHabbo().CurrentRoom.GetRoomUserManager().GetRoomUserByHabbo(Params[1]);
            if (TargetUser != null)
                TargetUser.Frozen = true;

            Session.SendWhisper("Successfully froze " + TargetClient.GetHabbo().Username + "!");
        }
    }
}
