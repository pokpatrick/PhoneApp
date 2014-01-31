using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Input;
using Microsoft.Phone.Controls;

using PhoneApp.Model;
using PhoneApp.Model.Data;
using System.Globalization;
using System.Windows.Data;

namespace PhoneApp.Page
{
    public partial class DayEvent : PhoneApplicationPage
    {
        #region Attributes
        private CalendarComponent calendar;
        private DateTime myDate;
        private List<Event> myEventsStartsTodayEndsToday;
        private List<Event> myEventsStartsAnotherDayEndsToday;
        private List<Event> myEventsStartsAnotherDayEndsAnotherDay;
        private List<Event> myEventsStartsTodayEndsAnotherDay;
        private bool[,] eventTabFill;
        private int eventWidth;
        #endregion

        #region Constructor
        public DayEvent()
        {
            InitializeComponent();
            calendar = CalendarComponent.Instance;
            myEventsStartsTodayEndsToday = new List<Event>();
            myEventsStartsAnotherDayEndsToday = new List<Event>();
            myEventsStartsAnotherDayEndsAnotherDay = new List<Event>();
            myEventsStartsTodayEndsAnotherDay = new List<Event>();
        }
        #endregion

        #region Events
        private void title_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            string _eventId = ((Event) ((TextBlock)sender).DataContext).Id.ToString();
            NavigationService.Navigate(new Uri("/Page/ViewEvent.xaml?id=" + _eventId, UriKind.Relative));
        }

        protected override void OnManipulationCompleted(ManipulationCompletedEventArgs e)
        {
            FrameworkElement _root = Application.Current.RootVisual
               as FrameworkElement;
            myDate = (DateTime)_root.DataContext;

            // swipe to the right (100 pixels to the right 
            // and -25 to 25 up or down in the translation)
            // change to nextday
            if (e.TotalManipulation.Translation.X > 100 &&
                (e.TotalManipulation.Translation.Y > -25 &&
                 e.TotalManipulation.Translation.Y < 25))
            {
                _root.DataContext = myDate.AddDays(-1);

                NavigationService.Navigate(new Uri("/Page/DayEvent.xaml?Refresh=" + DateTime.Now, UriKind.Relative));
            }

            // swipe to the left (100 pixels to the left 
            // and -25 to 25 up or down in the translation)
            // change to previous day
            if (e.TotalManipulation.Translation.X < -100 &&
                (e.TotalManipulation.Translation.Y > -25 &&
                 e.TotalManipulation.Translation.Y < 25))
            {
                _root.DataContext = myDate.AddDays(1);

                NavigationService.Navigate(new Uri("/Page/DayEvent.xaml?Refresh=" + DateTime.Now, UriKind.Relative));
            }
        }

        private void PhoneApplicationPage_BackKeyPress(object sender, System.ComponentModel.CancelEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Page/Home.xaml?tab=" + Home.ONGLET_CALENDAR, UriKind.Relative));
        }

        private void CreateIconButton_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/Page/CreateEvent.xaml?parameter=" + CreateEvent.TYPE_CREATE_FROM_DATE.ToString(), UriKind.Relative));
        }

        private void RefreshIconButton_Click(object sender, EventArgs e)
        {
            calendar.LoadAll(() =>
            {
                NavigationService.Navigate(new Uri("/Page/DayEvent.xaml?Refresh=" + DateTime.Now, UriKind.Relative));
            });
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            Events.Children.Clear();
            myEventsStartsTodayEndsToday.Clear();
            myEventsStartsAnotherDayEndsToday.Clear();
            myEventsStartsAnotherDayEndsAnotherDay.Clear();
            myEventsStartsTodayEndsAnotherDay.Clear();

            // grabbing date passed from Home.xaml
            FrameworkElement root = Application.Current.RootVisual as FrameworkElement;
            myDate = (DateTime)root.DataContext;

            // updating title to selected date
            PageTitle.Text = myDate.ToLongDateString();

            // grabbing events from the calendar
            ObservableCollection<Event> _events = calendar.Events;

            List<Event> _allMyEvents = new List<Event>();
            List<Event> _myEvents = new List<Event>();
            List<Event> _myAllDayEvents = new List<Event>();

            // formated selected date
            String _myFormattedDate = myDate.Year.ToString() + "-" + myDate.Month.ToString() + "-" + myDate.Day.ToString();

            _allMyEvents = calendar.getEventByDay(myDate);

            // getting the AllDay Events of the day
            foreach (Event e1 in _allMyEvents)
            {
                if (e1.Start.Date != null)
                    _myAllDayEvents.Add(e1);
            }

            // getting the non-AllDay Rvents of the day
            foreach (Event e1 in _allMyEvents)
            {

                if (e1.Start.Date == null)
                    _myEvents.Add(e1);

            }

            int _maxCol = maxCol(_myEvents, _myAllDayEvents);

            // creating a tab to keep track of the filling of the Day Cal
            eventTabFill = new bool[24, _maxCol];

            // setting the tab of the day cal as being empty
            for (int i = 0; i < 24; i++)
            {
                for (int j = 0; j < _maxCol; j++)
                {
                    eventTabFill[i, j] = false;
                }
            }

            // width of event boxes
            eventWidth = _maxCol == 0 ? ((int)(ContentPanel.ColumnDefinitions)[1].Width.Value) : ((int)(ContentPanel.ColumnDefinitions)[1].Width.Value / _maxCol);

            // sorting all the events in lists depending on their starting and ending date
            populateEventList(_myEvents);

            // creating the textbox and rectangle for all the events
            displayEventList(_myAllDayEvents, true, true);
            displayEventList(myEventsStartsAnotherDayEndsAnotherDay, true, true);
            displayEventList(myEventsStartsAnotherDayEndsToday, true, false);
            displayEventList(myEventsStartsTodayEndsAnotherDay, false, true);
            displayEventList(myEventsStartsTodayEndsToday, false, false);
        }

        private void ApplicationTitle_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Page/Home.xaml?tab=" + Home.ONGLET_TO_DO, UriKind.Relative));
        }

        #endregion

        #region Operations

        // calculing the number of columns needed to display all the events of the day
        private int maxCol(List<Event> events, List<Event> allDayEvents)
        {
            int _maxCol = 0;
            for (int i = 0; i < 24; i++)
            {
                int _curMaxCol = 0;
                foreach (Event e1 in events)
                {
                    int _startDay = Int32.Parse(e1.Start.DateTime.Substring(8, 2));
                    int _startYear = Int32.Parse(e1.Start.DateTime.Substring(0, 4));
                    int _startMonth = Int32.Parse(e1.Start.DateTime.Substring(5, 2));
                    int _startHour = Int32.Parse(e1.Start.DateTime.Substring(11, 2));

                    int _endDay = Int32.Parse(e1.End.DateTime.Substring(8, 2));
                    int _endYear = Int32.Parse(e1.End.DateTime.Substring(0, 4));
                    int _endMonth = Int32.Parse(e1.End.DateTime.Substring(5, 2));
                    int _endHour = Int32.Parse(e1.End.DateTime.Substring(11, 2));

                    int _myDateDay = Int32.Parse(Tools.addZeroFormatStringDate(myDate.Day));

                    if ((_endYear > _startYear) ||
                        (_endMonth > _startMonth) ||
                        ((_endDay > _myDateDay) && (_startDay < _myDateDay)) ||
                        ((_endDay > _myDateDay) && (_startDay == _myDateDay) && (_startHour <= i)) ||
                        ((_endDay == _myDateDay) && (_startDay < _myDateDay) && (_endHour >= i)) ||
                        ((_endDay == _myDateDay) && (_startDay == _myDateDay) && (_startHour <= i) && (_endHour >= i))
                        )
                        _curMaxCol++;
                }
                _maxCol = _maxCol >= _curMaxCol ? _maxCol : _curMaxCol;
            }

            // taking into account the AllDay events
            _maxCol += allDayEvents.Count;
            return _maxCol;
        }

        private void populateEventList(List<Event> events)
        {
            foreach (Event e in events)
            {
                String _startDay = e.Start.DateTime.Substring(8, 2);
                String _startYear = e.Start.DateTime.Substring(0, 4);
                String _startMonth = e.Start.DateTime.Substring(5, 2);

                String _endDay = e.End.DateTime.Substring(8, 2);
                String _endYear = e.End.DateTime.Substring(0, 4);
                String _endMonth = e.End.DateTime.Substring(5, 2);

                String _myDateDay = Tools.addZeroFormatStringDate(myDate.Day);
                String _myDateMonth = Tools.addZeroFormatStringDate(myDate.Month);
                String _myDateYear = myDate.Year.ToString();

                if (_myDateYear == _startYear && _myDateMonth == _startMonth && _myDateDay == _startDay)
                {
                    if (_myDateYear == _endYear && _myDateMonth == _endMonth && _myDateDay == _endDay)
                    {
                        myEventsStartsTodayEndsToday.Add(e);
                    }
                    else
                    {
                        myEventsStartsTodayEndsAnotherDay.Add(e);
                    }

                }
                else
                {
                    if (_myDateYear == _endYear && _myDateMonth == _endMonth && _myDateDay == _endDay)
                    {
                        myEventsStartsAnotherDayEndsToday.Add(e);
                    }
                    else
                    {
                        myEventsStartsAnotherDayEndsAnotherDay.Add(e);
                    }
                }

            }
        }

        private void displayEventList(List<Event> events, bool noStartHour, bool noEndhour)
        {
            // if the event last all day, only 1 iteration
            int _maxHour = (noStartHour && noEndhour) ? 1 : 24;

            // display the events that starts and ends at the selected date
            for (int i = 0; i < _maxHour; i++)
            {
                foreach (Event e in events)
                {

                    int _startHour = noStartHour ? 0 :Int32.Parse(e.Start.DateTime.Substring(11, 2));
                    int _endHour = noEndhour ? 0 : Int32.Parse(e.End.DateTime.Substring(11, 2));

                    if (_startHour == i)
                    {
                        // filling the tab of the day cal with the current event 
                        int k = 0;
                        while (eventTabFill[i, k] != false)
                        {
                            k++;
                        }

                        int _nbHour = _endHour == 00 ? 24 - _startHour : _endHour - _startHour;
                        for (int j = 0; j < _nbHour; j++)
                        {
                            eventTabFill[i + j, k] = true;
                        }

                        // getting R G B from the colorID of the event
                        string _eventColor = calendar.getEventColorById(e.ColorId != null ? e.ColorId : "1").Background;       
                        int _red = Int32.Parse(_eventColor.Substring(1, 2), NumberStyles.AllowHexSpecifier);
                        int _green = Int32.Parse(_eventColor.Substring(3, 2), NumberStyles.AllowHexSpecifier);
                        int _blue = Int32.Parse(_eventColor.Substring(5, 2), NumberStyles.AllowHexSpecifier);

                        // creating a rectangle colored with the event's color
                        if (_nbHour > 12)
                        {
                            Rectangle _rect = new Rectangle();
                            _rect.Height = 12 * 100;
                            _rect.Width = 10;
                            _rect.SetValue(Rectangle.FillProperty, new SolidColorBrush(Color.FromArgb((BitConverter.GetBytes(255))[0], (BitConverter.GetBytes(_red))[0], (BitConverter.GetBytes(_green))[0], (BitConverter.GetBytes(_blue))[0])));
                            _rect.Visibility = Visibility.Visible;
                            _rect.Margin = new Thickness((k * eventWidth), i * 100, 0, 0);
                            _rect.VerticalAlignment = VerticalAlignment.Top;
                            _rect.HorizontalAlignment = HorizontalAlignment.Left;

                            Rectangle _rect2 = new Rectangle();
                            _rect2.Height = (_nbHour - 12) * 100;
                            _rect2.Width = 10;
                            _rect2.SetValue(Rectangle.FillProperty, new SolidColorBrush(Color.FromArgb((BitConverter.GetBytes(255))[0], (BitConverter.GetBytes(_red))[0], (BitConverter.GetBytes(_green))[0], (BitConverter.GetBytes(_blue))[0])));
                            _rect2.Visibility = Visibility.Visible;
                            _rect2.Margin = new Thickness((k * eventWidth), i * 100 + 1200, 0, 0);
                            _rect2.VerticalAlignment = VerticalAlignment.Top;
                            _rect2.HorizontalAlignment = HorizontalAlignment.Left;
                            Events.Children.Add(_rect);
                            Events.Children.Add(_rect2);
                        }
                        else
                        {
                            Rectangle _rect = new Rectangle();
                            _rect.Height = _nbHour == 0 ? 30 : _nbHour * 100;
                            _rect.Width = 10;
                            _rect.SetValue(Rectangle.FillProperty, new SolidColorBrush(Color.FromArgb((BitConverter.GetBytes(255))[0], (BitConverter.GetBytes(_red))[0], (BitConverter.GetBytes(_green))[0], (BitConverter.GetBytes(_blue))[0])));
                            _rect.Visibility = Visibility.Visible;
                            _rect.Margin = new Thickness((k * eventWidth), i * 100, 0, 0);
                            _rect.VerticalAlignment = VerticalAlignment.Top;
                            _rect.HorizontalAlignment = HorizontalAlignment.Left;
                            Events.Children.Add(_rect);
                        }

                        // creating the textblock for the title of the event (clickable) colored with the event's color
                        TextBlock _title = new TextBlock();
                        _title.DataContext = e;
                        _title.SetBinding(TextBlock.TextProperty, new Binding("Summary"));
                        _title.SetValue(TextBlock.ForegroundProperty, new SolidColorBrush(Color.FromArgb((BitConverter.GetBytes(255))[0], (BitConverter.GetBytes(_red))[0], (BitConverter.GetBytes(_green))[0], (BitConverter.GetBytes(_blue))[0])));
                        _title.Height = 30;
                        _title.Width = eventWidth - 10;
                        _title.Margin = new Thickness((k * eventWidth) + 13, i * 100 + 3, 3, 3);
                        _title.VerticalAlignment = VerticalAlignment.Top;
                        _title.HorizontalAlignment = HorizontalAlignment.Left;
                        _title.TextWrapping = TextWrapping.Wrap;
                        _title.MouseLeftButtonDown += new MouseButtonEventHandler(title_MouseLeftButtonDown);

                        // creating the textblock for the description of the event
                        TextBlock _desc = new TextBlock();
                        _desc.DataContext = e;
                        _desc.SetBinding(TextBlock.TextProperty, new Binding("Description"));
                        _desc.Height = _nbHour == 0 ? 30 : 70 + ((_nbHour - 1) * 100);
                        _desc.Width = eventWidth - 10;
                        _desc.Margin = new Thickness((k * eventWidth) + 13, (i * 100) + 33, 3, 3);
                        _desc.VerticalAlignment = VerticalAlignment.Top;
                        _desc.HorizontalAlignment = HorizontalAlignment.Left;
                        _desc.TextWrapping = TextWrapping.Wrap;

                        Events.Children.Add(_title);
                        Events.Children.Add(_desc);

                    }
                }
            }
        }
        #endregion

    }
}
