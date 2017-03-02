using System;
using System.Linq;
using System.Text;
using System.Threading;
using System.Collections.Generic;

using log4net;
using Rocket.HabboHotel.Users;
using Rocket.Core;

namespace Rocket.HabboHotel.Cache.Process
{
    sealed class ProcessComponent
    {
        private static readonly ILog log = LogManager.GetLogger("Rocket.HabboHotel.Cache.Process.ProcessComponent");

        private Timer _timer = null;

        private bool _timerRunning = false;

       
        private bool _disabled = false;

       
        private AutoResetEvent _resetEvent = new AutoResetEvent(true);

       
        private static int _runtimeInSec = 1200;

       
        public ProcessComponent()
        {
        }

        
        public void Init()
        {
            this._timer = new Timer(new TimerCallback(Run), null, _runtimeInSec * 1000, _runtimeInSec * 1000);
        }

        public void Run(object State)
        {
            try
            {
                if (this._disabled)
                    return;

                if (this._timerRunning)
                {
                }

                this._resetEvent.Reset();

                List<UserCache> CacheList = RocketEmulador.GetGame().GetCacheManager().GetUserCache().ToList();
                if (CacheList.Count > 0)
                {
                    foreach (UserCache Cache in CacheList)
                    {
                        try
                        {
                            if (Cache == null)
                                continue;

                            UserCache Temp = null;

                            if (Cache.isExpired())
                                RocketEmulador.GetGame().GetCacheManager().TryRemoveUser(Cache.Id, out Temp);

                            Temp = null;
                        }
                        catch (Exception e)
                        {
                            Logging.LogCacheException(e.ToString());
                        }
                    }
                }

                CacheList = null;

                List<Habbo> CachedUsers = RocketEmulador.GetUsersCached().ToList();
                if (CachedUsers.Count > 0)
                {
                    foreach (Habbo Data in CachedUsers)
                    {
                        try
                        {
                            if (Data == null)
                                continue;

                            Habbo Temp = null;

                            if (Data.CacheExpired())
                                RocketEmulador.RemoveFromCache(Data.Id, out Temp);

                            if (Temp != null)
                                Temp.Dispose();

                            Temp = null;
                        }
                        catch (Exception e)
                        {
                            Logging.LogCacheException(e.ToString());
                        }
                    }
                }

                CachedUsers = null;
                
                this._timerRunning = false;

                this._resetEvent.Set();
            }
            catch (Exception e) { Logging.LogCacheException(e.ToString()); }
        }

      
        public void Dispose()
        {
         
            try
            {
                this._resetEvent.WaitOne(TimeSpan.FromMinutes(5));
            }
            catch { } // give up

         
            this._disabled = true;

            try
            {
                if (this._timer != null)
                    this._timer.Dispose();
            }
            catch { }

            this._timer = null;
        }
    }
}