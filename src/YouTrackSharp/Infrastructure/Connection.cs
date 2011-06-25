#region License
// Distributed under the BSD License
// =================================
// 
// Copyright (c) 2010-2011, Hadi Hariri
// All rights reserved.
// 
// Redistribution and use in source and binary forms, with or without
// modification, are permitted provided that the following conditions are met:
//     * Redistributions of source code must retain the above copyright
//       notice, this list of conditions and the following disclaimer.
//     * Redistributions in binary form must reproduce the above copyright
//       notice, this list of conditions and the following disclaimer in the
//       documentation and/or other materials provided with the distribution.
//     * Neither the name of Hadi Hariri nor the
//       names of its contributors may be used to endorse or promote products
//       derived from this software without specific prior written permission.
// 
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND
// ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED
// WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
// DISCLAIMED. IN NO EVENT SHALL <COPYRIGHT HOLDER> BE LIABLE FOR ANY
// DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES
// (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES;
// LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND
// ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
// (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS
// SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
// =============================================================
#endregion
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
        readonly IUriConstructor _uriConstructor;
        CookieCollection _authenticationCookie;
        string _username;

        public HttpStatusCode HttpStatusCode { get; private set; }

        public Connection(string host, int port = 80, bool useSSL = false)
        {
            string protocol = "http";

            _host = host;
            _port = port;


            if (useSSL)
            {
                protocol = "https";
            }

            _uriConstructor = new DefaultUriConstructor(protocol, _host, _port);
        }


        public T Get<T>(string command, params object[] parameters)
        {
            HttpClient httpRequest = CreateHttpRequest();

            string request = String.Format(command, parameters);


            try
            {
                var staticBody = httpRequest.Get(_uriConstructor.ConstructBaseUri(request)).StaticBody<T>();

                HttpStatusCode = httpRequest.Response.StatusCode;
                
                return staticBody;
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

        public void PostFile(string command, string filename)
        {
            HttpClient httpRequest = CreateHttpRequest();

            var contentType = GetFileContentType(filename);

            var files = new List<FileData>() { new FileData() { Filename = filename, ContentTransferEncoding = "binary", ContentType = contentType}};
            
            httpRequest.Post(command, null, files);
        }

        string GetFileContentType(string filename)
        {
            var mime = "application/octetstream";
            var ext = System.IO.Path.GetExtension(filename).ToLower();
            Microsoft.Win32.RegistryKey rk = Microsoft.Win32.Registry.ClassesRoot.OpenSubKey(ext);
            if (rk != null && rk.GetValue("Content Type") != null)
            mime = rk.GetValue("Content Type").ToString();
            return mime;
        }

        public T Post<T>(string command, object data, string accept)
        {
            HttpClient httpRequest = CreateHttpRequest();

            httpRequest.Request.Accept = accept;

            httpRequest.Post(_uriConstructor.ConstructBaseUri(command), data,
                             HttpContentTypes.ApplicationXWwwFormUrlEncoded);

            HttpStatusCode = httpRequest.Response.StatusCode;

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
                dynamic result = Post<dynamic>("user/login", credentials, HttpContentTypes.ApplicationXml);

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
    }
}