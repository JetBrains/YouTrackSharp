using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace YouTrackSharp.Internal
{
    /// <summary>
    /// A message handler that can be used by <see cref="T:System.Net.Http.HttpClient" />
    /// and provides bearer token authentication.
    /// </summary>
    internal class BearerTokenHttpClientHandler : HttpClientHandler
    {
        private readonly string _bearerToken;

        /// <summary>
        /// Creates an instance of the <see cref="BearerTokenHttpClientHandler" /> class.
        /// </summary>
        /// <param name="bearerToken">The bearer token to inject into HTTP request headers.</param>
        public BearerTokenHttpClientHandler(string bearerToken)
        {
            _bearerToken = bearerToken;
        }
        
        /// <inheritdoc />
        protected override Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request, CancellationToken cancellationToken)
        {
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _bearerToken);
            request.Headers.AcceptEncoding.Add(new StringWithQualityHeaderValue("gzip"));
            request.Headers.AcceptEncoding.Add(new StringWithQualityHeaderValue("deflate"));

            return base.SendAsync(request, cancellationToken);
        }
    }
}