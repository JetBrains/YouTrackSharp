using System;

namespace YouTrackSharp.Infrastructure
{
    class DefaultUriConstructor : IUriConstructor
    {
        string _protocol;
        string _host;
        int _port;

        public DefaultUriConstructor(string protocol, string host, int port)
        {
            _protocol = protocol;
            _port = port;
            _host = host;
        }

        /// <summary>
        /// Create base Uri for YouTrackConnection containing host, port and specific request
        /// </summary>
        /// <param name="request">Specific request</param>
        /// <param name="parameters">List of parameters</param>
        /// <returns>Uri</returns>
        public string ConstructUri(string request)
        {
            return String.Format("{0}://{1}:{2}/rest/{3}", _protocol, _host, _port, request);
        }
    }
}