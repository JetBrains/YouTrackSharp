using Newtonsoft.Json;

namespace YouTrackSharp.TimeTracking
{
	/// <summary>
	/// A class that represents YouTrack timetracking settings information.
	/// </summary>
	public class TimeTrackingSettings
	{
		/// <summary>
		/// Creates an instance of the <see cref="TimeTrackingSettings" /> class.
		/// </summary>
		public TimeTrackingSettings()
		{
		}

		/// <summary>
		/// Enabled.
		/// </summary>
		[JsonProperty("enabled")]
		public bool Enabled { get; set; }

		/// <summary>
		/// Field for Estimation.
		/// </summary>
		[JsonProperty("estimation")]
		public TimeField Estimation { get; set; }

		/// <summary>
		/// Field for SpentTime.
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
			/// Uri.
			/// </summary>
			[JsonProperty("url")]
			public string Url { get; set; }
		}
	}
}