using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace YouTrackSharp.Tests.Infrastructure
{
    /// <summary>
    /// This handler is used to return a json array from a range of give json strings, created from the $top and $skip
    /// parameters of the HTTP request.
    /// This handler can be used to simulate a server returning json arrays in batches. 
    /// </summary>
    public class JsonArrayHandler : HttpClientHandler
    {
        private readonly ICollection<string> _jsonObjects;
        public int RequestsReceived { get; private set; }

        /// <summary>
        /// Creates an instance of <see cref="JsonArrayHandler"/>
        /// </summary>
        /// <param name="jsonObjects">List of json objects that this instance will pick from</param>
        public JsonArrayHandler(params string[] jsonObjects)
        {
            _jsonObjects = jsonObjects;
            RequestsReceived = 0;
        }

        /// <inheritdoc />
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            RequestsReceived++;
            
            GetRequestedRange(request, _jsonObjects.Count, out int skip, out int count);
            string json = GetJsonArray(_jsonObjects, skip, count);

            HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
            response.Content = new StringContent(json);

            return Task.FromResult(response);
        }

        /// <summary>
        /// Creates a JSON array from a range of the given json strings.
        /// This allows to simulate returning a total number of elements, in batches. 
        /// </summary>
        /// <param name="jsonObjects">JSON objects from which the JSON array will be created</param>
        /// <param name="skip">Number of items to skip</param>
        /// <param name="count">Number of items to return</param>
        /// <returns>
        /// Json array
        /// </returns>
        private string GetJsonArray(ICollection<string> jsonObjects, int skip, int count)
        {
            IEnumerable<string> jsonObjectRange = jsonObjects.Skip(skip).Take(count);
            string json = $"[{string.Join(",", jsonObjectRange)}]";

            return json;
        }

        /// <summary>
        /// Parses the $skip and $top parameters from a Youtrack REST request URI, and computes the requested range
        /// of objects to return (capped by <see cref="maxIndex"/>).
        /// </summary>
        /// <param name="request">HTTP request</param>
        /// <param name="maxIndex">Max index (range will not go beyond that index, even if $skip + $top is greater</param>
        /// <param name="skip">Number of items to skip</param>
        /// <param name="count">Number of items to return</param>
        /// <returns>Range computed from request's $skip and $top</returns>
        private void GetRequestedRange(HttpRequestMessage request, int maxIndex, out int skip, out int count)
        {
            string requestUri = request.RequestUri.ToString();

            Match match = Regex.Match(requestUri, "&\\$top=(?<top>[0-9]+)(&\\$skip=(?<skip>[0-9]+))?|&\\$skip=(?<skip>[0-9]+)(&\\$top=(?<top>[0-9]+))?");

            count = maxIndex;
            if (match.Groups.ContainsKey("top") && match.Groups["top"].Success)
            {
                count = int.Parse(match.Groups["top"].Value);
            }

            skip = 0;
            if (match.Groups.ContainsKey("skip") && match.Groups["skip"].Success)
            {
                skip = int.Parse(match.Groups["skip"].Value);
            }
        }
    }
}