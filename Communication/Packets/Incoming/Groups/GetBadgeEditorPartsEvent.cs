using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;

using Rocket.Communication.Packets.Outgoing.Groups;

namespace Rocket.Communication.Packets.Incoming.Groups
{
    class GetBadgeEditorPartsEvent : IPacketEvent
    {
        public void Parse(HabboHotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            Session.SendMessage(new BadgeEditorPartsComposer(
                RocketEmulador.GetGame().GetGroupManager().Bases,
                RocketEmulador.GetGame().GetGroupManager().Symbols,
                RocketEmulador.GetGame().GetGroupManager().BaseColours,
                RocketEmulador.GetGame().GetGroupManager().SymbolColours,
                RocketEmulador.GetGame().GetGroupManager().BackGroundColours));
       
        }
    }
}
