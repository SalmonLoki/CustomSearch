using System.Collections.Generic;

namespace CustomSearch
{
    abstract class WebSearcher
    {
        public abstract List<SearchResult> Search(string keyword, int resultCount);
    }
}
