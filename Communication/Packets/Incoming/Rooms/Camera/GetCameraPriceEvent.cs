using System;

using Rocket.HabboHotel.GameClients;
using Rocket.Communication.Packets.Outgoing.Rooms.Camera;

namespace Rocket.Communication.Packets.Incoming.Rooms.Camera
{
    public class GetCameraPriceEvent : IPacketEvent
    {
        public void Parse(GameClient Session, ClientPacket Packet)
        {
            Session.SendMessage(new CameraPriceComposer(RocketEmulador.GetGame().GetCameraManager().PurchaseCoinsPrice, RocketEmulador.GetGame().GetCameraManager().PurchaseDucketsPrice, RocketEmulador.GetGame().GetCameraManager().PublishDucketsPrice));
        }
    }
}