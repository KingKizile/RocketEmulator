﻿using Rocket.Communication.Packets.Incoming;
using Rocket.HabboHotel.Rooms;
using Rocket.HabboHotel.Users;
using System;
using System.Collections.Concurrent;

namespace Rocket.HabboHotel.Items.Wired.Boxes.Effects
{
    internal class SetFreezeUserBox : IWiredItem
    {
        public Room Instance
        {
            get;
            set;
        }

        public Item Item
        {
            get;
            set;
        }

        public WiredBoxType Type
        {
            get
            {
                return WiredBoxType.EffectRemoveActorFromTeam;
            }
        }

        public ConcurrentDictionary<int, Item> SetItems
        {
            get;
            set;
        }

        public string StringData
        {
            get;
            set;
        }

        public bool BoolData
        {
            get;
            set;
        }

        public string ItemsData
        {
            get;
            set;
        }

        public SetFreezeUserBox(Room Instance, Item Item)
        {
            this.Instance = Instance;
            this.Item = Item;
            this.SetItems = new ConcurrentDictionary<int, Item>();
        }

        public void HandleSave(ClientPacket Packet)
        {
            Packet.PopInt();
        }

        public bool Execute(params object[] Params)
        {
            if (Params == null || Params.Length == 0)
            {
                return false;
            }
            Habbo Player = (Habbo)Params[0];
            if (Player == null || Player.GetClient() == null)
            {
                return false;
            }
            RoomUser User = Player.CurrentRoom.GetRoomUserManager().GetRoomUserByHabbo(Player.Username);
            if (User == null)
            {
                return false;
            }
            User.Frozen = !User.Frozen;
            User.freezeUserTicks = 1;
            Player.GetClient().GetHabbo().Effects().ApplyEffect(12);
            return true;
        }
    }
}
