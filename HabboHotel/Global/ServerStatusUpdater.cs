using System;
using System.Threading;
using log4net;
using Rocket.Database.Interfaces;


namespace Rocket.HabboHotel.Global
{
    public class ServerStatusUpdater : IDisposable
    {
        private static ILog log = LogManager.GetLogger("Mango.Global.ServerUpdater");

        private const int UPDATE_IN_SECS = 30;

        private Timer _timer;

        public ServerStatusUpdater()
        {
        }

        public void Init()
        {
            this._timer = new Timer(new TimerCallback(this.OnTick), null, TimeSpan.FromSeconds(UPDATE_IN_SECS), TimeSpan.FromSeconds(UPDATE_IN_SECS));

            Console.Title = "Rocket Emulador - 0 usuários online - 0 quartos online - 0 dia(s) 0 hora(s) ligado";

            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("       [FUNCIONANDO] => [Rocket Emu] =>  Status do emulador carregado com sucesso.");
        }

        public void OnTick(object Obj)
        {
            this.UpdateOnlineUsers();
        }

        private void UpdateOnlineUsers()
        {
            TimeSpan Uptime = DateTime.Now - RocketEmulador.ServerStarted;

            int UsersOnline = Convert.ToInt32(RocketEmulador.GetGame().GetClientManager().Count);
            int RoomCount = RocketEmulador.GetGame().GetRoomManager().Count;

            Console.Title = "Rocket Emulador - " + UsersOnline + " usuários online - " + RoomCount + " quartos online - " + Uptime.Days + " dia(s) " + Uptime.Hours + " hora(s) ligado";

            using (IQueryAdapter dbClient = RocketEmulador.GetDatabaseManager().GetQueryReactor())
            {
                dbClient.SetQuery("UPDATE `server_status` SET `users_online` = @users, `loaded_rooms` = @loadedRooms LIMIT 1;");
                dbClient.AddParameter("users", UsersOnline);
                dbClient.AddParameter("loadedRooms", RoomCount);
                dbClient.RunQuery();
            }

            int userPeak;
            using (IQueryAdapter dbClient = RocketEmulador.GetDatabaseManager().GetQueryReactor())
            {
                dbClient.SetQuery("SELECT `userpeak` FROM `server_status` LIMIT 1");
                userPeak = dbClient.getInteger();
            }
            if (UsersOnline > userPeak)
            {
                using (IQueryAdapter dbClient = RocketEmulador.GetDatabaseManager().GetQueryReactor())
                {
                    dbClient.SetQuery("UPDATE `server_status` SET `userpeak` = @userpeak LIMIT 1;");
                    dbClient.AddParameter("userpeak", UsersOnline);
                    dbClient.RunQuery();
                }
            }
        }


        public void Dispose()
        {
            using (IQueryAdapter dbClient = RocketEmulador.GetDatabaseManager().GetQueryReactor())
            {
                dbClient.RunQuery("UPDATE `server_status` SET `users_online` = '0', `loaded_rooms` = '0'");
            }

            this._timer.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}