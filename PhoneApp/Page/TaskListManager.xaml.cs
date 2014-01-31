using System;
using System.Collections.Generic;
using System.Windows;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using Microsoft.Phone.Controls;

using PhoneApp.Model;
using PhoneApp.Model.Data;
using RestSharp;
using Coding4Fun.Phone.Controls;

namespace PhoneApp.Page
{
    public partial class TaskListManager : PhoneApplicationPage
    {
        private TaskComponent _taskComponent = TaskComponent.Instance;
        private ObservableCollection<TaskList> _taskLists;
        private string _nextPage = String.Empty;
        private string _idTask = String.Empty;

        public TaskListManager()
        {
            InitializeComponent();
            _taskLists = _taskComponent.TaskLists;
            listBox.ItemsSource = _taskLists;
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            try
            {
                _nextPage = NavigationContext.QueryString["next"];
                _idTask = NavigationContext.QueryString["idTask"];
            }
            catch { }
        }

        /**
         * Go back to the previous page after select
         * 
         */
        private void listBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            TaskList taskList = listBox.SelectedItem as TaskList;
            if (taskList == null) return;
            string uri = _idTask != String.Empty ?
                string.Format("/Page/{0}?idTaskList={1}&title={2}&idTask={3}", _nextPage, taskList.Id, taskList.Title, _idTask)
            : string.Format("/Page/{0}?idTaskList={1}&title={2}", _nextPage, taskList.Id, taskList.Title);
            NavigationService.Navigate(new Uri(uri, UriKind.Relative));
        }

        /**
         * Button Delete Click handler
         * 
         */
        private void DeleteTaskList(object sender, RoutedEventArgs e)
        {
            TaskList taskList = (sender as Button).DataContext as TaskList;
            ListBoxItem pressedItem = this.listBox.ItemContainerGenerator.ContainerFromItem(taskList) as ListBoxItem;
            if (pressedItem != null)
            {
                MessageBoxResult res = MessageBox.Show(taskList.Title, "Are you sure you want to delete?", MessageBoxButton.OKCancel);
                if (res == MessageBoxResult.OK)
                {
                    progressBar.IsIndeterminate = true;
                    // start delete request
                    _taskComponent.DeleteTaskList(response =>
                    {
                        if (response.Data == null)
                        {
                            progressBar.IsIndeterminate = false;
                        }
                        else
                            MessageBox.Show("An error occured, please try again", "TaskList", MessageBoxButton.OK);
                    }, taskList);
                }
            }
        }

        /**
         * Button Edit Click handler
         * 
         */
        private void EditTaskList(object sender, RoutedEventArgs e)
        {
            TaskList taskList = (sender as Button).DataContext as TaskList;
            ListBoxItem pressedItem = this.listBox.ItemContainerGenerator.ContainerFromItem(taskList) as ListBoxItem;
            if (pressedItem != null)
            {
                InputPrompt input = new InputPrompt()
                {
                    Title = "Edit Task List",
                    Message = "Title",
                    IsCancelVisible = true,
                    Value = taskList.Title
                };
                input.Completed += (object obj, PopUpEventArgs<string, PopUpResult> args) =>
                {
                    if (args.PopUpResult == PopUpResult.Ok && args.Result.Length > 1)
                    {
                        progressBar.IsIndeterminate = true;
                        // start update request
                        _taskComponent.UpdateTaskList(response =>
                        {
                            progressBar.IsIndeterminate = false;
                            if (response.Data == null)
                            {
                                MessageBox.Show("An error occured, please try again", "TaskList", MessageBoxButton.OK);
                                return;
                            }
                            taskList.Title = args.Result;
                        }, new TaskList() { Id = taskList.Id, Title = args.Result });
                    }
                };
                input.Show();
            }
        }

        /**
         * Button Add Click handler
         * 
         */
        private void AddTaskList(object sender, EventArgs e)
        {
            InputPrompt input = new InputPrompt()
            {
                Title = "Create Task List",
                Message = "Title",
                IsCancelVisible = true
            };
            input.Completed += inputAdd_completed;
            input.Show();
        }

        /**
         * Input Add Prompt completed handler
         * 
         */
        private void inputAdd_completed(object sender, PopUpEventArgs<string, PopUpResult> e)
        {
            if (e.PopUpResult == PopUpResult.Ok)
            {
                if (e.Result.Length > 1)
                {
                    progressBar.IsIndeterminate = true;
                    _taskComponent.InsertTaskList(response =>
                    {
                        progressBar.IsIndeterminate = false;
                        if (response.Data == null)
                        {
                            MessageBox.Show("An error occured, please try again", "TaskList", MessageBoxButton.OK);
                            return;
                        }
                    }, new TaskList() { Title = e.Result });
                }
                else
                {
                    InputPrompt input = new InputPrompt()
                    {
                        Title = "Create Task List",
                        Message = "Title",
                        Value = e.Result,
                        IsCancelVisible = true
                    };
                    input.Completed += inputAdd_completed;
                    input.Show();
                }
            }
        }

        private void RefreshTaskList(object sender, EventArgs e)
        {
            progressBar.IsIndeterminate = true;
            _taskComponent.LoadTaskLists(response =>
            {
                progressBar.IsIndeterminate = false;
                if (response.Data == null)
                {
                    MessageBox.Show("An error occured, please try again", "TaskList", MessageBoxButton.OK);
                    return;
                }
                _taskLists = _taskComponent.TaskLists;
            });
        }

        private void ApplicationTitle_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Page/Home.xaml?tab=" + Home.ONGLET_TO_DO, UriKind.Relative));
        }

        private void PhoneApplicationPage_BackKeyPress(object sender, System.ComponentModel.CancelEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("You have to select a task's list", "TaskList", MessageBoxButton.OK);
            e.Cancel = true;
        }
    }
}