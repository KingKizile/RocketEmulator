using Rocket.Communication.Packets.Outgoing.Notifications;
using Rocket.Database.Interfaces;
using System.Data;
using System.Text;
namespace Rocket.HabboHotel.Rooms.Chat.Commands.iBlue.MOD
{
    class VerFakes : IChatCommand
    {
        public string PermissionRequired
        {
            get { return "comando_mod"; }
        }

        public string Parameters
        {
            get { return "%user%"; }
        }

        public string Description
        {
            get { return "Ver clones."; }
        }

        public void Execute(GameClients.GameClient session, Rooms.Room Room, string[] Params)
        {
            if (Params.Length == 1)
            {
                session.SendWhisper("Porfavor ingrese el nombre del usuario a revisar.");
                return;
            }

            string str2;
            IQueryAdapter adapter;
            string username = Params[1];
            DataTable table = null;
            StringBuilder builder = new StringBuilder();
            if (RocketEmulador.GetGame().GetClientManager().GetClientByUsername(username) != null)
            {
                str2 = RocketEmulador.GetGame().GetClientManager().GetClientByUsername(username).GetConnection().getIp();
                builder.AppendLine("Usuário :  " + username + " - Ip : " + str2);
                using (adapter = RocketEmulador.GetDatabaseManager().GetQueryReactor())
                {
                    adapter.SetQuery("SELECT id,username FROM users WHERE ip_last = @ip OR ip_reg = @ip");
                    adapter.AddParameter("ip", str2);
                    table = adapter.getTable();
                    builder.AppendLine("Clones encontrados: " + table.Rows.Count);
                    foreach (DataRow row in table.Rows)
                    {
                        builder.AppendLine(string.Concat(new object[] { "Id : ", row["id"], " - Username: ", row["username"] }));
                    }
                }
                session.SendMessage(new MOTDNotificationComposer(builder.ToString()));
            }
            else
            {
                using (adapter = RocketEmulador.GetDatabaseManager().GetQueryReactor())
                {
                    adapter.SetQuery("SELECT ip_last FROM users WHERE username = @username");
                    adapter.AddParameter("username", username);
                    str2 = adapter.getString();
                    builder.AppendLine("Username :  " + username + " - Ip : " + str2);
                    adapter.SetQuery("SELECT id,username FROM users WHERE ip_last = @ip OR ip_reg = @ip");
                    adapter.AddParameter("ip", str2);
                    table = adapter.getTable();
                    builder.AppendLine("Clones encontrados: " + table.Rows.Count);
                    foreach (DataRow row in table.Rows)
                    {
                        builder.AppendLine(string.Concat(new object[] { "Id : ", row["id"], " - Username: ", row["username"] }));
                    }
                }

                session.SendMessage(new MOTDNotificationComposer(builder.ToString()));
            }
            return;
        }
    }
}