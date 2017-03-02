using System;

namespace Rocket.Communication.Packets.Outgoing.Rooms.Camera
{
    public class CameraPriceComposer : ServerPacket
    {
        public CameraPriceComposer(int purchasePriceCoins, int purchasePriceDuckets, int publishPriceDuckets)
            : base(ServerPacketHeader.CameraPriceComposer)
        {
            base.WriteInteger(purchasePriceCoins);
            base.WriteInteger(purchasePriceDuckets);
            base.WriteInteger(publishPriceDuckets);
        }
    }
}