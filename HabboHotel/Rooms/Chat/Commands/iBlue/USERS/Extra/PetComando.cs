using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;

using Rocket.Communication.Packets.Outgoing.Rooms.Engine;

namespace Rocket.HabboHotel.Rooms.Chat.Commands.iBlue.USERS.Extra
{
    class PetComando : IChatCommand
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
            get { return "Se transforme em um Mascote"; }
        }

        public void Execute(GameClients.GameClient Session, Room Room, string[] Params)
        {
            RoomUser RoomUser = Session.GetHabbo().CurrentRoom.GetRoomUserManager().GetRoomUserByHabbo(Session.GetHabbo().Id);
            if (RoomUser == null)
                return;

            if (!Room.PetMorphsAllowed)
            {
                Session.SendNotification("Você não pode usar esse comando neste quarto!");
                if (Session.GetHabbo().PetId > 0)
                {
                    Session.SendNotification("Use ':pet Habbo' para voltar ao normal");
                    //Change the users Pet Id.
                    Session.GetHabbo().PetId = 0;

                    //Quickly remove the old user instance.
                    Room.SendMessage(new UserRemoveComposer(RoomUser.VirtualId));

                    //Add the new one, they won't even notice a thing!!11 8-)
                    Room.SendMessage(new UsersComposer(RoomUser));
                }
                return;
            }

            if (Params.Length == 1)
            {
                Session.SendNotification("Algo deu errado! Cheque a lista de Pets usando ':pet list' ");
                return;
            }

            if (Params[1].ToString().ToLower() == "lista")
            {
                Session.SendNotification("<b>Habbo</b> - Volte ao normal.<br> <b>Dog</b> - Vire um cachorro.<br> <b>Cat</b> - Vire um gato.<br> <b>Terrier</b> - Vire um pitbull.<br> <b>Croc</b> - Vire um Jacaré.<br> <b>Bear</b> - Vire um urso.<br> <b>Pig</b> - Vire um porco.<br> <b>Lion</b> - Vire um leão.<br> <b>Rhino</b> - Vire um rinoceronte.<br> <b>Spider</b> - Vire uma aranha.<br> <b>Turtle</b> - Vire uma tartaruga.<br> <b>Chick</b> - Vire uma galinha.<br> <b>Frog</b> - Vire um sapo.<br> <b>Drag</b> - Vire um dragão.<br> <b>Monkey</b> - Vire um macaco.<br> <b>Horse</b> - Vire um cavalo.<br> <b>Bunny</b> - Vire um coelho.<br> <b>Gnome</b> - Vire um gnomo.<br><br> <b>piglets</b> - Vire um Porco Filhote.<br> <b>kitten</b> - Vire um Gato Filhote.<br> <b>puppies</b> - Vire um Cachorro Filhote.");
                return;
            }

            int TargetPetId = GetPetIdByString(Params[1].ToString());
            if (TargetPetId == 0)
            {
                Session.SendNotification("Algo deu errado! Cheque a lista de Pets usando ':pet list' ");
                return;
            }

            //Change the users Pet Id.
            Session.GetHabbo().PetId = (TargetPetId == -1 ? 0 : TargetPetId);

            //Quickly remove the old user instance.
            Room.SendMessage(new UserRemoveComposer(RoomUser.VirtualId));

            //Add the new one, they won't even notice a thing!!11 8-)
            Room.SendMessage(new UsersComposer(RoomUser));

            //Tell them a quick message.
            if (Session.GetHabbo().PetId > 0)
                Session.SendNotification("Use ':pet Habbo' para voltar ao normal");
        }

        private int GetPetIdByString(string Pet)
        {
            switch (Pet.ToLower())
            {
                default:
                    return 0;
                case "habbo":
                    return -1;
                case "dog":
                    return 60;//This should be 0.
                case "cat":
                    return 1;
                case "terrier":
                    return 2;
                case "croc":
                case "croco":
                    return 3;
                case "bear":
                    return 4;
                case "liz":
                case "pig":
                case "kill":
                    return 5;
                case "lion":
                case "rawr":
                    return 6;
                case "rhino":
                    return 7;
                case "spider":
                    return 8;
                case "turtle":
                    return 9;
                case "chick":
                case "chicken":
                    return 10;
                case "frog":
                    return 11;
                case "drag":
                case "dragon":
                    return 12;
                case "monkey":
                    return 14;
                case "horse":
                    return 15;
                case "bunny":
                    return 17;
                case "pigeon":
                    return 21;
                case "demon":
                    return 23;
                case "gnome":
                    return 26;
                case "piglets":
                    return 30;
                case "kitten":
                    return 28;
                case "puppies":
                    return 29;
            }
        }
    }
}