using HtmlAgilityPack;
using Newtonsoft.Json;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Configuration;


namespace CustomSearch
{
    class BingWebSearcher : IWebSearcher
    {
        private string subscriptionKey = ConfigurationManager.AppSettings["bingSubscriptionKey"];
        private string customConfigurationID = ConfigurationManager.AppSettings["bingCustomConfig"];
        private string template = @"https://api.cognitive.microsoft.com/bingcustomsearch/v7.0/search?q={0}&customconfig={1}&count={2}&offset=0";

        public List<SearchResult> Search(string keyword, int resultCount)
        {
            string JsonString = null;
            string url = string.Format(template, keyword, customConfigurationID, resultCount);

            using (var client = new WebClient())
            {
                client.Headers["Ocp-Apim-Subscription-Key"] = subscriptionKey;

                HtmlDocument doc = new HtmlDocument();
                doc.LoadHtml(client.DownloadString(url));
                JsonString = doc.DocumentNode.InnerText;
            }

            Rootobject result = JsonConvert.DeserializeObject<Rootobject>(JsonString);
            return result?.webPages.value.Select(item => new SearchResult(item.url, winToUtf(item.name))).ToList();
        }

        private string winToUtf(string str)
        {
            byte[] winArr = Encoding.GetEncoding(1251).GetBytes(str);
            return Encoding.UTF8.GetString(winArr);
        }
    }
}
