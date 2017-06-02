using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using YouTrackSharp.Internal;

namespace YouTrackSharp
{
    /// <summary>
    /// A class that represents a connection against a YouTrack server instance and provides an authenticated
    /// <see cref="T:System.Net.Http.HttpClient" /> that uses a bearer token.
    /// </summary>
    public class BearerTokenConnection 
        : Connection
    {
        private HttpClient _httpClient;
        private bool _authenticated;
        
        private readonly string _bearerToken;

        /// <summary>
        /// Creates an instance of the <see cref="BearerTokenConnection" /> class.
        /// </summary>
        /// <param name="serverUrl">YouTrack server instance URL that will be connected against.</param>
        /// <param name="bearerToken">The bearer token to inject into HTTP request headers.</param>
        /// <exception cref="ArgumentException">
        /// The <paramref name="serverUrl" /> was null, empty  or did not represent a valid, absolute <see cref="T:System.Uri" />.
        /// </exception>
        public BearerTokenConnection(string serverUrl, string bearerToken)
            : base(serverUrl)
        {
            _bearerToken = bearerToken;
        }

        /// <inheritdoc />
        public override async Task<HttpClient> GetAuthenticatedHttpClient()
        {
            // Initialize HTTP client
            if (_httpClient == null)
            {
                _httpClient = new HttpClient(new BearerTokenHttpClientHandler(_bearerToken))
                {
                    BaseAddress = ServerUri
                };

                _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            }
            
            // Authenticate?
            if (!_authenticated)
            {
                var response = await _httpClient.GetAsync("rest/user/current");
                if (response.IsSuccessStatusCode)
                {
                    _authenticated = true;
                }
                else 
                {
                    var responseString = await response.Content.ReadAsStringAsync();
                    
                    throw new UnauthorizedConnectionException(
                        "Could not authenticate. Server did not return expected authentication response. Check the Response property for more details.", response.StatusCode, responseString);
                }
            }
            
            return _httpClient;
        }
    }
}