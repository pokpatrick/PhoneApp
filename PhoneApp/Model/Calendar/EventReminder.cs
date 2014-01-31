namespace PhoneApp.Model.Data
{
    public class EventReminder
    {

        private string _method;

        private System.Nullable<long> _minutes;

        /// <summary>The method used by this reminder. Possible values are:  
        ///- &quot;email&quot; - Reminders are sent via email. 
        ///- &quot;sms&quot; - Reminders are sent via SMS. 
        ///- &quot;popup&quot; - Reminders are sent via a UI popup.</summary>
        [Newtonsoft.Json.JsonPropertyAttribute("method")]
        public string Method
        {
            get
            {
                return this._method;
            }
            set
            {
                this._method = value;
            }
        }

        /// <summary>Number of minutes before the start of the event when the reminder should trigger.</summary>
        [Newtonsoft.Json.JsonPropertyAttribute("minutes")]
        public System.Nullable<long> Minutes
        {
            get
            {
                return this._minutes;
            }
            set
            {
                this._minutes = value;
            }
        }
    }
}
