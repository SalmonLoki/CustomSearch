using System.Collections.Generic;

namespace CustomSearch
{
    interface IWebSearcher
    {
        List<SearchResult> Search(string keyword, int resultCount);
    }
}
