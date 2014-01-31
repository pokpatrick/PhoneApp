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
using System.Text.RegularExpressions;
using System.Collections.ObjectModel;

using PhoneApp.Model;
using PhoneApp.Model.Data;
using RestSharp;

namespace PhoneApp.Page
{
    public partial class CreateEvent : PhoneApplicationPage
    {

        #region Attributes

        public class Reminder
        {
            private String _type, _format;
            private long _number;

            public long Number
            {
                get { return _number; }
                set { _number = value; }
            }

            public String Type
            {
                get { return _type; }
                set { _type = value; }
            }

            public String Format
            {
                get { return _format; }
                set { _format = value; }
            }

            public Reminder(String type, long number, String format)
            {
                Type = type;
                Number = number;
                Format = format;
            }
        }

        private CalendarComponent _calendarComponent = CalendarComponent.Instance;
        private ObservableCollection<Reminder> _listReminders = new ObservableCollection<Reminder>();
        private ObservableCollection<EventAttendee> _listGuests = new ObservableCollection<EventAttendee>();
        int _typeCall = TYPE_EMPTY;
        private String _idEvent = "";
        private String defaultMsgEventTitleTextBox = "Untitled Event";
        private String defaultMsgEmailTextBox = "Enter an email address";
        public const int TYPE_CREATE_FROM_DATE = 0;
        public const int TYPE_CREATE_FROM_NOTHING = 1;
        public const int TYPE_EDIT = 2;
        private const int TYPE_EMPTY = -1;
        private const int TYPE_TREATED = -2;
        private const int TYPE_CREATE_FROM_NOTHING_TREATED = -3;

        #endregion

        #region Constructor

        public CreateEvent()
        {
            InitializeComponent();
            listBoxGuests.ItemsSource = _listGuests;
            listBoxReminders.ItemsSource = _listReminders;
        }

        #endregion Constructor

        #region Events

        private void PhoneApplicationPage_BackKeyPress(object sender, System.ComponentModel.CancelEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Are you sure? This event won't be saved.", "Confirmation", MessageBoxButton.OKCancel);
            if (result == MessageBoxResult.OK)
            {
                if (_typeCall == TYPE_CREATE_FROM_NOTHING_TREATED)
                    NavigationService.Navigate(new Uri("/Page/Home.xaml?tab=" + Home.ONGLET_CALENDAR, UriKind.Relative));
                else
                    NavigationService.GoBack();
            }
            else
            {
                e.Cancel = true;
            }
        }



        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            if (_typeCall != TYPE_TREATED && _typeCall != TYPE_CREATE_FROM_NOTHING_TREATED)
            {
                // Whatever the type of call of the page, load the colors of an event the first time
                ColorDefinition def;
                String firstKey = "1";
                Dictionary<String, ColorDefinition> liste = _calendarComponent.Colors.Event;
                foreach (String keys in liste.Keys)
                {
                    Button but = new Button();
                    but.Tag = keys;

                    liste.TryGetValue(keys, out def);
                    but.Background = new SolidColorBrush(Tools.HexColor(def.Background));
                    but.Width = 40;
                    but.Click += new RoutedEventHandler(but_ButtonClick);

                    stackPanelColors.Children.Add(but);
                }
                liste.TryGetValue(firstKey, out def);
                buttonEvent.Background = new SolidColorBrush(Tools.HexColor(def.Background));
                buttonEvent.Tag = firstKey;
                if (_typeCall != TYPE_EDIT)
                {
                    // TYPE_CREATE_FROM_DATE or TYPE_CREATE_FROM_NOTHING
                    if (_typeCall == TYPE_CREATE_FROM_DATE)
                    {
                        // Sets the pickers to the day of the calendar where you were on the page before
                        FrameworkElement root = Application.Current.RootVisual as FrameworkElement;
                        DateTime myDate = (DateTime)root.DataContext;

                        textBoxStartDate.Value = myDate.Date;
                        textBoxEndDate.Value = myDate.Date;
                        textBoxStartTime.Value = myDate.AddHours(DateTime.Now.Hour).AddMinutes(DateTime.Now.Minute);
                        textBoxEndTime.Value = myDate.AddHours(DateTime.Now.Hour).AddMinutes(DateTime.Now.Minute);
                        _typeCall = TYPE_TREATED;
                    }
                    else
                        _typeCall = TYPE_CREATE_FROM_NOTHING_TREATED;

                    // Add an hour for the end time of the event 
                    textBoxEndTime.Value = (textBoxEndTime.Value.Value.AddHours(1));

                    // Add a day if the add of the hour changes the end day of the event
                    if (textBoxStartDate.Value.Value.Date.Day == textBoxEndTime.Value.Value.Date.Day - 1)
                        textBoxEndDate.Value = (textBoxEndDate.Value.Value.AddDays(1));
                    textBoxEventTitle.Text = defaultMsgEventTitleTextBox;
                    textBoxGuest.Text = defaultMsgEmailTextBox;

                }
                else
                {
                    loadViewEvent(_calendarComponent.getEventById(_idEvent));
                    PageTitle.Text = "Edit Event";
                    _typeCall = TYPE_TREATED;
                }
            }
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            // If this is the first time you load the page
            if (_typeCall == TYPE_EMPTY)
            {
                // Get the arguments in the uri
                String s = this.NavigationContext.QueryString["parameter"];
                _typeCall = int.Parse(s);
                if (_typeCall == TYPE_EDIT)
                    _idEvent = this.NavigationContext.QueryString["id"];
            }
        }

        private void ButtonSave_Click(object sender, EventArgs e)
        {
            DateTime sd = textBoxStartDate.Value.Value;
            DateTime ed = textBoxEndDate.Value.Value;
            DateTime st = textBoxStartTime.Value.Value;
            DateTime et = textBoxEndTime.Value.Value;

            // Unify date and time of the event
            DateTime start, end;


            start = new DateTime(sd.Year, sd.Month, sd.Day, st.Hour, st.Minute, st.Second);
            end = new DateTime(ed.Year, ed.Month, ed.Day, et.Hour, et.Minute, et.Second);

            // Checks that the start of the event occurs before the end of the event
            int test = start.CompareTo(end);
            if (test != -1)
            {
                MessageBox.Show("Sorry, you can't create an event that ends before it starts", "Your event", MessageBoxButton.OK);
                return;
            }

            Event _event = new Event();

            if (!_idEvent.Equals(""))
                // Edit event
                _event = _calendarComponent.getEventById(_idEvent);
            else
                // New event
                _event = new Event();

            // Convert the dates into the right format in String

            EventDateTime startD = new EventDateTime();
            EventDateTime endD = new EventDateTime();

            DateTime teste = new DateTime();

            if (checkBoxAllDay.IsChecked.Value)
            {
                // Fill the Date field of the event for AllDay
                teste = TimeZoneInfo.ConvertTime(start, TimeZoneInfo.Utc);
                startD.Date = Tools.convertDateToRFC3339(teste, true);
                teste = TimeZoneInfo.ConvertTime(end, TimeZoneInfo.Utc);
                endD.Date = Tools.convertDateToRFC3339(teste, true);
            }
            else
            {
                // Fill the DateTime field of the event for event which is not AllDay
                teste = TimeZoneInfo.ConvertTime(start, TimeZoneInfo.Utc);
                startD.DateTime = Tools.convertDateToRFC3339(teste, false);
                teste = TimeZoneInfo.ConvertTime(end, TimeZoneInfo.Utc);
                endD.DateTime = Tools.convertDateToRFC3339(teste, false);
            }

            // Fill all the other fields of the event
            _event.Start = startD;
            _event.End = endD;
            _event.Summary = textBoxEventTitle.Text;
            _event.GuestsCanSeeOtherGuests = checkBoxSee.IsChecked;
            _event.GuestsCanModify = checkBoxModify.IsChecked;
            _event.GuestsCanInviteOthers = checkBoxInvite.IsChecked;
            _event.Description = textBoxDescription.Text;
            _event.Location = textBoxWhere.Text;
            _event.ColorId = buttonEvent.Tag.ToString();

            if (radioButtonPrivate.IsChecked.Value)
            {
                _event.Visibility = "private";
            }
            else if (radioButtonPublic.IsChecked.Value)
            {
                _event.Visibility = "public";
            }

            foreach (EventAttendee attendee in _listGuests)
            {
                _event.Attendees.Add(attendee);
            }

            foreach (Reminder evR in _listReminders)
            {
                long number = evR.Number;
                long minutes = number;
                switch (evR.Format)
                {
                    case "hours":
                        minutes = number * 60;
                        break;
                    case "days":
                        minutes = number * 1440;
                        break;
                    case "weeks":
                        minutes = number * 10080;
                        break;
                }
                _event.Reminders.Overrides.Add(new EventReminder() { Method = evR.Type, Minutes = minutes });
            }

            // Insertion or update of the event
            if (!_idEvent.Equals(""))
                _calendarComponent.UpdateEvent(UpdatedEvent, _calendarComponent.getEventById(_idEvent));
            else
                _calendarComponent.InsertEvent(InsertedEvent, _event);
        }

        private void hyperlinkButtonAddReminders_Click(object sender, RoutedEventArgs e)
        {
            if (stackPanelReminder.Visibility.Equals(Visibility.Collapsed))
            {
                stackPanelReminder.Visibility = Visibility.Visible;
                hyperlinkButtonAddReminders.Content = "Add this reminder";
            }
            else
            {
                // Check of the new reminder
                long LngString;
                try
                {
                    LngString = Int64.Parse(textBoxNumberTime.Text);
                }
                catch (System.FormatException)
                {
                    MessageBox.Show("The format of your reminder is not correct", "New Reminder", MessageBoxButton.OK);
                    return;
                }

                Reminder r = new Reminder((listBoxTypeReminder.SelectedItem as ListBoxItem).Content.ToString(), LngString,
                    (listBoxTime.SelectedItem as ListBoxItem).Content.ToString());

                if (!_listReminders.Contains(r))
                {
                    _listReminders.Add(r);
                    stackPanelReminder.Visibility = Visibility.Collapsed;
                    hyperlinkButtonAddReminders.Content = "Add a reminder";
                }
                else
                    MessageBox.Show("You already add this reminder", "New Reminder", MessageBoxButton.OK);
            }
        }
        private void hyperlinkButtonDeleteReminders_Click(object sender, RoutedEventArgs e)
        {
            int a = listBoxReminders.SelectedIndex;
            if (a == -1)
            {
                MessageBox.Show("No reminder selected", "Delete Reminder", MessageBoxButton.OK);
                return;
            }
            _listReminders.RemoveAt(a);
            listBoxReminders.Items.RemoveAt(a);

        }

        private void ButtonAddGuest_Click(object sender, RoutedEventArgs e)
        {

            string pattern = @"^(?!\.)(""([^""\r\\]|\\[""\r\\])*""|"
    + @"([-a-z0-9!#$%&'*+/=?^_`{|}~]|(?<!\.)\.)*)(?<!\.)"
    + @"@[a-z0-9][\w\.-]*[a-z0-9]\.[a-z][a-z\.]*[a-z]$";
            string email = textBoxGuest.Text;
            string msg = "Enter mail adresses";

            // Checks the format of the email adress of the new guest
            Regex regex = new Regex(pattern, RegexOptions.IgnoreCase);
            if (!regex.IsMatch(email) && !email.Equals(msg))
                MessageBox.Show("Please try again", "Wrong email", MessageBoxButton.OK);


            else
            {
                // Check if the email is already in the list
                EventAttendee att = new EventAttendee();
                att.Email = email;
                int i;
                for (i = 0; i < _listGuests.Count && !_listGuests[i].Email.Equals(email); ++i) ;
                if (i == _listGuests.Count)
                {
                    // Add the email adress to the list of guests
                    _listGuests.Add(att);
                    textBoxGuest.Text = defaultMsgEmailTextBox;
                }
                else
                    MessageBox.Show("Please try again", "Guest already invited", MessageBoxButton.OK);

            }
        }

        private void checkBoxAllDay_Click(object sender, RoutedEventArgs e)
        {
            if (textBoxStartTime.Visibility.Equals(Visibility.Collapsed))
            {
                textBoxStartTime.Visibility = Visibility.Visible;
                textBoxEndTime.Visibility = Visibility.Visible;
            }
            else
            {
                textBoxStartTime.Visibility = Visibility.Collapsed;
                textBoxEndTime.Visibility = Visibility.Collapsed;
            }
        }

        private void ButtonDeleteReminder_Click(object sender, RoutedEventArgs e)
        {
            Reminder reminder = (sender as Button).DataContext as Reminder;
            ListBoxItem pressedItem = this.listBoxReminders.ItemContainerGenerator.ContainerFromItem(reminder) as ListBoxItem;
            if (pressedItem != null)
            {
                _listReminders.Remove(reminder);
            }
        }

        private void ButtonDeleteMail_Click(object sender, RoutedEventArgs e)
        {
            EventAttendee attendee = (sender as Button).DataContext as EventAttendee;
            ListBoxItem pressedItem = this.listBoxGuests.ItemContainerGenerator.ContainerFromItem(attendee) as ListBoxItem;
            int index = this.listBoxGuests.ItemContainerGenerator.IndexFromContainer(pressedItem);

            if (pressedItem != null)
            {
                _listGuests.Remove(attendee);
            }
        }

        private void textBoxEventTitle_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            if (textBoxEventTitle.Text == defaultMsgEventTitleTextBox)
                textBoxEventTitle.Text = "";
        }

        private void textBoxGuest_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            if (textBoxGuest.Text == defaultMsgEmailTextBox)
                textBoxGuest.Text = "";
        }

        private void hyperlinkAdvancedOptions_Click(object sender, RoutedEventArgs e)
        {
            if (gridAdvancedOptions.Visibility.Equals(Visibility.Collapsed))
                gridAdvancedOptions.Visibility = Visibility.Visible;
            else
                gridAdvancedOptions.Visibility = Visibility.Collapsed;
        }

        private void but_ButtonClick(object sender, RoutedEventArgs e)
        {
            Button b = (sender as Button);
            buttonEvent.Background = b.Background;
            buttonEvent.Tag = b.Tag;
        }

        private void ApplicationTitle_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Page/Home.xaml?tab=" + Home.ONGLET_TO_DO, UriKind.Relative));
        }

        #endregion

        #region Operations

        /**
         * Load the informations of the event to edit
         * @param[in] e : the event you are editing
         */
        public void loadViewEvent(Event e)
        {
            DateTime startDate;
            DateTime endDate;

            // Checks if the event is an AllDay Event
            if (e.Start.DateTime != null)
            {
                // Not AllDay Event
                startDate = Convert.ToDateTime(e.Start.DateTime);
                endDate = Convert.ToDateTime(e.End.DateTime);
                checkBoxAllDay.IsChecked = false;
            }
            else
            {
                // AllDay Event
                startDate = Convert.ToDateTime(e.Start.Date);
                endDate = Convert.ToDateTime(e.End.Date);
                checkBoxAllDay.IsChecked = true;
                textBoxStartTime.Visibility = Visibility.Collapsed;
                textBoxEndTime.Visibility = Visibility.Collapsed;
            }

            // Fills all the others fields of the form
            textBoxStartDate.Value = startDate;
            textBoxEndDate.Value = endDate;

            textBoxStartTime.Value = startDate;
            textBoxEndTime.Value = endDate;


            textBoxEventTitle.Text = e.Summary != null ? e.Summary : "";
            checkBoxSee.IsChecked = e.GuestsCanSeeOtherGuests != null ? e.GuestsCanSeeOtherGuests : false;
            checkBoxModify.IsChecked = e.GuestsCanModify != null ? e.GuestsCanModify : false;
            checkBoxInvite.IsChecked = e.GuestsCanInviteOthers != null ? e.GuestsCanInviteOthers : false;
            textBoxDescription.Text = e.Description != null ? e.Description : ""; ;
            textBoxWhere.Text = e.Location != null ? e.Location : "";
            if (e.Visibility != null)
            {
                if (e.Visibility.Equals("private"))
                    radioButtonPrivate.IsChecked = true;
                else if (e.Visibility.Equals("public"))
                    radioButtonPublic.IsChecked = true;
                else
                    radioButtonDefault.IsChecked = true;
            }

            if (e.Attendees != null)
            {
                foreach (EventAttendee att in e.Attendees)
                {
                    _listGuests.Add(att);
                }
            }

            if (e.Reminders != null && e.Reminders.Overrides != null)
            {
                foreach (EventReminder r in e.Reminders.Overrides)
                {
                    _listReminders.Add(new Reminder(r.Method, r.Minutes.Value, "minutes"));
                }
            }
            if (e.ColorId != null)
            {
                buttonEvent.Background = new SolidColorBrush(Tools.HexColor(_calendarComponent.getEventColorById(e.ColorId).Background));
                buttonEvent.Tag = e.ColorId;
            }
        }

        #endregion

        #region Callbacks

        private void InsertedEvent(IRestResponse<Event> response)
        {
            if (response.Data == null)
            {
                MessageBox.Show("An error occured, please try again", "Event", MessageBoxButton.OK);
                return;
            }
            if (_typeCall == TYPE_CREATE_FROM_NOTHING_TREATED)
                NavigationService.Navigate(new Uri("/Page/Home.xaml?tab=" + Home.ONGLET_CALENDAR, UriKind.Relative));          
            else
                NavigationService.GoBack();
        }

        private void UpdatedEvent(IRestResponse<Event> response)
        {
            if (response.Data == null)
            {
                MessageBox.Show("An error occured, please try again", "Event", MessageBoxButton.OK);
                return;
            }
            NavigationService.GoBack();
        }

        #endregion
    }
}
