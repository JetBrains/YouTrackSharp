namespace YouTrackSharp.Projects
{
    public interface IDataWrapper<T>
    {
        T[] Data { get; set; }
    }
}