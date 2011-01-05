using System.Collections.Generic;
using YouTrackSharp.Projects;
using YouTrackSharp.Server;

namespace YouTrackSharp.Infrastructure
{
    public interface IConnection
    {
        T Get<T>(string command, params object[] parameters);
        IEnumerable<TInternal> Get<TWrapper, TInternal>(string command) where TWrapper : IDataWrapper<TInternal>;
        T Post<T>(string command, object data, string accept);
        void Authenticate(string username, string password);
        User GetCurrentAuthenticatedUser();
        bool IsAuthenticated { get; }
    }
}