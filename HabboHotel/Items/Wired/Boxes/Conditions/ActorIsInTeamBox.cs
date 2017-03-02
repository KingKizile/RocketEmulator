using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Collections.Concurrent;

using Rocket.HabboHotel.Rooms;
using Rocket.HabboHotel.Users;
using Rocket.Communication.Packets.Incoming;
using Rocket.HabboHotel.Rooms.Games.Teams;

namespace Rocket.HabboHotel.Items.Wired.Boxes.Conditions
{
    internal class ActorIsInTeamBox : IWiredItem
    {
        public Room Instance
        {
            get;
            set;
        }

        public Item Item
        {
            get;
            set;
        }

        public WiredBoxType Type
        {
            get
            {
                return WiredBoxType.ConditionActorIsInTeamBox;
            }
        }

        public ConcurrentDictionary<int, Item> SetItems
        {
            get;
            set;
        }

        public string StringData
        {
            get;
            set;
        }

        public bool BoolData
        {
            get;
            set;
        }

        public string ItemsData
        {
            get;
            set;
        }

        public ActorIsInTeamBox(Room Instance, Item Item)
        {
            this.Instance = Instance;
            this.Item = Item;
            this.SetItems = new ConcurrentDictionary<int, Item>();
        }

        public void HandleSave(ClientPacket Packet)
        {
            int num = Packet.PopInt();
            this.StringData = Packet.PopInt().ToString();
        }

        public bool Execute(params object[] Params)
        {
            bool flag = Params.Length == 0 || this.Instance == null || string.IsNullOrEmpty(this.StringData);
            bool result;
            if (flag)
            {
                result = false;
            }
            else
            {
                Habbo habbo = (Habbo)Params[0];
                bool flag2 = habbo == null;
                if (flag2)
                {
                    result = false;
                }
                else
                {
                    RoomUser roomUserByHabbo = this.Instance.GetRoomUserManager().GetRoomUserByHabbo(habbo.Id);
                    bool flag3 = roomUserByHabbo == null;
                    if (flag3)
                    {
                        result = false;
                    }
                    else
                    {
                        bool flag4 = int.Parse(this.StringData) == 1 && roomUserByHabbo.Team == TEAM.RED;
                        if (flag4)
                        {
                            result = true;
                        }
                        else
                        {
                            bool flag5 = int.Parse(this.StringData) == 2 && roomUserByHabbo.Team == TEAM.GREEN;
                            if (flag5)
                            {
                                result = true;
                            }
                            else
                            {
                                bool flag6 = int.Parse(this.StringData) == 3 && roomUserByHabbo.Team == TEAM.BLUE;
                                if (flag6)
                                {
                                    result = true;
                                }
                                else
                                {
                                    bool flag7 = int.Parse(this.StringData) == 4 && roomUserByHabbo.Team == TEAM.YELLOW;
                                    result = flag7;
                                }
                            }
                        }
                    }
                }
            }
            return result;
        }
    }
}