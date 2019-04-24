using System.Net;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Linq;
using System.Linq;
using System.Configuration;

namespace CustomSearch
{
    class YandexWebSearcher : IWebSearcher
    {
        private string subscriptionKey = ConfigurationManager.AppSettings["yandexSubscriptionKey"];
        private string yandexLogin = ConfigurationManager.AppSettings["yandexUser"];
        private string template = @"https://yandex.com/search/xml?query={0}&l10n=en&user={1}&key={2}&count={3}";

        public SearchResult[] Search(string keyword, int resultCount)
        {
            string completeUrl = string.Format(template, keyword, yandexLogin, subscriptionKey, resultCount);

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(completeUrl);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            return xmlList(response);
        }

        private SearchResult[] xmlList(HttpWebResponse response)
        {
            XDocument xDoc = XDocument.Load(XmlReader.Create(response.GetResponseStream()));

            IEnumerable<XElement> query = xDoc.Descendants().Descendants("response").Descendants("results").Descendants("grouping")
                                          .Descendants("group").Descendants("doc");

            return query.Select(u => new SearchResult { Link = u.Element("url").Value, Name = u.Element("title").Value }).ToArray();
        }
    }
}
