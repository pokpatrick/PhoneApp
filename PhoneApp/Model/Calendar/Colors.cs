namespace PhoneApp.Model.Data
{
    public class Colors
    {

        private System.Collections.Generic.Dictionary<string, ColorDefinition> _calendar;

        private System.Collections.Generic.Dictionary<string, ColorDefinition> _event;

        private string _kind;

        private string _updated;

        private string _ETag;

        /// <summary>Palette of calendar colors, mapping from the color ID to its definition. An &apos;calendarListEntry&apos; resource refers to one of these color IDs in its &apos;color&apos; field. Read-only.</summary>
        [Newtonsoft.Json.JsonPropertyAttribute("calendar")]
        public System.Collections.Generic.Dictionary<string, ColorDefinition> Calendar
        {
            get
            {
                return this._calendar;
            }
            set
            {
                this._calendar = value;
            }
        }

        /// <summary>Palette of event colors, mapping from the color ID to its definition. An &apos;event&apos; resource may refer to one of these color IDs in its &apos;color&apos; field. Read-only.</summary>
        [Newtonsoft.Json.JsonPropertyAttribute("event")]
        public System.Collections.Generic.Dictionary<string, ColorDefinition> Event
        {
            get
            {
                return this._event;
            }
            set
            {
                this._event = value;
            }
        }

        /// <summary>Type of the resource (&quot;calendar#colors&quot;).</summary>
        [Newtonsoft.Json.JsonPropertyAttribute("kind")]
        public string Kind
        {
            get
            {
                return this._kind;
            }
            set
            {
                this._kind = value;
            }
        }

        /// <summary>Last modification time of the color palette (as a RFC 3339 timestamp). Read-only.</summary>
        [Newtonsoft.Json.JsonPropertyAttribute("updated")]
        public string Updated
        {
            get
            {
                return this._updated;
            }
            set
            {
                this._updated = value;
            }
        }

        public string ETag
        {
            get
            {
                return this._ETag;
            }
            set
            {
                this._ETag = value;
            }
        }

        /// <summary>Palette of calendar colors, mapping from the color ID to its definition. An &apos;calendarListEntry&apos; resource refers to one of these color IDs in its &apos;color&apos; field. Read-only.</summary>
        public class CalendarData : System.Collections.Generic.Dictionary<string, ColorDefinition>
        {
        }

        /// <summary>Palette of event colors, mapping from the color ID to its definition. An &apos;event&apos; resource may refer to one of these color IDs in its &apos;color&apos; field. Read-only.</summary>
        public class EventData : System.Collections.Generic.Dictionary<string, ColorDefinition>
        {
        }
    }
}
