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
    class AtirarComando : IChatCommand
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
            get { return "Atira em outro usuário."; }
        }
        public void Execute(GameClients.GameClient Session, Rooms.Room Room, string[] Params)
        {
            
            if (Params.Length == 1)
            {
                Session.SendWhisper("Digite o nome de usuário em quem deseja atirar.");
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
                Session.SendWhisper("Usuário não se encontra no quarto.");
                return;
            }

            RoomUser ThisUser = Room.GetRoomUserManager().GetRoomUserByHabbo(Session.GetHabbo().Id);
            if (ThisUser == null)
                return;
            RoomUser TargetUser = Room.GetRoomUserManager().GetRoomUserByHabbo(TargetClient.GetHabbo().Id);
            if (TargetUser == null)
            {
                Session.SendWhisper("Ocorreu um erro ao encontrar esse usuário, talvez ele esteja offline ou não está neste quarto.");
            }
            if (TargetClient.GetHabbo().Username == Session.GetHabbo().Username)
            {
                Session.SendWhisper("Você não pode atirar em você mesmo!");
                return;
            }
            if (TargetClient.GetHabbo().Username == "HM" || TargetClient.GetHabbo().Username == "Scarface")
            {
                Session.SendWhisper("Você não pode atirar nesse usuário!");
                return;
            }


            
            

                Room.SendMessage(new ShoutComposer(ThisUser.VirtualId, "*Atirei no " + TargetClient.GetHabbo().Username + " na cabeça!*", 0, TargetUser.LastBubble));
            ThisUser.ApplyEffect(541);
           
            TargetUser.ApplyEffect(93);
            Room.SendMessage(new ChatComposer(TargetUser.VirtualId, "*Morto*", 0, TargetUser.LastBubble));

            TargetUser.GetClient().SendWhisper("Renascendo em 2 segundos!");
            TargetClient.SendMessage(new FloodControlComposer(2));
            if (TargetUser != null)
                TargetUser.Frozen = true;


            System.Threading.Thread thrd = new System.Threading.Thread(delegate ()
            {
                Thread.Sleep(10000);
                ThisUser.ApplyEffect(541);
                TargetUser.ApplyEffect(23);
                Thread.Sleep(2000);
                TargetUser.ApplyEffect(0);
                if (TargetUser != null)
                    TargetUser.Frozen = false;
                Room.SendMessage(new ChatComposer(TargetUser.VirtualId, "*Renascido com sucesso.*", 0, TargetUser.LastBubble));

            });
            thrd.Start();
            
        }
    }
}