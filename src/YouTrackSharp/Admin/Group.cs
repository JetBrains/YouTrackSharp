using JsonFx.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace YouTrackSharp.Admin
{
	public class Group
	{
		public string Name { get; set; }
		public string Url { get; set; }

		[JsonIgnore]
		public string Id
		{
			get
			{
				return HttpUtility.UrlDecode(Url.Substring(Url.LastIndexOf('/') + 1));
			}
		}
	}
}
