using Rocket.Communication.Interfaces;
using Rocket.Communication.Packets.Outgoing.Rooms.Chat;
using Rocket.HabboHotel.GameClients;
using System.Threading;
namespace Rocket.HabboHotel.Rooms.Chat.Commands.iBlue.USERS
{ 
    internal class PunhetaComando: IChatCommand
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
                return "punheta.";
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
                        Room.SendMessage((IServerPacket)new ChatComposer(roomUserByHabbo1.VirtualId, "*Puxa calças para baixo e coloca pau para fora*", 0, 0), false);
                        Thread.Sleep(1000);
                        Room.SendMessage((IServerPacket)new ChatComposer(roomUserByHabbo1.VirtualId, "*Bate uma punheta como se não houvesse amanhã * ", 0, 0), false);
                        Thread.Sleep(3000);
                        Room.SendMessage((IServerPacket)new ChatComposer(roomUserByHabbo1.VirtualId, "*Ejacula EM toda parte! * Desculpe rapazes.", 0, 0), false);
                        Thread.Sleep(1000);
                        roomUserByHabbo1.ApplyEffect(9);
                        Thread.Sleep(3000);
                        roomUserByHabbo1.ApplyEffect(0);
                    }
                    else
                    {
                        Room.SendMessage((IServerPacket)new ChatComposer(roomUserByHabbo1.VirtualId, "*Abaixa a calça e começa a esfregar clitóris*", 0, 0), false);
                        Thread.Sleep(2000);
                        Room.SendMessage((IServerPacket)new ChatComposer(roomUserByHabbo1.VirtualId, "*Geeeeeeeemiidos mmmmmmmmmm*", 0, 0), false);
                        Thread.Sleep(2000);
                        Room.SendMessage((IServerPacket)new ChatComposer(roomUserByHabbo1.VirtualId, "*Ejaculação EM TODA PARTE!* Desculpa rapazes..", 0, 0), false);
                        Thread.Sleep(1000);
                        roomUserByHabbo1.ApplyEffect(9);
                        Thread.Sleep(3000);
                        roomUserByHabbo1.ApplyEffect(0);
                    }
                }
            }
        }
    }
}
