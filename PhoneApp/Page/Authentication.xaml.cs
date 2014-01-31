using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls.Primitives;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

using PhoneApp.Model;
using PhoneApp.Model.Data;
using RestSharp;
using Coding4Fun.Phone.Controls;

namespace PhoneApp.Page
{
    public partial class Authentication : PhoneApplicationPage
    {
        private OAuth _oAuth = OAuth.Instance;
        private TaskComponent _taskComponent = TaskComponent.Instance;
        private CalendarComponent _calendarComponent = CalendarComponent.Instance;

        public Authentication()
        {
            InitializeComponent();
            _oAuth.ConnectionTimedOut += StopProgressBar;
            _oAuth.AuthenticationFailed += StopProgressBar;
            email.Text = "dotnet.stl.api";
            passwd.Password = "azertyqsdfgh";
        }

        private void StopProgressBar(object sender, EventArgs e)
        {
            progressBar.IsIndeterminate = false;
        }

        private void login_Click(object sender, RoutedEventArgs e)
        {
            _oAuth.Logout();
            _oAuth.Email = email.Text;
            _oAuth.Passwd = passwd.Password;
            _oAuth.AutoLogin = remember.IsChecked != null;
            progressBar.IsIndeterminate = true;
            _taskComponent.LoadAll(TasksLoaded);
            _calendarComponent.LoadAll(null);
        }
        private void TasksLoaded()
        {
            NavigationService.Navigate(new Uri("/Page/Home.xaml?tab=" + Home.ONGLET_TO_DO, UriKind.Relative));
        }

        private void PhoneApplicationPage_BackKeyPress(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
        }

    }
}