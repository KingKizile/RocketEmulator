using Rocket.HabboHotel.Rooms;
using Rocket.Communication.Interfaces;
using Rocket.Communication.Packets.Outgoing.Rooms.Chat;
using Rocket.HabboHotel.GameClients;
using System;
using System.Threading;


namespace Rocket.HabboHotel.Rooms.Chat.Commands.iBlue.USERS
{
    class
       SexoComando : IChatCommand
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
                return "%usuario%";
            }
        }

        public string Description
        {
            get
            {
                return "Selecione o usuário com quem deseja fazer sexo.";
            }
        }
        // Coded By Hamada.
        public void Execute(GameClient Session, Room Room, string[] Params)
        {
            RoomUser roomUserByHabbo1 = Room.GetRoomUserManager().GetRoomUserByHabbo(Session.GetHabbo().Id);
            if (roomUserByHabbo1 == null)
                return;

            if (Params.Length == 0)
            {
                Session.SendWhisper("Você deve digitar o nome de usuário da pessoa que você deseja ter relações sexuais com.", 0);
            }
            GameClient TargetClient2 = RocketEmulador.GetGame().GetClientManager().GetClientByUsername(Params[1]);
            RoomUser TargetUser2 = Room.GetRoomUserManager().GetRoomUserByHabbo(TargetClient2.GetHabbo().Id);
            if (!TargetUser2.GetClient().GetHabbo().AllowCommands && !Session.GetHabbo().GetPermissions().HasRight("sex"))


            {
                Session.SendWhisper("Este usuário desativou os comandos!");
                return;
            }
            else
            {
                RoomUser roomUserByHabbo2 = Session.GetHabbo().CurrentRoom.GetRoomUserManager().GetRoomUserByHabbo(Params[1]);
                GameClient clientByUsername = RocketEmulador.GetGame().GetClientManager().GetClientByUsername(Params[1]);
                if (clientByUsername.GetHabbo().Username == Session.GetHabbo().Username)
                {

                    RoomUser Self = Room.GetRoomUserManager().GetRoomUserByHabbo(Session.GetHabbo().Id);
                    if (roomUserByHabbo1 == Self)
                    {
                        Session.SendWhisper("Você não pode fazer sexo com você mesmo!");
                        return;
                    }

                }

                else if (clientByUsername.GetHabbo().CurrentRoomId == Session.GetHabbo().CurrentRoomId && (Math.Abs(checked(roomUserByHabbo1.X - roomUserByHabbo2.X)) < 2 && Math.Abs(checked(roomUserByHabbo1.Y - roomUserByHabbo2.Y)) < 2))
                {
                    if ((Session.GetHabbo().sexWith == null || Session.GetHabbo().sexWith == "") && (clientByUsername.GetHabbo().Username != Session.GetHabbo().sexWith && Session.GetHabbo().Username != clientByUsername.GetHabbo().sexWith))
                    {
                        Session.GetHabbo().sexWith = clientByUsername.GetHabbo().Username;
                        clientByUsername.SendWhisper(Session.GetHabbo().Username + " Pediu para fazer sexo com você . Se deseja ter  " + Session.GetHabbo().Username + ", digite  \":sexo " + Session.GetHabbo().Username + "\"");
                        Session.SendWhisper(clientByUsername.GetHabbo().Username + " Foi enviado o seu pedido de sexo. Se eles respondem , você será capaz de ter relações sexuais.");
                    }
                    else if (roomUserByHabbo2 != null)
                    {
                        if (clientByUsername.GetHabbo().sexWith == Session.GetHabbo().Username)
                        {
                            if (roomUserByHabbo2.GetClient() != null && roomUserByHabbo2.GetClient().GetHabbo() != null)
                            {
                                if (clientByUsername.GetHabbo().CurrentRoomId == Session.GetHabbo().CurrentRoomId && (Math.Abs(checked(roomUserByHabbo1.X - roomUserByHabbo2.X)) < 2 && Math.Abs(checked(roomUserByHabbo1.Y - roomUserByHabbo2.Y)) < 2))
                                {
                                    clientByUsername.GetHabbo().sexWith = (string)null;
                                    Session.GetHabbo().sexWith = (string)null;
                                    if (Session.GetHabbo().Gender == "m")
                                    {
                                        Room.SendMessage((IServerPacket)new ChatComposer(roomUserByHabbo1.VirtualId, "*Agarra " + Params[1] + " por trás, e começar a ter relações sexuais*", 0, 0), false);
                                        Thread.Sleep(2000);
                                        roomUserByHabbo1.ApplyEffect(140);
                                        roomUserByHabbo2.ApplyEffect(140);
                                        Room.SendMessage((IServerPacket)new ChatComposer(roomUserByHabbo2.VirtualId, "*Dobra-se sobre , apresentando um buraco apertado para " + Session.GetHabbo().Username + " para inserir seu pênis em*", 0, 0), false);
                                        Thread.Sleep(2000);
                                        Room.SendMessage((IServerPacket)new ChatComposer(roomUserByHabbo1.VirtualId, "*Começa a empurrar -se " + Params[1] + " duro, fazendo-a gemer*", 0, 0), false);
                                        Room.SendMessage((IServerPacket)new ChatComposer(roomUserByHabbo1.VirtualId, "*Diga meu nome!, " + Params[1] + " Diga-o alto !*", 0, 0), false);
                                        Room.SendMessage((IServerPacket)new ShoutComposer(roomUserByHabbo2.VirtualId, " " + Session.GetHabbo().Username + " ", 0, 0), false);
                                        Room.SendMessage((IServerPacket)new ChatComposer(roomUserByHabbo2.VirtualId, "Gemeeeeeeendooooo*", 0, 0), false);
                                        Thread.Sleep(2000);
                                        Room.SendMessage((IServerPacket)new ChatComposer(roomUserByHabbo1.VirtualId, "*Desacelera um pouco como eu estou prestes a gozar*", 0, 0), false);
                                        Room.SendMessage((IServerPacket)new ChatComposer(roomUserByHabbo2.VirtualId, "*Tempos buraco para fazer o orgasmo melhor para " + Session.GetHabbo().Username + "*", 0, 0), false);
                                        Thread.Sleep(2000);
                                        Room.SendMessage((IServerPacket)new ChatComposer(roomUserByHabbo1.VirtualId, "*Explodes with cum inside " + Params[1] + "*", 0, 0), false);
                                        Room.SendMessage((IServerPacket)new ChatComposer(roomUserByHabbo2.VirtualId, "*Mordidas de lábio * Isso foi incrível!", 0, 0), false);
                                        Thread.Sleep(2000);
                                        roomUserByHabbo1.ApplyEffect(0);
                                        roomUserByHabbo2.ApplyEffect(0);
                                    }
                                    else
                                    {
                                        Room.SendMessage((IServerPacket)new ChatComposer(roomUserByHabbo2.VirtualId, "*Esfrega " + Session.GetHabbo().Username + "' partes íntimas*", 0, 0), false);
                                        Thread.Sleep(1000);
                                        roomUserByHabbo1.ApplyEffect(140);
                                        roomUserByHabbo2.ApplyEffect(140);
                                        Room.SendMessage((IServerPacket)new ChatComposer(roomUserByHabbo1.VirtualId, "*Toma calças*", 0, 0), false);
                                        Thread.Sleep(2000);
                                        Room.SendMessage((IServerPacket)new ChatComposer(roomUserByHabbo2.VirtualId, "*delicadamente cutuca " + Session.GetHabbo().Username + "' xoxota apertada*", 0, 0), false);
                                        Room.SendMessage((IServerPacket)new ChatComposer(roomUserByHabbo1.VirtualId, "*Aiiiiiin* Isso é bom .. Hehe", 0, 0), false);
                                        Thread.Sleep(2000);
                                        Room.SendMessage((IServerPacket)new ChatComposer(roomUserByHabbo2.VirtualId, "*Acelera e pica o mais profundo*", 0, 0), false);
                                        Room.SendMessage((IServerPacket)new ChatComposer(roomUserByHabbo1.VirtualId, "*ai, ain, aiiiiiin!*", 0, 0), false);
                                        Thread.Sleep(2000);
                                        Room.SendMessage((IServerPacket)new ChatComposer(roomUserByHabbo2.VirtualId, "*Recua afastado como " + Session.GetHabbo().Username + " esguichos em todos os lugares*", 0, 0), false);
                                        Room.SendMessage((IServerPacket)new ChatComposer(roomUserByHabbo1.VirtualId, "* Lábio mordidas * Isso foi incrível!", 0, 0), false);
                                        Thread.Sleep(1000);
                                        roomUserByHabbo1.ApplyEffect(0);
                                        roomUserByHabbo2.ApplyEffect(0);
                                    }

                                }
                                else
                                    Session.SendWhisper("Que o usuário está muito longe de ter sexo!", 0);
                            }
                            else
                                Session.SendWhisper("Ocorreu um erro ao encontrar esse usuário.", 0);
                        }
                        else
                            Session.SendWhisper("Este usuário não aceitou sua solicitação sexo. (ainda).", 0);
                    }
                    else
                        Session.SendWhisper("Este usuário não pôde ser encontrado no quarto", 0);
                }
                else
                    Session.SendWhisper("Que o usuário está muito longe de ter sexo !", 0);
            }
        }
    }
}
