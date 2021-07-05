namespace YouTrackSharp
{
    public partial class YouTrackClient : IYouTrackClient
    {
        private string _baseUrl;

        public string BaseUrl
        {
            get { return _baseUrl; }
            set { _baseUrl = value; }
        }
    }
}