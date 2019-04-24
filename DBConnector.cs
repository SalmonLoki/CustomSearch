using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace CustomSearch
{
    class DBConnector
    {
        public List<SearchResult> getOldResultsFromDB()
        {
            using (SearchContext searchContext = new SearchContext())
            {
                IQueryable<Result> dbResults = searchContext.Results;
                return dbResults.ToList().Select(u => new SearchResult(u.Link, u.Name)).ToList();
            }
        }

        public List<SearchResult> searchInDB(string keyword)
        {
            using (SearchContext searchContext = new SearchContext())
            {
                IQueryable<Result> dbResults = searchContext.Results.Where(u => u.Link.Contains(keyword) | u.Name.Contains(keyword));
                return dbResults.ToList().Select(u => new SearchResult(u.Link, u.Name)).ToList();
            }
        }

        public void updateDataInDB(List<SearchResult> oldResults, List<SearchResult> newResults, TextBox textBox)
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

                    foreach (SearchResult result in newResults)
                    {
                        searchContext.Results.Add(new Result
                        {
                            Link = result.Link,
                            Name = result.Name
                        });
                    }
                    searchContext.SaveChanges();
                }
            }
        }   
    }
}
