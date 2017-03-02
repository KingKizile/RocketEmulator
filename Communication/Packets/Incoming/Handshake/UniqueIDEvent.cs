using System;
using Rocket.Database.Interfaces;
using Rocket.HabboHotel.GameClients;
using Rocket.Communication.Packets.Outgoing.Handshake;

namespace Rocket.Communication.Packets.Incoming.Handshake
{
    public class UniqueIDEvent : IPacketEvent
    {
        public void Parse(GameClient Session, ClientPacket Packet)
        {
            string Junk = Packet.PopString();
            string MachineId = Packet.PopString();

            Session.MachineId = MachineId;

            Session.SendMessage(new SetUniqueIdComposer(MachineId));
        }
    }
}