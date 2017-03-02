using System;
using System.Linq;
using System.Text;

using Rocket.HabboHotel.Items.Televisions;
using Rocket.Communication.Packets.Outgoing.Rooms.Furni.YouTubeTelevisions;

namespace Rocket.Communication.Packets.Incoming.Rooms.Furni
{
    class ToggleYouTubeVideoEvent : IPacketEvent
    {
        public void Parse(HabboHotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            int ItemId = Packet.PopInt();//Item Id
            string VideoId = Packet.PopString(); //Video ID

            Session.SendMessage(new GetYouTubeVideoComposer(ItemId, VideoId));
        }
    }
}