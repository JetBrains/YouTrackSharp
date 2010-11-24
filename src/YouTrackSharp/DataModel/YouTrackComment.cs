using System;

namespace YouTrackSharp.DataModel
{
  [Serializable]
  public class YouTrackComment
  {
    public string Author { get; set; }
    public DateTime Created { get; set; }
    public bool Deleted { get; set; }
    public string IssueKey { get; set; }
    public string Text { get; set; }

    public bool Equals(YouTrackComment other)
    {
      if (ReferenceEquals(null, other)) return false;
      if (ReferenceEquals(this, other)) return true;
      return Equals(other.Author, Author) && other.Created.Equals(Created) && other.Deleted.Equals(Deleted) && Equals(other.IssueKey, IssueKey) && Equals(other.Text, Text);
    }

    public override bool Equals(object obj)
    {
      if (ReferenceEquals(null, obj)) return false;
      if (ReferenceEquals(this, obj)) return true;
      if (obj.GetType() != typeof(YouTrackComment)) return false;
      return Equals((YouTrackComment)obj);
    }

    public override int GetHashCode()
    {
      unchecked
      {
        int result = (Author != null ? Author.GetHashCode() : 0);
        result = (result * 397) ^ Created.GetHashCode();
        result = (result * 397) ^ Deleted.GetHashCode();
        result = (result * 397) ^ (IssueKey != null ? IssueKey.GetHashCode() : 0);
        result = (result * 397) ^ (Text != null ? Text.GetHashCode() : 0);
        return result;
      }
    }
  }
}
