using System;

namespace YouTrackSharp.DataModel
{
    [Serializable]
    public class YouTrackProjectBuild
    {
        public string Name { get; set; }
        public string AssembleDate { get; set; }

        public YouTrackProjectBuild()
        {
        }

        public YouTrackProjectBuild(string name, string assembleDate)
        {
            Name = name;
            AssembleDate = assembleDate;
        }

        public bool Equals(YouTrackProjectBuild other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(other.Name, Name) && Equals(other.AssembleDate, AssembleDate);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof (YouTrackProjectBuild)) return false;
            return Equals((YouTrackProjectBuild) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((Name != null ? Name.GetHashCode() : 0)*397) ^ (AssembleDate != null ? AssembleDate.GetHashCode() : 0);
            }
        }
    }
}
