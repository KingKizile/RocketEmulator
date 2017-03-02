using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;

using Rocket.Utilities;
using Rocket.HabboHotel.Users;
using Rocket.Communication.Packets.Outgoing.Handshake;

namespace Rocket.HabboHotel.Rooms.Chat.Commands.iBlue.USERS.Extra
{
    class MudarNome : IChatCommand
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
            get { return "Dá-lhe a opção de alterar seu nome de usuário."; }
        }

        public void Execute(GameClients.GameClient Session, Rooms.Room Room, string[] Params)
        {
            if (!this.CanChangeName(Session.GetHabbo()))
            {
                Session.SendNotification("Desculpa, parece que atualmente não tem a opção de alterar seu nome de usuário!");
                return;
            }

            Session.GetHabbo().ChangingName = true;
            Session.SendNotification("Por favor, esteja ciente de que se seu nome de usuário é considerado tão inapropriado, você será banido sem question.\r\rAlso nota que equipe não vai mudar seu nome de usuário novamente se você tiver um problema com o que você tem chosen.\r\rClose esta janela e clique em si mesmo para começar a escolher um novo nome de usuário!");
            Session.SendMessage(new UserObjectComposer(Session.GetHabbo()));
        }

        private bool CanChangeName(Habbo Habbo)
        {
            if (Habbo.Rank == 1 && Habbo.VIPRank == 0 && Habbo.LastNameChange == 0)
                return true;
            else if (Habbo.Rank == 1 && Habbo.VIPRank == 1 && (Habbo.LastNameChange == 0 || (RocketEmulador.GetUnixTimestamp() + 604800) > Habbo.LastNameChange))
                return true;
            else if (Habbo.Rank == 1 && Habbo.VIPRank == 2 && (Habbo.LastNameChange == 0 || (RocketEmulador.GetUnixTimestamp() + 86400) > Habbo.LastNameChange))
                return true;
            else if (Habbo.Rank == 1 && Habbo.VIPRank == 3)
                return true;
            else if (Habbo.GetPermissions().HasRight("mod_tool"))
                return true;

            return false;
        }
    }
}
