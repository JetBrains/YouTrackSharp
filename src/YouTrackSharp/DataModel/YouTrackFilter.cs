using System;

namespace YouTrackSharp.DataModel
{
    [Serializable]
    public class YouTrackFilter
    {
        public string Name { get; set; }
        public string Query { get; set; }

        public bool Equals(YouTrackFilter other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(other.Name, Name) && Equals(other.Query, Query);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof (YouTrackFilter)) return false;
            return Equals((YouTrackFilter) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((Name != null ? Name.GetHashCode() : 0)*397) ^ (Query != null ? Query.GetHashCode() : 0);
            }
        }
    }
}
