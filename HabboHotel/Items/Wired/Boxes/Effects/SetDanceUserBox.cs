using Rocket.Communication.Packets.Incoming;
using Rocket.Communication.Packets.Outgoing.Rooms.Avatar;
using Rocket.HabboHotel.Rooms;
using Rocket.HabboHotel.Users;
using System;
using System.Collections.Concurrent;

namespace Rocket.HabboHotel.Items.Wired.Boxes.Effects
{
    internal class SetDanceUserBox : IWiredItem
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
                return WiredBoxType.EffectGiveUserBadge;
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

        public SetDanceUserBox(Room Instance, Item Item)
        {
            this.Instance = Instance;
            this.Item = Item;
            this.SetItems = new ConcurrentDictionary<int, Item>();
        }

        public void HandleSave(ClientPacket Packet)
        {
            Packet.PopInt();
            string Dance = Packet.PopString();
            this.StringData = Dance;
        }

        public bool Execute(params object[] Params)
        {
            if (Params == null || Params.Length == 0)
            {
                return false;
            }
            Habbo Player = (Habbo)Params[0];
            if (Player == null || Player.GetClient() == null || string.IsNullOrWhiteSpace(this.StringData))
            {
                return false;
            }
            RoomUser ThisUser = Player.CurrentRoom.GetRoomUserManager().GetRoomUserByHabbo(Player.Id);
            if (ThisUser == null)
            {
                return false;
            }
            string Dance = this.StringData;
            Player.GetClient().SendMessage(new DanceComposer(ThisUser, Convert.ToInt32(Dance)));
            return true;
        }
    }
}
