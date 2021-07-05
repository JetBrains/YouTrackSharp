using System.Collections.Generic;
using Newtonsoft.Json;
using YouTrackSharp.Generated;

namespace YouTrackSharp.Management
{
	/// <summary>
	/// A class that represents YouTrack system wide time settings information.
	/// </summary>
	public class SystemWideTimeTrackingSettings
	{
		/// <summary>
		/// Creates an instance of the <see cref="SystemWideTimeTrackingSettings" /> class.
		/// </summary>
		public SystemWideTimeTrackingSettings()
		{
			WorkDays = new List<int>();
		}

		/// <summary>
		/// Creates an instance of the <see cref="SystemWideTimeTrackingSettings" /> class from api client entity.
		/// </summary>
		/// <param name="entity">Api client entity of type <see cref="GlobalTimeTrackingSettings"/> to convert from.</param>
		public static SystemWideTimeTrackingSettings FromApiEntity(WorkTimeSettings entity)
		{
			return new SystemWideTimeTrackingSettings()
			{
				MinutesADay = entity.MinutesADay ?? 0,
				DaysAWeek = entity.DaysAWeek ?? 0,
				WorkDays = entity.WorkDays,
			};
		}

		/// <summary>
		/// Converts to instance of the <see cref="GlobalTimeTrackingSettings" /> class used in api client.
		/// </summary>
		public WorkTimeSettings ToApiEntity()
		{
			var workTimeSettings = new WorkTimeSettings(){MinutesADay = MinutesADay, DaysAWeek = DaysAWeek};
			if (WorkDays != null)
			{
				workTimeSettings.WorkDays = WorkDays;
			}

			return workTimeSettings;
		}

		/// <summary>
		/// Hours A Day.
		/// </summary>
		[JsonProperty("minutesADay")]
		public int MinutesADay { get; set; }

		/// <summary>
		/// Days A Week.
		/// </summary>
		[JsonProperty("daysAWeek")]
		public int DaysAWeek { get; set; }

		/// <summary>
		/// WorkDays A Week.
		/// </summary>
		[JsonProperty("workWeek")]
		public ICollection<int> WorkDays { get; set; }
	}
}