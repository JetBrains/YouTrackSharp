using System;
using System.Net.Http;

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

        /// <summary>
        /// Initializes a new instance of the <see cref="T:YouTrackSharp.YouTrackErrorException" /> class
        /// with a specific message and <see cref="T:System.Net.Http.HttpResponseMessage"/>.
        /// </summary>
        /// <param name="message">A message that describes the current exception.</param>
        /// <param name="response">The <see cref="T:System.Net.Http.HttpResponseMessage"/> that was received from the YouTrack server.</param>
        public YouTrackErrorException(string message, HttpResponseMessage response) 
            : base(message)
        {
            Response = response;
        }
        
        /// <summary>
        /// The <see cref="T:System.Net.Http.HttpResponseMessage"/> that was received from the YouTrack server.
        /// </summary>
        public HttpResponseMessage Response { get; }
    }
}