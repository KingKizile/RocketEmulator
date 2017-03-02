using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using Rocket.Communication.Packets.Outgoing.Messenger;
using Rocket.Communication.Packets.Outgoing.Rooms.Chat;
using Rocket.Communication.Packets.Outgoing.Rooms.Notifications;

namespace Rocket.Communication.Packets.Incoming.Messenger
{
    class SendMsgEvent : IPacketEvent
    {
        public void Parse(HabboHotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            if (Session == null || Session.GetHabbo() == null || Session.GetHabbo().GetMessenger() == null)
                return;

            int userId = Packet.PopInt();
            if (userId == 0 || userId == Session.GetHabbo().Id)
                return;

            string message = RocketEmulador.GetGame().GetChatManager().GetFilter().CheckMessage(Packet.PopString());
           
            if (string.IsNullOrWhiteSpace(message))
                return;

            if (Session.GetHabbo().TimeMuted > 0)
            {
                Session.SendNotification("Bem, agora você está mudo: Você não pode enviar mensagens.");
                return;
            }


           // else
            // if (userId == 1000000005)
           // {
            //    RocketEmulador.GetGame().GetClientManager().StaffAlert(new NewConsoleMessageComposer(1000000005, Session.GetHabbo().Username + ": " + message), Session.GetHabbo().Id);

             //   return;
           // }
            else
             if (userId == 1000000004)
            {
                RocketEmulador.GetGame().GetClientManager().StaffAlert(new NewConsoleMessageComposer(1000000004, Session.GetHabbo().Username + ": " + message), Session.GetHabbo().Id);

                return;
            }
            else
                if (userId == 1000000003)
            {
                RocketEmulador.GetGame().GetClientManager().StaffAlert(new NewConsoleMessageComposer(1000000003, Session.GetHabbo().Username + ": " + message), Session.GetHabbo().Id);

                return;
            }
                else 

            if (userId == 1000000002)
            {
                RocketEmulador.GetGame().GetClientManager().StaffAlert(new NewConsoleMessageComposer(1000000002, Session.GetHabbo().Username + ": " + message), Session.GetHabbo().Id);
               
                return;
            }
            else
            if (userId == 1000000001)
            {
                RocketEmulador.GetGame().GetClientManager().StaffAlert(new NewConsoleMessageComposer(1000000001, Session.GetHabbo().Username + ": " + message), Session.GetHabbo().Id);

                return;
            }
        
            Session.GetHabbo().GetMessenger().SendInstantMessage(userId, message);

        }
    }
}