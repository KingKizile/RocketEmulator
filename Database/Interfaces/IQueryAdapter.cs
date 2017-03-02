using System;

namespace Rocket.Database.Interfaces
{
    public interface IQueryAdapter : IRegularQueryAdapter, IDisposable
    {
        long InsertQuery();
        void RunQuery();
        void runFastQuery(string v);
    }
}