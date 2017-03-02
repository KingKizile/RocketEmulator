using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using Rocket.Database.Interfaces;
using Rocket.HabboHotel.Users;

namespace Rocket.Communication.Packets.Incoming.Moderation
{
    class ModerationMuteEvent : IPacketEvent
    {
        public void Parse(HabboHotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            if (Session == null || Session.GetHabbo() == null || !Session.GetHabbo().GetPermissions().HasRight("mod_mute"))
                return;
            int UserId = Packet.PopInt();
            string Message = Packet.PopString();
            double Length = (Packet.PopInt() * 60);
            //string Unknown1 = Packet.PopString();
            //string Unknown2 = Packet.PopString();
            Habbo Habbo = null;
            Habbo = RocketEmulador.GetHabboById(UserId);
            if (Habbo == null)
            {
                Session.SendNotification("An error occoured whilst finding that user in the database.");
                return;
            }

            if (Habbo.GetPermissions().HasRight("mod_mute") && !Session.GetHabbo().GetPermissions().HasRight("mod_mute_any"))
            {
                Session.SendNotification("Oops, you cannot mute that user.");
                return;
            }

            using (IQueryAdapter dbClient = RocketEmulador.GetDatabaseManager().GetQueryReactor())
            {
                dbClient.RunQuery("UPDATE `users` SET `time_muted` = '" + Length + "' WHERE `id` = '" + Habbo.Id + "' LIMIT 1");
            }

            if (Habbo.GetClient() != null)
            {
                Length = 3600;
                Habbo.TimeMuted = Length;
                Habbo.GetClient().SendNotification("Você foi mutado por um moderador: " + Message + " e durará " + Length + " segundos.");
            }
        }
    }
}

