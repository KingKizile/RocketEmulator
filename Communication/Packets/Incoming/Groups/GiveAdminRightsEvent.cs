﻿using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;

using Rocket.HabboHotel.Users;
using Rocket.HabboHotel.Rooms;
using Rocket.HabboHotel.Groups;
using Rocket.Communication.Packets.Outgoing.Groups;
using Rocket.Communication.Packets.Outgoing.Rooms.Permissions;
using Rocket.HabboHotel.Cache;



namespace Rocket.Communication.Packets.Incoming.Groups
{
    class GiveAdminRightsEvent : IPacketEvent
    {
        public void Parse(HabboHotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            int GroupId = Packet.PopInt();
            int UserId = Packet.PopInt();

            Group Group = null;
            if (!RocketEmulador.GetGame().GetGroupManager().TryGetGroup(GroupId, out Group))
                return;

            if (Session.GetHabbo().Id != Group.CreatorId || !Group.IsMember(UserId))
                return;

            Habbo Habbo = RocketEmulador.GetHabboById(UserId);
            if (Habbo == null)
            {
                Session.SendNotification("Oops, an error occurred whilst finding this user.");
                return;
            }

            Group.MakeAdmin(UserId);
          
            Room Room = null;
            if (RocketEmulador.GetGame().GetRoomManager().TryGetRoom(Group.RoomId, out Room))
            {
                RoomUser User = Room.GetRoomUserManager().GetRoomUserByHabbo(UserId);
                if (User != null)
                {
                    if (!User.Statusses.ContainsKey("flatctrl 3"))
                        User.AddStatus("flatctrl 3", "");
                    User.UpdateNeeded = true;
                    if (User.GetClient() != null)
                        User.GetClient().SendMessage(new YouAreControllerComposer(3));
                }
            }

            Session.SendMessage(new GroupMemberUpdatedComposer(GroupId, Habbo, 1));
        }
    }
}
