using System.Diagnostics.CodeAnalysis;

namespace YouTrackSharp.Tests.Integration.AgileBoards
{
    [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
    public partial class AgileBoardServiceTests
    {
        public static string DemoBoardId => "108-2";
        public static string DemoBoardNamePrefix => "Test Board";

        public static string DemoSprintId => "109-2";
        public static string DemoSprintName => "First sprint";
    }
}