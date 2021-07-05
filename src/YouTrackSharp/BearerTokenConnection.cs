using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using JetBrains.Annotations;
using YouTrackSharp.Internal;

namespace YouTrackSharp
{
    /// <summary>
    /// A class that represents a connection against a YouTrack server instance and provides an authenticated
    /// <see cref="T:System.Net.Http.HttpClient" /> that uses a bearer token.
    /// </summary>
    [PublicAPI]
    public class BearerTokenConnection 
        : Connection
    {
        private HttpClient _httpClient;
        private YouTrackClient _youtrackAPI;
        private bool _authenticated;
        
        private readonly string _bearerToken;

        private readonly Action<HttpClientHandler> _configureHandler;
        
        private TimeSpan _timeout = TimeSpan.FromSeconds(100);

        /// <summary>
        /// Creates an instance of the <see cref="BearerTokenConnection" /> class.
        /// </summary>
        /// <param name="serverUrl">YouTrack server instance URL that will be connected against.</param>
        /// <param name="bearerToken">The bearer token to inject into HTTP request headers.</param>
        /// <param name="configureHandler">An action that configures the underlying <see cref="T:System.Net.HttpClientHandler" />, e.g. to override SSL settings.</param>
        /// <exception cref="ArgumentException">
        /// The <paramref name="serverUrl" /> was null, empty  or did not represent a valid, absolute <see cref="T:System.Uri" />.
        /// </exception>
        public BearerTokenConnection(string serverUrl, string bearerToken, Action<HttpClientHandler> configureHandler = null)
            : base(serverUrl)
        {
            _bearerToken = bearerToken;
            _configureHandler = configureHandler;
        }
        
        /// <summary>
        /// Gets or sets the timespan to wait before the request times out.
        /// </summary>
        /// <remarks>
        /// The default value is 100,000 milliseconds (100 seconds).
        /// </remarks>
        public TimeSpan Timeout 
        {
            get => _httpClient?.Timeout ?? _timeout;
            set
            {
                _timeout = value;
                if (_httpClient != null)
                {
                    _httpClient.Timeout = _timeout;
                }
            }
        } 

        /// <inheritdoc />
        public override async Task<YouTrackClient> GetAuthenticatedAPIClient()
        {
            // Initialize HTTP client
            if (_httpClient == null)
            {
                var handler = new BearerTokenHttpClientHandler(_bearerToken);

                _configureHandler?.Invoke(handler);

                _httpClient = new HttpClient(handler)
                {
                    BaseAddress = ServerUri,
                    Timeout = _timeout
                };

                _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(Constants.HttpContentTypes.ApplicationJson));
            }
            
            // Authenticate?
            if (_authenticated)
            {
                return _httpClient;
            }
            
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

            return _httpClient;
        }
    }
}