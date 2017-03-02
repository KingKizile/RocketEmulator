using Rocket.Communication.Interfaces;
using Rocket.Communication.Packets.Outgoing.Rooms.Chat;
using Rocket.HabboHotel.GameClients;
using System;
using System.Threading;

namespace Rocket.HabboHotel.Rooms.Chat.Commands.iBlue.VIP
{
    internal class PenisComando : IChatCommand
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
                return "Masturbe-se com esse lindo penis.";
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
                        Room.SendMessage((IServerPacket)new ChatComposer(roomUserByHabbo1.VirtualId, "*Ain, vou me masturbar com esse lindo penis!*", 0, 0), false);
                        roomUserByHabbo1.ApplyEffect(544);
                    }
                    else
                    {
                        Room.SendMessage((IServerPacket)new ChatComposer(roomUserByHabbo1.VirtualId, "*Ain, vou me masturbar com esse lindo penis!*", 0, 0), false);
                        roomUserByHabbo1.ApplyEffect(544);
                    }
                }
            }
        }
    }
}
