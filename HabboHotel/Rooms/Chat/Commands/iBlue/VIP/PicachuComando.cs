using Rocket.Communication.Interfaces;
using Rocket.Communication.Packets.Outgoing.Rooms.Chat;
using Rocket.HabboHotel.GameClients;
using System;
using System.Threading;

namespace Rocket.HabboHotel.Rooms.Chat.Commands.iBlue.VIP
{
    internal class PicachuComando : IChatCommand
    {
        public string PermissionRequired
        {
            get
            {
                return "comando_vip";
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
                return "Vire o famoso picachu.";
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
                        Room.SendMessage((IServerPacket)new ChatComposer(roomUserByHabbo1.VirtualId, "*Nossa, eu virei um picachu.*", 0, 0), false);
                        roomUserByHabbo1.ApplyEffect(547);
                    }
                    else
                    {
                        Room.SendMessage((IServerPacket)new ChatComposer(roomUserByHabbo1.VirtualId, "*Nossa, eu virei um picachu.*", 0, 0), false);
                        roomUserByHabbo1.ApplyEffect(547);
                    }
                }
            }
        }
    }
}
