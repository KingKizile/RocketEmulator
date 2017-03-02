using Rocket.Communication.Packets.Outgoing.Notifications;
using Rocket.HabboHotel.GameClients;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rocket.Communication.Packets.Incoming.Moderation
{
    class AmbassadorAlert : IPacketEvent
    {
        public void Parse(HabboHotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            if (Session.GetHabbo().Rank < RocketEmulador.AmbassadorMinRank) return;
            int userId = Packet.PopInt();
            GameClient user = RocketEmulador.GetGame().GetClientManager().GetClientByUserID(userId);
            if (user == null) return;
            user.SendMessage(new SuperNotificationComposer("", "${notification.ambassador.alert.warning.title}", "${notification.ambassador.alert.warning.message}", "", ""));
        }
    }
}
