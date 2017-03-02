using Rocket.Communication.Packets.Incoming;
using Rocket.HabboHotel.Rooms;
using Rocket.HabboHotel.Users;
using System;
using System.Runtime.CompilerServices;
using System.Collections.Concurrent;

namespace Rocket.HabboHotel.Items.Wired.Boxes.Effects
{
    internal class SetFixRoomBox : IWiredItem
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

        public SetFixRoomBox(Room Instance, Item Item)
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
            if (Player == null || Player.GetClient() == null || string.IsNullOrWhiteSpace(this.StringData))
            {
                return false;
            }
            if (Player.CurrentRoom.GetRoomUserManager().GetRoomUserByHabbo(Player.Username) == null)
            {
                return false;
            }
            Player.GetClient().SendWhisper("Sala desbugada!", 34);
            Player.CurrentRoom.GetGameMap().GenerateMaps(true);
            return true;
        }
    }
}
