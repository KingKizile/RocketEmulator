using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using Rocket.Database.Interfaces;


namespace Rocket.HabboHotel.Rooms.Chat.Commands.iBlue.USERS.Extra
{
    class SetMaxComando : IChatCommand
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
            get { return "Set the visitor limit to the room."; }
        }

        public void Execute(GameClients.GameClient Session, Rooms.Room Room, string[] Params)
        {
            if (!Room.CheckRights(Session, true))
                return;

            if (Params.Length == 1)
            {
                Session.SendNotification("Please enter a value for the room visitor limit.");
                return;
            }

            int MaxAmount;
            if (int.TryParse(Params[1], out MaxAmount))
            {
                if (MaxAmount == 0)
                {
                    MaxAmount = 10;
                    Session.SendNotification("visitor amount too low, visitor amount has been set to 10.");
                }
                else if (MaxAmount > 200 && !Session.GetHabbo().GetPermissions().HasRight("override_command_setmax_limit"))
                {
                    MaxAmount = 200;
                    Session.SendNotification("visitor amount too high for your rank, visitor amount has been set to 200.");
                }
                else
                    Session.SendNotification("visitor amount set to " + MaxAmount + ".");

                Room.UsersMax = MaxAmount;
                Room.RoomData.UsersMax = MaxAmount;
                using (IQueryAdapter dbClient = RocketEmulador.GetDatabaseManager().GetQueryReactor())
                {
                    dbClient.RunQuery("UPDATE `rooms` SET `users_max` = " + MaxAmount + " WHERE `id` = '" + Room.Id + "' LIMIT 1");
                }
            }
            else
                Session.SendNotification("Invalid amount, please enter a valid number.");
        }
    }
}
