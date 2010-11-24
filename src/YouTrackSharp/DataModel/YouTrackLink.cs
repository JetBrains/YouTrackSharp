using System;

namespace YouTrackSharp.DataModel
{
    public enum LinkDirection
    {
        Inward,
        Outward
    }

    [Serializable]
    public class YouTrackLink
    {
        public string LinkName { get; set; }
        public LinkDirection Direction { get; set; }
        public string DirectionDescription { get; set; } //Textual description of link direction (e.g. 'duplicates' or 'is duplicated by' depending on the direction
        public string SourceKey { get; set; }
        public string DestinationKey { get; set; }

        public bool Equals(YouTrackLink other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(other.LinkName, LinkName) && Equals(other.Direction, Direction) && Equals(other.DirectionDescription, DirectionDescription) && Equals(other.SourceKey, SourceKey) && Equals(other.DestinationKey, DestinationKey);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof (YouTrackLink)) return false;
            return Equals((YouTrackLink) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int result = (LinkName != null ? LinkName.GetHashCode() : 0);
                result = (result*397) ^ Direction.GetHashCode();
                result = (result*397) ^ (DirectionDescription != null ? DirectionDescription.GetHashCode() : 0);
                result = (result*397) ^ (SourceKey != null ? SourceKey.GetHashCode() : 0);
                result = (result*397) ^ (DestinationKey != null ? DestinationKey.GetHashCode() : 0);
                return result;
            }
        }
    }
}