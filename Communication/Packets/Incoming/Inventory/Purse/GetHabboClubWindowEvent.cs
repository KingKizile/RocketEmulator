using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using Rocket.Communication.Packets.Outgoing.Inventory.Purse;

namespace Rocket.Communication.Packets.Incoming.Inventory.Purse
{
    class GetHabboClubWindowEvent : IPacketEvent
    {
        public void Parse(HabboHotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            //Session.SendNotification("Habbo Club is free for all members, enjoy!");
            Session.SendMessage(new HabboClubCenterInfoMessageComposer());
        }
    }
}
