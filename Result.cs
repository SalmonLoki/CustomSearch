using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomSearch
{
    public class SearchResult : System.IEquatable<SearchResult>
    {
        public string Link { get; set; }
        public string Name { get; set; }

        public SearchResult(string link, string name)
        {
            Link = link;
            Name = name;
        }

        public bool Equals(SearchResult other)
        {
            if (other is null)
                return false;


            return this.Name.Equals(other.Name) &&
                this.Link.Equals(other.Link);
        }

        public override bool Equals(object obj) => Equals(obj as SearchResult);
        public override int GetHashCode() => (Link, Name).GetHashCode();
    }
}
