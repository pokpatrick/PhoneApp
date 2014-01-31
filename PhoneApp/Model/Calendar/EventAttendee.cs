namespace PhoneApp.Model.Data
{
    public class EventAttendee
    {

        private System.Nullable<long> _additionalGuests;

        private string _comment;

        private string _displayName;

        private string _email;

        private string _id;

        private System.Nullable<bool> _optional;

        private System.Nullable<bool> _organizer;

        private System.Nullable<bool> _resource;

        private string _responseStatus;

        private System.Nullable<bool> _self;

        /// <summary>Number of additional guests. Optional. The default is 0.</summary>
        [Newtonsoft.Json.JsonPropertyAttribute("additionalGuests")]
        public System.Nullable<long> AdditionalGuests
        {
            get
            {
                return this._additionalGuests;
            }
            set
            {
                this._additionalGuests = value;
            }
        }

        /// <summary>The attendee&apos;s response comment. Optional.</summary>
        [Newtonsoft.Json.JsonPropertyAttribute("comment")]
        public string Comment
        {
            get
            {
                return this._comment;
            }
            set
            {
                this._comment = value;
            }
        }

        /// <summary>The attendee&apos;s name, if available. Optional.</summary>
        [Newtonsoft.Json.JsonPropertyAttribute("displayName")]
        public string DisplayName
        {
            get
            {
                return this._displayName;
            }
            set
            {
                this._displayName = value;
            }
        }

        /// <summary>The attendee&apos;s email address, if available. This field must be present when adding an attendee.</summary>
        [Newtonsoft.Json.JsonPropertyAttribute("email")]
        public string Email
        {
            get
            {
                return this._email;
            }
            set
            {
                this._email = value;
            }
        }

        /// <summary>The attendee&apos;s Profile ID, if available.</summary>
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

        /// <summary>Whether this is an optional attendee. Optional. The default is False.</summary>
        [Newtonsoft.Json.JsonPropertyAttribute("optional")]
        public System.Nullable<bool> Optional
        {
            get
            {
                return this._optional;
            }
            set
            {
                this._optional = value;
            }
        }

        /// <summary>Whether the attendee is the organizer of the event. Read-only. The default is False.</summary>
        [Newtonsoft.Json.JsonPropertyAttribute("organizer")]
        public System.Nullable<bool> Organizer
        {
            get
            {
                return this._organizer;
            }
            set
            {
                this._organizer = value;
            }
        }

        /// <summary>Whether the attendee is a resource. Read-only. The default is False.</summary>
        [Newtonsoft.Json.JsonPropertyAttribute("resource")]
        public System.Nullable<bool> Resource
        {
            get
            {
                return this._resource;
            }
            set
            {
                this._resource = value;
            }
        }

        /// <summary>The attendee&apos;s response status. Possible values are:  
        ///- &quot;needsAction&quot; - The attendee has not responded to the invitation. 
        ///- &quot;declined&quot; - The attendee has declined the invitation. 
        ///- &quot;tentative&quot; - The attendee has tentatively accepted the invitation. 
        ///- &quot;accepted&quot; - The attendee has accepted the invitation.</summary>
        [Newtonsoft.Json.JsonPropertyAttribute("responseStatus")]
        public string ResponseStatus
        {
            get
            {
                return this._responseStatus;
            }
            set
            {
                this._responseStatus = value;
            }
        }

        /// <summary>Whether this entry represents the calendar on which this copy of the event appears. Read-only. The default is False.</summary>
        [Newtonsoft.Json.JsonPropertyAttribute("self")]
        public System.Nullable<bool> Self
        {
            get
            {
                return this._self;
            }
            set
            {
                this._self = value;
            }
        }
    }
}
