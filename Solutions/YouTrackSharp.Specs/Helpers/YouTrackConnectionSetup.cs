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
    using YouTrackSharp.Infrastructure;

    #endregion

    public class YouTrackConnectionSetup
    {
        protected static Connection connection;

        Establish context = () =>
        {
            connection = new Connection("youtrack.jetbrains.net");
        };
    }
}