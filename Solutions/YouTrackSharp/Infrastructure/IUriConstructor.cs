namespace YouTrackSharp.Infrastructure
{
    public interface IUriConstructor
    {
        string ConstructBaseUri(string request);
    }
}