using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;

using Rocket.HabboHotel.Rooms;
using Rocket.HabboHotel.GameClients;
using Rocket.Communication.Packets.Outgoing.Rooms.Chat;
using Rocket.HabboHotel.Users;

namespace Rocket.HabboHotel.Rooms.Chat.Commands.iBlue.VIP
{
    class MudarCor : IChatCommand
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
            get { return "off/red/orange/yellow/green/blue/purple/pink"; }
        }
        public void Execute(GameClients.GameClient Session, Rooms.Room Room, string[] Params)
        {
            if (Params.Length == 1)
            {
                Session.SendWhisper("Opâ, digite a cor que deseja!", 34);
                return;
            }
            string chatColour = Params[1];
            string Colour = chatColour.ToUpper();
            switch (chatColour)
            {
                case "none":
                case "black":
                case "off":
                    Session.GetHabbo().chatColour = "";
                    Session.SendWhisper("Você a opção mudar cor.", 34);
                    break;
                case "blue":
                case "red":
                case "orange":
                case "yellow":
                case "green":
                case "purple":
                case "pink":
                    Session.GetHabbo().chatColour = chatColour;
                    Session.SendWhisper("Você ativou a cor : " + Colour + "", 34);
                    break;
                default:
                    Session.SendWhisper("A cor digitada: " + Colour + " não existe!", 34);
                    break;
            }
            return;
        }
    }
}