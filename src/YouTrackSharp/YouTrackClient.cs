using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Net;
using System.Security.Authentication;
using EasyHttp.Http;

namespace YouTrackSharp
{
    public class YouTrackClient
    {
        readonly string _protocol;
        readonly string _host;
        readonly int _port;
        CookieCollection _authenticationCookie;
        readonly IJsonIssueConverter _jsonIssueConverter;


        /// <summary>
        /// Creates a new instance of YouTrackClient setting the appropriate host and port for successive calls. 
        /// </summary>
        /// <param name="host"></param>
        /// <param name="port"></param>
        /// <param name="useSsl"></param>
        public YouTrackClient(string host, int port = 80, bool useSsl = false)
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

            // TODO: Refactor to IoC
            _jsonIssueConverter = new JsonIssueConverter();
        }

        /// <summary>
        /// Indicates whether a successful login has already taken place
        /// <seealso cref="Login"/>
        /// </summary>
        public bool IsAuthenticated { get; private set; }

        /// <summary>
        /// Retrieves a list of issues 
        /// </summary>
        /// <param name="projectIdentifier">Project Identifier</param>
        /// <param name="max">[Optional] Maximum number of issues to return. Default is int.MaxValue</param>
        /// <param name="start">[Optional] The number by which to start the issues. Default is 0. Used for paging.</param>
        /// <returns>List of Issues</returns>
        public IList<Issue> GetIssues(string projectIdentifier, int max = int.MaxValue, int start = 0)
        {
            var httpClient = CreateHttpRequest();

            dynamic response = httpClient.Get(ConstructUri("project/issues/{0}?max={1}&after={2}", projectIdentifier, max, start));

            dynamic issues = response.DynamicBody.issue;

            var list = new List<Issue>();

            foreach (dynamic entry in issues)
            {
                var issue = _jsonIssueConverter.ConvertFromDynamic(entry);

                list.Add(issue);
            }

            return list;
        }


        /// <summary>
        /// Create base Uri for YouTrackClient containing host, port and specific request
        /// </summary>
        /// <param name="request">Specific request</param>
        /// <param name="parameters">List of parameters</param>
        /// <returns>Uri</returns>
        string ConstructUri(string request, params object[] parameters)
        {
            return String.Format("{0}://{1}:{2}/rest/{3}", _protocol, _host, _port, String.Format(request, parameters));
        }

        /// <summary>
        /// Logs in to YouTrackClient provided the correct username and password. If successful, <see cref="IsAuthenticated"/>will be true
        /// </summary>
        /// <param name="username">Username</param>
        /// <param name="password">Passowrd</param>
        public void Login(string username, string password)
        {
            var httpClient = CreateHttpRequest();

       
            dynamic credentials = new ExpandoObject();

            credentials.login = username;
            credentials.password = password;

            try
            {
                httpClient.Post(ConstructUri("user/login"), credentials, "application/x-www-form-urlencoded");

                dynamic result = httpClient.Response.DynamicBody;

                if (String.Compare(result.login, "ok", StringComparison.CurrentCultureIgnoreCase) != 0)
                {
                    throw new AuthenticationException(Language.YouTrackClient_Login_Authentication_Failed);
                }
                IsAuthenticated = true;
                _authenticationCookie = httpClient.Response.Cookie;
            }
            catch (HttpException)
            {
                IsAuthenticated = false;
            }


        }

        /// <summary>
        /// Retrieve an issue by id
        /// </summary>
        /// <param name="issueId">Id of the issue to retrieve</param>
        /// <returns>An instance of Issue if successful or InvalidRequestException if issues is not found</returns>
        public Issue GetIssue(string issueId)
        {
            var httpClient = CreateHttpRequest();

            httpClient.ThrowExceptionOnHttpError = true;

            try
            {
                var response = httpClient.Get(ConstructUri("issue/{0}", issueId));

                return _jsonIssueConverter.ConvertFromDynamicFields(response.DynamicBody);
            }
            catch (HttpException exception)
            {
                throw new InvalidRequestException(String.Format(Language.YouTrackClient_GetIssue_Issue_not_found___0_, issueId), exception);
            }
        }

        static HttpClient CreateHttpRequest()
        {
            var httpClient = new HttpClient();

            httpClient.Request.Accept = HttpContentTypes.ApplicationJson;
            
            return httpClient;
        }
    }
}