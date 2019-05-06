using System.Net;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Linq;
using System.Linq;
using System.Configuration;
using System.IO;
using static CustomSearch.CustomRequestResponse;

namespace CustomSearch
{
    public class YandexWebSearcher : IWebSearcher
    {
        private string subscriptionKey = ConfigurationManager.AppSettings["yandexSubscriptionKey"];
        private string yandexLogin = ConfigurationManager.AppSettings["yandexUser"];
        private string template = @"https://yandex.com/search/xml?query={0}&l10n=en&user={1}&key={2}&count={3}";

        IHttpWebRequest requestI;

        public YandexWebSearcher(IHttpWebRequest requestI = null)
        {
            this.requestI = requestI;
        }

        public SearchResult[] Search(string keyword, int resultCount)
        {
            string completeUrl = string.Format(template, keyword, yandexLogin, subscriptionKey, resultCount);

            IHttpWebRequest newRequest = requestI ?? new WrapHttpWebRequest((HttpWebRequest)WebRequest.Create(completeUrl));
            using (IHttpWebResponse response = newRequest.GetResponse() )
            {
                Stream stream = response.GetResponseStream();
                return XmlList(stream);
            }           
        }

        private SearchResult[] XmlList(Stream stream)
        {
            XDocument xDoc = XDocument.Load(XmlReader.Create(stream));

            IEnumerable<XElement> query = xDoc.Descendants().Descendants("response").Descendants("results").Descendants("grouping")
                                          .Descendants("group").Descendants("doc");

            return query.Select(u => new SearchResult { Link = u.Element("url").Value.Trim(), Name = u.Element("title").Value.Trim() }).ToArray();
        }
    }
}