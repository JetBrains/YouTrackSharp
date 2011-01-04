using System;
using System.Dynamic;
using System.Net;
using System.Security.Authentication;
using EasyHttp.Http;
using EasyHttp.Infrastructure;
using YouTrackSharp.Infrastructure;

namespace YouTrackSharp
{
    public class Connection
    {
        readonly string _host;
        readonly int _port;
        CookieCollection _authenticationCookie;
        readonly IUriConstructor _uriConstructor;

        public Connection(string host, int port, bool useSSL)
        {
            var protocol = "http";

            _host = host;
            _port = port;


            if (useSSL)
            {
                protocol = "https";
            }

            _uriConstructor = new DefaultUriConstructor(protocol, _host, _port);

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


        public void Authenticate(string username, string password)
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

        public bool IsAuthenticated { get; private set; }
    }
}