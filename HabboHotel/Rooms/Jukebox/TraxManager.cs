using System.Runtime.CompilerServices;
//using Rocket;
//using Rocket.Communication.Packets.Outgoing.Sound;
//using Rocket.Database.Interfaces;
//using Rocket.HabboHotel.Items;
//using Rocket.HabboHotel.Rooms;
//using System;
//using System.Collections;
//using System.Collections.Generic;
//using System.Data;
//using System.Linq;
//using System.Runtime.CompilerServices;

//namespace Rocket.HabboHotel.Rooms.TraxMachine
//{
//    public class RoomTraxManager
//    {
//        public int Capacity = 10;

//        private DataTable dataTable;

//        public Item ActualSongData
//        {
//            get
//            {
//                Item value;
//                IEnumerable<KeyValuePair<int, Item>> keyValuePairs = this.GetPlayLine().Reverse<KeyValuePair<int, Item>>();
//                int timestanpSinceStarted = this.TimestanpSinceStarted;
//                if (timestanpSinceStarted > this.TotalPlayListLength)
//                {
//                    return null;
//                }
//                using (IEnumerator<KeyValuePair<int, Item>> enumerator = keyValuePairs.GetEnumerator())
//                {
//                    while (enumerator.MoveNext())
//                    {
//                        KeyValuePair<int, Item> current = enumerator.Current;
//                        if (current.Key > timestanpSinceStarted)
//                        {
//                            continue;
//                        }
//                        value = current.Value;
//                        return value;
//                    }
//                    return null;
//                }
//                return value;
//            }
//        }

//        public int ActualSongTimePassed
//        {
//            get
//            {
//                Dictionary<int, Item> playLine = this.GetPlayLine();
//                int key = 0;
//                foreach (KeyValuePair<int, Item> keyValuePair in playLine)
//                {
//                    if (keyValuePair.Value != this.ActualSongData)
//                    {
//                        continue;
//                    }
//                    key = keyValuePair.Key;
//                }
//                return checked(this.TimestanpSinceStarted - key);
//            }
//        }

//        public Item AnteriorItem
//        {
//            get;
//            private set;
//        }

//        public TraxMusicData AnteriorMusic
//        {
//            get;
//            private set;
//        }

//        public bool IsPlaying
//        {
//            get;
//            private set;
//        }

//        public List<Item> Playlist
//        {
//            get;
//            private set;
//        }

//        public Room Room
//        {
//            get;
//            private set;
//        }

//        public Item SelectedDiscItem
//        {
//            get;
//            private set;
//        }

//        public int StartedPlayTimestanp
//        {
//            get;
//            private set;
//        }

//        public int TimestanpSinceStarted
//        {
//            get
//            {
//                return checked(checked((int)RocketEmulador.GetUnixTimestamp()) - this.StartedPlayTimestanp);
//            }
//        }

//        public int TotalPlayListLength
//        {
//            get
//            {
//                int length = 0;
//                foreach (Item playlist in this.Playlist)
//                {
//                    TraxMusicData music = TraxSoundManager.GetMusic(playlist.ExtradataInt);
//                    if (music == null)
//                    {
//                        continue;
//                    }
//                    length = checked(length + music.Length);
//                }
//                return length;
//            }
//        }

//        public RoomTraxManager(Room room)
//        {
//            this.Room = room;
//            room.OnFurnisLoad += new Room.FurnitureLoad(this.Room_OnFurnisLoad);
//            this.IsPlaying = false;
//            this.StartedPlayTimestanp = 0;
//            this.Playlist = new List<Item>();
//            this.SelectedDiscItem = null;
//            using (IQueryAdapter queryReactor = DatabaseManager.GetQueryReactor())
//            {
//                queryReactor.RunQuery(string.Concat("SELECT * FROM room_jukebox_songs WHERE room_id = '", this.Room.Id, "'"));
//                this.dataTable = queryReactor.getTable();
//            }
//        }

//        public bool AddDisc(Item item)
//        {
//            int num;
//            if (item.GetBaseItem().InteractionType != InteractionType.MUSIC_DISC)
//            {
//                return false;
//            }
//            if (!int.TryParse(item.ExtraData, out num))
//            {
//                return false;
//            }
//            if (TraxSoundManager.GetMusic(num) == null)
//            {
//                return false;
//            }
//            if (this.Playlist.Contains(item))
//            {
//                return false;
//            }
//            if (this.IsPlaying)
//            {
//                return false;
//            }
//            using (IQueryAdapter queryReactor = DatabaseManager.GetQueryReactor())
//            {
//                queryReactor.SetQuery("INSERT INTO room_jukebox_songs (room_id, item_id) VALUES (@room, @item)");
//                queryReactor.AddParameter("room", this.Room.Id);
//                queryReactor.AddParameter("item", item.Id);
//                queryReactor.RunQuery();
//            }
//            this.Playlist.Add(item);
//            this.Room.SendMessage(new SetJukeboxPlayListComposer(this.Room), false);
//            this.Room.SendMessage(new LoadJukeboxUserMusicItemsComposer(this.Room), false);
//            return true;
//        }

//        public void ClearPlayList()
//        {
//            if (this.IsPlaying)
//            {
//                this.StopPlayList();
//            }
//            this.Playlist.Clear();
//        }

//        public List<Item> GetAvaliableSongs()
//        {
//            return this.Room.GetRoomItemHandler().GetFloor.Where<Item>((Item c) => {
//                if (c.GetBaseItem().InteractionType != InteractionType.MUSIC_DISC)
//                {
//                    return false;
//                }
//                return !this.Playlist.Contains(c);
//            }).ToList<Item>();
//        }

//        public Item GetDiscItem(int id)
//        {
//            Item item;
//            List<Item>.Enumerator enumerator = this.Playlist.GetEnumerator();
//            try
//            {
//                while (enumerator.MoveNext())
//                {
//                    Item current = enumerator.Current;
//                    if (current.Id != id)
//                    {
//                        continue;
//                    }
//                    item = current;
//                    return item;
//                }
//                return null;
//            }
//            finally
//            {
//                ((IDisposable)enumerator).Dispose();
//            }
//            return item;
//        }

//        public TraxMusicData GetMusicByItem(Item item)
//        {
//            if (item == null)
//            {
//                return null;
//            }
//            return TraxSoundManager.GetMusic(item.ExtradataInt);
//        }

//        public int GetMusicIndex(Item item)
//        {
//            for (int i = 0; i < this.Playlist.Count; i++)
//            {
//                if (this.Playlist[i] == item)
//                {
//                    return i;
//                }
//            }
//            return 0;
//        }

//        public Dictionary<int, Item> GetPlayLine()
//        {
//            int length = 0;
//            Dictionary<int, Item> nums = new Dictionary<int, Item>();
//            foreach (Item playlist in this.Playlist)
//            {
//                TraxMusicData musicByItem = this.GetMusicByItem(playlist);
//                if (musicByItem == null)
//                {
//                    continue;
//                }
//                nums.Add(length, playlist);
//                length = checked(length + musicByItem.Length);
//            }
//            return nums;
//        }

//        public void OnCycle()
//        {
//            if (this.IsPlaying && this.ActualSongData != this.SelectedDiscItem)
//            {
//                this.AnteriorItem = this.SelectedDiscItem;
//                this.AnteriorMusic = this.GetMusicByItem(this.SelectedDiscItem);
//                this.SelectedDiscItem = this.ActualSongData;
//                if (this.SelectedDiscItem == null)
//                {
//                    this.StopPlayList();
//                }
//                this.Room.SendMessage(new SetJukeboxNowPlayingComposer(this.Room), false);
//            }
//        }

//        public void PlayPlaylist()
//        {
//            if (this.Playlist.Count == 0)
//            {
//                return;
//            }
//            this.StartedPlayTimestanp = checked((int)RocketEmulador.GetUnixTimestamp());
//            this.SelectedDiscItem = null;
//            this.IsPlaying = true;
//            this.SetJukeboxesState();
//        }

//        public bool RemoveDisc(int id)
//        {
//            Item discItem = this.GetDiscItem(id);
//            if (discItem == null)
//            {
//                return false;
//            }
//            if (this.IsPlaying)
//            {
//                return false;
//            }
//            using (IQueryAdapter queryReactor = DatabaseManager.GetQueryReactor())
//            {
//                queryReactor.RunQuery(string.Concat("DELETE FROM room_jukebox_songs WHERE item_id = '", discItem.Id, "'"));
//            }
//            this.Playlist.Remove(discItem);
//            this.Room.SendMessage(new SetJukeboxPlayListComposer(this.Room), false);
//            this.Room.SendMessage(new LoadJukeboxUserMusicItemsComposer(this.Room), false);
//            return true;
//        }

//        public bool RemoveDisc(Item item)
//        {
//            return this.RemoveDisc(item.Id);
//        }

//        private void Room_OnFurnisLoad()
//        {
//            foreach (DataRow row in this.dataTable.Rows)
//            {
//                int num = int.Parse(row["item_id"].ToString());
//                Item item = this.Room.GetRoomItemHandler().GetItem(num);
//                if (item == null)
//                {
//                    continue;
//                }
//                this.Playlist.Add(item);
//            }
//        }

//        public void SetJukeboxesState()
//        {
//            foreach (Item getFloor in this.Room.GetRoomItemHandler().GetFloor)
//            {
//                if (getFloor.GetBaseItem().InteractionType != InteractionType.JUKEBOX)
//                {
//                    continue;
//                }
//                getFloor.ExtraData = (this.IsPlaying ? "1" : "0");
//                getFloor.UpdateState();
//            }
//        }

//        public void StopPlayList()
//        {
//            this.IsPlaying = false;
//            this.StartedPlayTimestanp = 0;
//            this.SelectedDiscItem = null;
//            this.Room.SendMessage(new SetJukeboxNowPlayingComposer(this.Room), false);
//            this.SetJukeboxesState();
//        }

//        public void TriggerPlaylistState()
//        {
//            if (this.IsPlaying)
//            {
//                this.StopPlayList();
//                return;
//            }
//            this.PlayPlaylist();
//        }
//    }
//}