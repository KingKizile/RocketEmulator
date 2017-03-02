﻿using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;

using Rocket.HabboHotel.Quests;

namespace Rocket.Communication.Packets.Incoming.Messenger
{
    class RequestBuddyEvent : IPacketEvent
    {
        public void Parse(HabboHotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            if (Session == null || Session.GetHabbo() == null || Session.GetHabbo().GetMessenger() == null)
                return;

            if (Session.GetHabbo().GetMessenger().RequestBuddy(Packet.PopString()))
                RocketEmulador.GetGame().GetQuestManager().ProgressUserQuest(Session, QuestType.SOCIAL_FRIEND);
        }
    }
}
