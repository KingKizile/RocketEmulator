using Rocket.Communication.Packets.Incoming;
using Rocket.Communication.Packets.Outgoing.Inventory.Purse;
using Rocket.Communication.Packets.Outgoing.Rooms.Notifications;
using Rocket.HabboHotel.Rooms;
using Rocket.HabboHotel.Users;
using System;
using System.Collections.Concurrent;

namespace Rocket.HabboHotel.Items.Wired.Boxes.Effects
{
    internal class GiveUserDucketsBox : IWiredItem
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

        public GiveUserDucketsBox(Room Instance, Item Item)
        {
            this.Instance = Instance;
            this.Item = Item;
            this.SetItems = new ConcurrentDictionary<int, Item>();
        }

        public void HandleSave(ClientPacket Packet)
        {
            Packet.PopInt();
            string Ducket = Packet.PopString();
            this.StringData = Ducket;
        }

        public bool Execute(params object[] Params)
        {
            if (Params == null || Params.Length == 0)
            {
                return false;
            }
            Habbo Owner = RocketEmulador.GetHabboById(this.Item.UserID);
            Habbo Player = (Habbo)Params[0];
            if (Player == null || Player.GetClient() == null)
            {
                return false;
            }
            if (Owner == null || !Owner.GetPermissions().HasRight("room_item_wired_rewards"))
            {
                Player.GetClient().SendMessage(new RoomNotificationComposer("supernoti", "message", "Você não tem permissão para utilizar este wired..."));
                return false;
            }
            if (Player.CurrentRoom.GetRoomUserManager().GetRoomUserByHabbo(Player.Username) == null)
            {
                return false;
            }
            if (string.IsNullOrEmpty(this.StringData))
            {
                return false;
            }
            if (Convert.ToInt32(this.StringData) > 6)
            {
                Player.GetClient().SendWhisper("La cantidad de Duckets pasa de los limites.", 0);
                return false;
            }
            string Ducket = this.StringData;
            Player.Duckets = (Player.Duckets += Convert.ToInt32(Ducket));
            Player.GetClient().SendMessage(new HabboActivityPointNotificationComposer(Player.Duckets, Convert.ToInt32(Ducket), 0));
            Player.GetClient().SendMessage(new RoomNotificationComposer("command_notification_credits", "message", "Has recibido " + Ducket + " ducket(s)!"));
            return true;
        }
    }
}
