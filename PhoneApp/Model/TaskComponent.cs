using System;
using System.Net;
using System.Linq;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Windows;
using System.Threading;

using RestSharp;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json.Linq;
using Microsoft.Phone.Controls;
using PhoneApp.Model.Data;

namespace PhoneApp.Model
{
    public class TaskComponent
    {

        public class MyContractResolver : DefaultContractResolver
        {
            protected override IList<JsonProperty> CreateProperties(JsonObjectContract contract)
            {
                IList<JsonProperty> properties = base.CreateProperties(contract);
                properties =
                  properties.Where(p => !p.PropertyName.Contains("IsCompleted") && !p.PropertyName.Contains("IdList")).ToList();

                return properties;
            }
        }

        #region Singleton Pattern

        private static TaskComponent _instance;
        public static TaskComponent Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new TaskComponent();
                }
                return _instance;
            }
        }
        private TaskComponent()
        {
            _client.Timeout = Tools.requestTimeOut;
        }

        #endregion

        #region Attributes

        private OAuth _oAuth = OAuth.Instance;
        private RestClient _client = new RestClient("https://www.googleapis.com");
        private JsonSerializerSettings _serializerSettings = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore };
        private TaskLists __taskLists;
        private Dictionary<string, ObservableCollection<Task>> _tasks = new Dictionary<string, ObservableCollection<Task>>();
        private ObservableCollection<TaskList> _taskLists = new ObservableCollection<TaskList>();
        private object _sync = new Object();
        private int _requesting = 0;
        #endregion

        #region Properties

        public ObservableCollection<TaskList> TaskLists
        {
            get { return _taskLists; }
            set { _taskLists = value; }
        }

        public Dictionary<string, ObservableCollection<Task>> Tasks
        {
            get { return _tasks; }
        }

        #endregion

        /*
         * Load All TaskList & All Task
         * 
         * @param: AllLoaded : a callback
         */
        public void LoadAll(Action AllLoaded)
        {
            LoadTaskLists(response =>
            {
                int i = 1;
                foreach (TaskList list in response.Data.Items)
                    LoadTasks(res =>
                    {
                        if (i++ == response.Data.Items.Count && AllLoaded != null)
                            AllLoaded();
                    }, list.Id);
            });
        }

        #region TaskList

        /*
         * Load All TaskList
         * 
         * @param: TaskListLoaded : a callback
         */
        public void LoadTaskLists(Action<IRestResponse<TaskLists>> TaskListLoaded)
        {
            _oAuth.GetAccessCode(access_token =>
            {
                lock (_sync)
                {
                    _requesting++;
                    // building request
                    _client.Authenticator = new OAuth2AuthorizationRequestHeaderAuthenticator(access_token);
                    var request = new RestRequest("/tasks/v1/users/@me/lists", Method.GET);
                    LaunchTimer();
                    _client.ExecuteAsync<TaskLists>(request, response =>
                    {
                        lock (_sync)
                        {
                            _requesting--;
                            // handle request's callback
                            __taskLists = response.Data != null ? response.Data : __taskLists;
                            _taskLists.Clear();
                            if (__taskLists.Items != null)
                                foreach (TaskList taskList in __taskLists.Items)
                                    _taskLists.Add(taskList);
                            if (TaskListLoaded != null) TaskListLoaded(response);
                        }
                    });
                }
            });
        }

        /*
         * Insert a TaskList
         * 
         * @param: TaskListInserted : a callback
         * @param: t : TaskList 
         */
        public void InsertTaskList(Action<IRestResponse<TaskList>> TaskListInserted, TaskList t)
        {
            _oAuth.GetAccessCode(access_token =>
            {
                lock (_sync)
                {
                    _requesting++;
                    // building request
                    _client.Authenticator = new OAuth2AuthorizationRequestHeaderAuthenticator(access_token);
                    var request = new RestRequest("/tasks/v1/users/@me/lists", Method.POST);
                    // serialize TaskList
                    string taskListJson = JsonConvert.SerializeObject(t, Formatting.None, _serializerSettings);
                    request.AddParameter("application/json; charset=utf-8", taskListJson, ParameterType.RequestBody);
                    LaunchTimer();
                    _client.ExecuteAsync<TaskList>(request, response =>
                    {
                        lock (_sync)
                        {
                            _requesting--;
                            // handle request's callback - adding the taskList
                            if (response.Data != null && _taskLists != null)
                                _taskLists.Add(response.Data);
                            if (TaskListInserted != null) TaskListInserted(response);
                        }
                    });
                }
            });
        }

        /*
         * Update a TaskList
         * 
         * @param: TaskListInserted : a callback
         * @param: t : TaskList 
         */
        public void UpdateTaskList(Action<IRestResponse<TaskList>> TaskListUpdated, TaskList t)
        {
            _oAuth.GetAccessCode(access_token =>
            {
                lock (_sync)
                {
                    _requesting++;
                    // building request
                    _client.Authenticator = new OAuth2AuthorizationRequestHeaderAuthenticator(access_token);
                    var request = new RestRequest("/tasks/v1/users/@me/lists/" + t.Id, Method.PUT);
                    // serialize TaskList
                    string taskListJson = JsonConvert.SerializeObject(t, Formatting.None, _serializerSettings);
                    request.AddParameter("application/json; charset=utf-8", taskListJson, ParameterType.RequestBody);
                    LaunchTimer();
                    _client.ExecuteAsync<TaskList>(request, response =>
                    {
                        lock (_sync)
                        {
                            _requesting--;
                            // handle request's callback
                            if (response.Data != null)
                            {
                                // update Tasklist by removing/adding
                                foreach (TaskList _t in _taskLists)
                                {
                                    if (!t.Id.Equals(_t.Id)) continue;
                                    _taskLists.Remove(_t);
                                    _taskLists.Add(response.Data);
                                    break;
                                }
                            }
                            if (TaskListUpdated != null) TaskListUpdated(response);
                        }
                    });
                }
            });
        }

        /*
         * Delete a TaskList
         * 
         * @param: TaskListInserted : a callback
         * @param: t : TaskList 
         */
        public void DeleteTaskList(Action<IRestResponse<Error>> TaskListDeleted, TaskList t)
        {

            _oAuth.GetAccessCode(access_token =>
            {
                lock (_sync)
                {
                    _requesting++;
                    // building request
                    _client.Authenticator = new OAuth2AuthorizationRequestHeaderAuthenticator(access_token);
                    var request = new RestRequest("/tasks/v1/users/@me/lists/" + t.Id, Method.DELETE);
                    // serialize TaskList
                    string taskListJson = JsonConvert.SerializeObject(t, Formatting.None, _serializerSettings);
                    request.AddParameter("application/json; charset=utf-8", taskListJson, ParameterType.RequestBody);
                    LaunchTimer();
                    _client.ExecuteAsync<Error>(request, response =>
                    {
                        lock (_sync)
                        {
                            _requesting--;
                            // handle request's callback
                            if (response.Data == null && response.StatusCode == HttpStatusCode.NoContent && response.Data == null)
                            {
                                // remove the taskList
                                foreach (TaskList _t in _taskLists)
                                {
                                    if (!t.Id.Equals(_t.Id)) continue;
                                    _taskLists.Remove(_t);
                                    _tasks.Remove(_t.Id);
                                    break;
                                }
                            }
                            if (TaskListDeleted != null) TaskListDeleted(response);
                        }
                    });
                }
            });
        }

        #endregion

        #region Task

        /*
         * Load Tasks
         * 
         * @param: TasksLoaded : a callback
         * @param: tasklist : string 
         */
        public void LoadTasks(Action<IRestResponse<Tasks>> TasksLoaded, string tasklist)
        {

            _oAuth.GetAccessCode(access_token =>
            {
                lock (_sync)
                {
                    _requesting++;
                    // building request
                    _client.Authenticator = new OAuth2AuthorizationRequestHeaderAuthenticator(access_token);
                    var request = new RestRequest("/tasks/v1/lists/" + tasklist + "/tasks", Method.GET);
                    LaunchTimer();
                    _client.ExecuteAsync<Tasks>(request, response =>
                    {
                        lock (_sync)
                        {
                            _requesting--;
                            if (response.Data != null)
                            {
                                if (_tasks.ContainsKey(tasklist))
                                    _tasks.Remove(tasklist);
                                ObservableCollection<Task> tmp = new ObservableCollection<Task>();
                                if (response.Data.Items != null)
                                    foreach (Task task in response.Data.Items)
                                        tmp.Add(task);
                                _tasks.Add(tasklist, tmp);
                            }
                            if (TasksLoaded != null) TasksLoaded(response);
                        }
                    });
                }
            });
        }

        /*
         * Insert a Task
         * 
         * @param: TaskInserted : a callback
         * @param: tasklist : string 
         * @param: t : Task
         */
        public void InsertTask(Action<IRestResponse<Task>> TaskInserted, string tasklist, Task t)
        {

            _oAuth.GetAccessCode(access_token =>
            {
                lock (_sync)
                {
                    _requesting++;
                    // building request
                    _client.Authenticator = new OAuth2AuthorizationRequestHeaderAuthenticator(access_token);
                    var request = new RestRequest("/tasks/v1/lists/" + tasklist + "/tasks", Method.POST);
                    // serialize Task
                    string taskJson = JsonConvert.SerializeObject(t, Formatting.None,
                        new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
                    request.AddParameter("application/json; charset=utf-8", taskJson, ParameterType.RequestBody);
                    LaunchTimer();
                    _client.ExecuteAsync<Task>(request, response =>
                    {
                        lock (_sync)
                        {
                            _requesting--;
                            // handle request's callback
                            ObservableCollection<Task> tasks = null;
                            _tasks.TryGetValue(tasklist, out tasks);
                            // insert the task
                            if (response.Data != null && tasks != null)
                                tasks.Add(response.Data);
                            if (TaskInserted != null) TaskInserted(response);
                        }
                    });
                }
            });
        }

        /*
         * Update a Task
         * 
         * @param: TaskUpdated : a callback
         * @param: tasklist : string 
         * @param: t : Task
         */
        public void UpdateTask(Action<IRestResponse<Task>> TaskUpdated, string tasklist, Task t)
        {
            _oAuth.GetAccessCode(access_token =>
            {
                lock (_sync)
                {
                    _requesting++;
                    // building request
                    _client.Authenticator = new OAuth2AuthorizationRequestHeaderAuthenticator(access_token);
                    var request = new RestRequest("/tasks/v1/lists/" + tasklist + "/tasks/" + t.Id, Method.PUT);
                    // serialize Task
                    string taskJson = JsonConvert.SerializeObject(t, Formatting.None,
                        new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore, ContractResolver = new MyContractResolver() });
                    request.AddParameter("application/json; charset=utf-8", taskJson, ParameterType.RequestBody);
                    LaunchTimer();
                    _client.ExecuteAsync<Task>(request, response =>
                    {
                        lock (_sync)
                        {
                            _requesting--;
                            // handle request's callback
                            ObservableCollection<Task> tasks = null;
                            _tasks.TryGetValue(tasklist, out tasks);
                            if (response.Data != null && tasks != null)
                            {
                                // update the task
                                foreach (Task _t in tasks)
                                {
                                    if (!t.Id.Equals(_t.Id)) continue;
                                    _t.UpdateFrom(t);
                                    break;
                                }
                            }
                            if (TaskUpdated != null) TaskUpdated(response);
                        }
                    });
                }
            });
        }

        /*
         * Delete a Task
         * 
         * @param: TaskDeleted : a callback
         * @param: tasklist : string 
         * @param: t : Task
         */
        public void DeleteTask(Action<IRestResponse<Error>> TaskDeleted, string tasklist, Task t)
        {
            _oAuth.GetAccessCode(access_token =>
            {
                lock (_sync)
                {
                    _requesting++;
                    // building request
                    _client.Authenticator = new OAuth2AuthorizationRequestHeaderAuthenticator(access_token);
                    var request = new RestRequest("/tasks/v1/lists/" + tasklist + "/tasks/" + t.Id, Method.DELETE);
                    // serialize Task
                    string taskJson = JsonConvert.SerializeObject(t, Formatting.None,
                        new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
                    request.AddParameter("application/json; charset=utf-8", taskJson, ParameterType.RequestBody);
                    LaunchTimer();
                    _client.ExecuteAsync<Error>(request, response =>
                    {
                        lock (_sync)
                        {
                            _requesting--;
                            // handle request's callback
                            ObservableCollection<Task> tasks = null;
                            _tasks.TryGetValue(tasklist, out tasks);
                            if (tasks != null && response.StatusCode == HttpStatusCode.NoContent && response.Data == null)
                            {
                                // delete the task
                                tasks.Remove(t);
                            }
                            if (TaskDeleted != null) TaskDeleted(response);
                        }
                    });
                }
            });
        }

        #endregion


        private void LaunchTimer()
        {
            new Timer(new TimerCallback(OnTimeOut), _requesting, Tools.requestTimeOut, 0);
        }

        private void OnTimeOut(object state)
        {
            lock (_sync)
            {
                if ((int)state != _requesting) return;
            }
            Deployment.Current.Dispatcher.BeginInvoke(() =>
            {
                _requesting--;
                MessageBox.Show("Connection timed out.", "Network", MessageBoxButton.OK);
            });
        }

        /*
         * Return a TaskList
         * 
         * @param: id_tasklist : string
         */
        public TaskList getTaskListById(string id)
        {
            foreach (TaskList taskList in _taskLists)
            {
                if (taskList.Id.Equals(id)) return taskList;
            }
            throw new Exception("TaskList not found");
        }

        /*
         * Return a Task
         * 
         * @param: id_tasklist : string
         * @param: id_task : string 
         */
        public Task getTaskById(string tasklist, string id)
        {
            ObservableCollection<Task> tasks;
            _tasks.TryGetValue(tasklist, out tasks);
            if (tasks == null) throw new KeyNotFoundException();
            foreach (Task task in tasks)
            {
                if (!task.Id.Equals(id)) continue;
                task.IdList = tasklist;
                return task;
            }
            throw new Exception("Task not found");
        }


        /*
         * Return All Task from All TaskList Sorted By due descendant
         * 
         */
        public Dictionary<string, List<Task>> getAllTask()
        {
            List<Task> needsAction = new List<Task>();
            List<Task> completed = new List<Task>();

            foreach (KeyValuePair<string, ObservableCollection<Task>> entry in _tasks)
            {
                if (entry.Value == null) continue;
                foreach (Task task in entry.Value)
                {
                    if (task.Title == String.Empty && task.Due == null && task.Notes == null) continue;
                    task.IdList = entry.Key;
                    if (task.Status.Equals(Task.Statuses.Completed))
                        completed.Add(task);
                    else
                        needsAction.Add(task);
                }
            }

            // sorting 
            //needsAction.Sort();
            //completed.Sort();

            needsAction.Sort(delegate(Task t1, Task t2) { if (t2.Due == null) return 1; if (t1.Due == null) return -1; return t1.Due.CompareTo(t2.Due); });
            completed.Sort(delegate(Task t1, Task t2) { if (t2.Due == null) return 1; if (t1.Due == null) return -1; return t1.Due.CompareTo(t2.Due); });

            Dictionary<string, List<Task>> tmp = new Dictionary<string, List<Task>>();
            tmp.Add(Task.Statuses.NeedsAction, needsAction);
            tmp.Add(Task.Statuses.Completed, completed);
            return tmp;
        }

        /*
         * Return All Task from All TaskList Sorted By due descendant
         * 
         */
        public ObservableCollection<Task> getAllTaskSortedByDue()
        {
            ObservableCollection<Task> tmp = new ObservableCollection<Task>();


            Dictionary<string, List<Task>> tasks = getAllTask();
            List<Task> needsAction;
            tasks.TryGetValue(Task.Statuses.NeedsAction, out needsAction);
            List<Task> completed;
            tasks.TryGetValue(Task.Statuses.Completed, out completed);

            foreach (Task task in needsAction)
                tmp.Add(task);
            foreach (Task task in completed)
                tmp.Add(task);

            return tmp;
            /*foreach (KeyValuePair<string, ObservableCollection<Task>> entry in _tasks)
            {
                if (entry.Value == null) continue;
                foreach (Task task in entry.Value)
                {
                    if (task.Title == String.Empty && task.Due == null && task.Notes == null) continue;
                    task.IdList = entry.Key;
                    tmp.Add(task);
                }
            }*/


            //return new ObservableCollection<Task>(tmp.OrderBy(task => task.Due));
        }
    }
}
