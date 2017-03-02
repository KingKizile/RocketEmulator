﻿using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;

namespace Rocket.Communication.Packets.Incoming.Quests
{
    class StartQuestEvent : IPacketEvent
    {
        public void Parse(HabboHotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            int QuestId = Packet.PopInt();

            RocketEmulador.GetGame().GetQuestManager().ActivateQuest(Session, QuestId);
        }
    }
}
