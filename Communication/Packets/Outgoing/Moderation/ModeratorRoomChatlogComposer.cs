using System;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections.Generic;
using Rocket.Database.Interfaces;
using Rocket.HabboHotel.Rooms;
using Rocket.HabboHotel.Users;

using Rocket.Utilities;
using Rocket.HabboHotel.Cache;

namespace Rocket.Communication.Packets.Outgoing.Moderation
{
    class ModeratorRoomChatlogComposer : ServerPacket
    {
        public ModeratorRoomChatlogComposer(Room Room)
            : base(ServerPacketHeader.ModeratorRoomChatlogMessageComposer)
        {
            base.WriteByte(1);
            base.WriteShort(2);//Count
            base.WriteString("roomName");
            base.WriteByte(2);
            base.WriteString(Room.Name);
            base.WriteString("roomId");
            base.WriteByte(1);
            base.WriteInteger(Room.Id);

            DataTable Table = null;
            using (IQueryAdapter dbClient = RocketEmulador.GetDatabaseManager().GetQueryReactor())
            {
                dbClient.SetQuery("SELECT id,user_id,room_id, message,DATE_FORMAT(from_unixtime(timestamp),'%k:%i') FROM `chatlogs` WHERE `room_id` = @rid ORDER BY `id` DESC LIMIT 250");
                dbClient.AddParameter("rid", Room.Id);
                Table = dbClient.getTable();
            }

            base.WriteShort(Table.Rows.Count);
            if (Table != null)
            {
                foreach (DataRow Row in Table.Rows)
                {
                    UserCache Habbo = RocketEmulador.GetGame().GetCacheManager().GenerateUser(Convert.ToInt32(Row["user_id"]));
                    //FALTA ARREGLAR QUE SE VEA LA HORA
                    if (Habbo == null)
                    {
                        //base.WriteInteger(((int)RocketEmulador.GetUnixTimestamp() - Convert.ToInt32(Row["timestamp"])) * 1000);
                        //DateTime time = new DateTime(Convert.ToInt32(Row["timestamp"]));
                        //base.WriteString(time.ToString("hh:mm"));
                        //int time = ((int)RocketEmulador.GetUnixTimestamp() - Convert.ToInt32(Row["timestamp"])) * 1000;
                        //string time2 = time.ToString();
                        base.WriteString(Convert.ToString(Row[4]));
                        base.WriteInteger(-1);
                        base.WriteString("Unknown User");
                        base.WriteString(string.IsNullOrWhiteSpace(Convert.ToString(Row["message"])) ? "*user sent a blank message*" : Convert.ToString(Row["message"]));
                        base.WriteBoolean(false);
                    }
                    else
                    {
                        //base.WriteInteger(((int)RocketEmulador.GetUnixTimestamp() - Convert.ToInt32(Row["timestamp"])) * 1000);
                        //DateTime time = new DateTime(Convert.ToInt32(Row["timestamp"]));
                        //base.WriteString(time.ToString("hh:mm"));
                        //int time = ((int)RocketEmulador.GetUnixTimestamp() - Convert.ToInt32(Row["timestamp"])) * 1000;
                        //string time2 = time.ToString();
                        base.WriteString(Convert.ToString(Row[4]));
                        base.WriteInteger(Habbo.Id);
                        base.WriteString(Habbo.Username);
                        base.WriteString(string.IsNullOrWhiteSpace(Convert.ToString(Row["message"])) ? "*user sent a blank message*" : Convert.ToString(Row["message"]));
                        base.WriteBoolean(false);
                    }
                }
            }
        }
    }
}
