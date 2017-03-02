using Rocket.Communication.Packets.Incoming;
using Rocket.Communication.Packets.Outgoing.Rooms.Notifications;
using Rocket.HabboHotel.Rooms;
using Rocket.HabboHotel.Users;
using System;
using System.Collections.Concurrent;

namespace Rocket.HabboHotel.Items.Wired.Boxes.Effects
{
    internal class HandUserItemBox : IWiredItem
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

        public HandUserItemBox(Room Instance, Item Item)
        {
            this.Instance = Instance;
            this.Item = Item;
            this.SetItems = new ConcurrentDictionary<int, Item>();
        }

        public void HandleSave(ClientPacket Packet)
        {
            Packet.PopInt();
            string HandI = Packet.PopString();
            this.StringData = HandI;
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
            Habbo Owner = RocketEmulador.GetHabboById(Player.Id);
            if (Owner == null || !Owner.GetPermissions().HasRight("room_item_wired_rewards"))
            {
                Player.GetClient().SendMessage(new RoomNotificationComposer("supernoti", "message", "Você não tem permissão para utilizar este wired..."));
            }
            RoomUser User = Player.CurrentRoom.GetRoomUserManager().GetRoomUserByHabbo(Player.Username);
            if (User == null)
            {
                return false;
            }
            if (string.IsNullOrEmpty(this.StringData))
            {
                return false;
            }
            string HandI = this.StringData;
            User.CarryItem(Convert.ToInt32(HandI));
            return true;
        }
    }
}
