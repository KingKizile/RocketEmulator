using Rocket.Communication.Packets.Outgoing;
//using Rocket.Database;
using Rocket.HabboHotel.Users;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Rocket.Database.Interfaces;

using System.Text;
using System.Threading.Tasks;

namespace Rocket.HabboHotel.Groups.Forums
{
    public class GroupForumThreadPost
    {
        public int Id;
        public int UserId;
        public int Timestamp;
        public string Message;

        public int DeleterId;
        public int DeletedLevel;

        public GroupForumThread ParentThread;
        public GroupForumThreadPost(GroupForumThread parent, int id, int userid, int timestamp, string message, int deletedlevel, int deleterid)
        {

            ParentThread = parent;
            Id = id;
            UserId = userid;
            Timestamp = timestamp;
            Message = message;

            DeleterId = deleterid;
            DeletedLevel = deletedlevel;

        }

        public Habbo GetDeleter()
        {
            return RocketEmulador.GetHabboById(DeleterId);
        }

        public Habbo GetAuthor()
        {
            return RocketEmulador.GetHabboById(UserId);
        }

        public void SerializeData(ServerPacket Packet)
        {

            var User = GetAuthor();
            var oculterData = GetDeleter();
            Packet.WriteInteger(Id);
            Packet.WriteInteger(ParentThread.Posts.IndexOf(this));

            Packet.WriteInteger(User.Id);
            Packet.WriteString(User.Username);
            Packet.WriteString(User.Look);

            Packet.WriteInteger((int)(RocketEmulador.GetUnixTimestamp() - Timestamp));
            Packet.WriteString(Message);
            Packet.WriteByte(DeletedLevel * 10);
            Packet.WriteInteger(oculterData != null ? oculterData.Id : 0);
            Packet.WriteString(oculterData != null ? oculterData.Username : "Unknown");
            Packet.WriteInteger(242342340);
            Packet.WriteInteger(ParentThread.GetUserPosts(User.Id).Count);
        }
        public void Log(int postID, int userID, string type)
        {
            using (IQueryAdapter adap = RocketEmulador.GetDatabaseManager().GetQueryReactor())
            {
                adap.SetQuery("INSERT INTO group_forums_logs (thread_id, user_id, type) VALUES (@a, @duid, @type)");
                adap.AddParameter("type", type);
                adap.AddParameter("a", postID);
                adap.AddParameter("duid", userID);
                adap.RunQuery();
            }
        }
        internal void Save()
        {
            using (IQueryAdapter adap = RocketEmulador.GetDatabaseManager().GetQueryReactor())
            {
                adap.SetQuery("UPDATE group_forums_threads_views SET deleted_level = @dl, deleter_user_id = @duid WHERE id = @id");
                adap.AddParameter("dl", DeletedLevel);
                adap.AddParameter("duid", DeleterId);
                adap.AddParameter("id", Id);
                adap.RunQuery();
            }
        }
    }
}