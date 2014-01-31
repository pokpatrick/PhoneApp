namespace PhoneApp.Model.Data
{
    public class TaskList : System.ComponentModel.INotifyPropertyChanged
    {

        private string _etag;

        private string _id;

        private string _kind;

        private string _selfLink;

        private string _title;

        private string _updated;

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

        /// <summary>Task list identifier.</summary>
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

        /// <summary>Type of the resource. This is always &quot;tasks#taskList&quot;.</summary>
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

        /// <summary>URL pointing to this task list. Used to retrieve, update, or delete this task list.</summary>
        [Newtonsoft.Json.JsonPropertyAttribute("selfLink")]
        public string SelfLink
        {
            get
            {
                return this._selfLink;
            }
            set
            {
                this._selfLink = value;
            }
        }

        /// <summary>Title of the task list.</summary>
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
                FirePropertyChanged("Title");
            }
        }

        /// <summary>Last modification time of the task list (as a RFC 3339 timestamp).</summary>
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

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        private void FirePropertyChanged(string property)
        {
            var e = PropertyChanged;
            if (e != null)
                e(this, new System.ComponentModel.PropertyChangedEventArgs(property));
        }
    }
}
