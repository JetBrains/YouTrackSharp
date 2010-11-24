using System;
using System.Collections.Generic;
using YouTrackSharp.DataModel;
using YouTrackSharp.Infrastructure;

namespace YouTrackSharp.Users
{
    public class YouTrackUsers
    {
        private readonly YouTrackServer _youTrackServer;

        public YouTrackUsers(YouTrackServer youTrackServer)
        {
            _youTrackServer = youTrackServer;
        }

        /// <summary>
        /// Returns information about a user by user login
        /// </summary>
        /// <param name="login">User login</param>
        /// <returns>User information</returns>
        public YouTrackUser GetUser(string login)
        {
            return XmlConverter.ExtractUser(_youTrackServer.HttpGet(String.Format("admin/user/{0}", login), "GetUser"));
        }

        /// <summary>
        /// Returns the list of users who belong to given YouTrack user group
        /// </summary>
        /// <param name="groupName">User group name</param>
        /// <returns>List of logins</returns>
        public IEnumerable<string> GetUsers(string groupName)
        {
            int start = 0;
            int elementsProcessed;
            do
            {
                elementsProcessed = 0;
                string getGroupUri = String.Format("admin/user?group={0}&start={1}", groupName, start);
                string result = _youTrackServer.HttpGet(getGroupUri, "GetUsers");
                foreach (string login in XmlConverter.ExtractUserLogins(result))
                {
                    elementsProcessed++;
                    yield return login;
                }

                start += elementsProcessed;
            } while (elementsProcessed > 0);
        }


        /// <summary>
        /// Returns the list of user's saved searches
        /// </summary>
        /// <param name="userName">User login</param>
        /// <returns>List of saved searches</returns>
        public IEnumerable<YouTrackFilter> GetAllUserFilters(string userName)
        {
            string getUserFiltersUri = String.Format("user/filters/{0}", userName);
            return XmlConverter.ExtractFilters(_youTrackServer.HttpGet(getUserFiltersUri, "GetAllUserFilters"));
        }
    }
}