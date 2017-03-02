using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;

using Rocket.HabboHotel.Rooms;
using Rocket.HabboHotel.Groups;
using Rocket.HabboHotel.Items.Wired;

using Rocket.Communication.Packets.Outgoing.Rooms.Engine;
using Rocket.Communication.Packets.Outgoing.Rooms.Chat;
using Rocket.Communication.Packets.Outgoing.Users;
using Rocket.Communication.Packets.Outgoing.Navigator;

//using Rocket.HabboHotel.Rooms;

using Rocket.Database.Interfaces;
using Rocket.Communication.Packets.Outgoing.Inventory.Purse;
using System.Data;
using Rocket.HabboHotel.Rooms.AI.Speech;
using Rocket.HabboHotel.Rooms.AI;

namespace Rocket.Communication.Packets.Incoming.Rooms.Engine
{
    class GetRoomEntryDataEvent : IPacketEvent
    {
        public void Parse(HabboHotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            if (Session == null || Session.GetHabbo() == null)
                return;

            Room Room = Session.GetHabbo().CurrentRoom;
            if (Room == null)
                return;

            if (Session.GetHabbo().InRoom)
            {
                Room OldRoom;

                if (!RocketEmulador.GetGame().GetRoomManager().TryGetRoom(Session.GetHabbo().CurrentRoomId, out OldRoom))
                    return;

                if (OldRoom.GetRoomUserManager() != null)
                    OldRoom.GetRoomUserManager().RemoveUserFromRoom(Session, false, false);
            }

            if (!Room.GetRoomUserManager().AddAvatarToRoom(Session))
            {
                Room.GetRoomUserManager().RemoveUserFromRoom(Session, false, false);
                return;//TODO: Remove?
            }

            Room.SendObjects(Session);

            //Status updating for messenger, do later as buggy.

            try
            {
                if (Session.GetHabbo().GetMessenger() != null)
                    Session.GetHabbo().GetMessenger().OnStatusChanged(true);
            }
            catch { }

            if (Session.GetHabbo().GetStats().QuestID > 0)
                RocketEmulador.GetGame().GetQuestManager().QuestReminder(Session, Session.GetHabbo().GetStats().QuestID);

            Session.SendMessage(new RoomEntryInfoComposer(Room.RoomId, Room.CheckRights(Session, true)));
            Session.SendMessage(new RoomVisualizationSettingsComposer(Room.WallThickness, Room.FloorThickness, RocketEmulador.EnumToBool(Room.Hidewall.ToString())));

            RoomUser ThisUser = Room.GetRoomUserManager().GetRoomUserByHabbo(Session.GetHabbo().Username);

            if (ThisUser != null && Session.GetHabbo().PetId == 0)
                Room.SendMessage(new UserChangeComposer(ThisUser, false));

            Session.SendMessage(new RoomEventComposer(Room.RoomData, Room.RoomData.Promotion));

            if (Room.GetWired() != null)
                Room.GetWired().TriggerEvent(WiredBoxType.TriggerRoomEnter, Session.GetHabbo());

            if (RocketEmulador.GetUnixTimestamp() < Session.GetHabbo().FloodTime && Session.GetHabbo().FloodTime != 0)
                Session.SendMessage(new FloodControlComposer((int)Session.GetHabbo().FloodTime - (int)RocketEmulador.GetUnixTimestamp()));

            if (Room.OwnerId == Session.GetHabbo().Id)
            {
                DataRow dFrank = null;
                using (var dbClient = RocketEmulador.GetDatabaseManager().GetQueryReactor())
                {
                    dbClient.SetQuery("SELECT bot_frank FROM users WHERE id = '" + Session.GetHabbo().Id + "'");
                    dFrank = dbClient.getRow();
                }

                if (Convert.ToBoolean(dFrank["bot_frank"]) == false)
                {
                    using (var dbClient = RocketEmulador.GetDatabaseManager().GetQueryReactor())
                    {
                        dbClient.RunQuery("UPDATE users SET bot_frank = 'true' WHERE id = " + Session.GetHabbo().Id + ";");
                        dbClient.RunQuery("UPDATE users SET bot_user = 'false' WHERE id = " + Session.GetHabbo().Id + ";");
                    }
                    string Chat1 = RocketEmulador.GetDBConfig().DBData["frank.chat.1"];
                    string Chat2 = RocketEmulador.GetDBConfig().DBData["frank.chat.2"];
                    string Chat3 = RocketEmulador.GetDBConfig().DBData["frank.chat.3"];
                    string Chat4 = RocketEmulador.GetDBConfig().DBData["frank.chat.4"];
                    string Chat5 = RocketEmulador.GetDBConfig().DBData["frank.chat.5"];
                    string Credits = RocketEmulador.GetDBConfig().DBData["frank.give.credits"];
                    string Diamonds = RocketEmulador.GetDBConfig().DBData["frank.give.diamonds"];
                    string Duckets = RocketEmulador.GetDBConfig().DBData["frank.give.duckets"];
                    string Gotws = RocketEmulador.GetDBConfig().DBData["frank.give.gotws"];
                    string Furni = RocketEmulador.GetDBConfig().DBData["frank.give.furni"];

                    List<RandomSpeech> BotSpeechList = new List<RandomSpeech>();
                    Console.WriteLine("Só se mostra bot e nuxs 1 vez por usuario.");
                    int X = 0;
                    int Y = 0;
                    string hola = "false";
                    RoomUser BotUser = Room.GetRoomUserManager().DeployBot(new RoomBot(0, Session.GetHabbo().CurrentRoomId, "generic", "freeroam", "Frank", "Gerente do hotel", "hr-3194-38-36.hd-180-1.ch-220-1408.lg-285-73.sh-906-90.ha-3129-73.fa-1206-73.cc-3039-73", X, Y, 0, 4, 0, 0, 0, 0, ref BotSpeechList, "", 0, 0, false, 0, Convert.ToBoolean(hola), 1), null);
                    System.Threading.Thread.Sleep(4000); // Mensagem de Boas vindas.
                    BotUser.Chat(Chat1, false, 0);
                    System.Threading.Thread.Sleep(3000); // Segunda mensagem
                    BotUser.Chat(Chat2, false, 0);
                    System.Threading.Thread.Sleep(2000); // Terceira mensagem e entrega das moedas
                    if (!string.IsNullOrWhiteSpace(Credits) && !string.IsNullOrWhiteSpace(Diamonds) && !string.IsNullOrWhiteSpace(Duckets) && !string.IsNullOrWhiteSpace(Gotws))
                    {
                        BotUser.Chat("Você tem " + Credits + " créditos, " + Diamonds + " diamantes, " + Duckets + " duckets, " + Gotws + " estrelas e nubes!", false, 0);
                        Session.GetHabbo().Credits += Convert.ToInt32(RocketEmulador.GetDBConfig().DBData["frank.give.credits"]);
                        Session.GetHabbo().Diamonds += Convert.ToInt32(RocketEmulador.GetDBConfig().DBData["frank.give.diamonds"]);
                        Session.GetHabbo().Duckets += Convert.ToInt32(RocketEmulador.GetDBConfig().DBData["frank.give.duckets"]);
                        Session.GetHabbo().GOTWPoints += Convert.ToInt32(RocketEmulador.GetDBConfig().DBData["frank.give.gotws"]);
                        Session.SendMessage(new CreditBalanceComposer(Session.GetHabbo().Credits));

                    }
                    else if (!string.IsNullOrWhiteSpace(Credits) && !string.IsNullOrWhiteSpace(Diamonds) && !string.IsNullOrWhiteSpace(Duckets) && !string.IsNullOrWhiteSpace(Gotws))
                    {
                        BotUser.Chat("Você tem " + Credits + " créditos, " + Diamonds + " diamantes, " + Duckets + " duckets e " + Gotws + " estrelas!", false, 0);
                        Session.GetHabbo().Credits += Convert.ToInt32(RocketEmulador.GetDBConfig().DBData["frank.give.credits"]);
                        Session.GetHabbo().Diamonds += Convert.ToInt32(RocketEmulador.GetDBConfig().DBData["frank.give.diamonds"]);
                        Session.GetHabbo().Duckets += Convert.ToInt32(RocketEmulador.GetDBConfig().DBData["frank.give.duckets"]);
                        Session.GetHabbo().GOTWPoints += Convert.ToInt32(RocketEmulador.GetDBConfig().DBData["frank.give.gotws"]);
                        Session.SendMessage(new CreditBalanceComposer(Session.GetHabbo().Credits));
                        Session.SendMessage(new ActivityPointsComposer(Session.GetHabbo().Duckets, Session.GetHabbo().Diamonds, Session.GetHabbo().GOTWPoints));
                    }
                    else if (!string.IsNullOrWhiteSpace(Credits) && !string.IsNullOrWhiteSpace(Diamonds) && !string.IsNullOrWhiteSpace(Duckets))
                    {
                        BotUser.Chat("Você tem " + Credits + " créditos, " + Diamonds + " diamantes e " + Duckets + " duckets!", false, 0);
                        Session.GetHabbo().Credits += Convert.ToInt32(RocketEmulador.GetDBConfig().DBData["frank.give.credits"]);
                        Session.GetHabbo().Diamonds += Convert.ToInt32(RocketEmulador.GetDBConfig().DBData["frank.give.diamonds"]);
                        Session.GetHabbo().Duckets += Convert.ToInt32(RocketEmulador.GetDBConfig().DBData["frank.give.duckets"]);
                        Session.SendMessage(new CreditBalanceComposer(Session.GetHabbo().Credits));
                        Session.SendMessage(new ActivityPointsComposer(Session.GetHabbo().Duckets, Session.GetHabbo().Diamonds, Session.GetHabbo().GOTWPoints));
                    }
                    else if (!string.IsNullOrWhiteSpace(Credits) && !string.IsNullOrWhiteSpace(Diamonds))
                    {
                        BotUser.Chat("Você tem " + Credits + " créditos e " + Diamonds + " diamantes!", false, 0);
                        Session.GetHabbo().Credits += Convert.ToInt32(RocketEmulador.GetDBConfig().DBData["frank.give.credits"]);
                        Session.GetHabbo().Diamonds += Convert.ToInt32(RocketEmulador.GetDBConfig().DBData["frank.give.diamonds"]);
                        Session.SendMessage(new CreditBalanceComposer(Session.GetHabbo().Credits));
                        Session.SendMessage(new ActivityPointsComposer(Session.GetHabbo().Duckets, Session.GetHabbo().Diamonds, Session.GetHabbo().GOTWPoints));
                    }
                    else if (!string.IsNullOrWhiteSpace(Credits))
                    {
                        BotUser.Chat("Você tem " + Credits + " créditos!", false, 0);
                        Session.GetHabbo().Credits += Convert.ToInt32(RocketEmulador.GetDBConfig().DBData["frank.give.credits"]);
                        Session.SendMessage(new CreditBalanceComposer(Session.GetHabbo().Credits));
                    }
                    else
                    {
                        BotUser.Chat("Não iremos te dar nada por enquanto.", false, 0);
                    }
                    if (!string.IsNullOrWhiteSpace(Chat4) || !string.IsNullOrEmpty(Furni))
                    {
                        System.Threading.Thread.Sleep(4000); // Quarta mensagen, entregando o raro.
                        BotUser.Chat(Chat4, false, 0);
                        DataRow dFurni = null;
                        using (var dbClient = RocketEmulador.GetDatabaseManager().GetQueryReactor())
                        {
                            dbClient.SetQuery("SELECT public_name FROM furniture WHERE id = '" + Convert.ToInt32(Furni) + "'");
                            dFurni = dbClient.getRow();
                        }
                        Session.GetHabbo().GetInventoryComponent().AddNewItem(0, Convert.ToInt32(Furni), Convert.ToString(dFurni["public_name"]), 1, true, false, 0, 0);
                        Session.GetHabbo().GetInventoryComponent().UpdateItems(false);
                    }
                    System.Threading.Thread.Sleep(5000); // Quinta mensagem.
                    BotUser.Chat(Chat5, false, 0);
                    Room.GetGameMap().RemoveUserFromMap(BotUser, new System.Drawing.Point(0, 0));
                    Room.GetRoomUserManager().RemoveBot(BotUser.VirtualId, false);
                }
                else
                {
                    Console.WriteLine("O Bot não irá mais aparecer para esse usuário.");
                }
            }

        }

    }
}