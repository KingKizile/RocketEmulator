using Rocket.Database.Interfaces;
using Rocket.HabboHotel.Groups;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Runtime.CompilerServices;
namespace Rocket.HabboHotel.Rooms
{
    public class RoomData
    {
        public int Id;

        public int AllowPets;

        public int AllowPetsEating;

        public int RoomBlockingEnabled;

        public int Category;

        public string Description;

        public string Floor;

        public int FloorThickness;

        public Group Group;

        public int Hidewall;

        public string Landscape;

        public string ModelName;

        public string Name;

        public string OwnerName;

        public int OwnerId;

        public string Password;

        public int Score;

        public RoomAccess Access;

        public List<string> Tags;

        public string Type;

        public int UsersMax;

        public int UsersNow;

        public int WallThickness;

        public string Wallpaper;

        public int WhoCanBan;

        public int WhoCanKick;

        public int WhoCanMute;

        private RoomModel mModel;

        public int chatMode;

        public int chatSpeed;

        public int chatSize;

        public int extraFlood;

        public int chatDistance;

        public Dictionary<int, KeyValuePair<int, string>> WiredScoreBordDay;

        public Dictionary<int, KeyValuePair<int, string>> WiredScoreBordWeek;

        public Dictionary<int, KeyValuePair<int, string>> WiredScoreBordMonth;

        public List<int> WiredScoreFirstBordInformation = new List<int>();

        public int TradeSettings;

        public RoomPromotion _promotion;

        public bool PushEnabled;

        public bool PullEnabled;

        public bool SPushEnabled;

        public bool SPullEnabled;

        public bool EnablesEnabled;

        public bool RespectNotificationsEnabled;

        public bool PetMorphsAllowed;

        public RoomPromotion Promotion
        {
            get
            {
                return this._promotion;
            }
            set
            {
                this._promotion = value;
            }
        }

        public bool HasActivePromotion
        {
            get
            {
                return this.Promotion != null;
            }
        }
        //Room For Sale
        public bool roomForSale = false;
        public string roomSaleType = "";
        public int roomSaleCost = 0;
        public RoomModel Model
        {
            get
            {
                bool flag = this.mModel == null;
                if (flag)
                {
                    this.mModel = RocketEmulador.GetGame().GetRoomManager().GetModel(this.ModelName);
                }
                return this.mModel;
            }
        }

        public void Fill(DataRow Row)
        {
            this.Id = Convert.ToInt32(Row["id"]);
            this.Name = Convert.ToString(Row["caption"]);
            this.Description = Convert.ToString(Row["description"]);
            this.Type = Convert.ToString(Row["roomtype"]);
            this.OwnerId = Convert.ToInt32(Row["owner"]);
            this.OwnerName = "";
            using (IQueryAdapter queryReactor = RocketEmulador.GetDatabaseManager().GetQueryReactor())
            {
                queryReactor.SetQuery("SELECT `username` FROM `users` WHERE `id` = @owner LIMIT 1");
                queryReactor.AddParameter("owner", this.OwnerId);
                string @string = queryReactor.getString();
                bool flag = !string.IsNullOrEmpty(@string);
                if (flag)
                {
                    this.OwnerName = @string;
                }
            }
            this.Access = RoomAccessUtility.ToRoomAccess(Row["state"].ToString().ToLower());
            this.Category = Convert.ToInt32(Row["category"]);
            bool flag2 = !string.IsNullOrEmpty(Row["users_now"].ToString());
            if (flag2)
            {
                this.UsersNow = Convert.ToInt32(Row["users_now"]);
            }
            else
            {
                this.UsersNow = 0;
            }
            this.UsersMax = Convert.ToInt32(Row["users_max"]);
            this.ModelName = Convert.ToString(Row["model_name"]);
            this.Score = Convert.ToInt32(Row["score"]);
            this.Tags = new List<string>();
            this.AllowPets = Convert.ToInt32(Row["allow_pets"].ToString());
            this.AllowPetsEating = Convert.ToInt32(Row["allow_pets_eat"].ToString());
            this.RoomBlockingEnabled = Convert.ToInt32(Row["room_blocking_disabled"].ToString());
            this.Hidewall = Convert.ToInt32(Row["allow_hidewall"].ToString());
            this.Password = Convert.ToString(Row["password"]);
            this.Wallpaper = Convert.ToString(Row["wallpaper"]);
            this.Floor = Convert.ToString(Row["floor"]);
            this.Landscape = Convert.ToString(Row["landscape"]);
            this.FloorThickness = Convert.ToInt32(Row["floorthick"]);
            this.WallThickness = Convert.ToInt32(Row["wallthick"]);
            this.WhoCanMute = Convert.ToInt32(Row["mute_settings"]);
            this.WhoCanKick = Convert.ToInt32(Row["kick_settings"]);
            this.WhoCanBan = Convert.ToInt32(Row["ban_settings"]);
            this.chatMode = Convert.ToInt32(Row["chat_mode"]);
            this.chatSpeed = Convert.ToInt32(Row["chat_speed"]);
            this.chatSize = Convert.ToInt32(Row["chat_size"]);
            this.TradeSettings = Convert.ToInt32(Row["trade_settings"]);
            Group group = null;
            bool flag3 = RocketEmulador.GetGame().GetGroupManager().TryGetGroup(Convert.ToInt32(Row["group_id"]), out group);
            if (flag3)
            {
                this.Group = group;
            }
            else
            {
                this.Group = null;
            }
            string[] array = Row["tags"].ToString().Split(new char[]
            {
                ','
            });
            for (int i = 0; i < array.Length; i++)
            {
                string item = array[i];
                this.Tags.Add(item);
            }
            this.mModel = RocketEmulador.GetGame().GetRoomManager().GetModel(this.ModelName);
            this.PushEnabled = RocketEmulador.EnumToBool(Row["push_enabled"].ToString());
            this.PullEnabled = RocketEmulador.EnumToBool(Row["pull_enabled"].ToString());
            this.SPushEnabled = RocketEmulador.EnumToBool(Row["spush_enabled"].ToString());
            this.SPullEnabled = RocketEmulador.EnumToBool(Row["spull_enabled"].ToString());
            this.EnablesEnabled = RocketEmulador.EnumToBool(Row["enables_enabled"].ToString());
            this.RespectNotificationsEnabled = RocketEmulador.EnumToBool(Row["respect_notifications_enabled"].ToString());
            this.PetMorphsAllowed = RocketEmulador.EnumToBool(Row["pet_morphs_allowed"].ToString());
            this.WiredScoreBordDay = new Dictionary<int, KeyValuePair<int, string>>();
            this.WiredScoreBordWeek = new Dictionary<int, KeyValuePair<int, string>>();
            this.WiredScoreBordMonth = new Dictionary<int, KeyValuePair<int, string>>();
            using (IQueryAdapter queryReactor2 = RocketEmulador.GetDatabaseManager().GetQueryReactor())
            {
                List<bool> list = new List<bool>
                {
                    false,
                    false,
                    false
                };
                DateTime now = DateTime.Now;
                int num = Convert.ToInt32(now.ToString("MMddyyyy"));
                int num2 = Convert.ToInt32(now.ToString("MM"));
                int weekOfYear = CultureInfo.GetCultureInfo("Nl-nl").Calendar.GetWeekOfYear(DateTime.Now, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
                this.WiredScoreFirstBordInformation = new List<int>
                {
                    num,
                    num2,
                    weekOfYear
                };
                queryReactor2.SetQuery("SELECT * FROM wired_scorebord WHERE roomid = @id ORDER BY `punten` DESC ");
                queryReactor2.AddParameter("id", this.Id);
                foreach (DataRow dataRow in queryReactor2.getTable().Rows)
                {
                    int key = Convert.ToInt32(dataRow["userid"]);
                    string value = Convert.ToString(dataRow["username"]);
                    int key2 = Convert.ToInt32(dataRow["punten"]);
                    string a = Convert.ToString(dataRow["soort"]);
                    int num3 = Convert.ToInt32(dataRow["timestamp"]);
                    bool flag4 = a == "day" && !this.WiredScoreBordDay.ContainsKey(key) && !list[0];
                    if (flag4)
                    {
                        bool flag5 = num3 != num;
                        if (flag5)
                        {
                            list[0] = false;
                        }
                        bool flag6 = !list[0];
                        if (flag6)
                        {
                            this.WiredScoreBordDay.Add(key, new KeyValuePair<int, string>(key2, value));
                        }
                    }
                    bool flag7 = a == "month" && !this.WiredScoreBordMonth.ContainsKey(key) && !list[1];
                    if (flag7)
                    {
                        bool flag8 = num3 != num2;
                        if (flag8)
                        {
                            list[1] = false;
                        }
                        this.WiredScoreBordMonth.Add(key, new KeyValuePair<int, string>(key2, value));
                    }
                    bool flag9 = a == "week" && !this.WiredScoreBordWeek.ContainsKey(key) && !list[2];
                    if (flag9)
                    {
                        bool flag10 = num3 != weekOfYear;
                        if (flag10)
                        {
                            list[2] = false;
                        }
                        this.WiredScoreBordWeek.Add(key, new KeyValuePair<int, string>(key2, value));
                    }
                }
                bool flag11 = list[0];
                if (flag11)
                {
                    queryReactor2.RunQuery("DELETE FROM `wired_scorebord` WHERE `roomid`='" + this.Id + "' AND `soort`='day'");
                    this.WiredScoreBordDay.Clear();
                }
                bool flag12 = list[1];
                if (flag12)
                {
                    queryReactor2.RunQuery("DELETE FROM `wired_scorebord` WHERE `roomid`='" + this.Id + "' AND `soort`='month'");
                    this.WiredScoreBordMonth.Clear();
                }
                bool flag13 = list[2];
                if (flag13)
                {
                    queryReactor2.RunQuery("DELETE FROM `wired_scorebord` WHERE `roomid`='" + this.Id + "' AND `soort`='week'");
                    this.WiredScoreBordDay.Clear();
                }
            }
        }

        public void EndPromotion()
        {
            bool flag = !this.HasActivePromotion;
            if (!flag)
            {
                this.Promotion = null;
            }
        }
    }
}
