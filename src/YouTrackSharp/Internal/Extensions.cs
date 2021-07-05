using System;
using System.IO;
using System.Text;

namespace YouTrackSharp.Internal
{
    internal static class Extensions
    {
        public static string ConvertToBase64(this Stream stream)
        {
            using var memoryStream = new MemoryStream();
            stream.CopyTo(memoryStream);
            var bytes = memoryStream.ToArray();

            return Convert.ToBase64String(bytes);
        }
    }
}