using System;
using Rocket.HabboHotel.GameClients;
using Rocket.Communication.Packets.Outgoing.Rooms.Chat;
using System.Threading;

namespace Rocket.HabboHotel.Rooms.Chat.Commands.iBlue.USERS
{
    class
        AbracarComando : IChatCommand
    {
        public string PermissionRequired
        {
            get { return "comando_users"; }
        }
        public string Parameters
        {
            get { return "%username%"; }
        }
        public string Description
        {
            get { return "Abraçar outro usuário"; }
        }
        public void Execute(GameClients.GameClient Session, Rooms.Room Room, string[] Params)
        {
            if (Params.Length == 1)
            {
                Session.SendWhisper("Você deve digitar um nome de usuário!",34);
                return;
            }

            GameClient TargetClient = RocketEmulador.GetGame().GetClientManager().GetClientByUsername(Params[1]);
            if (TargetClient == null)
            {
                Session.SendWhisper("Que o usuário não pode ser encontrado, talvez eles estejam offline ou não no quarto.",34);
                return;
            }

            RoomUser User = Room.GetRoomUserManager().GetRoomUserByHabbo(TargetClient.GetHabbo().Id);
            if (User == null)
            {
                Session.SendWhisper("O usuário não pode ser encontrado, talvez eles estejam offline ou não no quarto.",34);
                return;
            }
            RoomUser Self = Room.GetRoomUserManager().GetRoomUserByHabbo(Session.GetHabbo().Id);
            if (User == Self)
            {
                Session.SendWhisper("Você não pode abraçar a si mesmo!",34);
                return;
            }
            RoomUser ThisUser = Room.GetRoomUserManager().GetRoomUserByHabbo(Session.GetHabbo().Id);
            if (ThisUser == null)
                return;

            if (Math.Abs(User.X - ThisUser.X) < 2 && Math.Abs(User.Y - ThisUser.Y) < 2)
            {
                Room.SendMessage(new ShoutComposer(ThisUser.VirtualId, "*Abraçou " + TargetClient.GetHabbo().Username + "*", 0, User.LastBubble));
                User.ApplyEffect(9);
                ThisUser.ApplyEffect(9);
                System.Threading.Thread thrd = new System.Threading.Thread(delegate ()
                {
                    Thread.Sleep(5000);
                    User.ApplyEffect(0);
                    ThisUser.ApplyEffect(0);
                });
                thrd.Start();
            }
            else
            {
                Session.SendWhisper("Esse utilizador é muito longe, tente se aproximar.",34);
                return;
            }
        }
    }
}