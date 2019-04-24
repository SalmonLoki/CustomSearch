using System;
using System.Data.Entity;

namespace CustomSearch
{
    class SearchContext : DbContext
    {
        public SearchContext() : base("SearchResultsConnectionString")
        {

        }
        public DbSet<Result> Results { get; set; }
    }
}
