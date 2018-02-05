using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace DataBase.Base.Service.Infrastructure
{
    public class DatabaseFactory : IDatabaseFactory
    {
        // Fields
        public IApplicationDb _dataContext;

        // Methods
        public DatabaseFactory(IApplicationDb applicationDb)
        {
            this._dataContext = applicationDb;
        }

        public IApplicationDb Get()
        {
            this._dataContext = this._dataContext ?? (this._dataContext = new ApplicationDB());
            this._dataContext.Database.Log = DataContext.ActionNames ?? (DataContext.ActionNames = new Action<string>(DataContext.context.GetLog));
            return this._dataContext;
        }

        // Nested Types
        [Serializable, CompilerGenerated]
        private sealed class DataContext
        {
            // Fields
            public static readonly DatabaseFactory.DataContext context = new DatabaseFactory.DataContext();
            public static Action<string> ActionNames;

            // Methods
            internal void GetLog(string log)
            {
                Trace.Write(log);
            }
        }
    }
}