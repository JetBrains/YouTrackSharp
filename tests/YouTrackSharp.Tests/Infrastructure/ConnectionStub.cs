using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace YouTrackSharp.Tests.Infrastructure
{
    /// <summary>
    /// Connection mock used to return predefined HTTP responses, for testing purposes.
    /// </summary>
    public class ConnectionStub : Connection
    {
        private Func<HttpRequestMessage, HttpResponseMessage> ExecuteRequest { get; }

        private TimeSpan TimeOut => TimeSpan.FromSeconds(100);

        /// <summary>
        /// Creates an instance of <see cref="ConnectionStub"/> with give response delegate
        /// </summary>
        /// <param name="executeRequest">Request delegate</param>
        public ConnectionStub(Func<HttpRequestMessage, HttpResponseMessage> executeRequest) : base(
            "http://fake.connection.com/")
        {
            ExecuteRequest = executeRequest;
        }

        /// <summary>
        /// Creates an <see cref="HttpClient"/> configured to return a predefined message and HTTP status
        /// on request.
        /// </summary>
        /// <returns><see cref="HttpClient"/> configured to return a predefined message and HTTP status</returns>
        public override Task<HttpClient> GetAuthenticatedHttpClient()
        {
            return Task.FromResult(CreateClient());
        }

        private HttpClient CreateClient()
        {
            HttpClient httpClient = new HttpClient(new HttpClientHandlerStub(ExecuteRequest));
            httpClient.BaseAddress = ServerUri;
            httpClient.Timeout = TimeOut;

            return httpClient;
        }
    }

    /// <summary>
    /// <see cref="HttpClientHandler"/> mock, that returns a predefined reply and HTTP status code. 
    /// </summary>
    public class HttpClientHandlerStub : HttpClientHandler
    {
        private Func<HttpRequestMessage, HttpResponseMessage> ExecuteRequest { get; }

        /// <summary>
        /// Creates an <see cref="HttpClientHandlerStub"/> instance that delegates HttpRequestMessages
        /// </summary>
        /// <param name="executeRequest">Request delegate</param>
        public HttpClientHandlerStub(Func<HttpRequestMessage, HttpResponseMessage> executeRequest)
        {
            ExecuteRequest = executeRequest;
        }

        /// <inheritdoc />
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
                                                               CancellationToken cancellationToken)
        {
            HttpResponseMessage reply = ExecuteRequest?.Invoke(request);

            return Task.FromResult(reply);
        }
    }
}