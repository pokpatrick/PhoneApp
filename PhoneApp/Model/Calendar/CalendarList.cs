namespace PhoneApp.Model.Data
{
    public class CalendarList
    {

        private string _etag;

        private System.Collections.Generic.List<CalendarListEntry> _items;

        private string _kind;

        private string _nextPageToken;

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

        /// <summary>Calendars that are present on the user&apos;s calendar list.</summary>
        [Newtonsoft.Json.JsonPropertyAttribute("items")]
        public System.Collections.Generic.List<CalendarListEntry> Items
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

        /// <summary>Type of the collection (&quot;calendar#calendarList&quot;).</summary>
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

        /// <summary>Token used to access the next page of this result.</summary>
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
    }
}
