namespace PhoneApp.Model.Data
{
    public class TimePeriod
    {

        private string _end;

        private string _start;

        /// <summary>The (exclusive) end of the time period.</summary>
        [Newtonsoft.Json.JsonPropertyAttribute("end")]
        public string End
        {
            get
            {
                return this._end;
            }
            set
            {
                this._end = value;
            }
        }

        /// <summary>The (inclusive) start of the time period.</summary>
        [Newtonsoft.Json.JsonPropertyAttribute("start")]
        public string Start
        {
            get
            {
                return this._start;
            }
            set
            {
                this._start = value;
            }
        }
    }
}
