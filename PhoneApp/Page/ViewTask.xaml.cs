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
    public partial class ViewTask : PhoneApplicationPage
    {
        private TaskComponent _taskComponent = TaskComponent.Instance;
        private string _idTaskList = String.Empty;
        private string _idTask = String.Empty;
        private Task _task = null;

        public ViewTask()
        {
            InitializeComponent();
        }


        /**
         * Button Add task Click handler
         *  Redirects to the page to add a task
         */
        private void ApplicationBarIconButton_Add(object sender, EventArgs e)
        {
            string uri =
                string.Format("/Page/CreateTask.xaml?idTaskList={0}&title={1}", _task.IdList, taskListTitle.Text);
            NavigationService.Navigate(new Uri(uri, UriKind.Relative));
        }


        /**
         * Button Edit Click handler
         *  Redirects to the page to edit the task
         */
        private void ApplicationBarIconButton_Edit(object sender, EventArgs e)
        {
            string uri =
                string.Format("/Page/CreateTask.xaml?idTaskList={0}&idTask={1}&taskListTitle={2}", _idTaskList, _idTask , taskListTitle);
            NavigationService.Navigate(new Uri(uri, UriKind.Relative));
        }

        /**
         * Button Delete Click handler
         *  Delete the task and 
         *  Redirects to last page visited
         */
        private void ApplicationBarIconButton_Delete(object sender, EventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Are you sure? This task will be deleted.", "Confirmation", MessageBoxButton.OKCancel);
            if (result == MessageBoxResult.OK)
            {
                _taskComponent.DeleteTask(TaskDeleted, _idTaskList, _task);
            }
        }


        private void TaskDeleted(IRestResponse<Error> response)
        {
            if (response.Data == null)
            {
                //MessageBox.Show("Task Deleted");
                NavigationService.Navigate(new Uri("/Page/Home.xaml?tab=" + Home.ONGLET_TO_DO, UriKind.Relative));
            }
            else MessageBox.Show("An error occured, please try again", "Task", MessageBoxButton.OK);

        }

        /*
         * Queries the necessary information from the URI and
         * Initializes the page fields
         */
        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (e.Uri.ToString().Contains("idTask="))
            {
                try
                {
                    _idTaskList = NavigationContext.QueryString["idTaskList"];
                    _idTask = NavigationContext.QueryString["idTask"];
                    
                    _task = _taskComponent.getTaskById(_idTaskList, _idTask);

                    taskListTitle.Text = _taskComponent.getTaskListById(_task.IdList).Title;
                    title.Text = _task.Title;
                    Notes.Text = _task.Notes;
                    deadlineDate.Text = DateTime.Parse(_task.Due).ToShortDateString();
                }
                catch { }
            }
            else
            {
                try
                {
                    _idTaskList = NavigationContext.QueryString["idTaskList"];
                    taskListTitle.Text = NavigationContext.QueryString["title"];
                }
                catch { }
            }
        }

        /*
         * Back Key Button click handler
         */
        protected override void OnBackKeyPress(System.ComponentModel.CancelEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Page/Home.xaml?tab=" + Home.ONGLET_TO_DO, UriKind.Relative));
        }

        private void ApplicationTitle_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Page/Home.xaml?tab=" + Home.ONGLET_TO_DO, UriKind.Relative));
        }
    }
}