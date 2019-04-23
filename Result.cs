using System;

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

        public override bool Equals(object obj) => obj is SearchResult ? Equals(obj as SearchResult) : false;

        public override int GetHashCode() => (Link, Name).GetHashCode();
    }
}
