﻿using System;

using Rocket.Communication.Packets.Incoming;
using Rocket.HabboHotel.Rooms;
using Rocket.HabboHotel.GameClients;
using Rocket.Communication.Packets.Outgoing.Rooms.Chat;

namespace Rocket.Communication.Packets.Incoming.Rooms.Chat
{
    public class CancelTypingEvent : IPacketEvent
    {
        public void Parse(GameClient Session, ClientPacket Packet)
        {
            if (!Session.GetHabbo().InRoom)
                return;

            Room Room = Session.GetHabbo().CurrentRoom;
            if (Room == null)
                return;

            RoomUser User = Room.GetRoomUserManager().GetRoomUserByHabbo(Session.GetHabbo().Username);
            if (User == null)
                return;

            Session.GetHabbo().CurrentRoom.SendMessage(new UserTypingComposer(User.VirtualId, false));
        }
    }
}