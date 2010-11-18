namespace YouTrackSharp
{
    public interface IUriConstructor
    {
        string ConstructUri(string request, params object[] parameters);
    }
}