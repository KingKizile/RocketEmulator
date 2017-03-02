using System;
using System.Linq;
using System.Text;
using MoreLinq;
using Rocket.Communication.Packets.Outgoing.Rooms.Furni.Wired;
using Rocket.HabboHotel.GameClients;
using Rocket.HabboHotel.Items;
using Rocket.HabboHotel.Items.Wired;
using Rocket.HabboHotel.Rooms;
using System;

namespace Rocket.Communication.Packets.Incoming.Rooms.Furni.Wired
{

    class SaveWiredConfigEvent : IPacketEvent
    {
        public void Parse(HabboHotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            if (Session == null || Session.GetHabbo() == null)
                return;

            if (!Session.GetHabbo().InRoom)
                return;

            int ItemId = Packet.PopInt();

            Session.SendMessage(new HideWiredConfigComposer());

            Room Room = Session.GetHabbo().CurrentRoom;
            if (Room == null)
                return;

            if (!Room.CheckRights(Session, false) && !Room.CheckRights(Session, true))
                return;

            Item SelectedItem = Room.GetRoomItemHandler().GetItem(ItemId);
            if (SelectedItem == null)
                return;

            IWiredItem Box = null;
            if (!Session.GetHabbo().CurrentRoom.GetWired().TryGet(ItemId, out Box))
                return;

            if (Box.Type == WiredBoxType.EffectGiveUserBadge && !Session.GetHabbo().GetPermissions().HasRight("room_item_wired_rewards"))
            {
                Session.SendNotification("You don't have the correct permissions to do this.");
                return;
            }

            Box.HandleSave(Packet);
            Session.GetHabbo().CurrentRoom.GetWired().SaveBox(Box);
        }
    }
}
