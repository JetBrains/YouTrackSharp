using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace YouTrackSharp.Tests.Infrastructure
{
    /// <summary>
    /// This handler returns a predefined json string on every request.
    /// </summary>
    public class JsonHandler : HttpClientHandler
    {
        private readonly string _json;

        /// <summary>
        /// Creates an instance of <see cref="JsonHandler"/>
        /// </summary>
        /// <param name="json">Json string that will be returned upon each request</param>
        public JsonHandler(string json)
        {
            _json = json;
        }

        /// <inheritdoc />
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
            response.Content = new StringContent(_json);

            return Task.FromResult(response);
        }
    }
}