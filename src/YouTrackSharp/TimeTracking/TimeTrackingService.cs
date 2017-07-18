using System;

namespace YouTrackSharp.TimeTracking
{
    /// <summary>
    /// A class that represents a REST API client for <a href="https://www.jetbrains.com/help/youtrack/standalone/Time-Tracking-User-Methods.html">YouTrack Time Tracking User Methods</a>.
    /// It uses a <see cref="Connection" /> implementation to connect to the remote YouTrack server instance.
    /// </summary>
    public partial class TimeTrackingService
    {
        private readonly Connection _connection;
        
        /// <summary>
        /// Creates an instance of the <see cref="TimeTrackingService" /> class.
        /// </summary>
        /// <param name="connection">A <see cref="Connection" /> instance that provides a connection to the remote YouTrack server instance.</param>
        public TimeTrackingService(Connection connection)
        {
            _connection = connection ?? throw new ArgumentNullException(nameof(connection));
        }
    }
}