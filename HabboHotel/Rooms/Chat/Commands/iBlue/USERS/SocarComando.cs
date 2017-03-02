using Rocket.Communication.Packets.Outgoing.Rooms.Chat;
using Rocket.HabboHotel.GameClients;
using System;
using System.Linq;

namespace Rocket.HabboHotel.Rooms.Chat.Commands.iBlue.USERS
{
 

    internal class SocarComando : IChatCommand
    {
        public void Execute(GameClient Session, Room Room, string[] Params)
        {
            if (Params.Length == 1)
            {
                Session.SendWhisper("Escreva o nome de quem você quer dar um soco. ':socar Fulano'", 0);
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
                    GameClient clientByUsername = RocketEmulador.GetGame().GetClientManager().GetClientByUsername(Params[1]);
                    if (clientByUsername == null)
                    {
                        Session.SendWhisper("Opâ, este usuário não está no quarto ou não está online.", 0);
                    }
                    else
                    {
                        RoomUser roomUserByHabbo = Room.GetRoomUserManager().GetRoomUserByHabbo(clientByUsername.GetHabbo().Id);
                        if (roomUserByHabbo == null)
                        {
                            Session.SendWhisper("Opâ, este usuário não está no quarto ou não está online.", 0);
                        }
                        else if (clientByUsername.GetHabbo().Username == Session.GetHabbo().Username)
                        {
                            Session.SendWhisper("Você deu um soco em você mesmo.", 0);
                        }
                        else if (roomUserByHabbo.TeleportEnabled)
                        {
                            Session.SendWhisper("Ops, não pode dar um soco em alguém usando teleporte.", 0);
                        }
                        else
                        {
                            RoomUser user2 = Room.GetRoomUserManager().GetRoomUserByHabbo(Session.GetHabbo().Id);
                            if (user2 != null)
                            {
                                if ((Math.Abs((int)(roomUserByHabbo.X - user2.X)) < 2) && (Math.Abs((int)(roomUserByHabbo.Y - user2.Y)) < 2))
                                {
                                    Room.SendMessage(new ChatComposer(user2.VirtualId, "*" + Params[1] + " você recebeu um soco na cara*", 0, user2.LastBubble), false);
                                    if (roomUserByHabbo.RotBody == 4)
                                    {
                                        roomUserByHabbo.Statusses.Add("lay", "1.0 null");
                                        roomUserByHabbo.Z -= 0.35;
                                        roomUserByHabbo.isLying = true;
                                        roomUserByHabbo.UpdateNeeded = true;
                                        roomUserByHabbo.ApplyEffect(0x9d);
                                    }
                                    if (user2.RotBody == 0)
                                    {
                                        roomUserByHabbo.Statusses.Add("lay", "1.0 null");
                                        roomUserByHabbo.Z -= 0.35;
                                        roomUserByHabbo.isLying = true;
                                        roomUserByHabbo.UpdateNeeded = true;
                                        roomUserByHabbo.ApplyEffect(0x9d);
                                    }
                                    if (user2.RotBody == 6)
                                    {
                                        roomUserByHabbo.Statusses.Add("lay", "1.0 null");
                                        roomUserByHabbo.Z -= 0.35;
                                        roomUserByHabbo.isLying = true;
                                        roomUserByHabbo.UpdateNeeded = true;
                                        roomUserByHabbo.ApplyEffect(0x9d);
                                    }
                                    if (user2.RotBody == 2)
                                    {
                                        roomUserByHabbo.Statusses.Add("lay", "1.0 null");
                                        roomUserByHabbo.Z -= 0.35;
                                        roomUserByHabbo.isLying = true;
                                        roomUserByHabbo.UpdateNeeded = true;
                                        roomUserByHabbo.ApplyEffect(0x9d);
                                    }
                                    if (user2.RotBody == 3)
                                    {
                                        roomUserByHabbo.Statusses.Add("lay", "1.0 null");
                                        roomUserByHabbo.Z -= 0.35;
                                        roomUserByHabbo.isLying = true;
                                        roomUserByHabbo.UpdateNeeded = true;
                                        roomUserByHabbo.ApplyEffect(0x9d);
                                    }
                                    if (user2.RotBody == 1)
                                    {
                                        roomUserByHabbo.Statusses.Add("lay", "1.0 null");
                                        roomUserByHabbo.Z -= 0.35;
                                        roomUserByHabbo.isLying = true;
                                        roomUserByHabbo.UpdateNeeded = true;
                                        roomUserByHabbo.ApplyEffect(0x9d);
                                    }
                                    if (user2.RotBody == 7)
                                    {
                                        roomUserByHabbo.Statusses.Add("lay", "1.0 null");
                                        roomUserByHabbo.Z -= 0.35;
                                        roomUserByHabbo.isLying = true;
                                        roomUserByHabbo.UpdateNeeded = true;
                                        roomUserByHabbo.ApplyEffect(0x9d);
                                    }
                                    if (user2.RotBody == 5)
                                    {
                                        roomUserByHabbo.Statusses.Add("lay", "1.0 null");
                                        roomUserByHabbo.Z -= 0.35;
                                        roomUserByHabbo.isLying = true;
                                        roomUserByHabbo.UpdateNeeded = true;
                                        roomUserByHabbo.ApplyEffect(0x9d);
                                    }
                                }
                                else
                                {
                                    Session.SendWhisper("Opâ, " + Params[1] + " você está longe demais para dar um soco.", 0);
                                }
                            }
                        }
                    }
                }
        }

        public string Description =>
            "Dá um soco em alguém";

        public string Parameters =>
            "%alvo%";

        public string PermissionRequired =>
            "comando_users";
    }
}

