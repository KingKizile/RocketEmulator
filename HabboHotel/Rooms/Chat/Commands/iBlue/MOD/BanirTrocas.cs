using System;
using Rocket.Database.Interfaces;
using Rocket.HabboHotel.Users;
namespace Rocket.HabboHotel.Rooms.Chat.Commands.iBlue.MOD
{
    class BanirTrocas : IChatCommand
    {
        public string PermissionRequired
        {
            get { return "comando_mod"; }
        }

        public string Parameters
        {
            get { return "%target% %length%"; }
        }

        public string Description
        {
            get { return "proibição de outro usuário Trocas."; }
        }

        public void Execute(GameClients.GameClient Session, Rooms.Room Room, string[] Params)
        {
            if (Params.Length == 1)
            {
                Session.SendWhisper("Por favor, digite um nome de usuário e um comprimento válido em dias (min. 1 dia, máximos 365 dias).");
                return;
            }

            Habbo Habbo = RocketEmulador.GetHabboByUsername(Params[1]);
            if (Habbo == null)
            {
                Session.SendWhisper("An error occoured whilst finding that user in the database.");
                return;
            }

            if (Convert.ToDouble(Params[2]) == 0)
            {
                using (IQueryAdapter dbClient = RocketEmulador.GetDatabaseManager().GetQueryReactor())
                {
                    dbClient.RunQuery("UPDATE `user_info` SET `trading_locked` = '0' WHERE `user_id` = '" + Habbo.Id + "' LIMIT 1");
                }

                if (Habbo.GetClient() != null)
                {
                    Habbo.TradingLockExpiry = 0;
                    Habbo.GetClient().SendWhisper("Sua proibição de Trocas excepcional foi removida.");
                }

                Session.SendWhisper("You have successfully removed " + Habbo.Username + "'s trade ban.");
                return;
            }

            double Days;
            if (double.TryParse(Params[2], out Days))
            {
                if (Days < 1)
                    Days = 1;

                if (Days > 365)
                    Days = 365;

                double Length = (RocketEmulador.GetUnixTimestamp() + (Days * 86400));
                using (IQueryAdapter dbClient = RocketEmulador.GetDatabaseManager().GetQueryReactor())
                {
                    dbClient.RunQuery("UPDATE `user_info` SET `trading_locked` = '" + Length + "', `trading_locks_count` = `trading_locks_count` + '1' WHERE `user_id` = '" + Habbo.Id + "' LIMIT 1");
                }

                if (Habbo.GetClient() != null)
                {
                    Habbo.TradingLockExpiry = Length;
                    Habbo.GetClient().SendWhisper("Suas trocas foram banidas até: " + Days + " dia(s)!");
                }

                Session.SendWhisper("Você tem sucesso comercial banido " + Habbo.Username + " for " + Days + " day(s).");
            }
            else
                Session.SendWhisper("Please enter a valid integer.");
        }
    }
}
