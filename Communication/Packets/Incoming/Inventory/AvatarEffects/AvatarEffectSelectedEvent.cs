﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Rocket.Communication.Packets.Incoming;
using Rocket.HabboHotel.Rooms;
using Rocket.HabboHotel.Users;
using Rocket.HabboHotel.GameClients;

namespace Rocket.Communication.Packets.Incoming.Inventory.AvatarEffects
{
    class AvatarEffectSelectedEvent : IPacketEvent
    {
        public void Parse(GameClient Session, ClientPacket Packet)
        {
            int EffectId = Packet.PopInt();
            if (EffectId < 0)
                EffectId = 0;

            if (!Session.GetHabbo().InRoom)
                return;

            Room Room = Session.GetHabbo().CurrentRoom;
            if (Room == null)
                return;

            RoomUser User = Room.GetRoomUserManager().GetRoomUserByHabbo(Session.GetHabbo().Id);
            if (User == null)
                return;

            if (EffectId != 0 && Session.GetHabbo().Effects().HasEffect(EffectId, true))
                User.ApplyEffect(EffectId);
        }
    }
}
