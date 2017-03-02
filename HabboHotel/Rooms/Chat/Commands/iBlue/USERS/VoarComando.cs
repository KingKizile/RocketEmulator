using Rocket.Communication.Interfaces;
using Rocket.Communication.Packets.Outgoing.Rooms.Chat;
using Rocket.HabboHotel.GameClients;
namespace Rocket.HabboHotel.Rooms.Chat.Commands.iBlue.USERS
{
    internal class VoarComando : IChatCommand
    {
        public string PermissionRequired
        {
            get
            {
                return "comando_users";
            }
        }

        public string Parameters
        {
            get
            {
                return "";
            }
        }

        public string Description
        {
            get
            {
                return "Voe pelo Haddo a fora.";
            }
        }

        public void Execute(GameClient Session, Room Room, string[] Params)
        {
            RoomUser roomUserByHabbo1 = Room.GetRoomUserManager().GetRoomUserByHabbo(Session.GetHabbo().Id);
            if (roomUserByHabbo1 == null)
                return;
            {
                {
                    if (Session.GetHabbo().Gender == "m")
                    {
                        Room.SendMessage((IServerPacket)new ChatComposer(roomUserByHabbo1.VirtualId, "*E lá vamos nois... E lá vamos nois...*", 0, 0), false);
                        roomUserByHabbo1.ApplyEffect(992);
                    }
                    else
                    {
                        Room.SendMessage((IServerPacket)new ChatComposer(roomUserByHabbo1.VirtualId, "*E lá vamos nois... E lá vamos nois...*", 0, 0), false);
                        roomUserByHabbo1.ApplyEffect(992);
                    }
                }
            }
        }
    }
}
