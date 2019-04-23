using System.Collections.Generic;

namespace CustomSearch
{
    interface IWebSearcher
    {
       SearchResult[] Search(string keyword, int resultCount);
    }
}
