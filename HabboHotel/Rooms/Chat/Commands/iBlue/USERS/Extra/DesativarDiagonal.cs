using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;

namespace Rocket.HabboHotel.Rooms.Chat.Commands.iBlue.USERS.Extra
{
    class DesativarDiagonal : IChatCommand
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
            get { return "Deseja desativar diagonal andando em seu quarto? Digite este comando!"; }
        }

        public void Execute(GameClients.GameClient Session, Rooms.Room Room, string[] Params)
        {
            if (!Room.CheckRights(Session, true))
            {
                Session.SendWhisper("Opa, só o proprietário desta sala pode executar este comando!");
                return;
            }

            Room.GetGameMap().DiagonalEnabled = !Room.GetGameMap().DiagonalEnabled;
            Session.SendWhisper("Atualizado com êxito o valor booleano diagonal para este quarto.");
        }
    }
}
