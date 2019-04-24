using System.Linq;
using Google.Apis.Customsearch.v1;
using Google.Apis.Customsearch.v1.Data;
using Google.Apis.Services;
using System.Configuration;

namespace CustomSearch
{
    class GoogleWebSearcher : IWebSearcher
    {
        private string subscriptionKey = ConfigurationManager.AppSettings["googleSubscriptionKey"];
        private string customSearchEngineID = ConfigurationManager.AppSettings["googleSearchEngineId"];

        public SearchResult[] Search(string keyword, int resultCount)
        {
            using (CustomsearchService Service = new CustomsearchService(
            new BaseClientService.Initializer
            {
                ApiKey = subscriptionKey,
            }
            )) {
                CseResource.ListRequest listRequest = Service.Cse.List(keyword);
                listRequest.Cx = customSearchEngineID;
                listRequest.Num = resultCount;
                Search search = listRequest.Execute();

                return search.Items?.Select(item => new SearchResult { Link = item.Link, Name = item.Title}).ToArray();
            }               
        }
    }
}
