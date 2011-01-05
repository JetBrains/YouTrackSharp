#region Settings
#pragma warning disable 169
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedMember.Local
#endregion

namespace YouTrackSharp.Specs.Helpers
{
    #region Using Directives

    using Machine.Specifications;
    using YouTrackSharp.Issues;

    #endregion

    public class AuthenticatedYouTrackConnectionForIssueSpecsSetup : AuthenticatedYouTrackConnectionSetup
    {
        protected static IssueManagement issueManagement;
        
        Establish context = () =>
        {
            issueManagement = new IssueManagement(connection);
        };
    }
}