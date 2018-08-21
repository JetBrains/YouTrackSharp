using System.Dynamic;

namespace YouTrackSharp.Issues.Interfaces
{
    public interface IIssue
    {
        dynamic AsDynamic();
        Field GetField(string fieldName);
        void SetField(string fieldName, object value);
        bool TryGetMember(GetMemberBinder binder, out object result);
        bool TrySetMember(SetMemberBinder binder, object value);
    }
}