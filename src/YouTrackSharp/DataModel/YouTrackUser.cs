using System;

namespace YouTrackSharp.DataModel
{
  [Serializable]
  public class YouTrackUser
  {
    public string UserName { get; set; }
    public string FullName { get; set; }

    public override string ToString()
    {
      return FullName;
    }

    public bool Equals(YouTrackUser other)
    {
      if (ReferenceEquals(null, other)) return false;
      if (ReferenceEquals(this, other)) return true;
      return Equals(other.UserName, UserName) && Equals(other.FullName, FullName);
    }

    public override bool Equals(object obj)
    {
      if (ReferenceEquals(null, obj)) return false;
      if (ReferenceEquals(this, obj)) return true;
      if (obj.GetType() != typeof(YouTrackUser)) return false;
      return Equals((YouTrackUser)obj);
    }

    public override int GetHashCode()
    {
      unchecked
      {
        return ((UserName != null ? UserName.GetHashCode() : 0) * 397) ^ (FullName != null ? FullName.GetHashCode() : 0);
      }
    }
  }
}
