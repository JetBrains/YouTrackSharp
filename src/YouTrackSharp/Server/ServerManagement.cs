using System;
using YouTrackSharp.Infrastructure;

namespace YouTrackSharp.Server
{
    public class ServerManagement
    {
        readonly Connection _connection;

        public ServerManagement(Connection connection)
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
    }
}