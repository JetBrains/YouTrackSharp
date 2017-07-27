using System;

namespace YouTrackSharp
{
    /// <summary>
    /// Represents an exception thrown when the YouTrack server returns an error.
    /// </summary>
    public class YouTrackErrorException 
        : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:YouTrackSharp.YouTrackErrorException" /> class.
        /// </summary>
        public YouTrackErrorException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:YouTrackSharp.YouTrackErrorException" /> class
        /// with a specific message, HTTP status code and HTTP response body.
        /// </summary>
        /// <param name="message">A message that describes the current exception.</param>
        public YouTrackErrorException(string message) 
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:YouTrackSharp.YouTrackErrorException" /> class
        /// with a specific message, HTTP status code and HTTP response body.
        /// </summary>
        /// <param name="message">A message that describes the current exception.</param>
        /// <param name="innerException">The inner <see cref="T:Sytem.Exception"/>.</param>
        public YouTrackErrorException(string message, Exception innerException) 
            : base(message, innerException)
        {
        }
    }
}