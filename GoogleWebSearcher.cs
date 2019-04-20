using System.Collections.Generic;
using System.Linq;
using Google.Apis.Customsearch.v1;
using Google.Apis.Customsearch.v1.Data;
using Google.Apis.Services;


namespace CustomSearch
{
    class GoogleWebSearcher : WebSearcher
    {
        private string googleSubscriptionKey = System.Configuration.ConfigurationManager.AppSettings["googleSubscriptionKey"];
        private string googleSearchEngineId = System.Configuration.ConfigurationManager.AppSettings["googleSearchEngineId"];
        private string googleAppName = System.Configuration.ConfigurationManager.AppSettings["googleAppName"];

        public override List<SearchResult> Search(string keyword, int resultCount)
        {
            using (CustomsearchService Service = new CustomsearchService(
            new BaseClientService.Initializer
            {
                ApplicationName = googleAppName,
                ApiKey = googleSubscriptionKey,
            }
            )) {
                CseResource.ListRequest listRequest = Service.Cse.List(keyword);
                listRequest.Cx = googleSearchEngineId;
                listRequest.Num = resultCount;
                Search search = listRequest.Execute();

                return search.Items?.Select(item => new SearchResult(item.Link, item.Title)).ToList();
            }               
        }
    }
}
