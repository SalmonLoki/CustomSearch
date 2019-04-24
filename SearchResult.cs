using System;

namespace CustomSearch
{
    public class SearchResult : IEquatable<SearchResult>
    {
        public string Link { get; set; }
        public string Name { get; set; }

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
