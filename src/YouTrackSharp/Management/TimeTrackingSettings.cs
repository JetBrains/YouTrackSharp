using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Newtonsoft.Json;
using YouTrackSharp.Generated;

namespace YouTrackSharp.Management
{
	/// <summary>
	/// A class that represents YouTrack timetracking settings information.
	/// </summary>
	public class TimeTrackingSettings
	{
		/// <summary>
		/// Creates an instance of the <see cref="TimeTrackingSettings" /> class from api client entity.
		/// </summary>
		/// <param name="entity">Api client entity of type <see cref="ProjectTimeTrackingSettings"/> to convert from.</param>
		internal static TimeTrackingSettings FromApiEntity(ProjectTimeTrackingSettings entity)
		{
			return new TimeTrackingSettings()
			{
				Enabled = entity.Enabled ?? false,
				Estimation = new TimeField(){Name = entity.Estimate?.Field.Name},
				SpentTime = new TimeField(){Name = entity.TimeSpent?.Field.Name}
			};
		}

		/// <summary>
		/// Converts to instance of the <see cref="ProjectTimeTrackingSettings" /> class used in api client.
		/// </summary>
		internal ProjectTimeTrackingSettings ToApiEntity(ICollection<CustomField> pcfList)
		{
			var projectTimeTracking = new ProjectTimeTrackingSettings(){Enabled = Enabled};
			
			if (Estimation != null)
			{
				var field = pcfList.SingleOrDefault(cf =>
					cf.Name.Equals(Estimation.Name, StringComparison.InvariantCultureIgnoreCase));
				if (field == null)
				{
					throw new YouTrackErrorException(Strings.Exception_BadRequest, (int)HttpStatusCode.BadRequest,
						$"Project custom field [ {Estimation.Name} ] not found.", null, null);
				}
				projectTimeTracking.Estimate = new BuildProjectCustomField(){Field = field};
			}

			if (SpentTime != null)
			{
				var field = pcfList.SingleOrDefault(cf =>
					cf.Name.Equals(SpentTime.Name, StringComparison.InvariantCultureIgnoreCase));
				if (field == null)
				{
					throw new YouTrackErrorException(Strings.Exception_BadRequest, (int)HttpStatusCode.BadRequest,
						$"Project custom field [ {SpentTime.Name} ] not found.", null, null);
				}
				projectTimeTracking.TimeSpent = new BuildProjectCustomField(){Field = field};
			}

			return projectTimeTracking;
		}
		
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
		}
	}
}