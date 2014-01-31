namespace PhoneApp.Model.Data
{
    public class Error
    {

        private string _domain;

        private string _reason;

        /// <summary>Domain, or broad category, of the error.</summary>
        [Newtonsoft.Json.JsonPropertyAttribute("domain")]
        public string Domain
        {
            get
            {
                return this._domain;
            }
            set
            {
                this._domain = value;
            }
        }

        /// <summary>Specific reason for the error. Some of the possible values are:  
        ///- &quot;groupTooBig&quot; - The group of users requested is too large for a single query. 
        ///- &quot;tooManyCalendarsRequested&quot; - The number of calendars requested is too large for a single query. 
        ///- &quot;notFound&quot; - The requested resource was not found. 
        ///- &quot;internalError&quot; - The API service has encountered an internal error.  Additional error types may be added in the future, so clients should gracefully handle additional error statuses not included in this list.</summary>
        [Newtonsoft.Json.JsonPropertyAttribute("reason")]
        public string Reason
        {
            get
            {
                return this._reason;
            }
            set
            {
                this._reason = value;
            }
        }
    }
}
