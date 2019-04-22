using System.Net;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Linq;
using System.Linq;
using System.Configuration;

namespace CustomSearch
{
    class YandexWebSearcher : WebSearcher
    {
        private string subscriptionKey = ConfigurationManager.AppSettings["yandexSubscriptionKey"];
        private string yandexLogin = ConfigurationManager.AppSettings["yandexUser"];
        private string template = @"https://yandex.com/search/xml?query={0}&l10n=en&user={1}&key={2}&count={3}";

        public override List<SearchResult> Search(string keyword, int resultCount)
        {
            string completeUrl = string.Format(template, keyword, yandexLogin, subscriptionKey, resultCount);

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(completeUrl);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            return xmlList(response);
        }

        private List<SearchResult> xmlList(HttpWebResponse response)
        {
            XDocument xDoc = XDocument.Load(XmlReader.Create(response.GetResponseStream()));

            var query = from c in xDoc.Descendants().Descendants("response").Descendants("results").Descendants("grouping").Descendants("group")
                        select c;

            List<SearchResult> result = new List<SearchResult>();
            foreach (var item in query)
                result.Add(new SearchResult(item.Element("doc").Element("url").Value, item.Element("doc").Element("title").Value));
            return result;
        }
    }
}
