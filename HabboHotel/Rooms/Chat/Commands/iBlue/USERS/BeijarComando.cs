using System;
using Rocket.HabboHotel.GameClients;
using Rocket.Communication.Packets.Outgoing.Rooms.Chat;
using System.Threading;
using System.Linq;

namespace Rocket.HabboHotel.Rooms.Chat.Commands.iBlue.USERS
{
    class
        BeijarComando : IChatCommand
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
            get { return "Beije outro usuario."; }
        }
        public void Execute(GameClients.GameClient Session, Rooms.Room Room, string[] Params)
        {
            if (Params.Length == 1)
            {
                Session.SendWhisper("Digite o nome do usuário!");
                return;
            }
            GameClient TargetClient2 = RocketEmulador.GetGame().GetClientManager().GetClientByUsername(Params[1]);
            RoomUser TargetUser2 = Room.GetRoomUserManager().GetRoomUserByHabbo(TargetClient2.GetHabbo().Id);
            if (!TargetUser2.GetClient().GetHabbo().AllowCommands && !Session.GetHabbo().GetPermissions().HasRight("sex"))


            {
                Session.SendWhisper("Este usuário desativou os comandos!");
                return;
            }
            GameClient TargetClient = RocketEmulador.GetGame().GetClientManager().GetClientByUsername(Params[1]);
            if (TargetClient == null)
            {
                Session.SendWhisper("O usuario selecionado esta offline ou não está no quarto.");
                return;
            }

            RoomUser User = Room.GetRoomUserManager().GetRoomUserByHabbo(TargetClient.GetHabbo().Id);
            if (User == null)
            {
                Session.SendWhisper("O usuario selecionado esta offline ou não está no quarto.");
                return;

            }
           
                    RoomUser Self = Room.GetRoomUserManager().GetRoomUserByHabbo(Session.GetHabbo().Id);
           
            if (User == Self)
            {
                Session.SendWhisper("You can't have kiss with yourself!");
                return;

            }

            RoomUser ThisUser = Room.GetRoomUserManager().GetRoomUserByHabbo(Session.GetHabbo().Id);
            if (ThisUser == null)
                return;

            if (Math.Abs(User.X - ThisUser.X) < 2 && Math.Abs(User.Y - ThisUser.Y) < 2)
            {
                Room.SendMessage(new ShoutComposer(ThisUser.VirtualId, "*beijando " + TargetClient.GetHabbo().Username + "*", 0, User.LastBubble));
                Room.SendMessage(new ChatComposer(User.VirtualId, "Aw :$", 0, User.LastBubble));
                User.ApplyEffect(9);
                ThisUser.ApplyEffect(9);
                System.Threading.Thread thrd = new System.Threading.Thread(delegate ()
                {
                    Thread.Sleep(2000);
                    User.ApplyEffect(0);
                    ThisUser.ApplyEffect(0);
                });
                thrd.Start();
            }
            else
            {
                Session.SendWhisper("That user is too far away, try getting closer.");
                return;
            }
        }
    }
}