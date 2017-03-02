using System;
using System.Collections.Generic;

using Rocket.HabboHotel.Catalog;

namespace Rocket.Communication.Packets.Outgoing.Catalog
{
    public class RecyclerRewardsComposer : ServerPacket
    {
        public RecyclerRewardsComposer()
            : base(ServerPacketHeader.RecyclerRewardsMessageComposer)
        {
            base.WriteInteger(0);// Count of items
        }
    }
}