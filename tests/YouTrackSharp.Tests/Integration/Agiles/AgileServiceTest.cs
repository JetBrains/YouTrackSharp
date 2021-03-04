using System.IO;
using System.Reflection;

namespace YouTrackSharp.Tests.Integration.Agiles
{
    public partial class AgileServiceTest
    {
        private static string DemoBoardId => "108-2";
        private static string DemoBoardNamePrefix => "Test Board597fb561-ea1f-4095-9636-859ae4439605";

        private static string DemoSprintId => "109-2";
        private static string DemoSprintName => "First sprint";

        private static string CompleteAgileJson => GetTextResource("YouTrackSharp.Tests.Resources.CompleteAgile.json");

        private static string GetTextResource(string name)
        {
            Assembly assembly = Assembly.GetExecutingAssembly();

            using Stream stream = assembly.GetManifestResourceStream(name);
            if (stream == null)
            {
                return string.Empty;
            }

            using StreamReader reader = new StreamReader(stream);

            return reader.ReadToEnd();
        }
    }
}