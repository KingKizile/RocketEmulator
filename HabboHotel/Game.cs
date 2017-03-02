
using log4net;

using Rocket.Communication.Packets;
using Rocket.HabboHotel.GameClients;
using Rocket.HabboHotel.Moderation;
using Rocket.HabboHotel.Support;
using Rocket.HabboHotel.Catalog;
using Rocket.HabboHotel.Items;
using Rocket.HabboHotel.Items.Televisions;
using Rocket.HabboHotel.Navigator;
using Rocket.HabboHotel.Rooms;
using Rocket.HabboHotel.Groups;
using Rocket.HabboHotel.Groups.Forums;

using Rocket.HabboHotel.Quests;
using Rocket.HabboHotel.Achievements;
using Rocket.HabboHotel.LandingView;
using Rocket.HabboHotel.Global;

using Rocket.HabboHotel.Games;

using Rocket.HabboHotel.Rooms.Chat;
using Rocket.HabboHotel.Talents;
using Rocket.HabboHotel.Bots;
using Rocket.HabboHotel.Cache;
using Rocket.HabboHotel.Rewards;
using Rocket.HabboHotel.Badges;
using Rocket.HabboHotel.Permissions;
using Rocket.HabboHotel.Subscriptions;
using System.Threading;
using System.Threading.Tasks;
using Rocket.HabboHotel.Camera;

namespace Rocket.HabboHotel
{
    public class Game
    {
        private static readonly ILog log = LogManager.GetLogger("Rocket.HabboHotel.Game");

        private readonly GroupForumManager _groupForumManager;
        private readonly PacketManager _packetManager;
        private readonly GameClientManager _clientManager;
        private readonly ModerationManager _modManager;
        private readonly ModerationTool _moderationTool;//TODO: Initialize from the moderation manager.
        private readonly ItemDataManager _itemDataManager;
        private readonly CatalogManager _catalogManager;
        private readonly TelevisionManager _televisionManager;//TODO: Initialize from the item manager.
        private readonly NavigatorManager _navigatorManager;
        private readonly RoomManager _roomManager;
        private readonly ChatManager _chatManager;
        private readonly GroupManager _groupManager;
        private readonly QuestManager _questManager;
        private readonly AchievementManager _achievementManager;
        private readonly TalentTrackManager _talentTrackManager;
        private readonly LandingViewManager _landingViewManager;//TODO: Rename class
        private readonly GameDataManager _gameDataManager;
        private readonly ServerStatusUpdater _globalUpdater;
        private readonly LanguageLocale _languageLocale;
        private readonly AntiMutant _antiMutant;
        private readonly BotManager _botManager;
        private readonly CacheManager _cacheManager;
        private readonly RewardManager _rewardManager;
        private readonly BadgeManager _badgeManager;
        private readonly PermissionManager _permissionManager;
        private readonly SubscriptionManager _subscriptionManager;

        private readonly CameraPhotoManager _cameraManager;

        private bool _cycleEnded;
        private bool _cycleActive;
        private Task _gameCycle;
        private int _cycleSleepTime = 25;

        public Game()
        {
            this._packetManager = new PacketManager();
            this._clientManager = new GameClientManager();
            this._modManager = new ModerationManager();
            this._moderationTool = new ModerationTool();
            this._groupForumManager = new GroupForumManager();
            this._itemDataManager = new ItemDataManager();
            this._itemDataManager.Init();

            this._catalogManager = new CatalogManager();
            this._catalogManager.Init(this._itemDataManager);

            this._televisionManager = new TelevisionManager();

            this._navigatorManager = new NavigatorManager();
            this._roomManager = new RoomManager();
            this._chatManager = new ChatManager();
            this._groupManager = new GroupManager();
            this._questManager = new QuestManager();
            this._achievementManager = new AchievementManager();
            this._talentTrackManager = new TalentTrackManager();

            this._landingViewManager = new LandingViewManager();
            this._gameDataManager = new GameDataManager();

            this._globalUpdater = new ServerStatusUpdater();
            this._globalUpdater.Init();

            this._languageLocale = new LanguageLocale();
            this._antiMutant = new AntiMutant();
            this._botManager = new BotManager();

            this._cacheManager = new CacheManager();
            this._rewardManager = new RewardManager();

            this._badgeManager = new BadgeManager();
            this._badgeManager.Init();

            this._permissionManager = new PermissionManager();
            this._permissionManager.Init();

            this._subscriptionManager = new SubscriptionManager();
            this._subscriptionManager.Init();

            this._cameraManager = new CameraPhotoManager();
            this._cameraManager.Init(this._itemDataManager);
        }

        public void StartGameLoop()
        {
            this._gameCycle = new Task(GameCycle);
            this._gameCycle.Start();

            this._cycleActive = true;
        }

        private void GameCycle()
        {
            while (this._cycleActive)
            {
                this._cycleEnded = false;

                RocketEmulador.GetGame().GetRoomManager().OnCycle();
                RocketEmulador.GetGame().GetClientManager().OnCycle();

                this._cycleEnded = true;
                Thread.Sleep(this._cycleSleepTime);
            }
        }

        public void StopGameLoop()
        {
            this._cycleActive = false;

            while (!this._cycleEnded)
            {
                Thread.Sleep(this._cycleSleepTime);
            }
        }

        public PacketManager GetPacketManager()
        {
            return _packetManager;
        }

        public GameClientManager GetClientManager()
        {
            return _clientManager;
        }

        public CatalogManager GetCatalog()
        {
            return _catalogManager;
        }

        public NavigatorManager GetNavigator()
        {
            return _navigatorManager;
        }

        public ItemDataManager GetItemManager()
        {
            return _itemDataManager;
        }

        public RoomManager GetRoomManager()
        {
            return _roomManager;
        }

        public AchievementManager GetAchievementManager()
        {
            return _achievementManager;
        }

        public TalentTrackManager GetTalentTrackManager()
        {
            return _talentTrackManager;
        }

        public ModerationTool GetModerationTool()
        {
            return _moderationTool;
        }

        public ModerationManager GetModerationManager()
        {
            return this._modManager;
        }

        public PermissionManager GetPermissionManager()
        {
            return this._permissionManager;
        }

        public SubscriptionManager GetSubscriptionManager()
        {
            return this._subscriptionManager;
        }

        public QuestManager GetQuestManager()
        {
            return this._questManager;
        }

        public GroupManager GetGroupManager()
        {
            return _groupManager;
        }
        public GroupForumManager GetGroupForumManager()
        {
            return _groupForumManager;
        }

        public LandingViewManager GetLandingManager()
        {
            return _landingViewManager;
        }
        public TelevisionManager GetTelevisionManager()
        {
            return _televisionManager;
        }

        public ChatManager GetChatManager()
        {
            return this._chatManager;
        }

        public GameDataManager GetGameDataManager()
        {
            return this._gameDataManager;
        }

        public LanguageLocale GetLanguageLocale()
        {
            return this._languageLocale;
        }

        public AntiMutant GetAntiMutant()
        {
            return this._antiMutant;
        }

        public BotManager GetBotManager()
        {
            return this._botManager;
        }

        public CacheManager GetCacheManager()
        {
            return this._cacheManager;
        }

        public RewardManager GetRewardManager()
        {
            return this._rewardManager;
        }

        public BadgeManager GetBadgeManager()
        {
            return this._badgeManager;
        }

        public CameraPhotoManager GetCameraManager()
        {
            return this._cameraManager;
        }
    }
}