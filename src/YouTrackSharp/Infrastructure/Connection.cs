using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Net;
using System.Security.Authentication;
using EasyHttp.Http;
using EasyHttp.Infrastructure;
using YouTrackSharp.Projects;
using YouTrackSharp.Server;

namespace YouTrackSharp.Infrastructure
{
    public class Connection : IConnection
    {
        readonly string _host;
        readonly int _port;
        CookieCollection _authenticationCookie;
        readonly IUriConstructor _uriConstructor;
        string _username;

        public Connection(string host, int port = 80, bool useSSL = false)
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


            try
            {
                return httpRequest.Get(_uriConstructor.ConstructBaseUri(request)).StaticBody<T>();
            }
            catch (HttpException httpException)
            {
                if (httpException.StatusCode == HttpStatusCode.Forbidden)
                {
                    throw new InvalidRequestException(Language.Connection_Get_Insufficient_rights);
                }
                throw;
            }
        }
        
        public IEnumerable<TInternal> Get<TWrapper, TInternal>(string command) where TWrapper : IDataWrapper<TInternal>
        {
            var response = Get<TWrapper>(command);

            if (response != null)
            {
                return response.Data;
            }
            return new List<TInternal>();
        }

        public T Post<T>(string command, object data, string accept)
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


                var result = Post<dynamic>("user/login", credentials, HttpContentTypes.ApplicationXml);

                if (String.Compare(result.login, "ok", StringComparison.CurrentCultureIgnoreCase) != 0)
                {
                    throw new AuthenticationException(Language.YouTrackClient_Login_Authentication_Failed);
                }

                IsAuthenticated = true;
                _username = username;
            }
            catch (HttpException)
            {
                IsAuthenticated = false;
                _username = String.Empty;
            }


        }

        public bool IsAuthenticated { get; private set; }

        public User GetCurrentAuthenticatedUser()
        {
            var user = Get<User>("user/current");

            if (user != null)
            {
                user.Username = _username;

                return user;
            }

            return null;
        }
    }
}