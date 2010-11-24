using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Net;
using System.Net.Cache;
using System.Net.Security;
using System.Security.Authentication;
using System.Text;
using System.Web;
using EasyHttp.Http;
using YouTrackSharp.Infrastructure;
using System.Linq;
using HttpException = EasyHttp.Http.HttpException;

namespace YouTrackSharp
{
    public class YouTrackServer
    {
        readonly string _protocol;
        readonly string _host;
        readonly int _port;
        readonly string _userAgent;
        CookieCollection _authenticationCookie;
        readonly IUriConstructor _uriConstructor;


        /// <summary>
        /// Creates a new instance of YouTrackServer setting the appropriate host and port for successive calls. 
        /// </summary>
        /// <param name="host"></param>
        /// <param name="port"></param>
        /// <param name="useSsl"></param>
        /// <param name="userAgent"></param>
        public YouTrackServer(string host, int port = 80, bool useSsl = false, string userAgent = "")
        {
            _host = host;
            _port = port;
            _userAgent = userAgent;

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
            var httpRequest = CreateHttpRequest();

            httpRequest.Request.Accept = HttpContentTypes.ApplicationXml;

            dynamic credentials = new ExpandoObject();

            credentials.login = username;
            credentials.password = password;

            try
            {
                httpRequest.Post(_uriConstructor.ConstructBaseUri("user/login"), credentials, HttpContentTypes.ApplicationXWwwFormUrlEncoded);

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

            return httpRequest.Get(_uriConstructor.ConstructBaseUri(request)).DynamicBody;
        }

        public dynamic Post(string command, object data)
        {
            var httpRequest = CreateHttpRequest();

            httpRequest.Post(_uriConstructor.ConstructBaseUri(command), data, HttpContentTypes.ApplicationXWwwFormUrlEncoded);

            return httpRequest.Response.DynamicBody;
        }

        public string HttpGet(string command, string action, params object [] p)
        {
            return SendRequest(new Uri(string.Format(_uriConstructor.ConstructBaseUri(command), p)), "GET", action);
        }

        public string HttpPost(string command, string action,params object [] p)
        {
            return SendRequest(new Uri(string.Format(_uriConstructor.ConstructBaseUri(command), p)), "POST", action);
        }

        private string SendRequest(Uri uri, string verb, string action)
        {
            KeyValuePair<HttpWebResponse, string> responsePair = SendRequest(uri, _authenticationCookie.Cast<Cookie>(), new KeyValuePair<string, string>[0], new KeyValuePair<HttpRequestHeader, string>[0], verb);
            CheckResponse(responsePair, action);
            return StripInvalidXmlSymbols(responsePair.Value.ToCharArray());
        }

        private static void CheckResponse(KeyValuePair<HttpWebResponse, string> responcePair, string operationName)
        {
            if (responcePair.Key.StatusCode != HttpStatusCode.OK)
                throw new ApplicationException(String.Format("Charisma operation {0} failed: {1}", operationName, responcePair.Key.StatusDescription));

            if (responcePair.Value.StartsWith("<error>"))
                throw new ApplicationException(string.Format("Charisma operation {1} failed: {0}", Util.QuoteIfNeeded(responcePair.Value.Substring("<error>".Length, responcePair.Value.Length - "<error></error>".Length)), operationName));
        }

        private KeyValuePair<HttpWebResponse, string> SendRequest(Uri uri, IEnumerable<Cookie> cookies, IEnumerable<KeyValuePair<string, string>> pairs, IEnumerable<KeyValuePair<HttpRequestHeader, string>> headers, string method)
        {
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(uri);
            req.AllowAutoRedirect = true;
            req.KeepAlive = false;
            req.ReadWriteTimeout = 30000;
            req.SendChunked = false;
            req.UserAgent = _userAgent;

            Encoding requestEncoding = Encoding.UTF8;
            foreach (KeyValuePair<HttpRequestHeader, string> x in headers)
                req.Headers.Add(x.Key, x.Value);
            req.Headers.Add(HttpRequestHeader.ContentEncoding, requestEncoding.WebName);

            req.CookieContainer = new CookieContainer();
            foreach (Cookie x in cookies)
                req.CookieContainer.Add(x);

            req.AuthenticationLevel = AuthenticationLevel.None;
            req.CachePolicy = new RequestCachePolicy(RequestCacheLevel.NoCacheNoStore);
            req.Method = method;
            req.Timeout = 600000;
            req.ContentType = "application/x-www-form-urlencoded";

            if (method == "POST")
                WriteRequestKeys(req, requestEncoding, pairs);
            HttpWebResponse res = (HttpWebResponse)req.GetResponse();

            if (res.StatusCode != HttpStatusCode.OK)
                throw new ApplicationException("Failed to login to tracker. Status code " + res.StatusCode + " " +
                                               res.StatusDescription);

            string response;
            using (StreamReader tw = res.CharacterSet != null ? new StreamReader(res.GetResponseStream(), Encoding.GetEncoding(res.CharacterSet)) : new StreamReader(res.GetResponseStream()))
                response = tw.ReadToEnd();

            return new KeyValuePair<HttpWebResponse, string>(res, response);
        }

        private static void WriteRequestKeys(WebRequest req, Encoding requestEncoding, IEnumerable<KeyValuePair<string, string>> pairs)
        {
            TextWriter tw = new StringWriter();
            Converter<string, string> escape = x => HttpUtility.UrlEncode(x);

            bool isFirst = true;
            foreach (KeyValuePair<string, string> pair in pairs)
            {
                if (!isFirst)
                    tw.Write("&");
                else
                    isFirst = false;
                tw.Write("{0}={1}", pair.Key, escape(pair.Value));
            }
            tw.Flush();

            using (Stream s = req.GetRequestStream())
            {
                byte[] bytes = requestEncoding.GetBytes(tw.ToString());
                s.Write(bytes, 0, bytes.Length);
            }
        }

        private static string StripInvalidXmlSymbols(char[] @in)
        {
            StringBuilder result = new StringBuilder();
            for (int i = 0; i < @in.Length; i++)
            {
                char ch = @in[i];
                if ((ch == 0x9) ||
                    (ch == 0xA) ||
                    (ch == 0xD) ||
                    ((ch >= 0x20) && (ch <= 0xD7FF)) ||
                    ((ch >= 0xE000) && (ch <= 0xFFFD)) ||
                    ((ch >= 0x10000) && (ch <= 0x10FFFF)))
                    result.Append(ch);
            }

            return result.ToString();
        }


        public static class Util
        {
            public static string QuoteIfNeeded(string s)
            {
                if (s == null)
                    return "<NULL>";

                return ((s.Length == 0) || (s.Contains(" "))) ? '“' + s + '”' : s;
            }
        }
    }
}