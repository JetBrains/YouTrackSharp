using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace YouTrackSharp.Generated
{
    // class stub for exposing additional json attribute 
    [JsonInheritanceAttribute("SimpleProjectCustomField", typeof(VersionProjectCustomField))]
    public partial class BundleProjectCustomField
    {
    }

    /// <summary>Represents project settings stub for fields not directly exposed in openapi model.</summary>
    public class SimpleProjectCustomField : BundleProjectCustomField
    {
    }

    public partial class YouTrackClient
    {
        private string _baseUrl;

        public string BaseUrl
        {
            get => _baseUrl ?? (_baseUrl = _httpClient.BaseAddress.ToString().TrimEnd('/') + "/api/");
            set => _baseUrl = value;
        }
    }

    public partial class UnlimitedVisibility
    {
        public string ToSinglePermittedGroup()
        {
            return this is LimitedVisibility visibility && visibility.PermittedGroups.Any()
                ? visibility.PermittedGroups.First().Name
                : "All Users";
        }
    }
}