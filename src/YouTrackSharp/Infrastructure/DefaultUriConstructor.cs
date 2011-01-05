using System;

namespace YouTrackSharp.Infrastructure
{
    internal class DefaultUriConstructor : IUriConstructor
    {
        readonly string _host;
        readonly int _port;
        readonly string _protocol;

        public DefaultUriConstructor(string protocol, string host, int port)
        {
            _protocol = protocol;
            _port = port;
            _host = host;
        }

        #region IUriConstructor Members

        /// <summary>
        /// Create base Uri for Server containing host, port and specific request
        /// </summary>
        /// <param name="request">Specific request</param>
        /// <returns>Uri</returns>
        public string ConstructBaseUri(string request)
        {
            return String.Format("{0}://{1}:{2}/rest/{3}", _protocol, _host, _port, request);
        }

        #endregion
    }
}