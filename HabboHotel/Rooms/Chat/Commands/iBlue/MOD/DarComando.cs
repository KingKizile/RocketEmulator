namespace Rocket.HabboHotel.Rooms.Chat.Commands.iBlue.MOD
{
    using Rocket;
    using Rocket.Communication.Packets.Outgoing.Inventory.Purse;
    using Rocket.HabboHotel.GameClients;
    using Rocket.HabboHotel.Rooms;
    using Rocket.HabboHotel.Rooms.Chat.Commands;
    using Rocket.HabboHotel.Users;
    using System;

    internal class DarComando : IChatCommand
    {
        public void Execute(GameClient Session, Room Room, string[] Params)
        {
            if (Params.Length == 1)
            {
                Session.SendWhisper("Por favor insira ! (moedas, topazios , ametistas)", 0);
            }
            else
            {
                GameClient clientByUsername = RocketEmulador.GetGame().GetClientManager().GetClientByUsername(Params[1]);
                if (clientByUsername == null)
                {
                    Session.SendWhisper("Opa, não conseguiu este usuário!", 0);
                }
                else
                {
                    int num;
                    string str = Params[2];
                    switch (str.ToLower())
                    {
                        case "coins":
                        case "credits":
                        case "moedas":
                        case "creditos":
                        case "ouro":
                            if (Session.GetHabbo().GetPermissions().HasCommand("comando_mod"))
                            {
                                if (int.TryParse(Params[3], out num))
                                {
                                    Habbo habbo = clientByUsername.GetHabbo();
                                    clientByUsername.GetHabbo().Credits = habbo.Credits += num;
                                    clientByUsername.SendMessage(new CreditBalanceComposer(clientByUsername.GetHabbo().Credits));
                                    if (clientByUsername.GetHabbo().Id != Session.GetHabbo().Id)
                                    {
                                        Session.SendWhisper(Session.GetHabbo().Username + " Ele enviou " + num.ToString() + " Crédito(s)!");
                                    }
                                    Session.SendWhisper(string.Concat(new object[] { "Você enviou ", num, " Crédito(s) a ", clientByUsername.GetHabbo().Username, "!" }), 0);
                                }
                                else
                                {
                                    Session.SendWhisper("Opa, somente quantidades em números..", 0);
                                }
                                break;
                            }
                            Session.SendWhisper("Opa, você não tem as permissões necessárias para usar este comando!", 0);
                            break;

                        case "pixels":
                        case "duckets":
                        case "ametistas":
                            if (Session.GetHabbo().GetPermissions().HasCommand("comando_mod"))
                            {
                                if (int.TryParse(Params[3], out num))
                                {
                                    Habbo habbo2 = clientByUsername.GetHabbo();
                                    habbo2.Duckets += num;
                                    clientByUsername.SendMessage(new HabboActivityPointNotificationComposer(clientByUsername.GetHabbo().Duckets, num, 0));
                                    if (clientByUsername.GetHabbo().Id != Session.GetHabbo().Id)
                                    {
                                        Session.SendWhisper(Session.GetHabbo().Username + " Ele enviou - lhe " + num.ToString() + " Ametista(s)!");
                                    }
                                    Session.SendWhisper(string.Concat(new object[] { "você enviou ", num, " Ametista(s) a ", clientByUsername.GetHabbo().Username, "!" }), 0);
                                }
                                else
                                {
                                    Session.SendWhisper("Opa, somente quantidades em números..", 0);
                                }
                                break;
                            }
                            Session.SendWhisper("Opa, você não tem permissão para enviar duckets!", 0);
                            break;

                        case "diamonds":
                        case "diamantes":
                        case "topazios":
                            if (Session.GetHabbo().GetPermissions().HasCommand("comando_mod"))
                            {
                                if (int.TryParse(Params[3], out num))
                                {
                                    Habbo habbo3 = clientByUsername.GetHabbo();
                                    habbo3.Diamonds += num;
                                    clientByUsername.SendMessage(new HabboActivityPointNotificationComposer(clientByUsername.GetHabbo().Diamonds, num, 5));
                                    if (clientByUsername.GetHabbo().Id != Session.GetHabbo().Id)
                                    {
                                        Session.SendWhisper(Session.GetHabbo().Username + " Ele enviou " + num.ToString() + " Topázio(s)!");
                                    }
                                    Session.SendWhisper(string.Concat(new object[] { "Você enviou ", num, " Topázio(s) a ", clientByUsername.GetHabbo().Username, "!" }), 0);
                                }
                                else
                                {
                                    Session.SendWhisper("Opa, somente quantidades em números..!", 0);
                                }
                                break;
                            }
                            Session.SendWhisper("Opa, você não tem as permissões necessárias para usar este comando!", 0);
                            break;

                        case "gotw":
                        case "ibluecoins":

                        case "gotwpoints":
                            if (Session.GetHabbo().GetPermissions().HasCommand("comando_mod"))
                            {
                                if (int.TryParse(Params[3], out num))
                                {
                                    clientByUsername.GetHabbo().GOTWPoints += num;
                                    clientByUsername.SendMessage(new HabboActivityPointNotificationComposer(clientByUsername.GetHabbo().GOTWPoints, num, 0x67));
                                    if (clientByUsername.GetHabbo().Id != Session.GetHabbo().Id)
                                    {
                                        Session.SendWhisper(Session.GetHabbo().Username + " Ele enviou " + num.ToString() + " Punto(s) GOTW!");
                                    }
                                    Session.SendWhisper(string.Concat(new object[] { "Você enviou ", num, " ponto(s) GOTW a ", clientByUsername.GetHabbo().Username, "!" }), 0);
                                }
                                else
                                {
                                    Session.SendWhisper("Opa, somente quantidades em números..!", 0);
                                }
                                break;
                            }
                            Session.SendWhisper("Opa, você não tem a permissão necessária para usar este comando!", 0);
                            break;

                        default:
                            Session.SendWhisper("'" + str + "' uma moeda não é válido!", 0);
                            break;
                    }
                }
            }
        }

        public string Description =>
            "";

        public string Parameters =>
            "%usuario% %tipo% %valor%";

        public string PermissionRequired =>
            "comando_mod";
    }
}

