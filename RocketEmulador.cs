using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using Rocket.Core;
using Rocket.HabboHotel;
using Rocket.HabboHotel.GameClients;
using Rocket.HabboHotel.Users;
using Rocket.HabboHotel.Users.UserDataManagement;
using Rocket.Net;
using Rocket.Utilities;
using log4net;
using System.Collections.Concurrent;
using Rocket.Communication.Packets.Outgoing.Moderation;
using Rocket.Communication.Encryption.Keys;
using Rocket.Communication.Encryption;
using Rocket.Database.Interfaces;
using Rocket.HabboHotel.Cache;
using Rocket.Database;
using System.Runtime.CompilerServices;
namespace Rocket
{
    public static class RocketEmulador
    {
        private static readonly ILog log = LogManager.GetLogger("Rocket.RocketEmulador");

        public const string PrettyVersion = "Rocket Emulador";
        public const string PrettyBuild = "2.0.0";

        private static ConfigurationData _configuration;
        private static ConfigurationData _configure;
        private static Encoding _defaultEncoding;
        private static ConnectionHandling _connectionManager;
        private static Game _game;
        private static DatabaseManager _manager;
        public static ConfigData ConfigData;
        public static MusSocket MusSystem;
        public static CultureInfo CultureInfo;

        public static bool Event = false;
        public static DateTime lastEvent;
        public static DateTime ServerStarted;

        private static readonly List<char> Allowedchars = new List<char>(new[]
            {
                'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l',
                'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x',
                'y', 'z', '1', '2', '3', '4', '5', '6', '7', '8', '9', '0', '-', '.'
            });

        private static ConcurrentDictionary<int, Habbo> _usersCached = new ConcurrentDictionary<int, Habbo>();

        public static string SWFRevision = "";
        public static int RankAMB;
        public static int RankPRO;
        public static int RankVIP;
        public static int RankLOC;
        public static int AmbassadorMinRank;
        public static int Oferta;
        public static int SpamUser;
        public static int SpamStaff;
        public static int Soccer1;
        public static int Soccer2;
        public static int Soccer3;
        public static int Soccer4;
        public static int Soccer5;
        public static int Soccer51;
        public static int Soccer6;
        public static int Soccer61;
        public static int Soccer7;
        public static int Soccer71;
        public static int Soccer8;
        public static int Soccer81;
        public static int Soccer9;
        public static int Soccer10;
        public static int Soccer11;
        public static int Soccer12;
        public static int Soccer13;
        public static int Soccer131;
        public static int Soccer14;
        public static int Soccer141;
        public static int Soccer15;
        public static int Soccer151;
        public static int Soccer16;
        public static int Soccer161;
        public static int Soccer17;
        public static int Soccer171;
       
        
        public static void Initialize()
        {

            ServerStarted = DateTime.Now;
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine("          ######                     #                   #     ");
            Console.WriteLine("          #     #                    #                   #     ");
            Console.WriteLine("          #     #   #####    #####   #   ##    #####   ######  ");
            Console.WriteLine("          ######   #     #  #        #  #     #     #    #   ");
            Console.WriteLine("          #   #    #     #  #        ###      #######    #     ");
            Console.WriteLine("          #    #   #     #  #        #  #     #          #     ");
            Console.WriteLine("          #     #   #####    #####   #   ##    #####      ###  ");

            Console.WriteLine("");
            Console.Title = " Carregando o Rocket Emulador, aguarde.";
            _defaultEncoding = Encoding.Default;

            Console.WriteLine("");
            Console.WriteLine("");

            CultureInfo = CultureInfo.CreateSpecificCulture("en-GB");


            try
            {

                _configuration = new ConfigurationData(Path.Combine(Application.StartupPath, @"rocket_system/configuração.ini"));
                _configure = new ConfigurationData(Path.Combine(Application.StartupPath, @"rocket_system/System.ini"));
                var connectionString = new MySqlConnectionStringBuilder
                {
                    ConnectionTimeout = 10,
                    Database = GetConfig().data["servidor.nomedb"],
                    DefaultCommandTimeout = 30,
                    Logging = false,
                    MaximumPoolSize = uint.Parse(GetConfig().data["servidor.pool.maxsize"]),
                    MinimumPoolSize = uint.Parse(GetConfig().data["servidor.pool.minsize"]),
                    Password = GetConfig().data["servidor.senha"],
                    Pooling = true,
                    Port = uint.Parse(GetConfig().data["servidor.porta"]),
                    Server = GetConfig().data["servidor.hostname"],
                    UserID = GetConfig().data["servidor.usuario"],

                    AllowZeroDateTime = true,
                    ConvertZeroDateTime = true,
                };





                _manager = new DatabaseManager(connectionString.ToString());



                if (!_manager.IsConnected())
                {
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine("       [FUNCIONANDO] => [Rocket Emu] => Falha ao tentar se conectar com a Database.");
                    Console.ReadKey(true);
                    Environment.Exit(1);
                    return;
                }

                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("       Atualmente licenciado para " + RocketData().data["licença.url"] + " ");
                Console.WriteLine("       " + RocketData().data["licença.hotelname"] + "     [ Compre uma licença, skype: vitornobre15 ]");
                Console.WriteLine("       Rocket Emulador [ Rocket Emu - Version 2.0 ]");
                Console.WriteLine("                 ");
                Console.WriteLine("       ────────────────────────────");
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("       [FUNCIONANDO] => [Rocket Emu] =>  Conectado na Database");

                //Reset our statistics first.
                using (IQueryAdapter dbClient = GetDatabaseManager().GetQueryReactor())
                {
                    dbClient.RunQuery("TRUNCATE `catalog_marketplace_data`");
                    dbClient.RunQuery("UPDATE `rooms` SET `users_now` = '0' WHERE `users_now` > '0';");
                    dbClient.RunQuery("UPDATE `users` SET `online` = '0' WHERE `online` = '1'");
                    dbClient.RunQuery("UPDATE `server_status` SET `users_online` = '0', `loaded_rooms` = '0'");
                }

                //Get the configuration & Game set.
                ConfigData = new ConfigData();
                _game = new Game();

                //Have our encryption ready.
                HabboEncryptionV2.Initialize(new RSAKeys());

                //Make sure MUS is working.
                MusSystem = new MusSocket(GetConfig().data["mus.tcp.bindip"], int.Parse(GetConfig().data["mus.tcp.port"]), GetConfig().data["mus.tcp.allowedaddr"].Split(Convert.ToChar(";")), 0);

                //Accept connections.
                _connectionManager = new ConnectionHandling(int.Parse(GetConfig().data["game.tcp.port"]), int.Parse(GetConfig().data["game.tcp.conlimit"]), int.Parse(GetConfig().data["game.tcp.conperip"]), GetConfig().data["game.tcp.enablenagles"].ToLower() == "true");
                _connectionManager.init();

                _game.StartGameLoop();
                TimeSpan TimeUsed = DateTime.Now - ServerStarted;
                AmbassadorMinRank = int.Parse(RocketData().data["embaixador.rank"]);
                RankAMB = int.Parse(RocketData().data["rocket_amb"]);
                RankLOC = int.Parse(RocketData().data["rocket_loc"]);
                RankPRO = int.Parse(RocketData().data["rocket_pro"]);
                RankVIP = int.Parse(RocketData().data["rocket_vip"]);
                Oferta = int.Parse(RocketData().data["oferta.tempo"]);
                SpamUser = int.Parse(RocketData().data["spam.user"]);
                SpamStaff = int.Parse(RocketData().data["spam.staff"]);
                Soccer1 = int.Parse(RocketData().data["soccer.1"]);
                Soccer2 = int.Parse(RocketData().data["soccer.2"]);
                Soccer3 = int.Parse(RocketData().data["soccer.3"]);
                Soccer4 = int.Parse(RocketData().data["soccer.4"]);
                Soccer5 = int.Parse(RocketData().data["soccer.5"]);
                Soccer51 = int.Parse(RocketData().data["soccer.6"]);
                Soccer6 = int.Parse(RocketData().data["soccer.6"]);
                Soccer61 = int.Parse(RocketData().data["soccer.61"]);
                Soccer7 = int.Parse(RocketData().data["soccer.7"]);
                Soccer71 = int.Parse(RocketData().data["soccer.71"]);
                Soccer8 = int.Parse(RocketData().data["soccer.8"]);
                Soccer81 = int.Parse(RocketData().data["soccer.81"]);
                Soccer9 = int.Parse(RocketData().data["soccer.9"]);
                Soccer10 = int.Parse(RocketData().data["soccer.10"]);
                Soccer11 = int.Parse(RocketData().data["soccer.11"]);
                Soccer12 = int.Parse(RocketData().data["soccer.12"]);
                Soccer13 = int.Parse(RocketData().data["soccer.13"]);
                Soccer131 = int.Parse(RocketData().data["soccer.131"]);
                Soccer14 = int.Parse(RocketData().data["soccer.14"]);
                Soccer141 = int.Parse(RocketData().data["soccer.141"]);
                Soccer15 = int.Parse(RocketData().data["soccer.15"]);
                Soccer151 = int.Parse(RocketData().data["soccer.151"]);
                Soccer16 = int.Parse(RocketData().data["soccer.16"]);
                Soccer161 = int.Parse(RocketData().data["soccer.161"]);
                Soccer17 = int.Parse(RocketData().data["soccer.17"]);
                Soccer171 = int.Parse(RocketData().data["soccer.171"]);







                {

                    Console.WriteLine();
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine("       [FUNCIONANDO] => [Rocket Emu] =>  Dados da Licença:  Rocket Emulador");
                    Console.WriteLine("                 ");
                    Console.WriteLine("       [FUNCIONANDO] => [Rocket Emu] =>  Dono da Licença : " + RocketData().data["licença.usuario"] + "");
                    Console.WriteLine("       [FUNCIONANDO] => [Rocket Emu] =>  Cod da Licença  : " + RocketData().data["licença.codigo"] + "");
                    Console.WriteLine("       [FUNCIONANDO] => [Rocket Emu] =>  IP Licenciado   : " + RocketData().data["licença.ip"] + "");
                    Console.WriteLine("       [FUNCIONANDO] => [Rocket Emu] =>  Licenciado de   : " + RocketData().data["licença.idata"] + "");
                    Console.WriteLine("       [FUNCIONANDO] => [Rocket Emu] =>  Licenciado até  : " + RocketData().data["licença.fdata"] + "");
                    Console.WriteLine("       [FUNCIONANDO] => [Rocket Emu] =>  Licença chegada com sucesso.");
                    Console.WriteLine("                 ");
                    Console.WriteLine("       [FUNCIONANDO] => [Rocket Emu] =>  O servidor está pronto para uso !");

                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("       ────────────────────────────");

                   
                




                }
            }
            catch (KeyNotFoundException e)
            {
                Logging.WriteLine("Verifique as configurações, Rocket emulador será desligado..", ConsoleColor.Red);
                Logging.WriteLine("Pressione qualquer tecla para ligar ...");
                Logging.WriteLine(e.ToString());
                Console.ReadKey(true);
                Environment.Exit(1);
                return;
            }
            catch (InvalidOperationException e)
            {
                Logging.WriteLine("Falha ao ligar a Rocket emulador: " + e.Message, ConsoleColor.Red);
                Logging.WriteLine("Pressione qualquer tecla para desligar ...");
                Console.ReadKey(true);
                Environment.Exit(1);
                return;
            }
            catch (Exception e)
            {
                Logging.WriteLine("Erro fatal ao iniciar.: " + e, ConsoleColor.Red);
                Logging.WriteLine("Pressione qualquer tecla para desligar");

                Console.ReadKey();
                Environment.Exit(1);

            }
        }

        public static bool EnumToBool(string Enum)
        {
            return (Enum == "1");
        }

        public static string BoolToEnum(bool Bool)
        {
            return (Bool == true ? "1" : "0");
        }

        public static int GetRandomNumber(int Min, int Max)
        {
            return RandomNumber.GenerateNewRandom(Min, Max);
        }

        public static double GetUnixTimestamp()
        {
            TimeSpan ts = (DateTime.UtcNow - new DateTime(2017, 1, 1, 0, 0, 0));
            return ts.TotalSeconds;
        }

        public static long Now()
        {
            TimeSpan ts = (DateTime.UtcNow - new DateTime(2017, 1, 1, 0, 0, 0));
            double unixTime = ts.TotalMilliseconds;
            return (long)unixTime;
        }

        public static string FilterFigure(string figure)
        {
            foreach (char character in figure)
            {
                if (!isValid(character))
                    return "sh-3338-93.ea-1406-62.hr-831-49.ha-3331-92.hd-180-7.ch-3334-93-1408.lg-3337-92.ca-1813-62";
            }

            return figure;
        }

        private static bool isValid(char character)
        {
            return Allowedchars.Contains(character);
        }

        public static bool IsValidAlphaNumeric(string inputStr)
        {
            inputStr = inputStr.ToLower();
            if (string.IsNullOrEmpty(inputStr))
            {
                return false;
            }

            for (int i = 0; i < inputStr.Length; i++)
            {
                if (!isValid(inputStr[i]))
                {
                    return false;
                }
            }

            return true;
        }

        public static string GetUsernameById(int UserId)
        {
            string Name = "Unknown User";

            GameClient Client = GetGame().GetClientManager().GetClientByUserID(UserId);
            if (Client != null && Client.GetHabbo() != null)
                return Client.GetHabbo().Username;

            UserCache User = RocketEmulador.GetGame().GetCacheManager().GenerateUser(UserId);
            if (User != null)
                return User.Username;

            using (IQueryAdapter dbClient = RocketEmulador.GetDatabaseManager().GetQueryReactor())
            {
                dbClient.SetQuery("SELECT `username` FROM `users` WHERE id = @id LIMIT 1");
                dbClient.AddParameter("id", UserId);
                Name = dbClient.getString();
            }

            if (string.IsNullOrEmpty(Name))
                Name = "Unknown User";

            return Name;
        }

        public static Habbo GetHabboById(int UserId)
        {
            try
            {
                GameClient Client = GetGame().GetClientManager().GetClientByUserID(UserId);
                if (Client != null)
                {
                    Habbo User = Client.GetHabbo();
                    if (User != null && User.Id > 0)
                    {
                        if (_usersCached.ContainsKey(UserId))
                            _usersCached.TryRemove(UserId, out User);
                        return User;
                    }
                }
                else
                {
                    try
                    {
                        if (_usersCached.ContainsKey(UserId))
                            return _usersCached[UserId];
                        else
                        {
                            UserData data = UserDataFactory.GetUserData(UserId);
                            if (data != null)
                            {
                                Habbo Generated = data.user;
                                if (Generated != null)
                                {
                                    Generated.InitInformation(data);
                                    _usersCached.TryAdd(UserId, Generated);
                                    return Generated;
                                }
                            }
                        }
                    }
                    catch { return null; }
                }
                return null;
            }
            catch
            {
                return null;
            }
        }

        public static Habbo GetHabboByUsername(String UserName)
        {
            try
            {
                using (IQueryAdapter dbClient = GetDatabaseManager().GetQueryReactor())
                {
                    dbClient.SetQuery("SELECT `id` FROM `users` WHERE `username` = @user LIMIT 1");
                    dbClient.AddParameter("user", UserName);
                    int id = dbClient.getInteger();
                    if (id > 0)
                        return GetHabboById(Convert.ToInt32(id));
                }
                return null;
            }
            catch { return null; }
        }



        public static void PerformShutDown()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("       [FUNCIONANDO] => [Rocket Emu] =>  Aguarde, vamos desligar o servidor.");

            Console.Title = "Rocket Emulador - Desligado!";

            RocketEmulador.GetGame().GetClientManager().SendMessage(new BroadcastMessageAlertComposer(RocketEmulador.GetGame().GetLanguageLocale().TryGetValue("shutdown_alert")));
            GetGame().StopGameLoop();
            Thread.Sleep(2500);
            GetConnectionManager().Destroy();//Stop listening.
            GetGame().GetPacketManager().UnregisterAll();//Unregister the packets.
            GetGame().GetPacketManager().WaitForAllToComplete();
            GetGame().GetClientManager().CloseAll();//Close all connections
            GetGame().GetRoomManager().Dispose();//Stop the game loop.

            using (IQueryAdapter dbClient = _manager.GetQueryReactor())
            {
                dbClient.RunQuery("TRUNCATE `catalog_marketplace_data`");
                dbClient.RunQuery("UPDATE `users` SET online = '0', `auth_ticket` = NULL");
                dbClient.RunQuery("UPDATE `rooms` SET `users_now` = '0' WHERE `users_now` > '0'");
                dbClient.RunQuery("UPDATE `server_status` SET `users_online` = '0', `loaded_rooms` = '0', `userpeak` = 0");
            }
            
                Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("       [FUNCIONANDO] => [Rocket Emu] =>  O servidor foi desligado com sucesso.");

            Thread.Sleep(1000);
            Environment.Exit(0);
        }

        public static ConfigurationData GetConfig()
        {
            return _configuration;
        }

        public static ConfigurationData RocketData()
        {
            return _configure;
        }

        public static ConfigData GetDBConfig()
        {
            return ConfigData;
        }

        public static Encoding GetDefaultEncoding()
        {
            return _defaultEncoding;
        }

        public static ConnectionHandling GetConnectionManager()
        {
            return _connectionManager;
        }

        public static Game GetGame()
        {
            return _game;
        }

        public static DatabaseManager GetDatabaseManager()
        {
            return _manager;
        }

        public static ICollection<Habbo> GetUsersCached()
        {
            return _usersCached.Values;
        }

        public static bool RemoveFromCache(int Id, out Habbo Data)
        {
            return _usersCached.TryRemove(Id, out Data);
        }
    }
}