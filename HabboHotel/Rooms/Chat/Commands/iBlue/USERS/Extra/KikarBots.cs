using System;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Collections.Generic;

using Rocket.HabboHotel.Rooms.AI;
using Rocket.HabboHotel.Rooms;
using Rocket.HabboHotel.GameClients;

using Rocket.HabboHotel.Users.Inventory.Bots;
using Rocket.Communication.Packets.Outgoing.Inventory.Bots;

using Rocket.Database.Interfaces;


namespace Rocket.HabboHotel.Rooms.Chat.Commands.iBlue.USERS.Extra
{
    class KikarBots : IChatCommand
    {
        public string PermissionRequired
        {
            get { return "comando_users"; }
        }

        public string Parameters
        {
            get { return ""; }
        }

        public string Description
        {
            get { return "Kick todos os bots do quarto."; }
        }

        public void Execute(GameClients.GameClient Session, Rooms.Room Room, string[] Params)
        {
            if (!Room.CheckRights(Session, true))
            {
                Session.SendNotification("Opa, somente o proprietário do quarto pode executar este comando!");
                return;
            }

            foreach (RoomUser User in Room.GetRoomUserManager().GetUserList().ToList())
            {
                if (User == null || User.IsPet || !User.IsBot)
                    continue;

                RoomUser BotUser = null;
                if (!Room.GetRoomUserManager().TryGetBot(User.BotData.Id, out BotUser))
                    return;

                using (IQueryAdapter dbClient = RocketEmulador.GetDatabaseManager().GetQueryReactor())
                {
                    dbClient.SetQuery("UPDATE `bots` SET `room_id` = '0' WHERE `id` = @id LIMIT 1");
                    dbClient.AddParameter("id", User.BotData.Id);
                    dbClient.RunQuery();
                }

                Session.GetHabbo().GetInventoryComponent().TryAddBot(new Bot(Convert.ToInt32(BotUser.BotData.Id), Convert.ToInt32(BotUser.BotData.ownerID), BotUser.BotData.Name, BotUser.BotData.Motto, BotUser.BotData.Look, BotUser.BotData.Gender));
                Session.SendMessage(new BotInventoryComposer(Session.GetHabbo().GetInventoryComponent().GetBots()));
                Room.GetRoomUserManager().RemoveBot(BotUser.VirtualId, false);
            }

            Session.SendNotification("Sucesso, removidos todos os bots.");
        }
    }
}
