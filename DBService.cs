using System;
using System.Collections.Generic;
using System.Linq;


namespace CustomSearch
{
    public class DBService
    {
        public SearchResult[] getOldResultsFromDB(SearchContext searchContextArg = null)
        {
            using (SearchContext searchContext = searchContextArg ?? new SearchContext())
            {
                return searchContext.Results
                .AsNoTracking()
                .Select(u => new SearchResult
                {
                    Link = u.Link,
                    Name =u.Name
                }).ToArray();
            }
        }

        public SearchResult[] searchInDB(string keyword, SearchContext searchContextArg = null)
        {
            using (SearchContext searchContext = searchContextArg ?? new SearchContext())
            {
                if (keyword.Length == 0) {
                    throw new System.ArgumentNullException();
                } else
                return  searchContext.Results
                .AsNoTracking()
                .Where(u => u.Link.Contains(keyword) | u.Name.Contains(keyword))
                .Select(u => new SearchResult
                {
                    Link = u.Link,
                    Name = u.Name
                }).ToArray();
            }
        }

        public string updateDataInDB(SearchResult[] oldResults, SearchResult[] newResults,
            SearchContext searchContextArg = null)
        {
            string textBoxText;
            if (oldResults.SequenceEqual(newResults))
            {
                textBoxText = "Данные не устарели";
            }
            else
            {
                textBoxText = "Данные устарели...Обновление данных. ";

                using (SearchContext searchContext = searchContextArg ?? new SearchContext())
                {
                    searchContext.ExecuteSqlCommand("TRUNCATE TABLE Results");
                    searchContext.SaveChanges();

                    searchContext.AutoDetectChangesEnabled = false;
                    foreach (SearchResult result in newResults)
                    {
                        searchContext.Results.Add(new Result
                        {
                            Link = result.Link,
                            Name = result.Name
                        });
                    }
                    searchContext.DetectChanges();
                    searchContext.SaveChanges();
                }
            }
            return textBoxText;
        }
    }
}
