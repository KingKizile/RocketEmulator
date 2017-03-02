using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;

using Rocket.HabboHotel.Talents;
using Rocket.Communication.Packets.Outgoing.Talents;

namespace Rocket.Communication.Packets.Incoming.Talents
{
    class GetTalentTrackEvent : IPacketEvent
    {
        public void Parse(HabboHotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            string Type = Packet.PopString();

            ICollection<TalentTrackLevel> Levels = RocketEmulador.GetGame().GetTalentTrackManager().GetLevels();

            Session.SendMessage(new TalentTrackComposer(Levels, Type));
        }
    }
}
