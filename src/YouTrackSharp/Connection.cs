using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

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
                throw new ArgumentException("The server URL is invalid. Please provide a valid URL to a self-hosted YouTrack instance or YouTrack InCloud.", nameof(serverUrl));
            }

            ServerUri = serverUri;
        }
        
        /// <summary>
        /// Ensures a trailing slash is present for the given string.
        /// </summary>
        /// <param name="url">URL represented as a <see cref="T:System.String" /></param>
        /// <returns>A <see cref="T:System.String" /> with traling slash based on <paramref name="url" />.</returns>
        protected static string EnsureTrailingSlash(string url)
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
        public abstract Task<HttpClient> GetAuthenticatedHttpClient();
        
        /// <summary>
        /// Gets the build number of the YouTrack server instance.
        /// </summary>
        /// <returns>Build number of the YouTrack server instance. When the YouTrack server does not support returning its build number, the value <value>-1</value> will be returned.</returns>
        /// <exception cref="T:System.Net.HttpRequestException">When the call to the remote YouTrack server instance failed.</exception>
        public async Task<int> GetBuildNumber()
        {
            var client = await GetAuthenticatedHttpClient();
            var response = await client.GetAsync($"api/config?fields=build");

            if (response.StatusCode == HttpStatusCode.NotFound || response.StatusCode == HttpStatusCode.BadRequest)
            {
                return -1;
            }

            response.EnsureSuccessStatusCode();
            
            var responseJson = JObject.Parse(await response.Content.ReadAsStringAsync());
            if (responseJson["build"] != null)
            {
                return responseJson["build"].Value<int>();
            }
            
            return -1;
        }
    }
}