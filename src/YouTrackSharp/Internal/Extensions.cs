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

        public static DateTime TimestampToDateTime(this long timestamp)
        {
            return new DateTime(timestamp);
        }

        public static TimeSpan MinutesToTimeSpan(this int? minutes)
        {
            return new TimeSpan(0, minutes ?? 0, 0);
        }

        public static long DateTimeToUnixTimestamp(this DateTime? dateTime)
        {
            return (new DateTimeOffset(dateTime ?? DateTime.Now)).ToUnixTimeMilliseconds();
        }
    }
}