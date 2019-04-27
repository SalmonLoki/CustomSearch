using System;
using System.Data.Entity;

namespace CustomSearch
{
    public class SearchContext : DbContext
    {
        public SearchContext() : base("SearchResultsConnectionString")
        {

        }
        public virtual DbSet<Result> Results { get; set; }


        public virtual int ExecuteSqlCommand(string sql)
        {
            return Database.ExecuteSqlCommand(sql);
        }
        public virtual int ExecuteSqlCommand(string sql, params object[] parameters)
        {
            return Database.ExecuteSqlCommand(sql, parameters);
        }
        public virtual int ExecuteSqlCommand(TransactionalBehavior transactionalBehavior, string sql, params object[] parameters)
        {
            return Database.ExecuteSqlCommand(transactionalBehavior, sql, parameters);
        }
        public virtual bool AutoDetectChangesEnabled
        {
            get => Configuration.AutoDetectChangesEnabled;
            set => Configuration.AutoDetectChangesEnabled = value;
        }
        public virtual void DetectChanges()
        {
            ChangeTracker.DetectChanges();
        }
    }
}
