using System;
using System.Collections.Generic;
using System.Data;


using log4net;
using Rocket.Database.Interfaces;

namespace Rocket.HabboHotel.Global
{
    public class LanguageLocale
    {
        private Dictionary<string, string> _values = new Dictionary<string, string>();

        private static readonly ILog log = LogManager.GetLogger("Rocket.HabboHotel.Global.LanguageLocale");

        public LanguageLocale()
        {
            this._values = new Dictionary<string, string>();

            this.Init();
        }

        public void Init()
        {
            if (this._values.Count > 0)
                this._values.Clear();

            using (IQueryAdapter dbClient = RocketEmulador.GetDatabaseManager().GetQueryReactor())
            {
                dbClient.SetQuery("SELECT * FROM `server_locale`");
                DataTable Table = dbClient.getTable();

                if (Table != null)
                {
                    foreach (DataRow Row in Table.Rows)
                    {
                        this._values.Add(Row["key"].ToString(), Row["value"].ToString());
                    }
                }
            }

            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("       [FUNCIONANDO] => [Rocket Emu] =>  Carregando as configurações da tela inicial.");
        }

        public string TryGetValue(string value)
        {
            return this._values.ContainsKey(value) ? this._values[value] : "Missing language locale for [" + value + "]";
        }
    }
}