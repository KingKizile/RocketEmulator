using System;
using System.Runtime.CompilerServices;
using Rocket.HabboHotel.GameClients;
using Rocket.Communication.Packets.Outgoing.Rooms.Notifications;
using Rocket.Database.Interfaces;
using Rocket.Communication.Packets.Outgoing.Inventory.Purse;
using Rocket.HabboHotel.Users;
namespace Rocket.HabboHotel.Rooms.Chat.Commands.iBlue.MOD
{
    class EventoWins : IChatCommand
    {
        public string PermissionRequired
        {
            get { return "comando_mod"; }
        }

        public string Parameters
        {
            get { return "nick nivel 0 0"; }
        } 
        public string Description
        {
            get { return "Com esse comando é possível dar pontos no hall e emblema de nível."; }
        }
        String MeuParame3 = RocketEmulador.RocketData().data["pontos.premiar"];
        String MeuParame4 = RocketEmulador.RocketData().data["diamantes.premiar"];
        public void Execute(GameClients.GameClient Session, Rooms.Room Room, string[] Params)

        {
            {

                using (IQueryAdapter dbClient = RocketEmulador.GetDatabaseManager().GetQueryReactor())
                {
                    dbClient.SetQuery("UPDATE users SET username = @nome, pontos_eventos = pontos_eventos+ @pontos, nivel_eventos = @nivel WHERE username = @nome");
                    dbClient.AddParameter("nome", Params[1]);
                    dbClient.AddParameter("pontos", MeuParame3);
                    dbClient.AddParameter("nivel", Params[2]);
                    dbClient.RunQuery();

                }
            }
            string nome = Room.Name;
            if (Params.Length == 1)

            {
                Session.SendWhisper("Por favor digite uma mensagem para enviar.");
            }
            else
            {
                GameClient clientByUsername = RocketEmulador.GetGame().GetClientManager().GetClientByUsername(Params[1]);
                if (clientByUsername == null)
                {
                    Session.SendWhisper("Opa, não conseguimos achar este usuário!", 34);
                }

                else
                {

                    Session.SendWhisper("Você deu " + MeuParame3 + " pontos para o usuário " + Params[1] + ".", 34);

                    int num;

                    RocketEmulador.GetGame().GetClientManager().SendMessage(new RoomNotificationComposer("eventos", "message", "O usuário " + Params[1] + " ganhou o evento: " + nome));

                    {
                        if (int.TryParse(MeuParame4, out num))
                        {
                            clientByUsername.SendWhisper("O promotor: " + Session.GetHabbo().Username + " te pagou " + num.ToString() + " Topázio(s), por ganhar o evento!", 34);
                            Habbo habbo3 = clientByUsername.GetHabbo();
                            habbo3.Diamonds += num;
                            clientByUsername.SendMessage(new HabboActivityPointNotificationComposer(clientByUsername.GetHabbo().Diamonds, num, 5));
                            if (clientByUsername.GetHabbo().Id != Session.GetHabbo().Id)
                            {

                            }
                            Session.SendWhisper(string.Concat(new object[] { "Você pagou ", num, " Topázio(s) ao ", clientByUsername.GetHabbo().Username, "!" }), 34);

                            clientByUsername.SendWhisper("Parabéns você ganhou o emblema de nível " + Params[2] + " com sucesso.", 34);
                            if (clientByUsername != null)
                            {
                                if (!clientByUsername.GetHabbo().GetBadgeComponent().HasBadge(Params[2]))
                                {
                                    clientByUsername.GetHabbo().GetBadgeComponent().GiveBadge(Params[2], true, clientByUsername);
                                    if (clientByUsername.GetHabbo().Id != Session.GetHabbo().Id)


                                        Session.SendWhisper("Você deu com êxito o emblema: " + Params[2] + " para o usuário: " + Params[1] + "!", 34);
                                }
                                else
                                    Session.SendWhisper("O usuário " + Params[1] + " já recebeu o emblema: (" + Params[2] + ") !", 34);
                                return;
                            }
                            else
                            {
                            }

                            {
                            }
                        }
                    }
                }
            }
        }
    }
}









