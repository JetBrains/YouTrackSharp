namespace YouTrackSharp
{
    public interface IJsonIssueConverter
    {
        /// <summary>
        /// Convert a dynamic issue to a static typed Issue
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        Issue ConvertFromDynamic(dynamic source);
        Issue ConvertFromDynamicFields(dynamic source);
    }
}