using System.Collections;
using System.Collections.Generic;
using Rocket.HabboHotel.Achievements;
using Rocket.HabboHotel.Items;
using Rocket.HabboHotel.Rooms.AI;
using Rocket.HabboHotel.Rooms;
using Rocket.HabboHotel.Users.Badges;
using Rocket.HabboHotel.Users.Inventory;
using Rocket.HabboHotel.Users.Messenger;
using Rocket.HabboHotel.Users.Relationships;
using System.Collections.Concurrent;

namespace Rocket.HabboHotel.Users.UserDataManagement
{
    public class UserData
    {
        public int userID;
        public Habbo user;

        public Dictionary<int, Relationship> Relations;
        public ConcurrentDictionary<string, UserAchievement> achievements;
        public List<Badge> badges;
        public List<int> favouritedRooms;
        public Dictionary<int, MessengerRequest> requests;
        public Dictionary<int, MessengerBuddy> friends;
        public List<int> ignores;
        public Dictionary<int, int> quests;
        public List<RoomData> rooms;

        public UserData(int userID, ConcurrentDictionary<string, UserAchievement> achievements, List<int> favouritedRooms, List<int> ignores,
            List<Badge> badges, Dictionary<int, MessengerBuddy> friends, Dictionary<int, MessengerRequest> requests, List<RoomData> rooms, Dictionary<int, int> quests, Habbo user,
            Dictionary<int, Relationship> Relations)
        {
            this.userID = userID;
            this.achievements = achievements;
            this.favouritedRooms = favouritedRooms;
            this.ignores = ignores;
            this.badges = badges;
            this.friends = friends;
            this.requests = requests;
            this.rooms = rooms;
            this.quests = quests;
            this.user = user;
            this.Relations = Relations;
        }
    }
}