using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Collections.Concurrent;

using Rocket.Communication.Packets.Incoming;
using Rocket.HabboHotel.Rooms;
using Rocket.HabboHotel.Users;
using Rocket.Communication.Packets.Outgoing.Rooms.Chat;

namespace Rocket.HabboHotel.Items.Wired.Boxes.Effects
{
    class SetEnableUserBox : IWiredItem
    {
        public Room Instance { get; set; }

        public Item Item { get; set; }

        public WiredBoxType Type { get { return WiredBoxType.EffectKickUser; } }

        public ConcurrentDictionary<int, Item> SetItems { get; set; }

        public string StringData { get; set; }

        public bool BoolData { get; set; }

        public string ItemsData { get; set; }

        public SetEnableUserBox(Room Instance, Item Item)
        {
            this.Instance = Instance;
            this.Item = Item;
            this.SetItems = new ConcurrentDictionary<int, Item>();
        }

        public void HandleSave(ClientPacket Packet)
        {
            int Unknown = Packet.PopInt();
            string Enable = Packet.PopString();

            this.StringData = Enable;
        }

        public bool Execute(params object[] Params)
        {
            if (Params == null || Params.Length == 0)
                return false;

            Habbo Player = (Habbo)Params[0];
            if (Player == null || Player.GetClient() == null || string.IsNullOrWhiteSpace(StringData))
                return false;

            RoomUser User = Player.CurrentRoom.GetRoomUserManager().GetRoomUserByHabbo(Player.Username);
            if (User == null)
                return false;

            string Enable = StringData;

            if (Convert.ToInt32(Enable) == 102)
                return false;

            if (Convert.ToInt32(Enable) == 187)
                return false;

            if (Convert.ToInt32(Enable) == 189)
                return false;

            if (Convert.ToInt32(Enable) == 178)
                return false;

            if (Convert.ToInt32(Enable) == 188)
                return false;

            Player.GetClient().GetHabbo().Effects().ApplyEffect(Convert.ToInt32(Enable));
            return true;
        }
    }
}