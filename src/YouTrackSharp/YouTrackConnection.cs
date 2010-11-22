using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Net;
using System.Security.Authentication;
using EasyHttp.Http;
using YouTrackSharp.Infrastructure;

namespace YouTrackSharp
{
    public class YouTrackConnection
    {
        readonly string _protocol;
        readonly string _host;
        readonly int _port;
        CookieCollection _authenticationCookie;
        readonly IUriConstructor _uriConstructor;


        /// <summary>
        /// Creates a new instance of YouTrackConnection setting the appropriate host and port for successive calls. 
        /// </summary>
        /// <param name="host"></param>
        /// <param name="port"></param>
        /// <param name="useSsl"></param>
        public YouTrackConnection(string host, int port = 80, bool useSsl = false)
        {
            _host = host;
            _port = port;

            if (useSsl)
            {
                _protocol = "https";
            }
            else
            {
                _protocol = "http";
            }

            _uriConstructor = new DefaultUriConstructor(_protocol, _host, _port);
        }

        /// <summary>
        /// Indicates whether a successful login has already taken place
        /// <seealso cref="Login"/>
        /// </summary>
        public bool IsAuthenticated { get; private set; }


        /// <summary>
        /// Logs in to YouTrackConnection provided the correct username and password. If successful, <see cref="IsAuthenticated"/>will be true
        /// </summary>
        /// <param name="username">Username</param>
        /// <param name="password">Passowrd</param>
        public void Login(string username, string password)
        {
            var httpRequest = CreateHttpRequest();

            httpRequest.Request.Accept = HttpContentTypes.ApplicationXml;

            dynamic credentials = new ExpandoObject();

            credentials.login = username;
            credentials.password = password;

            try
            {
                httpRequest.Post(_uriConstructor.ConstructUri("user/login"), credentials, HttpContentTypes.ApplicationXWwwFormUrlEncoded);

                dynamic result = httpRequest.Response.DynamicBody;

                if (String.Compare(result.login, "ok", StringComparison.CurrentCultureIgnoreCase) != 0)
                {
                    throw new AuthenticationException(Language.YouTrackClient_Login_Authentication_Failed);
                }
                IsAuthenticated = true;
                _authenticationCookie = httpRequest.Response.Cookie;
            }
            catch (HttpException)
            {
                IsAuthenticated = false;
            }


        }

        HttpClient CreateHttpRequest()
        {
            var httpClient = new HttpClient();

            httpClient.Request.Accept = HttpContentTypes.ApplicationJson;
            httpClient.ThrowExceptionOnHttpError = true;
            if (_authenticationCookie != null)
            {
                httpClient.Request.Cookies = new CookieCollection();
                httpClient.Request.Cookies.Add(_authenticationCookie);
            }
            

            return httpClient;
        }

        public dynamic Get(string command, params object[] parameters)
        {
            var httpRequest = CreateHttpRequest();

            var request = String.Format(command, parameters);

            return httpRequest.Get(_uriConstructor.ConstructUri(request)).DynamicBody;
        }

        public dynamic Post(string command, object data)
        {
            var httpRequest = CreateHttpRequest();

            httpRequest.Post(_uriConstructor.ConstructUri(command), data, HttpContentTypes.ApplicationXWwwFormUrlEncoded);

            return httpRequest.Response.DynamicBody;
        }
    }
}