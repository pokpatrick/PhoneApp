namespace PhoneApp.Model.Data
{
    public class EventDateTime
    {

        private string _date;

        private string _dateTime;

        private string _timeZone;

        /// <summary>The date, in the format &quot;yyyy-mm-dd&quot;, if this is an all-day event.</summary>
        [Newtonsoft.Json.JsonPropertyAttribute("date")]
        public string Date
        {
            get
            {
                return this._date;
            }
            set
            {
                this._date = value;
            }
        }

        /// <summary>The time, as a combined date-time value (formatted according to RFC 3339). A time zone offset is required unless a time zone is explicitly specified in &apos;timeZone&apos;.</summary>
        [Newtonsoft.Json.JsonPropertyAttribute("dateTime")]
        public string DateTime
        {
            get
            {
                return this._dateTime;
            }
            set
            {
                this._dateTime = value;
            }
        }

        /// <summary>The name of the time zone in which the time is specified (e.g. &quot;Europe/Zurich&quot;). Optional. The default is the time zone of the calendar.</summary>
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
