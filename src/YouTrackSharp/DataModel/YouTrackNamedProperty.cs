namespace YouTrackSharp.DataModel
{
    public class YouTrackNamedProperty
    {
        public string Name { get; set; }

        public bool Equals(YouTrackNamedProperty other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(other.Name, Name);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof(YouTrackNamedProperty)) return false;
            return Equals((YouTrackNamedProperty)obj);
        }

        public override int GetHashCode()
        {
            return (Name != null ? Name.GetHashCode() : 0);
        }

        public override string ToString()
        {
            return Name ?? "NULL";
        }
    }
}
