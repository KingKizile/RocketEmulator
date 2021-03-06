﻿using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;

using Rocket.HabboHotel.Rooms;
using Rocket.HabboHotel.GameClients;
using Rocket.Communication.Packets.Outgoing.Rooms.Engine;
using Rocket.Database.Interfaces;


namespace Rocket.HabboHotel.Rooms.Chat.Commands.iBlue.USERS.Extra
{
    class CopiarComando : IChatCommand
    {
        public string PermissionRequired
        {
            get { return "comando_users"; }
        }

        public string Parameters
        {
            get { return "%username%"; }
        }

        public string Description
        {
            get { return "Liking someone elses swag? Copy it!"; }
        }

        public void Execute(GameClients.GameClient Session, Rooms.Room Room, string[] Params)
        {
            if (Params.Length == 1)
            {
                Session.SendNotification("Please enter the username of the user you wish to mimic.");
                return;
            }

            GameClient TargetClient = RocketEmulador.GetGame().GetClientManager().GetClientByUsername(Params[1]);
            if (TargetClient == null)
            {
                Session.SendNotification("An error occoured whilst finding that user, maybe they're not online.");
                return;
            }

            if (!TargetClient.GetHabbo().AllowMimic)
            {
                Session.SendNotification("Oops, you cannot mimic this user - sorry!");
                return;
            }

            RoomUser TargetUser = Session.GetHabbo().CurrentRoom.GetRoomUserManager().GetRoomUserByHabbo(TargetClient.GetHabbo().Id);
            if (TargetUser == null)
            {
                Session.SendNotification("An error occoured whilst finding that user, maybe they're not online or in this room.");
                return;
            }

            Session.GetHabbo().Gender = TargetUser.GetClient().GetHabbo().Gender;
            Session.GetHabbo().Look = TargetUser.GetClient().GetHabbo().Look;

            using (IQueryAdapter dbClient = RocketEmulador.GetDatabaseManager().GetQueryReactor())
            {
                dbClient.SetQuery("UPDATE `users` SET `gender` = @gender, `look` = @look WHERE `id` = @id LIMIT 1");
                dbClient.AddParameter("gender", Session.GetHabbo().Gender);
                dbClient.AddParameter("look", Session.GetHabbo().Look);
                dbClient.AddParameter("id", Session.GetHabbo().Id);
                dbClient.RunQuery();
            }

            RoomUser User = Room.GetRoomUserManager().GetRoomUserByHabbo(Session.GetHabbo().Id);
            if (User != null)
            {
                Session.SendMessage(new UserChangeComposer(User, true));
                Room.SendMessage(new UserChangeComposer(User, false));
            }
        }
    }
}