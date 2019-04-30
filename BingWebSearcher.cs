using Newtonsoft.Json;
using System.Text;
using System.Linq;
using System.Configuration;
using System.Net.Http;


namespace CustomSearch
{
    public class BingWebSearcher : IWebSearcher
    {
        private readonly string subscriptionKey = ConfigurationManager.AppSettings["bingSubscriptionKey"];
        private readonly string customConfigurationID = ConfigurationManager.AppSettings["bingCustomConfig"];
        private readonly string template = @"https://api.cognitive.microsoft.com/bingcustomsearch/v7.0/search?q={0}&customconfig={1}&count={2}&offset=0";

        HttpClient httpClient;
        public BingWebSearcher(HttpClient httpClient = null)
        {
            this.httpClient = httpClient ?? new HttpClient();
        }

        public SearchResult[] Search(string keyword, int resultCount)
        {           
            string url = string.Format(template, keyword, customConfigurationID, resultCount);
            using (var client = httpClient)
            {
                client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", subscriptionKey);
                using (HttpResponseMessage httpResponseMessage = client.GetAsync(url).Result)
                {             
                    string responseContent = httpResponseMessage.Content.ReadAsStringAsync().Result;
                    RootObject result = JsonConvert.DeserializeObject<RootObject>(responseContent);
                    return result?.webPages.value.Select(item => new SearchResult { Link = item.url, Name = item.name }).ToArray();
                }
            }                    
        }


        private string Win1251ToUTF8(string str)
        {
            byte[] winArr = Encoding.GetEncoding(1251).GetBytes(str);
            return Encoding.UTF8.GetString(winArr);
        }
    }
}
