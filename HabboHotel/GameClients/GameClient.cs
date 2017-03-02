using System;
using Rocket.Net;
using Rocket.Core;
using Rocket.Communication.Packets.Incoming;
using Rocket.HabboHotel.Rooms;
using Rocket.HabboHotel.Users;
using Rocket.Communication.Interfaces;
using Rocket.HabboHotel.Users.UserDataManagement;
using ConnectionManager;
using Rocket.Communication.Packets.Outgoing.Sound;
using Rocket.Communication.Packets.Outgoing.Rooms.Chat;
using Rocket.Communication.Packets.Outgoing.Handshake;
using Rocket.Communication.Packets.Outgoing.Navigator;
using Rocket.Communication.Packets.Outgoing.Moderation;
using Rocket.Communication.Packets.Outgoing.Inventory.AvatarEffects;
using Rocket.Communication.Packets.Outgoing.Inventory.Achievements;
using Rocket.Communication.Encryption.Crypto.Prng;
using Rocket.HabboHotel.Users.Messenger.FriendBar;
using Rocket.Communication.Packets.Outgoing.BuildersClub;
using Rocket.HabboHotel.Moderation;
using Rocket.Database.Interfaces;
using Rocket.HabboHotel.Subscriptions;
using Rocket.HabboHotel.Permissions;
using Rocket.Communication.Packets.Outgoing.Notifications;
using Rocket.Communication.Packets.Outgoing;
using Rocket.Communication.Packets.Outgoing.Offer;
using Rocket.Communication.Packets.Outgoing.Talents;
using Rocket.Communication.Packets.Outgoing.Rooms.Notifications;
using Rocket.Communication.Packets.Outgoing.Nux;
using Rocket.Communication.Packets.Incoming.GameCenter;

namespace Rocket.HabboHotel.GameClients
{
    public class GameClient
    {
        private readonly int _id;
        private Habbo _habbo;
        public string MachineId;
        private bool _disconnected;
        public ARC4 RC4Client = null;
        private GamePacketParser _packetParser;
        private ConnectionInformation _connection;
        public int PingCount { get; set; }

        public GameClient(int ClientId, ConnectionInformation pConnection)
        {
            this._id = ClientId;
            this._connection = pConnection;
            this._packetParser = new GamePacketParser(this);

            this.PingCount = 0;
        }

        private void SwitchParserRequest()
        {
            _packetParser.SetConnection(_connection);
            _packetParser.onNewPacket += parser_onNewPacket;
            byte[] data = (_connection.parser as InitialPacketParser).currentData;
            _connection.parser.Dispose();
            _connection.parser = _packetParser;
            _connection.parser.handlePacketData(data);
        }

        private void parser_onNewPacket(ClientPacket Message)
        {
            try
            {
                RocketEmulador.GetGame().GetPacketManager().TryExecutePacket(this, Message);
            }
            catch (Exception e)
            {
                Logging.LogPacketException(Message.ToString(), e.ToString());
            }
        }

        private void PolicyRequest()
        {
            _connection.SendData(RocketEmulador.GetDefaultEncoding().GetBytes("<?xml version=\"1.0\"?>\r\n" +
                   "<!DOCTYPE cross-domain-policy SYSTEM \"/xml/dtds/cross-domain-policy.dtd\">\r\n" +
                   "<cross-domain-policy>\r\n" +
                   "<allow-access-from domain=\"*\" to-ports=\"1-31111\" />\r\n" +
                   "</cross-domain-policy>\x0"));
        }


        public void StartConnection()
        {
            if (_connection == null)
                return;

            this.PingCount = 0;

            (_connection.parser as InitialPacketParser).PolicyRequest += PolicyRequest;
            (_connection.parser as InitialPacketParser).SwitchParserRequest += SwitchParserRequest;
            _connection.startPacketProcessing();
        }

        public bool TryAuthenticate(string AuthTicket)
        {
            try
            {
                byte errorCode = 0;
                UserData userData = UserDataFactory.GetUserData(AuthTicket, out errorCode);
                if (errorCode == 1 || errorCode == 2)
                {
                    Disconnect();
                    return false;
                }

                #region Ban Checking
         
                ModerationBan BanRecord = null;
                if (!string.IsNullOrEmpty(MachineId))
                {
                    if (RocketEmulador.GetGame().GetModerationManager().IsBanned(MachineId, out BanRecord))
                    {
                        if (RocketEmulador.GetGame().GetModerationManager().MachineBanCheck(MachineId))
                        {
                            Disconnect();
                            return false;
                        }
                    }
                }

                if (userData.user != null)
                {
                
                    BanRecord = null;
                    if (RocketEmulador.GetGame().GetModerationManager().IsBanned(userData.user.Username, out BanRecord))
                    {
                        if (RocketEmulador.GetGame().GetModerationManager().UsernameBanCheck(userData.user.Username))
                        {
                            Disconnect();
                            return false;
                        }
                    }
                }
                #endregion

                RocketEmulador.GetGame().GetClientManager().RegisterClient(this, userData.userID, userData.user.Username);
                this._habbo = userData.user;
                bool flag8 = this._habbo != null;
                if (flag8)
                {
                    userData.user.Init(this, userData);
                    this.SendMessage(new AuthenticationOKComposer());
                    this.SendMessage(new AvatarEffectsComposer(this._habbo.Effects().GetAllEffects));
                    this.SendMessage(new NavigatorSettingsComposer(this._habbo.HomeRoom));
                    this.SendMessage(new FavouritesComposer(userData.user.FavoriteRooms));
                    this.SendMessage(new FigureSetIdsComposer(this._habbo.GetClothing().GetClothingAllParts));
                    this.SendMessage(new UserRightsComposer(this._habbo.Rank));
                    this.SendMessage(new AvailabilityStatusComposer());
                   
                    this.SendMessage(new TalentTrackLevelComposer());
                    this.SendMessage(new TargetOfferMessageComposer());
                    this.SendMessage(new AchievementScoreComposer(this._habbo.GetStats().AchievementPoints));
         
                    ServerPacket serverPacket = new ServerPacket(879);
                    serverPacket.WriteString("club_habbo");
                    serverPacket.WriteInteger(0);
                    serverPacket.WriteInteger(0);
                    serverPacket.WriteInteger(0);
                    serverPacket.WriteInteger(2);
                    serverPacket.WriteBoolean(false);
                    serverPacket.WriteBoolean(false);
                    serverPacket.WriteInteger(0);
                    serverPacket.WriteInteger(0);
                    serverPacket.WriteInteger(0);
                    this.SendMessage(serverPacket);
                    this.SendMessage(new BuildersClubMembershipComposer());
                    this.SendMessage(new CfhTopicsInitComposer());
                    this.SendMessage(new BadgeDefinitionsComposer(RocketEmulador.GetGame().GetAchievementManager()._achievements));
                    this.SendMessage(new SoundSettingsComposer(this._habbo.ClientVolume, this._habbo.ChatPreference, this._habbo.AllowMessengerInvites, this._habbo.FocusPreference, FriendBarStateUtility.GetInt(this._habbo.FriendbarState)));
                    bool flag9 = this.GetHabbo().GetMessenger() != null;
                    if (flag9)
                    {
                        this.GetHabbo().GetMessenger().OnStatusChanged(true);
                    }
                    bool flag10 = !string.IsNullOrEmpty(this.MachineId);
                    if (flag10)
                    {
                        bool flag11 = this._habbo.MachineId != this.MachineId;
                        if (flag11)
                        {
                            using (IQueryAdapter queryReactor = RocketEmulador.GetDatabaseManager().GetQueryReactor())
                            {
                                queryReactor.SetQuery("UPDATE `users` SET `machine_id` = @MachineId WHERE `id` = @id LIMIT 1");
                                queryReactor.AddParameter("MachineId", this.MachineId);
                                queryReactor.AddParameter("id", this._habbo.Id);
                                queryReactor.RunQuery();
                            }
                        }
                        this._habbo.MachineId = this.MachineId;
                    }

                    PermissionGroup PermissionGroup = null;
                    if (RocketEmulador.GetGame().GetPermissionManager().TryGetGroup(_habbo.Rank, out PermissionGroup))
                    {
                        if (!String.IsNullOrEmpty(PermissionGroup.Badge))
                            if (!_habbo.GetBadgeComponent().HasBadge(PermissionGroup.Badge))
                                _habbo.GetBadgeComponent().GiveBadge(PermissionGroup.Badge, true, this);
                    }

                    SubscriptionData SubData = null;
                    if (RocketEmulador.GetGame().GetSubscriptionManager().TryGetSubscriptionData(this._habbo.VIPRank, out SubData))
                    {
                        if (!String.IsNullOrEmpty(SubData.Badge))
                        {
                            if (!_habbo.GetBadgeComponent().HasBadge(SubData.Badge))
                                _habbo.GetBadgeComponent().GiveBadge(SubData.Badge, true, this);
                        }
                    }

                    if (!RocketEmulador.GetGame().GetCacheManager().ContainsUser(_habbo.Id))
                        RocketEmulador.GetGame().GetCacheManager().GenerateUser(_habbo.Id);

                    _habbo.InitProcess();

                    if (userData.user.GetPermissions().HasRight("mod_tickets"))
                    {
                        SendMessage(new ModeratorInitComposer(
                          RocketEmulador.GetGame().GetModerationManager().UserMessagePresets,
                          RocketEmulador.GetGame().GetModerationManager().RoomMessagePresets,
                          RocketEmulador.GetGame().GetModerationManager().UserActionPresets,
                          RocketEmulador.GetGame().GetModerationTool().GetTickets));
                    }
                    
                    {
                        string hotelName = RocketEmulador.RocketData().data["hotelname"];
                        if (!string.IsNullOrWhiteSpace(RocketEmulador.GetDBConfig().DBData["welcome_message"]))
                            SendMessage(new MOTDNotificationComposer(RocketEmulador.GetDBConfig().DBData["welcome_message"]));

                   
                        SendMessage(new RoomNotificationComposer("entrar", "message", "Bem-vindo" +
                " " + userData.user.GetClient().GetHabbo().Username + " " +
                "ao " + hotelName + "!"));


                        if (GetHabbo().Rank == 10)
                        {
                            RocketEmulador.GetGame().GetClientManager().SendMessage(new RoomNotificationComposer("login", "message", "O PROGRAMADOR" +
                            " " + userData.user.GetClient().GetHabbo().Username + " " +
                            "entrou no " + hotelName + "!"));
                        }
                        if (GetHabbo().Rank == 9)
                        {
                            RocketEmulador.GetGame().GetClientManager().SendMessage(new RoomNotificationComposer("login", "message", "O CEO" +
                            " " + userData.user.GetClient().GetHabbo().Username + " " +
                            "entrou no " + hotelName + "!"));
                        }

                        if (GetHabbo().Rank == 8)
                        {
                            RocketEmulador.GetGame().GetClientManager().SendMessage(new RoomNotificationComposer("login", "message", "O GERENTE" +
                            " " + userData.user.GetClient().GetHabbo().Username + " " +
                            "entrou no " + hotelName + "!"));
                        }

                        if (GetHabbo().Rank == 7)
                        {
                            RocketEmulador.GetGame().GetClientManager().SendMessage(new RoomNotificationComposer("login", "message", "O ADM" +
                            " " + userData.user.GetClient().GetHabbo().Username + " " +
                            "entrou no " + hotelName + "!"));
                        }

                        if (GetHabbo().Rank == 6)
                        {
                            RocketEmulador.GetGame().GetClientManager().SendMessage(new RoomNotificationComposer("login", "message", "O MOD" +
                            " " + userData.user.GetClient().GetHabbo().Username + " " +
                            "entrou no " + hotelName + "!"));
                        }

                        if (GetHabbo().Rank == 5)
                        {
                            RocketEmulador.GetGame().GetClientManager().SendMessage(new RoomNotificationComposer("login", "message", "O PROMOTOR" +
                            " " + userData.user.GetClient().GetHabbo().Username + " " +
                            "entrou no " + hotelName + "!"));
                        }

                        if (GetHabbo().Rank == 4)
                        {
                            RocketEmulador.GetGame().GetClientManager().SendMessage(new RoomNotificationComposer("login", "message", "O EMB" +
                            " " + userData.user.GetClient().GetHabbo().Username + " " +
                            "entrou no " + hotelName + "!"));
                        }

                        if (GetHabbo().Rank == 3)
                        {
                            RocketEmulador.GetGame().GetClientManager().SendMessage(new RoomNotificationComposer("login", "message", "O VIP" +
                            " " + userData.user.GetClient().GetHabbo().Username + " " +
                            "entrou no " + hotelName + "!"));
                        }




                        if (GetHabbo().Rank == 2)
                        {
                            RocketEmulador.GetGame().GetClientManager().SendMessage(new RoomNotificationComposer("login", "message", "O LOCUTOR" +
                            " " + userData.user.GetClient().GetHabbo().Username + " " +
                            "entrou no " + hotelName + "!"));
                        }

                        Console.WriteLine("Um novo usuário se conectou seu nick é: " + userData.user.GetClient().GetHabbo().Username + " e seu ip: " + userData.user.GetClient().GetConnection().getIp(), "Rocket.Users",
       ConsoleColor.DarkGreen);
                    }
                    
                }
            }
 

            catch (Exception e)
            {
                Logging.LogCriticalException("Bug during user login: " + e);
            }
            return false;
        }


        public void SendWhisper(string Message, int Colour = 0)
        {
            if (this == null || GetHabbo() == null || GetHabbo().CurrentRoom == null)
                return;

            RoomUser User = GetHabbo().CurrentRoom.GetRoomUserManager().GetRoomUserByHabbo(GetHabbo().Username);
            if (User == null)
                return;

            SendMessage(new WhisperComposer(User.VirtualId, Message, 0, (Colour == 0 ? User.LastBubble : Colour)));
        }

        public void SendNotification(string Message)
        {
            SendMessage(new BroadcastMessageAlertComposer(Message));
        }

        public void SendMessage(IServerPacket Message)
        {
            byte[] bytes = Message.GetBytes();

            if (Message == null)
                return;

            if (GetConnection() == null)
                return;

            GetConnection().SendData(bytes);
        }

        public int ConnectionID
        {
            get { return _id; }
        }

        public ConnectionInformation GetConnection()
        {
            return _connection;
        }

        public Habbo GetHabbo()
        {
            return _habbo;
        }

        public void Disconnect()
        {
            try
            {
                if (GetHabbo() != null)
                {
                    using (IQueryAdapter dbClient = RocketEmulador.GetDatabaseManager().GetQueryReactor())
                    {
                        dbClient.RunQuery(GetHabbo().GetQueryString);


                    }

                    GetHabbo().OnDisconnect();
                }
            }
            catch (Exception e)
            {
                Logging.LogException(e.ToString());
            }


            if (!_disconnected)
            {
                if (_connection != null)
                    _connection.Dispose();
                _disconnected = true;
            }
        }

        public void Dispose()
        {
            if (GetHabbo() != null)
                GetHabbo().OnDisconnect();

            this.MachineId = string.Empty;
            this._disconnected = true;
            this._habbo = null;
            this._connection = null;
            this.RC4Client = null;
            this._packetParser = null;
        }
    }
}