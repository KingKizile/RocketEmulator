using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Threading;

using Rocket.HabboHotel.Rooms;
using Rocket.HabboHotel.GameClients;
using Rocket.Communication.Packets.Outgoing.Rooms.Chat;

namespace Rocket.HabboHotel.Rooms.Chat.Commands.iBlue.VIP
{
    class
        ChutarComando : IChatCommand
    {
        public string PermissionRequired
        {
            get { return "comando_vip"; }
        }
        public string Parameters
        {
            get { return "%username%"; }
        }
        public string Description
        {
            get { return "Chutar outro usuário"; }
        }
        public void Execute(GameClients.GameClient Session, Rooms.Room Room, string[] Params)
        {
            if (Params.Length == 1)
            {
                Session.SendWhisper("Você deve digitar um nome de usuário!");
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
                Session.SendWhisper("Que o usuário não pode ser encontrado, talvez eles estejam offline ou não no quarto.");
                return;
            }

            RoomUser User = Room.GetRoomUserManager().GetRoomUserByHabbo(TargetClient.GetHabbo().Id);
            if (User == null)
            {
                Session.SendWhisper("O usuário não pode ser encontrado, talvez eles estejam offline ou não no quarto.");
                return;
            }
            RoomUser Self = Room.GetRoomUserManager().GetRoomUserByHabbo(Session.GetHabbo().Id);
            if (User == Self)
            {
                Session.SendWhisper("Você não pode se chutar!");
                return;
            }
            RoomUser ThisUser = Room.GetRoomUserManager().GetRoomUserByHabbo(Session.GetHabbo().Id);
            if (ThisUser == null)
                return;

            if (Math.Abs(User.X - ThisUser.X) < 2 && Math.Abs(User.Y - ThisUser.Y) < 2)
            {
                Room.SendMessage(new ShoutComposer(ThisUser.VirtualId, "*Chutei " + TargetClient.GetHabbo().Username + "*", 0, User.LastBubble));
                User.Statusses.Add("sit", "1.0");
                User.Z -= 0.35;
                User.isSitting = true;
                User.UpdateNeeded = true;
                User.ApplyEffect(53);
                Room.SendMessage(new ChatComposer(User.VirtualId, "*Levei um chute do " + Session.GetHabbo().Username + "*", 0, User.LastBubble));

                User.GetClient().SendWhisper("Aguarde por 5 segundos, enquanto isso você não pode se mover..");

                System.Threading.Thread thrd = new System.Threading.Thread(delegate ()
                {
                    Thread.Sleep(5000);
                    User.ApplyEffect(0);
                    if (User.isSitting)
                    {
                        User.Statusses.Remove("sit");
                        User.Z += 0.35;
                        User.isSitting = true;
                        User.UpdateNeeded = true;
                        Room.SendMessage(new ChatComposer(User.VirtualId, "*Levantando-se*", 0, User.LastBubble));
                    }

                });
                thrd.Start();
            }
            else
            {
                Session.SendWhisper("Esse utilizador é muito longe, tente se aproximar.");
                return;
            }
        }
    }
}