using System;
using JsonFx.Json;

namespace YouTrackSharp.Admin
{
    public class Version
    {
        private static readonly DateTime _epoch = new DateTime(1970, 1, 1);

        public string Value { get; set; }
        public Int64? ReleaseDate { get; set; }
        public bool Released { get; set; }
        public bool Archived { get; set; }

        [JsonIgnore]
        public DateTime? ReleaseDateTime
        {
            get
            {
                if (ReleaseDate == null)
                {
                    return null;
                }

                return _epoch + TimeSpan.FromMilliseconds(ReleaseDate.Value);
            }

            set
            {
                if (value == null)
                {
                    ReleaseDate = null;
                }

                ReleaseDate = (Int64)(value.Value - _epoch).TotalMilliseconds;
            }
        }
    }
}