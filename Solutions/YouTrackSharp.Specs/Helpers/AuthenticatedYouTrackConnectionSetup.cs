#region Settings
#pragma warning disable 169
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedMember.Local
#endregion

namespace YouTrackSharp.Specs.Helpers
{
    using Machine.Specifications;

    public class AuthenticatedYouTrackConnectionSetup : YouTrackConnectionSetup
    {
        Establish context = () =>
        {
            connection.Authenticate("youtrackapi", "youtrackapi");
        };
    }
}