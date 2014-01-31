using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Phone.Controls;
using PhoneApp.Model.Data;
using PhoneApp.Model;
using WPControls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.IO.IsolatedStorage;
using Microsoft.Phone.Shell;
using Coding4Fun.Phone.Controls;

namespace PhoneApp.Page
{
    public partial class Home : PhoneApplicationPage
    {
        public const int ONGLET_TO_DO = 0;
        public const int ONGLET_CALENDAR = 1;

        private TaskComponent _taskComponent = TaskComponent.Instance;
        private CalendarComponent _calendarComponent = CalendarComponent.Instance;
        private BitmapImage _toDoBackground = new BitmapImage(new Uri("/Images/PivotBackground1.png", UriKind.Relative));
        private BitmapImage _calendarBackground = new BitmapImage(new Uri("/Images/PivotBackground2.png", UriKind.Relative));
        private int _selectedIndex = -1;

        // class used to customize the calendar component
        public class CalendarColorBackgroundConverter : IDateToBrushConverter
        {
            static SolidColorBrush blackColorBrush = new SolidColorBrush(System.Windows.Media.Color.FromArgb(100, 255, 255, 255));
            static SolidColorBrush todayColorBrush = new SolidColorBrush(System.Windows.Media.Color.FromArgb(100, 150, 150, 150));
            static SolidColorBrush Foreground = new SolidColorBrush(System.Windows.Media.Colors.Black);

            public Brush Convert(DateTime currentCalendarCaseDate, bool isSelected, Brush defaultValue, BrushType brushType)
            {                
                if (brushType == BrushType.Background)
                {
                    // background brush color when the calendar case is selected
                    if (currentCalendarCaseDate == DateTime.Today) 
                        return todayColorBrush;
                    else
                        return blackColorBrush;
                }
                else
                {
                    if (currentCalendarCaseDate == DateTime.Today)
                        return blackColorBrush;
                    return Foreground;
                }
            }
        }

        public Home()
        {
            InitializeComponent();
            this.Cal.SelectedDate = DateTime.Today;
            taskListBox.ItemsSource = _taskComponent.getAllTaskSortedByDue();
            this.Cal.ColorConverter = new CalendarColorBackgroundConverter();
            //this.Cal.Opacity = 0.25;
        }

        #region Operations

        /*
         * Refresh tasks displayed on the page
         */
        private void RefreshTasks()
        {
            _taskComponent.LoadAll(() =>
            {
                taskListBox.ItemsSource = _taskComponent.getAllTaskSortedByDue();
            });
        }

        /*
         * Show task
         */
        private void ShowTask()
        {
            if (taskListBox.SelectedItem == null) return;
            Task task = taskListBox.SelectedItem as Task;
            string uri =
                string.Format("/Page/ViewTask.xaml?idTask={0}&idTaskList={1}", task.Id, task.IdList);
            NavigationService.Navigate(new Uri(uri, UriKind.Relative));
        }

        private void FadeInOut(DependencyObject target, Storyboard sb, bool isFadeIn)
        {
            Duration d = new Duration(TimeSpan.FromSeconds(0.2));
            DoubleAnimation daFade = new DoubleAnimation();
            daFade.Duration = d;
            if (isFadeIn)
            {
                daFade.From = 1.00;
                daFade.To = 0.00;
            }
            else
            {
                daFade.From = 0.00;
                daFade.To = 1.00;
            }

            sb.Duration = d;
            sb.Children.Add(daFade);
            Storyboard.SetTarget(daFade, target);
            Storyboard.SetTargetProperty(daFade, new PropertyPath("Opacity"));

            sb.Begin();
        }

        private void sb_Completed(object sender, EventArgs e)
        {
            BitmapImage bitmapImage = null;
            if (PivotHome.SelectedIndex == ONGLET_TO_DO)
                bitmapImage = _toDoBackground;
            else if (PivotHome.SelectedIndex == ONGLET_CALENDAR)
                bitmapImage = _calendarBackground;
            ImageBrush imageBrush = new ImageBrush();
            imageBrush.ImageSource = bitmapImage;

            PivotHome.Background = imageBrush;
            Storyboard sbFadeOut = new Storyboard();

            FadeInOut(PivotHome.Background, sbFadeOut, false);
        }

        #endregion

        #region Calendar Events

        private void Cal_DateClicked(object sender, WPControls.SelectionChangedEventArgs e)
        {

            FrameworkElement root = Application.Current.RootVisual
                as FrameworkElement;
            root.DataContext = e.SelectedDate;

            NavigationService.Navigate(new Uri("/Page/DayEvent.xaml", UriKind.Relative));
        }

        #endregion

        #region Task Events

        private void AddTask_Click(object sender, EventArgs e)
        {
            if (PivotHome.SelectedIndex == ONGLET_TO_DO)
                NavigationService.Navigate(new Uri("/Page/CreateTask.xaml", UriKind.Relative));
            else if (PivotHome.SelectedIndex == ONGLET_CALENDAR)
                NavigationService.Navigate(new Uri("/Page/CreateEvent.xaml?parameter=" + CreateEvent.TYPE_CREATE_FROM_NOTHING.ToString(), UriKind.Relative));
        }

        private void DeleteTask_Click(object sender, EventArgs e)
        {
            if (taskListBox.SelectedIndex > -1) // Item Selected
            {
                // Remove Selected
                taskListBox.Items.RemoveAt(taskListBox.SelectedIndex);
            }
        }

        private void CheckBox_Click(object sender, RoutedEventArgs e)
        {
            Task task = (sender as CheckBox).DataContext as Task;
            ListBoxItem pressedItem = taskListBox.ItemContainerGenerator.ContainerFromItem(task) as ListBoxItem;
            if (pressedItem != null)
            {
                int index = taskListBox.ItemContainerGenerator.IndexFromContainer(pressedItem);
                task.Status = task.Status.Equals(Task.Statuses.NeedsAction) ? Task.Statuses.Completed : Task.Statuses.NeedsAction;
                _taskComponent.UpdateTask(res =>
                {
                    taskListBox.ItemsSource = _taskComponent.getAllTaskSortedByDue();
                }, task.IdList, task);
            }
        }

        /**
         * Button Refresh tasks Click handler
         */
        private void RefreshTasks_Click(object sender, EventArgs e)
        {
            RefreshTasks();
        }

        private void taskListBox_DoubleTap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            ShowTask();
        }

        private void taskListBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            ShowTask();
        }

        #endregion

        #region Events

        private void PhoneApplicationPage_BackKeyPress(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
        }

        private void ApplicationBarMenuItem_Logout_Click(object sender, EventArgs e)
        {
            MessageBoxResult warning = MessageBox.Show("Are you sure you want to logout?", "Confirmation", MessageBoxButton.OKCancel);
            if (warning == MessageBoxResult.Cancel)
                return;
            IsolatedStorageSettings.ApplicationSettings["AutoLogin"] = false;
            OAuth.Instance.Logout();
            NavigationService.Navigate(new Uri("/Page/Authentication.xaml", UriKind.Relative));
        }

        private void ApplicationBarMenuItem_About_Click(object sender, EventArgs e)
        {
            AboutPrompt about = new AboutPrompt();
            about.Title = "GooCalTask";
            about.Footer = "Université Pierre et Marie Curie 2012-2013";
            about.VersionNumber = "Version 1.0";
            about.Body = new TextBlock { Text = "Developed By :\n\n-DOUANT Vincent\n-HING Joel\n-INTHAVIXAY David\n-JEAN-BAPTISTE Guerline\n-POK Patrick", TextWrapping = TextWrapping.Wrap };
            about.Show();
        }

        private void PivotHome_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (PivotHome.SelectedIndex < 0 || _selectedIndex == PivotHome.SelectedIndex) return;
            _selectedIndex = PivotHome.SelectedIndex;
            ApplicationBarIconButton button = ApplicationBar.Buttons[1] as ApplicationBarIconButton;
            if (PivotHome.SelectedIndex == ONGLET_TO_DO)
                button.Text = "Add Task";
            else if (PivotHome.SelectedIndex == ONGLET_CALENDAR)
                button.Text = "Add Event";
            Storyboard sbFadeIn = new Storyboard();
            sbFadeIn.Completed += new EventHandler(sb_Completed);
            FadeInOut(PivotHome.Background, sbFadeIn, true);
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            try
            {

                String s = this.NavigationContext.QueryString["tab"];
                PivotHome.SelectedIndex = int.Parse(s);
            }
            catch { }
            RefreshTasks();
        }

        #endregion       
    }
}
