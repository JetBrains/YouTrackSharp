using System;
using System.Threading.Tasks;
using YouTrackSharp.Generated;

namespace YouTrackSharp
{
    /// <summary>
    /// Abstract base class that represents a connection against a YouTrack server instance and provides
    /// an authenticated <see cref="T:System.Net.Http.HttpClient" />.
    /// </summary>
    public abstract class Connection
    {
        /// <summary>
        /// YouTrack server instance URI that will be connected against.
        /// </summary>
        protected Uri ServerUri { get; }
        
        /// <summary>
        /// Creates an instance of the <see cref="Connection" /> class.
        /// </summary>
        /// <param name="serverUrl">YouTrack server instance URL that will be connected against.</param>
        /// <exception cref="ArgumentException">
        /// The <paramref name="serverUrl" /> was null, empty or did not represent a valid,
        /// absolute <see cref="T:System.Uri" />.
        /// </exception>
        protected Connection(string serverUrl)
        {
            if (string.IsNullOrEmpty(serverUrl)
                || !Uri.TryCreate(EnsureTrailingSlash(serverUrl), UriKind.Absolute, out var serverUri))
            {
                throw new ArgumentException(Strings.ServerUrlIsInvalid, nameof(serverUrl));
            }

            ServerUri = serverUri;
        }
        
        /// <summary>
        /// Ensures a trailing slash is present for the given string.
        /// </summary>
        /// <param name="url">URL represented as a <see cref="T:System.String" /></param>
        /// <returns>A <see cref="T:System.String" /> with traling slash based on <paramref name="url" />.</returns>
        private static string EnsureTrailingSlash(string url)
        {
            if (!url.EndsWith("/"))
            {
                return url + "/";
            }

            return url;
        }

        /// <summary>
        /// Provides an authenticated <see cref="T:System.Net.Http.HttpClient" />
        /// which can be used by REST API client implementations.
        /// </summary>
        /// <returns>An authenticated <see cref="T:System.Net.Http.HttpClient" />.</returns>
        /// <exception cref="UnauthorizedConnectionException">The connection could not be authenticated.</exception>
        public abstract Task<YouTrackClient> GetAuthenticatedApiClient();
    }
}