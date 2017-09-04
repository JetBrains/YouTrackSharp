﻿using System.Collections.Generic;
using Newtonsoft.Json;

namespace YouTrackSharp.TimeTracking
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
			WorkDays = new List<SubValue<int>>();
		}

		/// <summary>
		/// Hours A Day
		/// </summary>
		[JsonProperty("hoursADay")]
		public int HoursADay { get; set; }

		/// <summary>
		/// Days A Week
		/// </summary>
		[JsonProperty("daysAWeek")]
		public int DaysAWeek { get; set; }

		/// <summary>
		/// WorkDays A Week
		/// </summary>
		[JsonProperty("workWeek")]
		public ICollection<SubValue<int>> WorkDays { get; set; }
	}
}