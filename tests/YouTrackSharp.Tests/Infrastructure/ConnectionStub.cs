using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace YouTrackSharp.Tests.Infrastructure
{
    /// <summary>
    /// Connection mock used to return predefined HTTP responses, for testing purposes.
    /// </summary>
    public class ConnectionStub : Connection
    {
        private readonly HttpClientHandler _handler;
        private TimeSpan TimeOut => TimeSpan.FromSeconds(100);

        /// <summary>
        /// Creates an instance of <see cref="ConnectionStub"/> with give response delegate
        /// </summary>
        /// <param name="handler">
        /// <see cref="HttpClientHandler"/> to associate to this connection.
        /// This can be used to pass a stub handler for testing purposes.
        /// </param>
        public ConnectionStub(HttpClientHandler handler) : base("http://fake.connection.com/")
        {
            _handler = handler;
        }

        /// <summary>
        /// Creates an <see cref="HttpClient"/> configured to return a predefined message and HTTP status
        /// on request.
        /// </summary>
        /// <returns><see cref="HttpClient"/> configured to return a predefined message and HTTP status</returns>
        public override Task<HttpClient> GetAuthenticatedHttpClient()
        {
            HttpClient httpClient = new HttpClient(_handler);
            httpClient.BaseAddress = ServerUri;
            httpClient.Timeout = TimeOut;
            
            return Task.FromResult<HttpClient>(httpClient);
        }
    }
}