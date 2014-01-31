namespace PhoneApp.Model.Data
{
    public class Event : System.ComponentModel.INotifyPropertyChanged
    {
        public sealed class Transparencies
        {
            private Transparencies() {}
            public const string Opaque = "opaque";
            public const string Transparency = "transparency";
            public readonly string[] List = { "opaque", "transparency" };
        }
        public sealed class Visibilities
        {
            private Visibilities() { }
            public const string Default = "default";
            public const string Public = "public";
            public const string Private = "private";
            public const string Confidential = "confidential";
            public readonly string[] List = { "default", "public", "private", "confidential" };
        }
        public sealed class Statuses
        {
            private Statuses() { }
            public const string Confirmed = "confirmed";
            public const string Tentative = "tentative";
            public const string Cancelled = "cancelled";
            public readonly string[] List = { "confirmed", "tentative", "cancelled" };
        }        

        private System.Nullable<bool> _anyoneCanAddSelf;

        private System.Collections.Generic.List<EventAttendee> _attendees = new System.Collections.Generic.List<EventAttendee>();

        private System.Nullable<bool> _attendeesOmitted;

        private string _colorId;

        private string _created;

        private Event.CreatorData _creator;

        private string _description;

        private EventDateTime _end = new EventDateTime();

        private System.Nullable<bool> _endTimeUnspecified;

        private string _etag;

        private Event.ExtendedPropertiesData _extendedProperties;

        private Event.GadgetData _gadget;

        private System.Nullable<bool> _guestsCanInviteOthers;

        private System.Nullable<bool> _guestsCanModify;

        private System.Nullable<bool> _guestsCanSeeOtherGuests;

        private string _hangoutLink;

        private string _htmlLink;

        private string _iCalUID;

        private string _id;

        private string _kind;

        private string _location;

        private System.Nullable<bool> _locked;

        private Event.OrganizerData _organizer;

        private EventDateTime _originalStartTime;

        private System.Nullable<bool> _privateCopy;

        private System.Collections.Generic.List<string> _recurrence = new System.Collections.Generic.List<string>();

        private string _recurringEventId;

        private Event.RemindersData _reminders;

        private System.Nullable<long> _sequence;

        private EventDateTime _start = new EventDateTime();

        private string _status;

        private string _summary;

        private string _transparency;

        private string _updated;

        private string _visibility;

        /// <summary>Whether anyone can invite themselves to the event. Optional. The default is False.</summary>
        [Newtonsoft.Json.JsonPropertyAttribute("anyoneCanAddSelf")]
        public System.Nullable<bool> AnyoneCanAddSelf
        {
            get
            {
                return this._anyoneCanAddSelf;
            }
            set
            {
                this._anyoneCanAddSelf = value;
                FirePropertyChanged("AnyoneCanAddSelf");
            }
        }

        /// <summary>The attendees of the event.</summary>
        [Newtonsoft.Json.JsonPropertyAttribute("attendees")]
        public System.Collections.Generic.List<EventAttendee> Attendees
        {
            get
            {
                return this._attendees;
            }
            set
            {
                this._attendees = value;
                FirePropertyChanged("Attendees");
            }
        }

        /// <summary>Whether attendees may have been omitted from the event&apos;s representation. When retrieving an event, this may be due to a restriction specified by the &apos;maxAttendee&apos; query parameter. When updating an event, this can be used to only update the participant&apos;s response. Optional. The default is False.</summary>
        [Newtonsoft.Json.JsonPropertyAttribute("attendeesOmitted")]
        public System.Nullable<bool> AttendeesOmitted
        {
            get
            {
                return this._attendeesOmitted;
            }
            set
            {
                this._attendeesOmitted = value;
                FirePropertyChanged("AttendeesOmitted");
            }
        }

        /// <summary>The color of the event. This is an ID referring to an entry in the &quot;event&quot; section of the colors definition (see the &quot;colors&quot; endpoint). Optional.</summary>
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
                FirePropertyChanged("ColorId");
            }
        }

        /// <summary>Creation time of the event (as a RFC 3339 timestamp). Read-only.</summary>
        [Newtonsoft.Json.JsonPropertyAttribute("created")]
        public string Created
        {
            get
            {
                return this._created;
            }
            set
            {
                this._created = value;
                FirePropertyChanged("Created");
            }
        }

        /// <summary>The creator of the event. Read-only.</summary>
        [Newtonsoft.Json.JsonPropertyAttribute("creator")]
        public Event.CreatorData Creator
        {
            get
            {
                return this._creator;
            }
            set
            {
                this._creator = value;
                FirePropertyChanged("Creator");
            }
        }

        /// <summary>Description of the event. Optional.</summary>
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
                FirePropertyChanged("Description");
            }
        }

        [Newtonsoft.Json.JsonPropertyAttribute("end")]
        public EventDateTime End
        {
            get
            {
                return this._end;
            }
            set
            {
                this._end = value;
                FirePropertyChanged("End");
            }
        }

        /// <summary>Whether the end time is actually unspecified. An end time is still provided for compatibility reasons, even if this attribute is set to True. The default is False.</summary>
        [Newtonsoft.Json.JsonPropertyAttribute("endTimeUnspecified")]
        public System.Nullable<bool> EndTimeUnspecified
        {
            get
            {
                return this._endTimeUnspecified;
            }
            set
            {
                this._endTimeUnspecified = value;
                FirePropertyChanged("EndTimeUnspecified");
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
                FirePropertyChanged("ETag");
            }
        }

        /// <summary>Extended properties of the event.</summary>
        [Newtonsoft.Json.JsonPropertyAttribute("extendedProperties")]
        public Event.ExtendedPropertiesData ExtendedProperties
        {
            get
            {
                return this._extendedProperties;
            }
            set
            {
                this._extendedProperties = value;
                FirePropertyChanged("ExtendedProperties");
            }
        }

        /// <summary>A gadget that extends this event.</summary>
        [Newtonsoft.Json.JsonPropertyAttribute("gadget")]
        public Event.GadgetData Gadget
        {
            get
            {
                return this._gadget;
            }
            set
            {
                this._gadget = value;
                FirePropertyChanged("Gadget");
            }
        }

        /// <summary>Whether attendees other than the organizer can invite others to the event. Optional. The default is False.</summary>
        [Newtonsoft.Json.JsonPropertyAttribute("guestsCanInviteOthers")]
        public System.Nullable<bool> GuestsCanInviteOthers
        {
            get
            {
                return this._guestsCanInviteOthers;
            }
            set
            {
                this._guestsCanInviteOthers = value;
                FirePropertyChanged("GuestsCanInviteOthers");
            }
        }

        /// <summary>Whether attendees other than the organizer can modify the event. Optional. The default is False.</summary>
        [Newtonsoft.Json.JsonPropertyAttribute("guestsCanModify")]
        public System.Nullable<bool> GuestsCanModify
        {
            get
            {
                return this._guestsCanModify;
            }
            set
            {
                this._guestsCanModify = value;
                FirePropertyChanged("GuestsCanModify");
            }
        }

        /// <summary>Whether attendees other than the organizer can see who the event&apos;s attendees are. Optional. The default is False.</summary>
        [Newtonsoft.Json.JsonPropertyAttribute("guestsCanSeeOtherGuests")]
        public System.Nullable<bool> GuestsCanSeeOtherGuests
        {
            get
            {
                return this._guestsCanSeeOtherGuests;
            }
            set
            {
                this._guestsCanSeeOtherGuests = value;
                FirePropertyChanged("Completed");
            }
        }

        /// <summary>An absolute link to the Google+ hangout associated with this event. Read-only.</summary>
        [Newtonsoft.Json.JsonPropertyAttribute("GuestsCanSeeOtherGuests")]
        public string HangoutLink
        {
            get
            {
                return this._hangoutLink;
            }
            set
            {
                this._hangoutLink = value;
                FirePropertyChanged("HangoutLink");
            }
        }

        /// <summary>An absolute link to this event in the Google Calendar Web UI. Read-only.</summary>
        [Newtonsoft.Json.JsonPropertyAttribute("htmlLink")]
        public string HtmlLink
        {
            get
            {
                return this._htmlLink;
            }
            set
            {
                this._htmlLink = value;
                FirePropertyChanged("HtmlLink");
            }
        }

        /// <summary>Event ID in the iCalendar format.</summary>
        [Newtonsoft.Json.JsonPropertyAttribute("iCalUID")]
        public string ICalUID
        {
            get
            {
                return this._iCalUID;
            }
            set
            {
                this._iCalUID = value;
                FirePropertyChanged("ICalUID");
            }
        }

        /// <summary>Identifier of the event.</summary>
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
                FirePropertyChanged("Id");
            }
        }

        /// <summary>Type of the resource (&quot;calendar#event&quot;).</summary>
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
                FirePropertyChanged("Kind");
            }
        }

        /// <summary>Geographic location of the event as free-form text. Optional.</summary>
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
                FirePropertyChanged("Location");
            }
        }

        /// <summary>Whether this is a locked event copy where no changes can be made to the main event fields &quot;summary&quot;, &quot;description&quot;, &quot;location&quot;, &quot;start&quot;, &quot;end&quot; or &quot;recurrence&quot;. The default is False. Read-Only.</summary>
        [Newtonsoft.Json.JsonPropertyAttribute("locked")]
        public System.Nullable<bool> Locked
        {
            get
            {
                return this._locked;
            }
            set
            {
                this._locked = value;
                FirePropertyChanged("Locked");
            }
        }

        /// <summary>The organizer of the event. If the organizer is also an attendee, this is indicated with a separate entry in &apos;attendees&apos; with the &apos;organizer&apos; field set to True. To change the organizer, use the &quot;move&quot; operation. Read-only, except when importing an event.</summary>
        [Newtonsoft.Json.JsonPropertyAttribute("organizer")]
        public Event.OrganizerData Organizer
        {
            get
            {
                return this._organizer;
            }
            set
            {
                this._organizer = value;
                FirePropertyChanged("Organizer");
            }
        }

        [Newtonsoft.Json.JsonPropertyAttribute("originalStartTime")]
        public EventDateTime OriginalStartTime
        {
            get
            {
                return this._originalStartTime;
            }
            set
            {
                this._originalStartTime = value;
                FirePropertyChanged("OriginalStartTime");
            }
        }

        /// <summary>Whether this is a private event copy where changes are not shared with other copies on other calendars. Optional. Immutable.</summary>
        [Newtonsoft.Json.JsonPropertyAttribute("privateCopy")]
        public System.Nullable<bool> PrivateCopy
        {
            get
            {
                return this._privateCopy;
            }
            set
            {
                this._privateCopy = value;
                FirePropertyChanged("PrivateCopy");
            }
        }

        /// <summary>List of RRULE, EXRULE, RDATE and EXDATE lines for a recurring event. This field is omitted for single events or instances of recurring events.</summary>
        [Newtonsoft.Json.JsonPropertyAttribute("recurrence")]
        public System.Collections.Generic.List<string> Recurrence
        {
            get
            {
                return this._recurrence;
            }
            set
            {
                this._recurrence = value;
                FirePropertyChanged("Recurrence");
            }
        }

        /// <summary>For an instance of a recurring event, this is the event ID of the recurring event itself. Immutable.</summary>
        [Newtonsoft.Json.JsonPropertyAttribute("recurringEventId")]
        public string RecurringEventId
        {
            get
            {
                return this._recurringEventId;
            }
            set
            {
                this._recurringEventId = value;
                FirePropertyChanged("RecurringEventId");
            }
        }

        /// <summary>Information about the event&apos;s reminders for the authenticated user.</summary>
        [Newtonsoft.Json.JsonPropertyAttribute("reminders")]
        public Event.RemindersData Reminders
        {
            get
            {
                return this._reminders;
            }
            set
            {
                this._reminders = value;
                FirePropertyChanged("Reminders");
            }
        }

        /// <summary>Sequence number as per iCalendar.</summary>
        [Newtonsoft.Json.JsonPropertyAttribute("sequence")]
        public System.Nullable<long> Sequence
        {
            get
            {
                return this._sequence;
            }
            set
            {
                this._sequence = value;
                FirePropertyChanged("Sequence");
            }
        }

        [Newtonsoft.Json.JsonPropertyAttribute("start")]
        public EventDateTime Start
        {
            get
            {
                return this._start;
            }
            set
            {
                this._start = value;
                FirePropertyChanged("Start");
            }
        }

        /// <summary>Status of the event. Optional. Possible values are:  
        ///- &quot;confirmed&quot; - The event is confirmed. This is the default status. 
        ///- &quot;tentative&quot; - The event is tentatively confirmed. 
        ///- &quot;cancelled&quot; - The event is cancelled.</summary>
        [Newtonsoft.Json.JsonPropertyAttribute("status")]
        public string Status
        {
            get
            {
                return this._status;
            }
            set
            {
                this._status = value;
                FirePropertyChanged("Status");
            }
        }

        /// <summary>Title of the event.</summary>
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
                FirePropertyChanged("Summary");
            }
        }

        /// <summary>Whether the event blocks time on the calendar. Optional. Possible values are:  
        ///- &quot;opaque&quot; - The event blocks time on the calendar. This is the default value. 
        ///- &quot;transparent&quot; - The event does not block time on the calendar.</summary>
        [Newtonsoft.Json.JsonPropertyAttribute("transparency")]
        public string Transparency
        {
            get
            {
                return this._transparency;
            }
            set
            {
                this._transparency = value;
                FirePropertyChanged("Transparency");
            }
        }

        /// <summary>Last modification time of the event (as a RFC 3339 timestamp). Read-only.</summary>
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
                FirePropertyChanged("Updated");
            }
        }

        /// <summary>Visibility of the event. Optional. Possible values are:  
        ///- &quot;default&quot; - Uses the default visibility for events on the calendar. This is the default value. 
        ///- &quot;public&quot; - The event is public and event details are visible to all readers of the calendar. 
        ///- &quot;private&quot; - The event is private and only event attendees may view event details. 
        ///- &quot;confidential&quot; - The event is private. This value is provided for compatibility reasons.</summary>
        [Newtonsoft.Json.JsonPropertyAttribute("visibility")]
        public string Visibility
        {
            get
            {
                return this._visibility;
            }
            set
            {
                this._visibility = value;
                FirePropertyChanged("Visibility");
            }
        }

        /// <summary>The creator of the event. Read-only.</summary>
        public class CreatorData
        {

            private string _displayName;

            private string _email;

            private string _id;

            private System.Nullable<bool> _self;

            /// <summary>The creator&apos;s name, if available.</summary>
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

            /// <summary>The creator&apos;s email address, if available.</summary>
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

            /// <summary>The creator&apos;s Profile ID, if available.</summary>
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

            /// <summary>Whether the creator corresponds to the calendar on which this copy of the event appears. Read-only. The default is False.</summary>
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

        /// <summary>Extended properties of the event.</summary>
        public class ExtendedPropertiesData
        {

            private ExtendedPropertiesData.PrivateData _private;

            private ExtendedPropertiesData.SharedData _shared;

            /// <summary>Properties that are private to the copy of the event that appears on this calendar.</summary>
            [Newtonsoft.Json.JsonPropertyAttribute("private")]
            public ExtendedPropertiesData.PrivateData Private
            {
                get
                {
                    return this._private;
                }
                set
                {
                    this._private = value;
                }
            }

            /// <summary>Properties that are shared between copies of the event on other attendees&apos; calendars.</summary>
            [Newtonsoft.Json.JsonPropertyAttribute("shared")]
            public ExtendedPropertiesData.SharedData Shared
            {
                get
                {
                    return this._shared;
                }
                set
                {
                    this._shared = value;
                }
            }

            /// <summary>Properties that are private to the copy of the event that appears on this calendar.</summary>
            public class PrivateData : System.Collections.Generic.Dictionary<string, string>
            {
            }

            /// <summary>Properties that are shared between copies of the event on other attendees&apos; calendars.</summary>
            public class SharedData : System.Collections.Generic.Dictionary<string, string>
            {
            }
        }

        /// <summary>A gadget that extends this event.</summary>
        public class GadgetData
        {

            private string _display;

            private System.Nullable<long> _height;

            private string _iconLink;

            private string _link;

            private GadgetData.PreferencesData _preferences;

            private string _title;

            private string _type;

            private System.Nullable<long> _width;

            /// <summary>The gadget&apos;s display mode. Optional. Possible values are:  
            ///- &quot;icon&quot; - The gadget displays next to the event&apos;s title in the calendar view. 
            ///- &quot;chip&quot; - The gadget displays when the event is clicked.</summary>
            [Newtonsoft.Json.JsonPropertyAttribute("display")]
            public string Display
            {
                get
                {
                    return this._display;
                }
                set
                {
                    this._display = value;
                }
            }

            /// <summary>The gadget&apos;s height in pixels. Optional.</summary>
            [Newtonsoft.Json.JsonPropertyAttribute("height")]
            public System.Nullable<long> Height
            {
                get
                {
                    return this._height;
                }
                set
                {
                    this._height = value;
                }
            }

            /// <summary>The gadget&apos;s icon URL.</summary>
            [Newtonsoft.Json.JsonPropertyAttribute("iconLink")]
            public string IconLink
            {
                get
                {
                    return this._iconLink;
                }
                set
                {
                    this._iconLink = value;
                }
            }

            /// <summary>The gadget&apos;s URL.</summary>
            [Newtonsoft.Json.JsonPropertyAttribute("link")]
            public string Link
            {
                get
                {
                    return this._link;
                }
                set
                {
                    this._link = value;
                }
            }

            /// <summary>Preferences.</summary>
            [Newtonsoft.Json.JsonPropertyAttribute("preferences")]
            public GadgetData.PreferencesData Preferences
            {
                get
                {
                    return this._preferences;
                }
                set
                {
                    this._preferences = value;
                }
            }

            /// <summary>The gadget&apos;s title.</summary>
            [Newtonsoft.Json.JsonPropertyAttribute("title")]
            public string Title
            {
                get
                {
                    return this._title;
                }
                set
                {
                    this._title = value;
                }
            }

            /// <summary>The gadget&apos;s type.</summary>
            [Newtonsoft.Json.JsonPropertyAttribute("type")]
            public string Type
            {
                get
                {
                    return this._type;
                }
                set
                {
                    this._type = value;
                }
            }

            /// <summary>The gadget&apos;s width in pixels. Optional.</summary>
            [Newtonsoft.Json.JsonPropertyAttribute("width")]
            public System.Nullable<long> Width
            {
                get
                {
                    return this._width;
                }
                set
                {
                    this._width = value;
                }
            }

            /// <summary>Preferences.</summary>
            public class PreferencesData : System.Collections.Generic.Dictionary<string, string>
            {
            }
        }

        /// <summary>The organizer of the event. If the organizer is also an attendee, this is indicated with a separate entry in &apos;attendees&apos; with the &apos;organizer&apos; field set to True. To change the organizer, use the &quot;move&quot; operation. Read-only, except when importing an event.</summary>
        public class OrganizerData
        {

            private string _displayName;

            private string _email;

            private string _id;

            private System.Nullable<bool> _self;

            /// <summary>The organizer&apos;s name, if available.</summary>
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

            /// <summary>The organizer&apos;s email address, if available.</summary>
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

            /// <summary>The organizer&apos;s Profile ID, if available.</summary>
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

            /// <summary>Whether the organizer corresponds to the calendar on which this copy of the event appears. Read-only. The default is False.</summary>
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

        /// <summary>Information about the event&apos;s reminders for the authenticated user.</summary>
        public class RemindersData
        {

            private System.Collections.Generic.List<EventReminder> _overrides;

            private System.Nullable<bool> _useDefault;

            /// <summary>If the event doesn&apos;t use the default reminders, this lists the reminders specific to the event, or, if not set, indicates that no reminders are set for this event.</summary>
            [Newtonsoft.Json.JsonPropertyAttribute("overrides")]
            public System.Collections.Generic.List<EventReminder> Overrides
            {
                get
                {
                    return this._overrides;
                }
                set
                {
                    this._overrides = value;
                }
            }

            /// <summary>Whether the default reminders of the calendar apply to the event.</summary>
            [Newtonsoft.Json.JsonPropertyAttribute("useDefault")]
            public System.Nullable<bool> UseDefault
            {
                get
                {
                    return this._useDefault;
                }
                set
                {
                    this._useDefault = value;
                }
            }
        }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        private void FirePropertyChanged(string property)
        {
            var e = PropertyChanged;
            if (e != null)
                e(this, new System.ComponentModel.PropertyChangedEventArgs(property));
        }

        public void UpdateFrom(Event other)
        {
            if (other.AnyoneCanAddSelf != null) AnyoneCanAddSelf = other.AnyoneCanAddSelf;
            if (other.Attendees != null) Attendees = other.Attendees;
            if (other.AttendeesOmitted != null) AttendeesOmitted = other.AttendeesOmitted;
            if (other.ColorId != null) ColorId = other.ColorId;
            if (other.Created != null) Created = other.Created;
            if (other.Creator != null) Creator = other.Creator;
            if (other.Description != null) Description = other.Description;
            if (other.End != null) End = other.End;
            if (other.EndTimeUnspecified != null) EndTimeUnspecified = other.EndTimeUnspecified;
            if (other.ExtendedProperties != null) ExtendedProperties = other.ExtendedProperties;
            if (other.Gadget != null) Gadget = other.Gadget;
            if (other.GuestsCanInviteOthers != null) GuestsCanInviteOthers = other.GuestsCanInviteOthers;
            if (other.GuestsCanModify != null) GuestsCanModify = other.GuestsCanModify;
            if (other.GuestsCanSeeOtherGuests != null) GuestsCanSeeOtherGuests = other.GuestsCanSeeOtherGuests;
            if (other.HangoutLink != null) HangoutLink = other.HangoutLink;
            if (other.HtmlLink != null) HtmlLink = other.HtmlLink;
            if (other.ICalUID != null) ICalUID = other.ICalUID;
            if (other.Location != null) Location = other.Location;
            if (other.Locked != null) Locked = other.Locked;
            if (other.Organizer != null) Organizer = other.Organizer;
            if (other.OriginalStartTime != null) OriginalStartTime = other.OriginalStartTime;
            if (other.PrivateCopy != null) PrivateCopy = other.PrivateCopy;
            if (other.Recurrence != null) Recurrence = other.Recurrence;
            if (other.RecurringEventId != null) RecurringEventId = other.RecurringEventId;
            if (other.Reminders != null) Reminders = other.Reminders;
            if (other.Sequence != null) Sequence = other.Sequence;
            if (other.Start != null) Start = other.Start;
            if (other.Summary != null) Summary = other.Summary;
            if (other.Transparency != null) Transparency = other.Transparency;
            if (other.Updated != null) Updated = other.Updated;
            if (other.Visibility != null) Visibility = other.Visibility;
        }
    }
}
