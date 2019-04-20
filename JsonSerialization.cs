using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomSearch
{
    public class Rootobject
    {
        public string _type { get; set; }
        public Webpages webPages { get; set; }
        public Rankingresponse rankingResponse { get; set; }
    }

    public class Webpages
    {
        public string webSearchUrl { get; set; }
        public int totalEstimatedMatches { get; set; }
        public Value[] value { get; set; }
    }

    public class Value
    {
        public string id { get; set; }
        public string name { get; set; }
        public string url { get; set; }
        public string displayUrl { get; set; }
        public string snippet { get; set; }
        public DateTime dateLastCrawled { get; set; }
    }

    public class Rankingresponse
    {
        public Mainline mainline { get; set; }
    }

    public class Mainline
    {
        public Item[] items { get; set; }
    }

    public class Item
    {
        public string answerType { get; set; }
        public int resultIndex { get; set; }
        public Value1 value { get; set; }
    }

    public class Value1
    {
        public string id { get; set; }
    }
}
