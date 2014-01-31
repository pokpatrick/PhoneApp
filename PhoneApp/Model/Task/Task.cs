namespace PhoneApp.Model.Data
{
    public class Task : System.ComponentModel.INotifyPropertyChanged, System.IComparable<Task>, System.IEquatable<Task>
    {
        public sealed class Statuses
        {
            private Statuses() { }
            public const string NeedsAction = "needsAction";
            public const string Completed = "completed";
            public readonly string[] List = { "needsAction", "completed" };
        }

        private string _completed;

        private System.Nullable<bool> _deleted;

        private string _due;

        private string _etag;

        private System.Nullable<bool> _hidden;

        private string _id;

        private string _idList;

        private string _kind;

        private System.Collections.Generic.List<Task.LinksData> _links;

        private string _notes;

        private string _parent;

        private string _position;

        private string _selfLink;

        private string _status;

        private string _title;

        private string _updated;

        /// <summary>Completion date of the task (as a RFC 3339 timestamp). This field is omitted if the task has not been completed.</summary>
        [Newtonsoft.Json.JsonPropertyAttribute("completed")]
        public string Completed
        {
            get
            {
                return this._completed;
            }
            set
            {
                this._completed = value;
                FirePropertyChanged("Completed");
            }
        }

        /// <summary>Flag indicating whether the task has been deleted. The default if False.</summary>
        [Newtonsoft.Json.JsonPropertyAttribute("deleted")]
        public System.Nullable<bool> Deleted
        {
            get
            {
                return this._deleted;
            }
            set
            {
                this._deleted = value;
                FirePropertyChanged("Deleted");
            }
        }

        /// <summary>Due date of the task (as a RFC 3339 timestamp). Optional.</summary>
        [Newtonsoft.Json.JsonPropertyAttribute("due")]
        public string Due
        {
            get
            {
                return this._due;
            }
            set
            {
                this._due = value;
                FirePropertyChanged("Due");
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

        /// <summary>Flag indicating whether the task is hidden. This is the case if the task had been marked completed when the task list was last cleared. The default is False. This field is read-only.</summary>
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

        /// <summary>Task identifier.</summary>
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

        /// <summary>Type of the resource. This is always &quot;tasks#task&quot;.</summary>
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

        /// <summary>Collection of links. This collection is read-only.</summary>
        [Newtonsoft.Json.JsonPropertyAttribute("links")]
        public System.Collections.Generic.List<Task.LinksData> Links
        {
            get
            {
                return this._links;
            }
            set
            {
                this._links = value;
            }
        }

        /// <summary>Notes describing the task. Optional.</summary>
        [Newtonsoft.Json.JsonPropertyAttribute("notes")]
        public string Notes
        {
            get
            {
                return this._notes;
            }
            set
            {
                this._notes = value;
                FirePropertyChanged("Notes");
            }
        }

        /// <summary>Parent task identifier. This field is omitted if it is a top-level task. This field is read-only. Use the &quot;move&quot; method to move the task under a different parent or to the top level.</summary>
        [Newtonsoft.Json.JsonPropertyAttribute("parent")]
        public string Parent
        {
            get
            {
                return this._parent;
            }
            set
            {
                this._parent = value;
            }
        }

        /// <summary>String indicating the position of the task among its sibling tasks under the same parent task or at the top level. If this string is greater than another task&apos;s corresponding position string according to lexicographical ordering, the task is positioned after the other task under the same parent task (or at the top level). This field is read-only. Use the &quot;move&quot; method to move the task to another position.</summary>
        [Newtonsoft.Json.JsonPropertyAttribute("position")]
        public string Position
        {
            get
            {
                return this._position;
            }
            set
            {
                this._position = value;
            }
        }

        /// <summary>URL pointing to this task. Used to retrieve, update, or delete this task.</summary>
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

        /// <summary>Status of the task. This is either &quot;needsAction&quot; or &quot;completed&quot;.</summary>
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
                if(value.Equals(Task.Statuses.NeedsAction))
                    Completed = null;
                FirePropertyChanged("IsCompleted");
            }
        }

        /// <summary>Title of the task.</summary>
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

        /// <summary>Last modification time of the task (as a RFC 3339 timestamp).</summary>
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

        public string IdList
        {
            get
            {
                return this._idList;
            }
            set
            {
                this._idList = value;
            }
        }

        public bool IsCompleted
        {
            get
            {
                if(this._status != null)
                    return Status.Equals(Statuses.Completed);
                return false;
            }
            set
            {
                this._status = this._status.Equals(Statuses.Completed) ? Statuses.NeedsAction : Statuses.Completed;
                FirePropertyChanged("IsCompleted");
            }
        }

        public class LinksData
        {

            private string _description;

            private string _link;

            private string _type;

            /// <summary>The description. In HTML speak: Everything between &lt;a&gt; and &lt;/a&gt;.</summary>
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

            /// <summary>The URL.</summary>
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

            /// <summary>Type of the link, e.g. &quot;email&quot;.</summary>
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
        }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        private void FirePropertyChanged(string property)
        {
            var e = PropertyChanged;
            if (e != null)
                e(this, new System.ComponentModel.PropertyChangedEventArgs(property));
        }

        public int CompareTo(Task other)
        {
            if (other.Due == null) return 1;
            if (Due == null) return -1;
            return Due.CompareTo(other.Due);
        }

        public void UpdateFrom(Task other)
        {
            if (other.Completed != null) Completed = other.Completed;
            if (other.Deleted != null) Deleted = other.Deleted;
            if (other.Due != null) Due = other.Due;
            if (other.Notes != null) Notes = other.Notes;
            if (other.Parent != null) Parent = other.Parent;
            if (other.Position != null) Position = other.Position;
            if (other.SelfLink != null) SelfLink = other.SelfLink;
            if (other.Status != null) Status = other.Status;
            if (other.Title != null) Title = other.Title;
            if (other.Updated != null) Updated = other.Updated;
        }

        public bool Equals(Task other)
        {
            return Id.Equals(other.Id);
        }
    }
}
