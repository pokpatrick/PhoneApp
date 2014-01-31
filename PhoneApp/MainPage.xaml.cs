using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

using PhoneApp.Model;
using PhoneApp.Model.Data;
using PhoneApp.Page;

namespace PhoneApp
{
    public partial class MainPage : PhoneApplicationPage
    {
        private OAuth _oAuth = OAuth.Instance;
        private TaskComponent _taskComponent = TaskComponent.Instance;
        private CalendarComponent _calendarComponent = CalendarComponent.Instance;

        // Constructeur
        public MainPage()
        {
            InitializeComponent();
        }

        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            if (OAuth.Instance.HasAuthenticated)
            {
                _calendarComponent.LoadAll(null);
                _taskComponent.LoadAll(() =>
                {
                    NavigationService.Navigate(new Uri("/Page/Home.xaml?tab=" + Home.ONGLET_TO_DO, UriKind.Relative));
                });
            }
            else if (!OAuth.Instance.AutoLogin)
            {
                NavigationService.Navigate(new Uri("/Page/Authentication.xaml", UriKind.Relative));
            }
            else
            {
                _oAuth.Logout();
                _oAuth.GetAccessCode(code =>
                {
                    _calendarComponent.LoadAll(null);
                    _taskComponent.LoadAll(() =>
                    {
                        NavigationService.Navigate(new Uri("/Page/Home.xaml?tab=" + Home.ONGLET_TO_DO, UriKind.Relative));
                    });
                });
            }
        }
    }
}
