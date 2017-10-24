using System.Diagnostics;
using Newtonsoft.Json;

namespace YouTrackSharp.Issues
{
    /// <summary>
    /// Represents a YouTrack issue field change.
    /// </summary>
    [DebuggerDisplay("{Name}: {GetDebuggerTransition()}")]
    public class FieldChange
    {
        #region JsonProperties

        [JsonProperty("oldValue")]
        // ReSharper disable once InconsistentNaming
        private object _oldValue
        {
            get => From.Value;
            set => From.Value = value;
        }

        [JsonProperty("value")]
        // ReSharper disable once InconsistentNaming
        private object _value
        {
            get => To.Value;
            set => To.Value = value;
        }

        [JsonProperty("newValue")]
        // ReSharper disable once InconsistentNaming
        private object _newValue
        {
            get => To.Value;
            set => To.Value = value;
        }

        #endregion

        /// <summary>
        /// Creates an instance of the <see cref="FieldChange" /> class.
        /// </summary>
        public FieldChange()
        {
            From = new Field();
            To = new Field();
        }

        /// <summary>
        /// Name of the field.
        /// </summary>
        [JsonProperty("name")]
        public string Name
        {
            get => To.Name;
            private set
            {
                From.Name = value;
                To.Name = value;
            }
        }

        /// <summary>
        /// <see cref="Field" /> representing the original value. Can be <value>null</value>.
        /// </summary>
        [JsonIgnore]
        public Field From { get; }

        /// <summary>
        /// <see cref="Field" /> representing the new value. Can be <value>null</value>.
        /// </summary>
        [JsonIgnore]
        public Field To { get; }

        /// <summary>
        /// Is this a transition from an older value to a newer one?
        /// </summary>
        [JsonIgnore]
        public bool IsTransition => From.Value != null;

        private string GetDebuggerTransition()
        {
            return IsTransition
                ? $"{From.Value} -> {To.Value}" 
                : To.Value.ToString();
        }
    }
}