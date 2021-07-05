using System.Collections.Generic;

namespace YouTrackSharp.Generated
{
    public partial class YouTrackClient
    {
        private string _hubUrl;
        
        internal string HubApiUrl
        {
            get { return _hubUrl; }
            set { _hubUrl = value; }
        }
        
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>single HubApiUser</returns>
        /// <exception cref="YouTrackErrorException">A server side error occurred.</exception>
        internal async System.Threading.Tasks.Task<HubApiUser> HubApiUserGetAsync(string id, string fields = null, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))
        {
            if (id == null)
                throw new System.ArgumentNullException("id");
            
            var urlBuilder_ = new System.Text.StringBuilder();
            urlBuilder_.Append(HubApiUrl != null ? HubApiUrl.TrimEnd('/') : "").Append("/users/{id}?");
            urlBuilder_.Replace("{id}", System.Uri.EscapeDataString(ConvertToString(id, System.Globalization.CultureInfo.InvariantCulture)));
            if (fields != null)
            {
                urlBuilder_.Append(System.Uri.EscapeDataString("fields") + "=").Append(System.Uri.EscapeDataString(ConvertToString(fields, System.Globalization.CultureInfo.InvariantCulture))).Append("&");
            }
            urlBuilder_.Length--;
    
            var client_ = _httpClient;
            var disposeClient_ = false;
            try
            {
                using (var request_ = new System.Net.Http.HttpRequestMessage())
                {
                    request_.Method = new System.Net.Http.HttpMethod("GET");
                    request_.Headers.Accept.Add(System.Net.Http.Headers.MediaTypeWithQualityHeaderValue.Parse("application/json"));
    
                    PrepareRequest(client_, request_, urlBuilder_);
    
                    var url_ = urlBuilder_.ToString();
                    request_.RequestUri = new System.Uri(url_, System.UriKind.RelativeOrAbsolute);
    
                    PrepareRequest(client_, request_, url_);
    
                    var response_ = await client_.SendAsync(request_, System.Net.Http.HttpCompletionOption.ResponseHeadersRead, cancellationToken).ConfigureAwait(false);
                    var disposeResponse_ = true;
                    try
                    {
                        var headers_ = System.Linq.Enumerable.ToDictionary(response_.Headers, h_ => h_.Key, h_ => h_.Value);
                        if (response_.Content != null && response_.Content.Headers != null)
                        {
                            foreach (var item_ in response_.Content.Headers)
                                headers_[item_.Key] = item_.Value;
                        }
    
                        ProcessResponse(client_, response_);
    
                        var status_ = (int)response_.StatusCode;
                        if (status_ == 200)
                        {
                            var objectResponse_ = await ReadObjectResponseAsync<HubApiUser>(response_, headers_, cancellationToken).ConfigureAwait(false);
                            if (objectResponse_.Object == null)
                            {
                                throw new YouTrackErrorException("Response was null which was not expected.", status_, objectResponse_.Text, headers_, null);
                            }
                            return objectResponse_.Object;
                        }
                        else
                        {
                            var responseData_ = response_.Content == null ? null : await response_.Content.ReadAsStringAsync().ConfigureAwait(false);
                            throw new YouTrackErrorException("The HTTP status code of the response was not expected (" + status_ + ").", status_, responseData_, headers_, null);
                        }
                    }
                    finally
                    {
                        if (disposeResponse_)
                            response_.Dispose();
                    }
                }
            }
            finally
            {
                if (disposeClient_)
                    client_.Dispose();
            }
        }

        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>single HubApiUsersPage</returns>
        /// <exception cref="YouTrackErrorException">A server side error occurred.</exception>
        internal async System.Threading.Tasks.Task<HubApiUsersPage> HubApiUsersGetAsync(string query = null, string fields = null, int? skip = null, int? top = null, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))
        {
            var urlBuilder_ = new System.Text.StringBuilder();
            urlBuilder_.Append(HubApiUrl != null ? HubApiUrl.TrimEnd('/') : "").Append("/users?");
            if (query != null)
            {
                urlBuilder_.Append(System.Uri.EscapeDataString("query") + "=").Append(System.Uri.EscapeDataString(ConvertToString(fields, System.Globalization.CultureInfo.InvariantCulture))).Append("&");
            }
            if (fields != null)
            {
                urlBuilder_.Append(System.Uri.EscapeDataString("fields") + "=").Append(System.Uri.EscapeDataString(ConvertToString(fields, System.Globalization.CultureInfo.InvariantCulture))).Append("&");
            }
            if (skip != null)
            {
                urlBuilder_.Append(System.Uri.EscapeDataString("$skip") + "=").Append(System.Uri.EscapeDataString(ConvertToString(skip, System.Globalization.CultureInfo.InvariantCulture))).Append("&");
            }
            if (top != null)
            {
                urlBuilder_.Append(System.Uri.EscapeDataString("$top") + "=").Append(System.Uri.EscapeDataString(ConvertToString(top, System.Globalization.CultureInfo.InvariantCulture))).Append("&");
            }
            urlBuilder_.Length--;
    
            var client_ = _httpClient;
            var disposeClient_ = false;
            try
            {
                using (var request_ = new System.Net.Http.HttpRequestMessage())
                {
                    request_.Method = new System.Net.Http.HttpMethod("GET");
                    request_.Headers.Accept.Add(System.Net.Http.Headers.MediaTypeWithQualityHeaderValue.Parse("application/json"));
    
                    PrepareRequest(client_, request_, urlBuilder_);
    
                    var url_ = urlBuilder_.ToString();
                    request_.RequestUri = new System.Uri(url_, System.UriKind.RelativeOrAbsolute);
    
                    PrepareRequest(client_, request_, url_);
    
                    var response_ = await client_.SendAsync(request_, System.Net.Http.HttpCompletionOption.ResponseHeadersRead, cancellationToken).ConfigureAwait(false);
                    var disposeResponse_ = true;
                    try
                    {
                        var headers_ = System.Linq.Enumerable.ToDictionary(response_.Headers, h_ => h_.Key, h_ => h_.Value);
                        if (response_.Content != null && response_.Content.Headers != null)
                        {
                            foreach (var item_ in response_.Content.Headers)
                                headers_[item_.Key] = item_.Value;
                        }
    
                        ProcessResponse(client_, response_);
    
                        var status_ = (int)response_.StatusCode;
                        if (status_ == 200)
                        {
                            var objectResponse_ = await ReadObjectResponseAsync<HubApiUsersPage>(response_, headers_, cancellationToken).ConfigureAwait(false);
                            if (objectResponse_.Object == null)
                            {
                                throw new YouTrackErrorException("Response was null which was not expected.", status_, objectResponse_.Text, headers_, null);
                            }
                            return objectResponse_.Object;
                        }
                        else
                        {
                            var responseData_ = response_.Content == null ? null : await response_.Content.ReadAsStringAsync().ConfigureAwait(false);
                            throw new YouTrackErrorException("The HTTP status code of the response was not expected (" + status_ + ").", status_, responseData_, headers_, null);
                        }
                    }
                    finally
                    {
                        if (disposeResponse_)
                            response_.Dispose();
                    }
                }
            }
            finally
            {
                if (disposeClient_)
                    client_.Dispose();
            }
        }
    }
    
    internal class HubApiUsersPage
    {
        [Newtonsoft.Json.JsonProperty("users", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public System.Collections.Generic.ICollection<HubApiUser> Users { get; set; }
    }
    
    internal class HubApiUser
    {
        [Newtonsoft.Json.JsonProperty("id", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string Id { get; set; }
        
        [Newtonsoft.Json.JsonProperty("guest", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public bool? Guest { get; set; }
        
        [Newtonsoft.Json.JsonProperty("login", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string Login { get; set; }
        
        [Newtonsoft.Json.JsonProperty("name", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string Name { get; set; }
        
        [Newtonsoft.Json.JsonProperty("profile", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public HubApiProfile Profile { get; set; }
    }
    
    internal class HubApiProfile
    {
        [Newtonsoft.Json.JsonProperty("email", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public HubApiEmail Email { get; set; }
        
        [Newtonsoft.Json.JsonProperty("jabber", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public HubApiJabber Jabber { get; set; }
    }
    
    internal class HubApiEmail
    {
        [Newtonsoft.Json.JsonProperty("email", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string Email { get; set; }
    }
    
    internal class HubApiJabber
    {
        [Newtonsoft.Json.JsonProperty("jabber", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string Jabber { get; set; }
    }
}