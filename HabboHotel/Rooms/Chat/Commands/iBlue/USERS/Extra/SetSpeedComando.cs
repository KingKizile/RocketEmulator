﻿using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;

namespace Rocket.HabboHotel.Rooms.Chat.Commands.iBlue.USERS.Extra
{
    class SetSpeedComando : IChatCommand
    {
        public string PermissionRequired
        {
            get { return "comando_users"; }
        }

        public string Parameters
        {
            get { return "%value%"; }
        }

        public string Description
        {
            get { return "Set the speed of the rollers in the current room."; }
        }

        public void Execute(GameClients.GameClient Session, Rooms.Room Room, string[] Params)
        {
            if (!Room.CheckRights(Session, true))
                return;

            if (Params.Length == 1)
            {
                Session.SendNotification("Please enter a value for the roller speed.");
                return;
            }

            int Speed;
            if (int.TryParse(Params[1], out Speed))
            {
                Session.GetHabbo().CurrentRoom.GetRoomItemHandler().SetSpeed(Speed);
            }
            else
                Session.SendNotification("Invalid amount, please enter a valid number.");
        }
    }
}