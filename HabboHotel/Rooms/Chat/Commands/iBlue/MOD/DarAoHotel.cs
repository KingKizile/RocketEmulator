using System.Linq;
using Rocket.Communication.Packets.Outgoing.Inventory.Purse;
using Rocket.Database.Interfaces;
using Rocket.HabboHotel.GameClients;
namespace Rocket.HabboHotel.Rooms.Chat.Commands.iBlue.MOD
{
    internal class DarAoHotel : IChatCommand
    {
        public string PermissionRequired
        {
            get { return "comando_mod"; }
        }

        public string Parameters
        {
            get { return "%type% %amount%"; }
        }

        public string Description
        {
            get { return "moedas de atualização para todos em seu banco de dados."; }
        }

        public void Execute(GameClient session, Room room, string[] Params)
        {
            if (Params.Length == 1)
            {
                session.SendWhisper("Please enter a currency type! (coins, duckets, diamonds, gotw)");
                return;
            }

            string updateVal = Params[1];
            switch (updateVal.ToLower())
            {
                case "coins":
                case "creditos":
                case "moedas":
                case "credits":
                    {
                        if (!session.GetHabbo().GetPermissions().HasCommand("command_give_coins"))
                        {
                            session.SendWhisper("Opa, parece que você não tem as permissões para usar este comando!");
                            break;
                        }
                        int amount;
                        if (int.TryParse(Params[2], out amount))
                        {
                            foreach (GameClient client in RocketEmulador.GetGame().GetClientManager().GetClients.ToList())
                            {
                                client.GetHabbo().Credits += amount;
                                client.SendMessage(new CreditBalanceComposer(client.GetHabbo().Credits));
                            }
                            using (IQueryAdapter dbClient = RocketEmulador.GetDatabaseManager().GetQueryReactor())
                            {
                                dbClient.RunQuery("UPDATE users SET credits = credits + " + amount);
                            }
                            break;
                        }
                        session.SendWhisper("Opa, que parece ser uma quantidade inválida!");
                        break;
                    }
                case "pixels":
                case "duckets":
                case "ametistas":
                    {
                        if (!session.GetHabbo().GetPermissions().HasCommand("command_give_pixels"))
                        {
                            session.SendWhisper("Opa, parece que você não tem as permissões para usar este comando!");
                            break;
                        }
                        int amount;
                        if (int.TryParse(Params[2], out amount))
                        {
                            foreach (GameClient client in RocketEmulador.GetGame().GetClientManager().GetClients.ToList())
                            {
                                client.GetHabbo().Duckets += amount;
                                client.SendMessage(new HabboActivityPointNotificationComposer(
                                    client.GetHabbo().Duckets, amount));
                            }
                            using (IQueryAdapter dbClient = RocketEmulador.GetDatabaseManager().GetQueryReactor())
                            {
                                dbClient.RunQuery("UPDATE users SET activity_points = activity_points + " + amount);
                            }
                            break;
                        }
                        session.SendWhisper("Opa, que parece ser uma quantidade inválida!");
                        break;
                    }
                case "diamonds":
                case "topazios":
                case "diamantes":
                    {
                        if (!session.GetHabbo().GetPermissions().HasCommand("command_give_diamonds"))
                        {
                            session.SendWhisper("Opa, parece que você não tem as permissões para usar este comando!");
                            break;
                        }
                        int amount;
                        if (int.TryParse(Params[2], out amount))
                        {
                            foreach (GameClient client in RocketEmulador.GetGame().GetClientManager().GetClients.ToList())
                            {
                                client.GetHabbo().Diamonds += amount;
                                client.SendMessage(new HabboActivityPointNotificationComposer(client.GetHabbo().Diamonds,
                                    amount,
                                    5));
                            }
                            using (IQueryAdapter dbClient = RocketEmulador.GetDatabaseManager().GetQueryReactor())
                            {
                                dbClient.RunQuery("UPDATE users SET vip_points = vip_points + " + amount);
                            }
                            break;
                        }
                        session.SendWhisper("Opa, que parece ser uma quantidade inválida!");
                        break;
                    }
                case "gotw":
                case "gotwpoints":
                    {
                        if (!session.GetHabbo().GetPermissions().HasCommand("command_give_gotw"))
                        {
                            session.SendWhisper("Opa, parece que você não tem as permissões para usar este comando!");
                            break;
                        }
                        int amount;
                        if (int.TryParse(Params[2], out amount))
                        {
                            foreach (GameClient client in RocketEmulador.GetGame().GetClientManager().GetClients.ToList())
                            {
                                client.GetHabbo().GOTWPoints = client.GetHabbo().GOTWPoints + amount;
                                client.SendMessage(new HabboActivityPointNotificationComposer(client.GetHabbo().GOTWPoints,
                                    amount, 103));
                            }
                            using (IQueryAdapter dbClient = RocketEmulador.GetDatabaseManager().GetQueryReactor())
                            {
                                dbClient.RunQuery("UPDATE users SET gotw_points = gotw_points + " + amount);
                            }
                            break;
                        }
                        session.SendWhisper("Opa, que parece ser uma quantidade inválida!");
                        break;
                    }
            }
        }
    }
}