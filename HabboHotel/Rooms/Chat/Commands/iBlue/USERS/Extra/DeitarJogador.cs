﻿using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;

using Rocket.HabboHotel.Rooms;

namespace Rocket.HabboHotel.Rooms.Chat.Commands.iBlue.USERS
{
    class DeitarJogador : IChatCommand
    {
        public string PermissionRequired
        {
            get { return "comando_users"; }
        }

        public string Parameters
        {
            get { return ""; }
        }

        public string Description
        {
            get { return "Permite que você deite-se no quarto, sem precisar de uma cama."; }
        }

        public void Execute(GameClients.GameClient Session, Rooms.Room Room, string[] Params)
        {
            RoomUser User = Room.GetRoomUserManager().GetRoomUserByHabbo(Session.GetHabbo().Id);
            if (User == null)
                return;

            if (!Room.GetGameMap().ValidTile(User.X + 2, User.Y + 2) && !Room.GetGameMap().ValidTile(User.X + 1, User.Y + 1))
            {
                Session.SendWhisper("Opa, não posso deitar aqui - tente em outro lugar!");
                return;
            }

            if (User.Statusses.ContainsKey("sit") || User.isSitting || User.RidingHorse || User.IsWalking)
                return;

            if (Session.GetHabbo().Effects().CurrentEffect > 0)
                Session.GetHabbo().Effects().ApplyEffect(0);

            if (!User.Statusses.ContainsKey("lay"))
            {
                if ((User.RotBody % 2) == 0)
                {
                    if (User == null)
                        return;

                    try
                    {
                        User.Statusses.Add("lay", "1.0 null");
                        User.Z -= 0.35;
                        User.isLying = true;
                        User.UpdateNeeded = true;
                    }
                    catch { }
                }
                else
                {
                    User.RotBody--;//
                    User.Statusses.Add("lay", "1.0 null");
                    User.Z -= 0.35;
                    User.isLying = true;
                    User.UpdateNeeded = true;
                }

            }
            else
            {
                User.Z += 0.35;
                User.Statusses.Remove("lay");
                User.Statusses.Remove("1.0");
                User.isLying = false;
                User.UpdateNeeded = true;
            }
        }
    }
}