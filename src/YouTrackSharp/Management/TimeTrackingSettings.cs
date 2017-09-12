using Newtonsoft.Json;

namespace YouTrackSharp.Management
{
	/// <summary>
	/// A class that represents YouTrack timetracking settings information.
	/// </summary>
	public class TimeTrackingSettings
	{
		/// <summary>
		/// Is time tracking enabled?
		/// </summary>
		[JsonProperty("enabled")]
		public bool Enabled { get; set; }

		/// <summary>
		/// Field that contains Estimation data.
		/// </summary>
		[JsonProperty("estimation")]
		public TimeField Estimation { get; set; }

		/// <summary>
		/// Field that contains SpentTime data.
		/// </summary>
		[JsonProperty("spentTime")]
		public TimeField SpentTime { get; set; }

		public class TimeField
		{
			/// <summary>
			/// Name of the field.
			/// </summary>
			[JsonProperty("name")]
			public string Name { get; set; }

			/// <summary>
			/// Url of the field.
			/// </summary>
			[JsonProperty("url")]
			public string Url { get; set; }
		}
	}
}