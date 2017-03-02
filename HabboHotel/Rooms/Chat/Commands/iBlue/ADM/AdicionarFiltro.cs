using Rocket.Database.Interfaces;

namespace Rocket.HabboHotel.Rooms.Chat.Commands.iBlue.ADM
{
    class AdicionarFiltro : IChatCommand
    {
        public string PermissionRequired
        {
            get { return "comando_adm"; }
        }

        public string Parameters
        {
            get { return "Palavra"; }
        }

        public string Description
        {
            get { return "Adicione uma palavra ao filtro"; }
        }

        public void Execute(GameClients.GameClient Session, Rooms.Room Room, string[] Params)
        {
                    
                string BannedWord = Params[1];

                if (!string.IsNullOrWhiteSpace(BannedWord))
                {
                using (IQueryAdapter dbClient = RocketEmulador.GetDatabaseManager().GetQueryReactor())
                {
                    dbClient.SetQuery("INSERT INTO wordfilter (`word`, `addedby`, `replacement`, `bannable`) VALUES " +
                        "(@ban, @username, 'Está palavra está proibida' , '1');");
                    dbClient.AddParameter("ban", BannedWord.ToLower());
                    dbClient.AddParameter("username", Session.GetHabbo().Username);

                    dbClient.RunQuery();

                    RocketEmulador.GetGame().GetChatManager().GetFilter().Init();

                    Session.SendWhisper("'" + BannedWord + "' a palavra foi adicionada com sucesso.");

                }  
                }
            }
        }
    }
