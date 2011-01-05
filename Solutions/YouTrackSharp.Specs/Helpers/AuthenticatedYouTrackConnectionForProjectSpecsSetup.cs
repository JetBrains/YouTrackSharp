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
    using YouTrackSharp.Projects;

    #endregion

    public class AuthenticatedYouTrackConnectionForProjectSpecsSetup : AuthenticatedYouTrackConnectionSetup
    {
        protected static ProjectManagement projectManagement;
        
        Establish context = () =>
        {
            projectManagement = new ProjectManagement(connection);
        };
    }
}