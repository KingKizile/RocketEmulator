/*using Rocket.Communication.Packets.Outgoing;
using Rocket.Communication.Packets.Outgoing.Nux;
using Rocket.Database.Interfaces;
using Rocket.HabboHotel.GameClients;
using Rocket.HabboHotel.Users;
using System;

namespace Rocket.Communication.Packets.Incoming.Nucs
{
    internal class RoomNucsAlerta : IPacketEvent
    {
        public void Parse(GameClient Session, ClientPacket Packet)
        {
            Habbo habbo = Session.GetHabbo();
            bool flag = !habbo.PassedNuxNavigator && !habbo.PassedNuxCatalog && !habbo.PassedNuxChat && !habbo.PassedNuxDuckets && !habbo.PassedNuxItems;
            if (flag)
            {
                Session.SendMessage(new NuxAlertComposer("helpBubble/add/BOTTOM_BAR_NAVIGATOR/Esse é o nosso navegador vai poder entrar nos quartos e fazer amizades."));
                habbo.PassedNuxNavigator = true;
            }
            bool flag2 = habbo.PassedNuxNavigator && !habbo.PassedNuxCatalog && !habbo.PassedNuxChat && !habbo.PassedNuxDuckets && !habbo.PassedNuxItems;
            if (flag2)
            {
                Session.SendMessage(new NuxAlertComposer("helpBubble/add/BOTTOM_BAR_CATALOG/Aqui você encontra todos mobis !"));
                habbo.PassedNuxCatalog = true;
            }
            else
            {
                bool flag3 = habbo.PassedNuxNavigator && habbo.PassedNuxCatalog && !habbo.PassedNuxChat && !habbo.PassedNuxDuckets && !habbo.PassedNuxItems;
                if (flag3)
                {
                    Session.SendMessage(new NuxAlertComposer("helpBubble/add/CHAT_INPUT/Aqui é seu Chat para falar com os usuários."));
                    habbo.PassedNuxChat = true;
                }
                else
                {
                    bool flag4 = habbo.PassedNuxNavigator && habbo.PassedNuxCatalog && habbo.PassedNuxChat && !habbo.PassedNuxDuckets && !habbo.PassedNuxItems;
                    if (flag4)
                    {
                        Session.SendMessage(new NuxAlertComposer("helpBubble/add/DUCKETS_BUTTON/Essa parte você vai saber quantos diamantes tem ou moedas ."));
                        habbo.PassedNuxDuckets = true;
                    }
                    else
                    {
                        bool flag5 = habbo.PassedNuxNavigator && habbo.PassedNuxCatalog && habbo.PassedNuxChat && habbo.PassedNuxDuckets && !habbo.PassedNuxItems;
                        if (flag5)
                        {
                            Session.SendMessage(new NuxAlertComposer("helpBubble/add/BOTTOM_BAR_INVENTORY/Esse é o inventário aonde seus mobis ficam !."));
                            habbo.PassedNuxItems = true;
                        }
                    }
                }
            }
            bool flag6 = habbo.PassedNuxNavigator && habbo.PassedNuxCatalog && habbo.PassedNuxChat && habbo.PassedNuxDuckets && habbo.PassedNuxItems;
            if (flag6)
            {
                Session.SendMessage(new NuxAlertComposer("nux/lobbyoffer/show"));
                habbo._NUX = false;
                using (IQueryAdapter queryReactor = RocketEmulador.GetDatabaseManager().GetQueryReactor())
                {
                    queryReactor.RunQuery("UPDATE users SET nux_user = 'false' WHERE id = " + Session.GetHabbo().Id + ";");
                }
                ServerPacket serverPacket = new ServerPacket(2344);
                serverPacket.WriteInteger(0);
                Session.SendMessage(serverPacket);
            }
        }
    }
}*/
