using Rocket.Communication.Packets.Outgoing.Rooms.Notifications;
using Rocket.Database.Interfaces;
using Rocket.HabboHotel.GameClients;
using System;
using System.Data;
using System.Runtime.CompilerServices;
namespace Rocket.HabboHotel.Rooms.Chat.Commands.iBlue.VIP
{
    internal class CasarComando : IChatCommand
    {
        private string _Casado;

        private string _Pedido;

        private string _TargetPedido;

        private string _TargetCasado;

        public string PermissionRequired
        {
            get
            {
                return "comando_vip";
            }
        }

        public string Parameters
        {
            get
            {
                return "%acao% %username%";
            }
        }

        public string Description
        {
            get
            {
                return "Comando para casamento!";
            }
        }

        public void Execute(GameClient Session, Room Room, string[] Params)
        {
            Session.SendWhisper("[Atenção] para qualquer ação será necessário relogar no hotel para atualizar. [Atenção]", 34);
            bool flag = Params.Length == 1;
            if (flag)
            {
                Session.SendWhisper("Para se casar use :casar pedido <nick>", 34);
                Session.SendWhisper("Para divórcio use :casar divorcio", 34);
                Session.SendWhisper("Para aceitar um pedido use :casar aceitar", 34);
                Session.SendWhisper("Para negar um pedido use :casar negar", 34);

            }
          
            else
            {

                bool flag2 = Params.Length > 2;
                if (flag2)
                {
                    GameClient TargetClient = RocketEmulador.GetGame().GetClientManager().GetClientByUsername(Params[2]);
                    using (IQueryAdapter dbClient = RocketEmulador.GetDatabaseManager().GetQueryReactor())
                    {
                        dbClient.SetQuery("SELECT * FROM `users` WHERE `id` = " + TargetClient.GetHabbo().Id);
                        DataRow GetCasado = dbClient.getRow();
                        bool flag3 = GetCasado["casado"] != null || Convert.ToString(GetCasado["casado"]) == "";
                        if (flag3)
                        {
                            this._TargetCasado = Convert.ToString(GetCasado["casado"]);
                        }
                        else
                        {
                            this._TargetCasado = "";
                        }
                        bool flag4 = GetCasado["pedidodecasamento"] != null || Convert.ToString(GetCasado["pedidodecasamento"]) == "";
                        if (flag4)
                        {
                            this._TargetPedido = Convert.ToString(GetCasado["pedidodecasamento"]);
                        }
                        else
                        {
                            this._TargetPedido = "";
                        }
                    }
                }
                using (IQueryAdapter dbClient2 = RocketEmulador.GetDatabaseManager().GetQueryReactor())
                {
                    dbClient2.SetQuery("SELECT * FROM `users` WHERE `id` = " + Session.GetHabbo().Id);
                    DataRow GetCasado2 = dbClient2.getRow();
                    bool flag5 = GetCasado2["casado"] != null || Convert.ToString(GetCasado2["casado"]) == "";
                    if (flag5)
                    {
                        this._Casado = Convert.ToString(GetCasado2["casado"]);
                    }
                    else
                    {
                        this._Casado = "";
                    }
                    bool flag6 = GetCasado2["pedidodecasamento"] != null || Convert.ToString(GetCasado2["pedidodecasamento"]) == "";
                    if (flag6)
                    {
                        this._Pedido = Convert.ToString(GetCasado2["pedidodecasamento"]);
                    }
                    else
                    {
                        this._Pedido = "";
                    }
                }
                bool flag7 = Params[1] == "pedido";
                if (flag7)
                {
                    using (IQueryAdapter dbClient3 = RocketEmulador.GetDatabaseManager().GetQueryReactor())
                    {
                        bool flag8 = this._TargetCasado == null || this._TargetCasado == "";
                        if (flag8)
                        {
                            bool flag9 = this._TargetPedido == null || this._TargetPedido == "";
                            if (flag9)
                            {
                                GameClient TargetClient2 = RocketEmulador.GetGame().GetClientManager().GetClientByUsername(Params[2]);
                                dbClient3.SetQuery("UPDATE users SET pedidodecasamento='" + Session.GetHabbo().Username + "' WHERE id = @UserId");
                                dbClient3.AddParameter("UserId", TargetClient2.GetHabbo().Id);
                                dbClient3.RunQuery();
                                TargetClient2.SendMessage(new RoomNotificationComposer("Wooow! *o*", "<font size='15'><b>" + Session.GetHabbo().Username + "</b> te pediu em casamento!</font>\n Para aceitar use ':casar aceitar' ou para negar use ':casar negar'", "", "", ""));
                            }
                            else
                            {
                                Session.SendWhisper("Opss... Esse usuário parece já ter um pedido para casamento.", 34);
                            }
                        }
                        else
                        {
                            Session.SendWhisper("Opsss... Esse usuário já é casado!", 34);
                        }
                    }
                }
                bool flag10 = Params[1] == "aceitar";
                if (flag10)
                {
                    using (IQueryAdapter dbClient4 = RocketEmulador.GetDatabaseManager().GetQueryReactor())
                    {
                        dbClient4.SetQuery("SELECT * FROM `users` WHERE `id` = " + Session.GetHabbo().Id);
                        DataRow GetCasado3 = dbClient4.getRow();
                        string PedidoDe = Convert.ToString(GetCasado3["pedidodecasamento"]);
                        bool flag11 = PedidoDe != "";
                        if (flag11)
                        {
                            GameClient TargetClient3 = RocketEmulador.GetGame().GetClientManager().GetClientByUsername(PedidoDe);
                            using (IQueryAdapter SaveCasamento = RocketEmulador.GetDatabaseManager().GetQueryReactor())
                            {
                                SaveCasamento.SetQuery("UPDATE `users` SET pedidodecasamento='', casado=@User WHERE id = " + Session.GetHabbo().Id);
                                SaveCasamento.AddParameter("@User", PedidoDe);
                                SaveCasamento.RunQuery();
                            }
                            using (IQueryAdapter SaveCasamento2 = RocketEmulador.GetDatabaseManager().GetQueryReactor())
                            {
                                SaveCasamento2.SetQuery("UPDATE `users` SET pedidodecasamento='', casado=@User WHERE id = " + TargetClient3.GetHabbo().Id);
                                SaveCasamento2.AddParameter("@User", Session.GetHabbo().Username);
                                SaveCasamento2.RunQuery();
                            }

                           Session.SendWhisper("Casados! *oo*"  + PedidoDe + " agora é seu novo amor! Você pode se divorciar usando :casar divorcio",34);
                            TargetClient3.SendWhisper("Casados! *oo*"  + Session.GetHabbo().Username + " agora é seu novo amor! Você pode se divorciar usando :casar divorcio",34);
                                                   
                           
                            using (IQueryAdapter dbClient = RocketEmulador.GetDatabaseManager().GetQueryReactor())
                            {
                                // Altera a missão.
                                dbClient.RunQuery("UPDATE users SET motto = 'Casado com " + Session.GetHabbo().Username + "' WHERE Id = '" + TargetClient3.GetHabbo().Id + "' LIMIT 1");
                                dbClient.RunQuery("UPDATE users SET motto = 'Casado com "  +TargetClient3.GetHabbo().Username + "' WHERE Id = '" + Session.GetHabbo().Id + "' LIMIT 1");
                                dbClient.RunQuery();
                                // Coloca o usuário 1 no <3
                                dbClient.SetQuery("INSERT INTO user_relationships (`user_id`, `target`, `type`) VALUES " + "(@userid, @target, '1');");
                                dbClient.AddParameter("userid", Session.GetHabbo().Id);
                                dbClient.AddParameter("target", TargetClient3.GetHabbo().Id);
                                dbClient.RunQuery();
                                // Coloca o usuário 2 no <3 
                                dbClient.SetQuery("INSERT INTO user_relationships (`user_id`, `target`, `type`) VALUES " + "(@userid, @target, '1');");
                                dbClient.AddParameter("userid", TargetClient3.GetHabbo().Id);
                                dbClient.AddParameter("target", Session.GetHabbo().Id);
                                dbClient.RunQuery();

                                RocketEmulador.GetGame().GetClientManager().SendMessage(new RoomNotificationComposer("casar", "message", "O usuário " + TargetClient3.GetHabbo().Username + " acaba de se casar com o usuário: " + Session.GetHabbo().Username));

                            }
                        }

                        else
                        {
                            Session.SendWhisper("Nenhum Pedido! :(", 34);
                        }
                    }
                }
                bool flag12 = Params[1] == "negar";
                if (flag12)
                {
                    using (IQueryAdapter dbClient5 = RocketEmulador.GetDatabaseManager().GetQueryReactor())
                    {
                        dbClient5.SetQuery("SELECT * FROM `users` WHERE `id` = " + Session.GetHabbo().Id);
                        DataRow GetCasado4 = dbClient5.getRow();
                        string PedidoDe2 = Convert.ToString(GetCasado4["pedidodecasamento"]);
                        bool flag13 = PedidoDe2 != "";
                        if (flag13)
                        {
                            using (IQueryAdapter SaveCasamento3 = RocketEmulador.GetDatabaseManager().GetQueryReactor())
                            {
                                SaveCasamento3.SetQuery("UPDATE `users` SET pedidodecasamento='' WHERE id = " + Session.GetHabbo().Id);
                                SaveCasamento3.RunQuery();
                            }
                            Session.SendWhisper("Você negou o pedido de " + PedidoDe2 + "!", 0);
                            GameClient TargetClient4 = RocketEmulador.GetGame().GetClientManager().GetClientByUsername(PedidoDe2);
                            TargetClient4.SendMessage(new RoomNotificationComposer("Sentimos muito...", "<b>" + Session.GetHabbo().Username + "</b> não aceitou seu pedido de casamento...!\n\nNão fique triste, você ainda pode casar com outras pessoas!", "", "", ""));
                        }
                        else
                        {
                            Session.SendWhisper("Você não tem pedidos!", 34);
                        }
                    }
                }

                bool flag14 = Params[1] == "divorcio";
                if (flag14)

                {
                   
                    using (IQueryAdapter dbClient6 = RocketEmulador.GetDatabaseManager().GetQueryReactor())
                    {
                        dbClient6.SetQuery("SELECT * FROM `users` WHERE `id` = " + Session.GetHabbo().Id);
                        DataRow GetCasado5 = dbClient6.getRow();
                        string CasadoCom = Convert.ToString(GetCasado5["casado"]);
                        bool flag15 = CasadoCom != "";
                        if (flag15)

                        {
                            GameClient TargetClient3 = RocketEmulador.GetGame().GetClientManager().GetClientByUsername(CasadoCom);
                            using (IQueryAdapter SaveCasamento4 = RocketEmulador.GetDatabaseManager().GetQueryReactor())
                            {
                                SaveCasamento4.SetQuery("UPDATE `users` SET pedidodecasamento='', casado='' WHERE id = " + Session.GetHabbo().Id);
                                SaveCasamento4.RunQuery();
                                // Deleta o <3 do usuário 1
                                SaveCasamento4.SetQuery("DELETE FROM `user_relationships` WHERE `user_id` = '" + Session.GetHabbo().Id + "' AND `target` = @target LIMIT 1");
                                SaveCasamento4.AddParameter("target", TargetClient3.GetHabbo().Id);
                                SaveCasamento4.RunQuery();
                                // Deleta o <3 do usuário 2
                                SaveCasamento4.SetQuery("DELETE FROM `user_relationships` WHERE `user_id` = '" + TargetClient3.GetHabbo().Id + "' AND `target` = @target LIMIT 1");
                                SaveCasamento4.AddParameter("target", Session.GetHabbo().Id);
                                SaveCasamento4.RunQuery();
                                // Deleta a missão
                                SaveCasamento4.RunQuery("UPDATE users SET motto = '' WHERE Id = '" + TargetClient3.GetHabbo().Id + "' LIMIT 1");
                                SaveCasamento4.RunQuery("UPDATE users SET motto = '' WHERE Id = '" + Session.GetHabbo().Id + "' LIMIT 1");
                            }
                            Session.SendWhisper("Você divorciou de " + CasadoCom + "!", 34);
                            GameClient TargetClient5 = RocketEmulador.GetGame().GetClientManager().GetClientByUsername(CasadoCom);
                            TargetClient5.SendMessage(new RoomNotificationComposer("</3", "<b>" + Session.GetHabbo().Username + "</b> acabou com o casamento!\n", "", "", ""));
                        }
                        else
                        {
                            Session.SendWhisper("Você não é casado!", 34);
                        }
                    }
                }
            }
        }
    }
}
