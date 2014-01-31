using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using PhoneApp.Model;
using PhoneApp.Model.Data;
using RestSharp;



namespace PhoneApp.Page
{
    public partial class CreateTask : PhoneApplicationPage
    {
        private TaskComponent _taskComponent = TaskComponent.Instance;
        private string _idTaskList = String.Empty;
        private string _idTask = String.Empty;
        private Task _task;

        public CreateTask()
        {
            InitializeComponent();
        }

        /**
         * Button Save Click handler
         *  Either create a task or update the existing task
         */
        private void ApplicationBarIconButton_Save(object sender, EventArgs e)
        {
            if (_task == null)
            {
                _task = new Task();
                _task.Title = taskTitle.Text;
                _task.Status = Task.Statuses.NeedsAction;
                _task.Notes = Notes.Text;
                DateTime dt = (DateTime)Deadline.Value;
                DateTime dTime = new DateTime(dt.Year, dt.Month, dt.Day);
                _task.Due = Tools.convertDateToRFC3339(dTime, false);
                if (_idTaskList == string.Empty)
                {
                    MessageBoxResult warning = MessageBox.Show("No tasklist has been selected. This task will be saved in the default list.", "Confirmation", MessageBoxButton.OKCancel);
                    if (warning == MessageBoxResult.Cancel)
                    {
                        NavigationService.GoBack();
                    }
                    else
                    {
                        _idTaskList = "@default";
                        _taskComponent.InsertTask((response) =>
                        {
                            if (response.Data != null)
                            {
                                _idTask = response.Data.Id;
                                //MessageBox.Show("Your task has been added");
                                NavigationService.GoBack();
                            }
                            else MessageBox.Show("An error occured, please try again", "Task", MessageBoxButton.OK);
                        }, _idTaskList, _task);
                    }
                }
                else
                {
                    _task.IdList = _idTaskList;
                    _taskComponent.InsertTask((response) =>
                    {
                        if (response.Data != null)
                        {
                            //MessageBox.Show("Your task has been added");
                            String uri = string.Format("/Page/Home.xaml?tab=" + Home.ONGLET_TO_DO);
                            NavigationService.Navigate(new Uri(uri, UriKind.Relative));
                        }
                        else MessageBox.Show("An error occured, please try again", "Task", MessageBoxButton.OK);
                    }, _idTaskList, _task);
                }
            }
            else
            {
                _task.Title = taskTitle.Text;
                _task.Status = Task.Statuses.NeedsAction;
                _task.Notes = Notes.Text;
                DateTime dt2 = (DateTime)Deadline.Value;
                DateTime dTime2 = new DateTime(dt2.Year, dt2.Month, dt2.Day);
                _task.Due = Tools.convertDateToRFC3339(dTime2, false);
                _taskComponent.UpdateTask(TaskUpdated, _idTaskList, _task);
            }
        }



        private void TaskUpdated(IRestResponse<Task> response)
        {
            if (response.Data == null)
            {
                MessageBox.Show("An error occured, please try again", "Task", MessageBoxButton.OK);
                return;
            }
            //MessageBox.Show("Your task has been updated");
            //_taskComponent.LoadTasks((response2) => { }, _idTaskList);
            string uri =
            string.Format("/Page/ViewTask.xaml?idTask={0}&idTaskList={1}", _task.Id, _task.IdList);
            NavigationService.Navigate(new Uri(uri, UriKind.Relative));
        }


        /*
         * Query the necessary information from the URI and
         * Initializes the page fields
         */
        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            if (_task != null)
            {
                PageTitle.Text = "Edit Task";
                taskTitle.Text = _task.Title != null ? _task.Title : ""; ;
                Notes.Text = _task.Notes != null ? _task.Notes : "";
                Deadline.Value = _task.Due != null ? DateTime.Parse(_task.Due) : DateTime.Today;
                taskListButton.IsEnabled = false;
                taskListButton.Opacity = 0;
                taskListTitle.Width = 350;
            }
            else if (e.Uri.ToString().Contains("idTask="))
            {
                try
                {
                    _idTaskList = NavigationContext.QueryString["idTaskList"];
                    _idTask = NavigationContext.QueryString["idTask"];
                    _task = _taskComponent.getTaskById(_idTaskList, _idTask);
                    taskTitle.Text = _task.Title != null ? _task.Title : ""; ;
                    Notes.Text = _task.Notes != null ? _task.Notes : "";
                    Deadline.Value = _task.Due != null ? DateTime.Parse(_task.Due) : DateTime.Today;
                    PageTitle.Text = "Edit Task";
                    taskListTitle.Text = _taskComponent.getTaskListById(_idTaskList).Title;
                    taskListButton.IsEnabled = false;
                    taskListButton.Opacity = 0;
                    taskListTitle.Width = 350;
                }
                catch
                {
                }
            }
            else
            {
                try
                {
                    _idTaskList = NavigationContext.QueryString["idTaskList"];
                    taskListTitle.Text = NavigationContext.QueryString["title"];
                    if (e.Uri.ToString().Contains("taskNotes="))
                    {
                        Notes.Text = NavigationContext.QueryString["taskNotes"];
                        Deadline.Value = DateTime.ParseExact(NavigationContext.QueryString["taskDeadline"], "yyyyMMddHHmmss", null);
                        taskTitle.Text = NavigationContext.QueryString["taskTitle"];
                    }

                }
                catch { }
                PageTitle.Text = "New Task";
            }

        }


        /**
         * Button Select TaskList Click handler
         *  Redirects to the tasklist manager page to select a Tasklist
         *  
         */
        private void Select_taskList(object sender, RoutedEventArgs e)
        {
            string uri = string.Format("/Page/TaskListManager.xaml?next=CreateTask.xaml&taskNotes={0}&taskTitle={1}&taskDeadline={2}", Notes.Text, taskTitle.Text, ((DateTime)Deadline.Value).ToString("yyyyMMddHHmmss"));
            NavigationService.Navigate(new Uri(uri, UriKind.Relative));
        }

        /*
         * Back Key Button click handler
         */
        protected override void OnBackKeyPress(System.ComponentModel.CancelEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Are you sure? The modifications won't be saved.", "Confirmation", MessageBoxButton.OKCancel);
            if (result == MessageBoxResult.OK)
            {
                var lastPage = NavigationService.BackStack.FirstOrDefault();

                if (lastPage != null && lastPage.Source.ToString().Contains("/TaskListManager.xaml") || lastPage.Source.ToString().Contains("/Home.xaml"))
                {
                    NavigationService.Navigate(new Uri("/Page/Home.xaml?tab=" + Home.ONGLET_TO_DO, UriKind.Relative));
                }
                else
                {
                    NavigationService.GoBack();
                }

            }
            else
            {
                e.Cancel = true;
            }
        }

        private void ApplicationTitle_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Page/Home.xaml?tab=" + Home.ONGLET_TO_DO, UriKind.Relative));
        }
    }
}