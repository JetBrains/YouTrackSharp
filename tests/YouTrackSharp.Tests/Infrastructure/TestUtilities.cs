using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace YouTrackSharp.Tests.Infrastructure
{
    public static class TestUtilities
    {
        public static async Task<Stream> GenerateAttachmentStream(string contents)
        {
            var stream = new MemoryStream();
            using (var streamWriter = new StreamWriter(stream, Encoding.UTF8, 4096, leaveOpen: true))
            {
                await streamWriter.WriteAsync(contents);
            }

            stream.Position = 0;

            return stream;
        }
    }
}