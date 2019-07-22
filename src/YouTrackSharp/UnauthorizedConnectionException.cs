using System;
using System.Net;
using JetBrains.Annotations;

namespace YouTrackSharp
{
    /// <summary>
    /// Represents an exception thrown by <see cref="T:YouTrackSharp.Connection" /> when authentication
    /// with the remote YouTrack server fails.
    /// </summary>
    [PublicAPI]
    public class UnauthorizedConnectionException 
        : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:YouTrackSharp.UnauthorizedConnectionException" /> class.
        /// </summary>
        public UnauthorizedConnectionException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:YouTrackSharp.UnauthorizedConnectionException" /> class
        /// with a specific message, HTTP status code and HTTP response body.
        /// </summary>
        /// <param name="message">A message that describes the current exception.</param>
        /// <param name="statusCode">The <see cref="T:System.Net.Http.HttpStatusCode" /> that was received while authenticating against the YouTrack server.</param>
        /// <param name="response">The HTTP response body that was received while authenticating against the YouTrack server.</param>
        public UnauthorizedConnectionException(string message, HttpStatusCode statusCode, string response) 
            : base(message)
        {
            StatusCode = statusCode;
            Response = response;
        }
        
        /// <summary>
        /// The <see cref="T:System.Net.Http.HttpStatusCode" /> that was received while authenticating against the YouTrack server.
        /// </summary>
        public HttpStatusCode StatusCode { get; }
        
        /// <summary>
        /// The HTTP response body that was received while authenticating against the YouTrack server.
        /// </summary>
        public string Response { get; }
    }
}