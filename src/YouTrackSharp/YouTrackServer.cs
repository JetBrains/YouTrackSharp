using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Net;
using System.Security.Authentication;
using EasyHttp.Http;
using EasyHttp.Infrastructure;
using YouTrackSharp.Infrastructure;
using YouTrackSharp.Issues;

namespace YouTrackSharp
{
    public class YouTrackServer
    {
        readonly string _protocol;
        readonly string _host;
        readonly int _port;
        CookieCollection _authenticationCookie;
        readonly IUriConstructor _uriConstructor;


        /// <summary>
        /// Creates a new instance of YouTrackServer setting the appropriate host and port for successive calls. 
        /// </summary>
        /// <param name="host"></param>
        /// <param name="port"></param>
        /// <param name="useSsl"></param>
        public YouTrackServer(string host, int port = 80, bool useSsl = false)
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
        /// Logs in to YouTrackServer provided the correct username and password. If successful, <see cref="IsAuthenticated"/>will be true
        /// </summary>
        /// <param name="username">Username</param>
        /// <param name="password">Passowrd</param>
        public void Login(string username, string password)
        {


            dynamic credentials = new ExpandoObject();

            credentials.login = username;
            credentials.password = password;

            try
            {

            
                var result = Post("user/login", credentials, HttpContentTypes.ApplicationXml);

                if (String.Compare(result.login, "ok", StringComparison.CurrentCultureIgnoreCase) != 0)
                {
                    throw new AuthenticationException(Language.YouTrackClient_Login_Authentication_Failed);
                }
 
                IsAuthenticated = true;
            
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

        
      
        public T Get<T>(string command, params object[] parameters)
        {
            var httpRequest = CreateHttpRequest();

            var request = String.Format(command, parameters);

            return httpRequest.Get(_uriConstructor.ConstructBaseUri(request)).StaticBody<T>();
        }

        public dynamic Post(string command, object data, string accept)
        {
            var httpRequest = CreateHttpRequest();

            httpRequest.Request.Accept = accept;

            httpRequest.Post(_uriConstructor.ConstructBaseUri(command), data, HttpContentTypes.ApplicationXWwwFormUrlEncoded);

            _authenticationCookie = httpRequest.Response.Cookie;
 
            return httpRequest.Response.DynamicBody;

        }

       
    }
}