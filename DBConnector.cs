using System;
using System.Linq;
using System.Windows.Forms;

namespace CustomSearch
{
    class DBConnector
    {
        public SearchResult[] getOldResultsFromDB()
        {
            using (SearchContext searchContext = new SearchContext())
            {
                return searchContext.Results
                .Select(u => new SearchResult
                {
                    Link = u.Link,
                    Name =u.Name
                }).ToArray();
            }
        }

        public SearchResult[] searchInDB(string keyword)
        {
            using (SearchContext searchContext = new SearchContext())
            {
                return searchContext.Results
                .Where(u => u.Link.Contains(keyword) | u.Name.Contains(keyword))
                .Select(u => new SearchResult
                {
                    Link = u.Link,
                    Name = u.Name
                }).ToArray();
            }
        }

        public void updateDataInDB(SearchResult[] oldResults, SearchResult[] newResults, TextBox textBox)
        {
            if (oldResults.SequenceEqual(newResults))
            {
                textBox.Text = "Данные не устарели";
            }
            else
            {
                textBox.Text = "Данные устарели...Обновление данных. ";

                using (SearchContext searchContext = new SearchContext())
                {
                    searchContext.Database.ExecuteSqlCommand("TRUNCATE TABLE Results");
                    searchContext.SaveChanges();

                    searchContext.Configuration.AutoDetectChangesEnabled = false;
                    foreach (SearchResult result in newResults)
                    {
                        searchContext.Results.Add(new Result
                        {
                            Link = result.Link,
                            Name = result.Name
                        });
                    }
                    searchContext.ChangeTracker.DetectChanges();
                    searchContext.SaveChanges();
                }
            }
        }   
    }
}
