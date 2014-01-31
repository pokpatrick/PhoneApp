namespace PhoneApp.Model.Data
{
    public class TaskLists
    {

        private string _etag;

        private System.Collections.Generic.List<TaskList> _items = new System.Collections.Generic.List<TaskList>();

        private string _kind;

        private string _nextPageToken;

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

        /// <summary>Collection of task lists.</summary>
        [Newtonsoft.Json.JsonPropertyAttribute("items")]
        public System.Collections.Generic.List<TaskList> Items
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

        /// <summary>Type of the resource. This is always &quot;tasks#taskLists&quot;.</summary>
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

        /// <summary>Token that can be used to request the next page of this result.</summary>
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
