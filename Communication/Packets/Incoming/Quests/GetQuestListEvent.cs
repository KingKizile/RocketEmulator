using System.Collections.Generic;
using Rocket.HabboHotel.GameClients;
using Rocket.HabboHotel.Quests;
using Rocket.Communication.Packets.Incoming;

namespace Rocket.Communication.Packets.Incoming.Quests
{
    public class GetQuestListEvent : IPacketEvent
    {
        public void Parse(GameClient Session, ClientPacket Packet)
        {
            RocketEmulador.GetGame().GetQuestManager().GetList(Session, null);
        }
    }
}