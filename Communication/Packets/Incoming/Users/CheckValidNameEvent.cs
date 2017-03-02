﻿using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;

using Rocket.Communication.Packets.Outgoing.Users;

using Rocket.Database.Interfaces;


namespace Rocket.Communication.Packets.Incoming.Users
{
    class CheckValidNameEvent : IPacketEvent
    {
        public void Parse(HabboHotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            bool InUse = false;
            string Name = Packet.PopString();

            using (IQueryAdapter dbClient = RocketEmulador.GetDatabaseManager().GetQueryReactor())
            {
                dbClient.SetQuery("SELECT COUNT(0) FROM `users` WHERE `username` = @name LIMIT 1");
                dbClient.AddParameter("name", Name);
                InUse = dbClient.getInteger() == 1;
            }

            char[] Letters = Name.ToLower().ToCharArray();
            string AllowedCharacters = "abcdefghijklmnopqrstuvwxyz.,_-;:?!1234567890";

            foreach (char Chr in Letters)
            {
                if (!AllowedCharacters.Contains(Chr))
                {
                    Session.SendMessage(new NameChangeUpdateComposer(Name, 4));
                    return;
                }
            }

            if (RocketEmulador.GetGame().GetChatManager().GetFilter().IsFiltered(Name))
            {
                Session.SendMessage(new NameChangeUpdateComposer(Name, 4));
                return;
            }

            if (!Session.GetHabbo().GetPermissions().HasRight("mod_tool") && Name.ToLower().Contains("mod") || Name.ToLower().Contains("adm") || Name.ToLower().Contains("Hotel") || Name.ToLower().Contains("admin") || Name.ToLower().Contains("m0d"))
            {
                Session.SendMessage(new NameChangeUpdateComposer(Name, 4));
                return;
            }
            else if (!Name.ToLower().Contains("mod") && (Session.GetHabbo().Rank == 2 || Session.GetHabbo().Rank == 3))
            {
                Session.SendMessage(new NameChangeUpdateComposer(Name, 4));
                return;
            }
            else if (Name.Length > 15)
            {
                Session.SendMessage(new NameChangeUpdateComposer(Name, 3));
                return;
            }
            else if (Name.Length < 3)
            {
                Session.SendMessage(new NameChangeUpdateComposer(Name, 2));
                return;
            }
            else if (InUse)
            {
                ICollection<string> Suggestions = new List<string>();
                for (int i = 100; i < 103; i++)
                {
                    Suggestions.Add(i.ToString());
                }

                Session.SendMessage(new NameChangeUpdateComposer(Name, 5, Suggestions));
                return;
            }
            else
            {
                Session.SendMessage(new NameChangeUpdateComposer(Name, 0));
                return;
            }
        }
    }
}
