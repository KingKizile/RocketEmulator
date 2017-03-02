using Rocket.Communication.Packets.Outgoing.Catalog;
using Rocket.HabboHotel.GameClients;
using Rocket.HabboHotel.Rooms.AI;
using Rocket.Communication.Packets.Incoming;

namespace Rocket.Communication.Packets.Incoming.Catalog
{
    public class GetSellablePetBreedsEvent : IPacketEvent
    {
        public void Parse(GameClient Session, ClientPacket Packet)
        {
            string Type = Packet.PopString();
            string PacketType = "";
            int PetId = RocketEmulador.GetGame().GetCatalog().GetPetRaceManager().GetPetId(Type, out PacketType);

            Session.SendMessage(new SellablePetBreedsComposer(PacketType, PetId, RocketEmulador.GetGame().GetCatalog().GetPetRaceManager().GetRacesForRaceId(PetId)));
        }
    }
}