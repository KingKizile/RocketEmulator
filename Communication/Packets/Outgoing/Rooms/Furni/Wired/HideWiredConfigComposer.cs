﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rocket.Communication.Packets.Outgoing.Rooms.Furni.Wired
{
    class HideWiredConfigComposer : ServerPacket
    {
        public HideWiredConfigComposer()
            : base(ServerPacketHeader.HideWiredConfigMessageComposer)
        {
        }
    }
}