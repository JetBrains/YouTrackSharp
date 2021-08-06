using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using JetBrains.Annotations;
using YouTrackSharp.Generated;
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
        private YouTrackClient _youTrackClient;
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
        public override async Task<HttpClient> GetAuthenticatedRawClient()
        {
            if (_youTrackClient == null)
            {
                await GetAuthenticatedApiClient();
            }

            return _httpClient;
        }

        /// <inheritdoc />
        public override async Task<YouTrackClient> GetAuthenticatedApiClient()
        {
            // Initialize HTTP client
            if (_youTrackClient == null)
            {
                var handler = new BearerTokenHttpClientHandler(_bearerToken);

                _configureHandler?.Invoke(handler);
                
                handler.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;

                _httpClient = new HttpClient(handler)
                {
                    BaseAddress = ServerUri,
                    Timeout = _timeout
                };

                _youTrackClient = new YouTrackClient(_httpClient);
            }
            
            // Authenticate?
            if (_authenticated)
            {
                return _youTrackClient;
            }

            try
            {
                _authenticated = true;
                var response = await _youTrackClient.UsersMeAsync("id,guest");
                if (response.Guest == true || response.Guest == null)
                {
                    throw new UnauthorizedConnectionException(
                        Strings.Exception_CouldNotAuthenticate, (HttpStatusCode)200, "YouTrack responds that current user is guest");
                }
            }
            catch (YouTrackErrorException e)
            {
                throw new UnauthorizedConnectionException(
                    Strings.Exception_CouldNotAuthenticate, (HttpStatusCode)e.StatusCode, e.Response);
            }

            try
            {
                var me = await _youTrackClient.HubApiUserGetAsync("me", "guest");
                if (me.Guest == true || me.Guest == null)
                {
                    throw new UnauthorizedConnectionException(
                        Strings.Exception_CouldNotAuthenticate, HttpStatusCode.OK, "Hub responds that current user is guest");
                }
            }
            catch (YouTrackErrorException e)
            {
                throw new UnauthorizedConnectionException(
                    Strings.Exception_CouldNotAuthenticate, (HttpStatusCode)e.StatusCode, e.Response);
            }

            return _youTrackClient;
        }
    }
}