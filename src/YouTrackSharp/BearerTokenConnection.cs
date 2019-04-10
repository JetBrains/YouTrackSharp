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

        private Action<HttpClientHandler> _configureHandler;

        /// <summary>
        /// Creates an instance of the <see cref="BearerTokenConnection" /> class.
        /// </summary>
        /// <param name="serverUrl">YouTrack server instance URL that will be connected against.</param>
        /// <param name="bearerToken">The bearer token to inject into HTTP request headers.</param>
        /// <exception cref="ArgumentException">
        /// The <paramref name="serverUrl" /> was null, empty  or did not represent a valid, absolute <see cref="T:System.Uri" />.
        /// </exception>
        public BearerTokenConnection(string serverUrl, string bearerToken)
            : this(serverUrl, bearerToken, null)
        {
        }

        /// <summary>
        /// Creates an instance of the <see cref="BearerTokenConnection" /> class.
        /// </summary>
        /// <param name="serverUrl">YouTrack server instance URL that will be connected against.</param>
        /// <param name="bearerToken">The bearer token to inject into HTTP request headers.</param>
        /// <param name="configureHandler">An action that configures the underlying <see cref="T:System.Net.HttpClientHandler" />, e.g. to override SSL settings.</param>
        /// <exception cref="ArgumentException">
        /// The <paramref name="serverUrl" /> was null, empty  or did not represent a valid, absolute <see cref="T:System.Uri" />.
        /// </exception>
        public BearerTokenConnection(string serverUrl, string bearerToken, Action<HttpClientHandler> configureHandler)
            : base(serverUrl)
        {
            _bearerToken = bearerToken;
            _configureHandler = configureHandler;
        }

        /// <inheritdoc />
        public override async Task<HttpClient> GetAuthenticatedHttpClient()
        {
            // Initialize HTTP client
            if (_httpClient == null)
            {
                var handler = new BearerTokenHttpClientHandler(_bearerToken);

                _configureHandler?.Invoke(handler);

                _httpClient = new HttpClient(handler)
                {
                    BaseAddress = ServerUri
                };

                _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(Constants.HttpContentTypes.ApplicationJson));
            }
            
            // Authenticate?
            if (!_authenticated)
            {
                var response = await _httpClient.GetAsync("api/admin/users/me");
                if (response.IsSuccessStatusCode)
                {
                    _authenticated = true;
                }
                else 
                {
                    var responseString = await response.Content.ReadAsStringAsync();
                    
                    throw new UnauthorizedConnectionException(
                        Strings.Exception_CouldNotAuthenticate, response.StatusCode, responseString);
                }
            }
            
            return _httpClient;
        }
    }
}