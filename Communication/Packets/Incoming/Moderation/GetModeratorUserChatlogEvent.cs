﻿using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;

using Rocket.HabboHotel.Users;
using Rocket.HabboHotel.Support;
using Rocket.Communication.Packets.Outgoing.Moderation;

namespace Rocket.Communication.Packets.Incoming.Moderation
{
    class GetModeratorUserChatlogEvent : IPacketEvent
    {
        public void Parse(HabboHotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            if (Session == null || Session.GetHabbo() == null)
                return;

            if (!Session.GetHabbo().GetPermissions().HasRight("mod_tool"))
                return;

            int UserId = Packet.PopInt();
            Habbo Habbo = RocketEmulador.GetHabboById(UserId);

            if (Habbo == null)
            {
                Session.SendNotification("Oops, we couldn't find this user.");
                return;
            }

            try
            {
                Session.SendMessage(new ModeratorUserChatlogComposer(UserId));
            }
            catch { Session.SendNotification("Overflow :/"); }
        }
    }
}
