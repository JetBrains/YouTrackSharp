using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace YouTrackSharp
{
    /// <summary>
    /// A class that represents a connection against a YouTrack server instance and provides
    /// an authenticated <see cref="T:System.Net.Http.HttpClient" /> that uses a username/password combination.
    /// </summary>
    public class UsernamePasswordConnection : Connection
    {
        private HttpClient _httpClient;
        private bool _authenticated;
        
        private readonly string _username;
        private readonly string _password;

        /// <summary>
        /// Creates an instance of the <see cref="UsernamePasswordConnection" /> class.
        /// </summary>
        /// <param name="serverUrl">YouTrack server instance URL that will be connected against.</param>
        /// <param name="username">The username to be used when authenticating.</param>
        /// <param name="password">The password to be used when authenticating.</param>
        /// <exception cref="ArgumentException">
        /// The <paramref name="serverUrl" /> was null, empty or did not represent a valid, absolute <see cref="T:System.Uri" />.
        /// </exception>
        public UsernamePasswordConnection(string serverUrl, string username, string password)
            : base(serverUrl)
        {
            _username = username;
            _password = password;
        }

        /// <inheritdoc />
        public override async Task<HttpClient> GetAuthenticatedHttpClient()
        {
            // Initialize HTTP client
            if (_httpClient == null)
            {
                _httpClient = new HttpClient(new HttpClientHandler
                {
                    CookieContainer = new CookieContainer(),
                    UseCookies = true
                })
                {
                    BaseAddress = ServerUri
                };

                _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(Constants.HttpContentTypes.ApplicationJson));
            }
            
            // Authenticate?
            if (!_authenticated)
            {
                var formData = new Dictionary<string, string>
                {
                    { "login", _username },
                    { "password", _password }
                };
            
                var response = await _httpClient.PostAsync("rest/user/login", new FormUrlEncodedContent(formData));

                if (response.IsSuccessStatusCode)
                {
                    var responseString = await response.Content.ReadAsStringAsync();
                    if (string.Equals(responseString, "<login>ok</login>", StringComparison.OrdinalIgnoreCase))
                    {
                        _authenticated = true;
                    }
                    else
                    {
                        throw new UnauthorizedConnectionException(
                            Strings.Exception_CouldNotAuthenticate, response.StatusCode, responseString);
                    }
                }

                if (!_authenticated)
                {
                    throw new UnauthorizedConnectionException(
                        Strings.Exception_CouldNotAuthenticate, response.StatusCode, response.ReasonPhrase);
                }
            }
            
            return _httpClient;
        }
    }
}