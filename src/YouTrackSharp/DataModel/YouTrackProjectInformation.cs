using System;

namespace YouTrackSharp.DataModel
{
  [Serializable]
  public class YouTrackProjectInformation
  {
    public string ProjectName { get; set; }
    public string ProjectKey { get; set; }
    public YouTrackUser[] Assignees { get; set; }
    public string[] Versions { get; set; }
    public string[] Subsystems { get; set; }

    public bool Equals(YouTrackProjectInformation other)
    {
      if (ReferenceEquals(null, other)) return false;
      if (ReferenceEquals(this, other)) return true;
      return Equals(other.ProjectName, ProjectName) && Equals(other.ProjectKey, ProjectKey) && Equals(other.Assignees, Assignees) && Equals(other.Versions, Versions) && Equals(other.Subsystems, Subsystems);
    }

    public override bool Equals(object obj)
    {
      if (ReferenceEquals(null, obj)) return false;
      if (ReferenceEquals(this, obj)) return true;
      if (obj.GetType() != typeof(YouTrackProjectInformation)) return false;
      return Equals((YouTrackProjectInformation)obj);
    }

    public override int GetHashCode()
    {
      unchecked
      {
        int result = (ProjectName != null ? ProjectName.GetHashCode() : 0);
        result = (result * 397) ^ (ProjectKey != null ? ProjectKey.GetHashCode() : 0);
        result = (result * 397) ^ (Assignees != null ? Assignees.GetHashCode() : 0);
        result = (result * 397) ^ (Versions != null ? Versions.GetHashCode() : 0);
        result = (result * 397) ^ (Subsystems != null ? Subsystems.GetHashCode() : 0);
        return result;
      }
    }
  }
}
