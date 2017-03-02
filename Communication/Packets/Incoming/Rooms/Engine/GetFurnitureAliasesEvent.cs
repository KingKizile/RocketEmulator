using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Rocket.Communication.Packets.Incoming;
using Rocket.HabboHotel.GameClients;
using Rocket.Communication.Packets.Outgoing.Rooms.Engine;

namespace Rocket.Communication.Packets.Incoming.Rooms.Engine
{
    class GetFurnitureAliasesEvent : IPacketEvent
    {
        public void Parse(GameClient Session, ClientPacket Packet)
        {
            Session.SendMessage(new FurnitureAliasesComposer());
        }
    }
}
