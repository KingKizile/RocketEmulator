using Rocket.Database.Interfaces;

namespace Rocket.HabboHotel.Rooms.Chat.Commands.iBlue.ADM
{
    class AdicionarQuarto : IChatCommand
    {
        public string PermissionRequired
        {
            get { return "comando_mod"; }
        }

        public string Parameters
        {
            get { return "Palavra"; }
        }

        public string Description
        {
            get { return "Digte  :addquarto para informações."; }
        }

        public void Execute(GameClients.GameClient Session, Rooms.Room Room, string[] Params)
        {
            Session.SendWhisper("Para adicionar um quarto a uma das lista de quartos recomendados",34);
            Session.SendWhisper("Exemplo: :addquarto 1 /rocket-navigator/pingpong.png", 34);
            Session.SendWhisper("Exemplo: Exemplo de link da imagem: /rocket-navigator/pingpong.png", 34);
            Session.SendWhisper("Categoria 1 = Quartos Públicos", 34);
            Session.SendWhisper("Categoria 38 = Quartos AeA", 34);
            Session.SendWhisper("Categoria 39 = Quartos Recomendados", 34);
            string BannedWord = Room.Name;
            string MeuParams1 = Params[1];
            string MeuParams2 = Params[2];
            if (!string.IsNullOrWhiteSpace(BannedWord))
            {
                using (IQueryAdapter dbClient = RocketEmulador.GetDatabaseManager().GetQueryReactor())
                {
                    dbClient.SetQuery("INSERT INTO navigator_publics (`room_id`, `caption`, `description`, `image_url`, `order_num`, `enabled`, `cat_id`) VALUES " +
                        "(@room, @nome, '', @image, '1','1',@cat_id );");
                    dbClient.AddParameter("room", Room.Id);
                    dbClient.AddParameter("nome", BannedWord);
                    dbClient.AddParameter("cat_id", MeuParams1);
                    dbClient.AddParameter("image", MeuParams2);

                    dbClient.RunQuery();

                    RocketEmulador.GetGame().GetNavigator().Init();

                    Session.SendWhisper("'o quarto " + BannedWord + "' foi adicionado com sucesso.");

                }
            }
        }
    }
}
