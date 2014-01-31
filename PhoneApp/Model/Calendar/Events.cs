namespace PhoneApp.Model.Data
{
    public class Events
    {

        private string _accessRole;

        private System.Collections.Generic.List<EventReminder> _defaultReminders;

        private string _description;

        private string _etag;

        private System.Collections.Generic.List<Event> _items;

        private string _kind;

        private string _nextPageToken;

        private string _summary;

        private string _timeZone;

        private string _updated;

        /// <summary>The user&apos;s access role for this calendar. Read-only. Possible values are:  
        ///- &quot;none&quot; - The user has no access. 
        ///- &quot;freeBusyReader&quot; - The user has read access to free/busy information. 
        ///- &quot;reader&quot; - The user has read access to the calendar. Private events will appear to users with reader access, but event details will be hidden. 
        ///- &quot;writer&quot; - The user has read and write access to the calendar. Private events will appear to users with writer access, and event details will be visible. 
        ///- &quot;owner&quot; - The user has ownership of the calendar. This role has all of the permissions of the writer role with the additional ability to see and manipulate ACLs.</summary>
        [Newtonsoft.Json.JsonPropertyAttribute("accessRole")]
        public string AccessRole
        {
            get
            {
                return this._accessRole;
            }
            set
            {
                this._accessRole = value;
            }
        }

        /// <summary>The default reminders on the calendar for the authenticated user. These reminders apply to all events on this calendar that do not explicitly override them (i.e. do not have &apos;reminders.useDefault&apos; set to &apos;true&apos;).</summary>
        [Newtonsoft.Json.JsonPropertyAttribute("defaultReminders")]
        public System.Collections.Generic.List<EventReminder> DefaultReminders
        {
            get
            {
                return this._defaultReminders;
            }
            set
            {
                this._defaultReminders = value;
            }
        }

        /// <summary>Description of the calendar. Read-only.</summary>
        [Newtonsoft.Json.JsonPropertyAttribute("description")]
        public string Description
        {
            get
            {
                return this._description;
            }
            set
            {
                this._description = value;
            }
        }

        /// <summary>ETag of the collection.</summary>
        [Newtonsoft.Json.JsonPropertyAttribute("etag")]
        public string ETag
        {
            get
            {
                return this._etag;
            }
            set
            {
                this._etag = value;
            }
        }

        /// <summary>List of events on the calendar.</summary>
        [Newtonsoft.Json.JsonPropertyAttribute("items")]
        public System.Collections.Generic.List<Event> Items
        {
            get
            {
                return this._items;
            }
            set
            {
                this._items = value;
            }
        }

        /// <summary>Type of the collection (&quot;calendar#events&quot;).</summary>
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

        /// <summary>Token used to access the next page of this result. Omitted if no further results are available.</summary>
        [Newtonsoft.Json.JsonPropertyAttribute("nextPageToken")]
        public string NextPageToken
        {
            get
            {
                return this._nextPageToken;
            }
            set
            {
                this._nextPageToken = value;
            }
        }

        /// <summary>Title of the calendar. Read-only.</summary>
        [Newtonsoft.Json.JsonPropertyAttribute("summary")]
        public string Summary
        {
            get
            {
                return this._summary;
            }
            set
            {
                this._summary = value;
            }
        }

        /// <summary>The time zone of the calendar. Read-only.</summary>
        [Newtonsoft.Json.JsonPropertyAttribute("timeZone")]
        public string TimeZone
        {
            get
            {
                return this._timeZone;
            }
            set
            {
                this._timeZone = value;
            }
        }

        /// <summary>Last modification time of the calendar (as a RFC 3339 timestamp). Read-only.</summary>
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
    }
}
