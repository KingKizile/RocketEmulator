﻿using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;

using Rocket.HabboHotel.Items;
using Rocket.HabboHotel.Items.Wired;

namespace Rocket.Communication.Packets.Outgoing.Rooms.Furni.Wired
{
    internal class WiredTriggerConfigComposer : ServerPacket
    {
        public WiredTriggerConfigComposer(IWiredItem Box, List<int> BlockedItems)
            : base(ServerPacketHeader.WiredTriggerConfigMessageComposer)
        {
            base.WriteBoolean(false);
            base.WriteInteger(5);

            base.WriteInteger(Box.SetItems.Count);
            foreach (Item Item in Box.SetItems.Values.ToList())
            {
                base.WriteInteger(Item.Id);
            }

            base.WriteInteger(Box.Item.GetBaseItem().SpriteId);
            base.WriteInteger(Box.Item.Id);
            base.WriteString(Box.StringData);

            base.WriteInteger(Box is IWiredCycle ? 1 : 0);
            if (Box is IWiredCycle)
            {
                IWiredCycle Cycle = (IWiredCycle)Box;
                base.WriteInteger(Cycle.Delay);
            }
            base.WriteInteger(0);
            base.WriteInteger(WiredBoxTypeUtility.GetWiredId(Box.Type));
            base.WriteInteger(BlockedItems.Count());
            if (BlockedItems.Count() > 0)
            {
                foreach (int Id in BlockedItems.ToList())
                    base.WriteInteger(Id);
            }
        }
    }
}