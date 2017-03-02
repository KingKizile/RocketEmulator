using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;

//using Rocket.HabboHotel.Support;
//using Rocket.HabboHotel.Rooms.Chat.Moderation;
using Rocket.Communication.Packets.Outgoing.Moderation;
using Rocket.HabboHotel.Moderation;

namespace Rocket.Communication.Packets.Incoming.Moderation
{
    class ReportCameraPhotoEvent : IPacketEvent
    {
        public void Parse(HabboHotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            if (Session == null || Session.GetHabbo() == null)
                return;

           // if (RocketEmulador.GetGame().GetModerationManager().UsersHasPendingTicket(Session.GetHabbo().Id))
            {
                Session.SendMessage(new BroadcastMessageAlertComposer("You currently already have a pending ticket, please wait for a response from a moderator."));
                return;
            }

            int photoId;

            if (!int.TryParse(Packet.PopString(), out photoId))
            {
                return;
            }

            int roomId = Packet.PopInt();
            int creatorId = Packet.PopInt();
            int categoryId = Packet.PopInt();

            //RocketEmulador.GetGame().GetModerationManager().SendNewTicket(Session, categoryId, creatorId, "", new List<string>(), (int) ModerationSupportTicketType.PHOTO, photoId);
            RocketEmulador.GetGame().GetClientManager().ModAlert("A new support ticket has been submitted!");
        }
    }
}