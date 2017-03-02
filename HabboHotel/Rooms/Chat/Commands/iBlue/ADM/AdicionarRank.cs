using System.Linq;
using Rocket.HabboHotel.GameClients;
using Rocket.Database.Interfaces;
using System.Runtime.CompilerServices;
namespace Rocket.HabboHotel.Rooms.Chat.Commands.iBlue.ADM
{
    class AdicionarRank : IChatCommand
    {
        public string PermissionRequired
        {
            get { return "comando_ceo"; }
        }

        public string Parameters
        {
            get { return "Nome + Rank"; }
        }

        public string Description
        {
            get { return "Adiciona ou remove rank de um usuário."; }
        }

        public void Execute(GameClients.GameClient Session, Rooms.Room Room, string[] Params)
        {

            string Cargo = Params[2];                {
                using (IQueryAdapter dbClient = RocketEmulador.GetDatabaseManager().GetQueryReactor())
                {
                    dbClient.SetQuery("UPDATE users SET username = @nome, rank = @rank WHERE username = @nome");
                    dbClient.AddParameter("nome", Params[1]);
                    dbClient.AddParameter("rank", Params[2]);
                   

                    dbClient.RunQuery();



                    RocketEmulador.GetGame().GetPermissionManager().Init();

                    foreach (GameClient Client in RocketEmulador.GetGame().GetClientManager().GetClients.ToList())
                    {
                        if (Client == null || Client.GetHabbo() == null || Client.GetHabbo().GetPermissions() == null)
                            continue;

                        Client.GetHabbo().GetPermissions().Init(Client.GetHabbo());
                    }
                    Session.SendWhisper("Você deu o rank: " + Cargo + " ao usuário "+ Params[1] +".");
                    }
                }
            }
        }
    }
