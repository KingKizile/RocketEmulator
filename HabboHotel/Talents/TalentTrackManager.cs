﻿using System;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections.Generic;

using log4net;
using Rocket.Database.Interfaces;

using Rocket.HabboHotel.Achievements;

namespace Rocket.HabboHotel.Talents
{
    public class TalentTrackManager
    {
        private static ILog log = LogManager.GetLogger("Rocket.HabboHotel.Talents.TalentManager");

        private readonly Dictionary<int, TalentTrackLevel> _citizenshipLevels;

        public TalentTrackManager()
        {
            this._citizenshipLevels = new Dictionary<int, TalentTrackLevel>();

            this.Init();
        }

        public void Init()
        {
            DataTable GetTable = null;
            using (IQueryAdapter dbClient = RocketEmulador.GetDatabaseManager().GetQueryReactor())
            {
                dbClient.SetQuery("SELECT `type`,`level`,`data_actions`,`data_gifts` FROM `talents`");
                GetTable = dbClient.getTable();
            }

            if (GetTable != null)
            {
                foreach (DataRow Row in GetTable.Rows)
                {
                    this._citizenshipLevels.Add(Convert.ToInt32(Row["level"]), new TalentTrackLevel(Convert.ToString(Row["type"]), Convert.ToInt32(Row["level"]), Convert.ToString(Row["data_actions"]), Convert.ToString(Row["data_gifts"])));
                }
            }
        }

        public ICollection<TalentTrackLevel> GetLevels()
        {
            return this._citizenshipLevels.Values;
        }
    }
}
