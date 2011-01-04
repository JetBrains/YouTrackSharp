using System.Collections.Generic;
using YouTrackSharp.Issues;

namespace YouTrackSharp
{
    public class Server
    {
        public Connection Connection { get; private set; }



        /// <summary>
        /// Creates a new instance of Server setting the appropriate host and port for successive calls. 
        /// </summary>
        /// <param name="host"></param>
        /// <param name="port"></param>
        /// <param name="useSsl"></param>
        public Server(string host, int port = 80, bool useSsl = false)
        {
            Connection = new Connection(host, port, useSsl);
        }

        /// <summary>
        /// Indicates whether a successful login has already taken place
        /// <seealso cref="Login"/>
        /// </summary>
        public bool IsAuthenticated 
        { 
            get { return Connection.IsAuthenticated; }
        }


        /// <summary>
        /// Logs in to Server provided the correct username and password. If successful, <see cref="IsAuthenticated"/>will be true
        /// </summary>
        /// <param name="username">Username</param>
        /// <param name="password">Passowrd</param>
        public void Login(string username, string password)
        {
            Connection.Authenticate(username, password);
        }

       
    }
}