using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Threading;
using System.Runtime.CompilerServices;
using Rocket.HabboHotel.Rooms;
using Rocket.HabboHotel.GameClients;
using Rocket.Communication.Packets.Outgoing.Rooms.Chat;

namespace Rocket.HabboHotel.Rooms.Chat.Commands.iBlue.VIP
{
    class
        RasteiraComando : IChatCommand
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
            get { return "Dar uma rasteira em alguém"; }
        }
        public void Execute(GameClients.GameClient Session, Rooms.Room Room, string[] Params)
        {
            if (Params.Length == 1)
            {
                Session.SendWhisper("Você deve digitar um nome de usuário !");
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
                Session.SendWhisper("Que o usuário não pode ser encontrado, talvez eles estejam offline ou não no quarto .");
                return;
            }

            RoomUser User = Room.GetRoomUserManager().GetRoomUserByHabbo(TargetClient.GetHabbo().Id);
            if (User == null)
            {
                Session.SendWhisper("O usuário não pode ser encontrado , talvez eles estejam offline ou não no quarto .");
                return;
            }
            RoomUser Self = Room.GetRoomUserManager().GetRoomUserByHabbo(Session.GetHabbo().Id);
            if (User == Self)
            {
                Session.SendWhisper("Você não pode dar uma rasteira em você mesmo.");
                return;
            }
            if (TargetClient.GetHabbo().Username == "HM" || TargetClient.GetHabbo().Username == "Scarface")
            {
                Session.SendWhisper("Você não pode dar uma rasteira nesse usuário.");
                return;
            }
            RoomUser ThisUser = Room.GetRoomUserManager().GetRoomUserByHabbo(Session.GetHabbo().Id);
            if (ThisUser == null)
                return;

            if (Math.Abs(User.X - ThisUser.X) < 2 && Math.Abs(User.Y - ThisUser.Y) < 2)
            {
                Room.SendMessage(new ChatComposer(ThisUser.VirtualId, "*RASTEIRA*", 0, User.LastBubble));
                User.Statusses.Add("lay", "1.0");
                User.Z -= 0.35;
                User.isLying = true;
                User.UpdateNeeded = true;
                User.ApplyEffect(53);
                Room.SendMessage(new ChatComposer(User.VirtualId, "*Levei uma rasteira do " + Session.GetHabbo().Username + "*", 0, User.LastBubble));

                User.GetClient().SendWhisper("Se não há movimento dentro de 5 segundos , você estará automaticamente .");

                System.Threading.Thread thrd = new System.Threading.Thread(delegate ()
                {
                    Thread.Sleep(5000);
                    User.ApplyEffect(0);
                    if (User.isLying)
                    {
                        User.Statusses.Remove("lay");
                        User.Z += 0.35;
                        User.isLying = true;
                        User.UpdateNeeded = true;
                        Room.SendMessage(new ChatComposer(User.VirtualId, "*Levantando-se.*", 0, User.LastBubble));
                    }

                });
                thrd.Start();
            }
            else
            {
                Session.SendWhisper("Esse utilizador é muito longe , tente se aproximar.");
                return;
            }
        }
    }
}