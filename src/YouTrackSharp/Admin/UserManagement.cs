using System;
using System.Collections.Generic;
using YouTrackSharp.Infrastructure;
using YouTrackSharp.Server;

namespace YouTrackSharp.Admin
{
    public class UserManagement
    {
        readonly IConnection _connection;

        public UserManagement(IConnection connection)
        {
            _connection = connection;
        }


        public User GetUserByUserName(string username)
        {
            var user = _connection.Get<User>(String.Format("user/bylogin/{0}", username));

            if (user != null)
            {
                user.Username = username;
                return user;
            }
            throw new InvalidRequestException(Language.Server_GetUserByUserName_User_does_not_exist);
        }

        public IEnumerable<Filter> GetFiltersByUsername(string username)
        {
            return _connection.Get<MultipleFilterWrapper, Filter>(String.Format("user/filters/{0}", username));
        }
    }
}