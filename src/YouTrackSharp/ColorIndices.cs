using JetBrains.Annotations;

namespace YouTrackSharp
{
    /// <summary>
    /// Represents the color indices used by YouTrack, as explained on <a href="https://www.jetbrains.com/help/youtrack/devportal/Color-Indices.html">Color Indices List</a>.
    /// </summary>
    [PublicAPI]
    public static class ColorIndices
    {
        /// <summary>
        /// Supported color indices. The array index corresponds to the documented <a href="https://www.jetbrains.com/help/youtrack/devportal/Color-Indices.html">color index</a>.
        /// </summary>
        public static readonly YouTrackColor[] Colors = {
            new YouTrackColor { Foreground = "#444", Background = "#fff" },
            new YouTrackColor { Foreground = "#fff", Background = "#8d5100" },
            new YouTrackColor { Foreground = "#fff", Background = "#ce6700" },
            new YouTrackColor { Foreground = "#fff", Background = "#409600" },
            new YouTrackColor { Foreground = "#fff", Background = "#0070e4" },
            new YouTrackColor { Foreground = "#fff", Background = "#900052" },
            new YouTrackColor { Foreground = "#fff", Background = "#0050a1" },
            new YouTrackColor { Foreground = "#fff", Background = "#2f9890" },
            new YouTrackColor { Foreground = "#fff", Background = "#8e1600" },
            new YouTrackColor { Foreground = "#fff", Background = "#dc0083" },
            new YouTrackColor { Foreground = "#fff", Background = "#7dbd36" },
            new YouTrackColor { Foreground = "#fff", Background = "#ff7123" },
            new YouTrackColor { Foreground = "#fff", Background = "#ff7bc3" },
            new YouTrackColor { Foreground = "#444", Background = "#fed74a" },
            new YouTrackColor { Foreground = "#444", Background = "#b7e281" },
            new YouTrackColor { Foreground = "#45818e", Background = "#d8f7f3" },
            new YouTrackColor { Foreground = "#888", Background = "#e6e6e6" },
            new YouTrackColor { Foreground = "#4da400", Background = "#e6f6cf" },
            new YouTrackColor { Foreground = "#b45f06", Background = "#ffee9c" },
            new YouTrackColor { Foreground = "#444", Background = "#ffc8ea" },
            new YouTrackColor { Foreground = "#fff", Background = "#e30000" },
            new YouTrackColor { Foreground = "#3d85c6", Background = "#e0f1fb" },
            new YouTrackColor { Foreground = "#dc5766", Background = "#fce5f1" },
            new YouTrackColor { Foreground = "#b45f06", Background = "#f7e9c1" },
            new YouTrackColor { Foreground = "#444", Background = "#92e1d5" },
            new YouTrackColor { Foreground = "#444", Background = "#a6e0fc" },
            new YouTrackColor { Foreground = "#444", Background = "#e0c378" },
            new YouTrackColor { Foreground = "#444", Background = "#bababa" },
            new YouTrackColor { Foreground = "#fff", Background = "#25beb2" },
            new YouTrackColor { Foreground = "#fff", Background = "#42a3df" },
            new YouTrackColor { Foreground = "#fff", Background = "#878787" },
            new YouTrackColor { Foreground = "#fff", Background = "#4d4d4d" },
            new YouTrackColor { Foreground = "#fff", Background = "#246512" },
            new YouTrackColor { Foreground = "#fff", Background = "#00665e" },
            new YouTrackColor { Foreground = "#fff", Background = "#553000" },
            new YouTrackColor { Foreground = "#fff", Background = "#1a1a1a" }
        };
    }
}