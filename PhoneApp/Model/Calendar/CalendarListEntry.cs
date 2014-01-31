namespace PhoneApp.Model.Data
{
    public class CalendarListEntry
    {

        private string _accessRole;

        private string _backgroundColor;

        private string _colorId;

        private System.Collections.Generic.List<EventReminder> _defaultReminders;

        private string _description;

        private string _etag;

        private string _foregroundColor;

        private System.Nullable<bool> _hidden;

        private string _id;

        private string _kind;

        private string _location;

        private System.Nullable<bool> _selected;

        private string _summary;

        private string _summaryOverride;

        private string _timeZone;

        /// <summary>The effective access role that the authenticated user has on the calendar. Read-only. Possible values are:  
        ///- &quot;freeBusyReader&quot; - Provides read access to free/busy information. 
        ///- &quot;reader&quot; - Provides read access to the calendar. Private events will appear to users with reader access, but event details will be hidden. 
        ///- &quot;writer&quot; - Provides read and write access to the calendar. Private events will appear to users with writer access, and event details will be visible. 
        ///- &quot;owner&quot; - Provides ownership of the calendar. This role has all of the permissions of the writer role with the additional ability to see and manipulate ACLs.</summary>
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

        /// <summary>The main color of the calendar in the format &apos;#0088aa&apos;. This property supersedes the index-based colorId property. Optional.</summary>
        [Newtonsoft.Json.JsonPropertyAttribute("backgroundColor")]
        public string BackgroundColor
        {
            get
            {
                return this._backgroundColor;
            }
            set
            {
                this._backgroundColor = value;
            }
        }

        /// <summary>The color of the calendar. This is an ID referring to an entry in the &quot;calendar&quot; section of the colors definition (see the &quot;colors&quot; endpoint). Optional.</summary>
        [Newtonsoft.Json.JsonPropertyAttribute("colorId")]
        public string ColorId
        {
            get
            {
                return this._colorId;
            }
            set
            {
                this._colorId = value;
            }
        }

        /// <summary>The default reminders that the authenticated user has for this calendar.</summary>
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

        /// <summary>Description of the calendar. Optional. Read-only.</summary>
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

        /// <summary>ETag of the resource.</summary>
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

        /// <summary>The foreground color of the calendar in the format &apos;#ffffff&apos;. This property supersedes the index-based colorId property. Optional.</summary>
        [Newtonsoft.Json.JsonPropertyAttribute("foregroundColor")]
        public string ForegroundColor
        {
            get
            {
                return this._foregroundColor;
            }
            set
            {
                this._foregroundColor = value;
            }
        }

        /// <summary>Whether the calendar has been hidden from the list. Optional. The default is False.</summary>
        [Newtonsoft.Json.JsonPropertyAttribute("hidden")]
        public System.Nullable<bool> Hidden
        {
            get
            {
                return this._hidden;
            }
            set
            {
                this._hidden = value;
            }
        }

        /// <summary>Identifier of the calendar.</summary>
        [Newtonsoft.Json.JsonPropertyAttribute("id")]
        public string Id
        {
            get
            {
                return this._id;
            }
            set
            {
                this._id = value;
            }
        }

        /// <summary>Type of the resource (&quot;calendar#calendarListEntry&quot;).</summary>
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

        /// <summary>Geographic location of the calendar as free-form text. Optional. Read-only.</summary>
        [Newtonsoft.Json.JsonPropertyAttribute("location")]
        public string Location
        {
            get
            {
                return this._location;
            }
            set
            {
                this._location = value;
            }
        }

        /// <summary>Whether the calendar content shows up in the calendar UI. Optional. The default is False.</summary>
        [Newtonsoft.Json.JsonPropertyAttribute("selected")]
        public System.Nullable<bool> Selected
        {
            get
            {
                return this._selected;
            }
            set
            {
                this._selected = value;
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

        /// <summary>The summary that the authenticated user has set for this calendar. Optional.</summary>
        [Newtonsoft.Json.JsonPropertyAttribute("summaryOverride")]
        public string SummaryOverride
        {
            get
            {
                return this._summaryOverride;
            }
            set
            {
                this._summaryOverride = value;
            }
        }

        /// <summary>The time zone of the calendar. Optional. Read-only.</summary>
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
    }
}
