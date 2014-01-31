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
using PhoneApp.Model.Data;
using PhoneApp.Model;
using RestSharp;

namespace PhoneApp.Page
{
    public partial class ViewEvent : PhoneApplicationPage
    {

        #region Attributes

        private Event _event = new Event();
        private CalendarComponent _calendarComponent = CalendarComponent.Instance;
        private String _idEvent;

        #endregion

        #region Constructor

        public ViewEvent()
        {
            InitializeComponent();
        }

        #endregion

        #region Events 

        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            textBoxTitle.Text = _event.Summary;

            if (_event.Start.DateTime != null)
            {
                DateTime date = DateTime.Parse(_event.Start.DateTime);
                textBlockStartDate.Text = date.ToString();
                date = DateTime.Parse(_event.End.DateTime);
                textBlockEndDate.Text = date.ToString();
            }
            else
            {
                textBlockStartDate.Text = _event.Start.Date;
                textBlockEndDate.Text = _event.End.Date;
            }

            textBlockLocation.Text = _event.Location != null ? _event.Location : "";
            textBlockDescription.Text = _event.Description != null ? _event.Description : "";
            textBoxPrivacy.Text = _event.Visibility != null ? _event.Visibility : Event.Visibilities.Default;
            listBoxReminders.Items.Clear();
            if (_event.Reminders.Overrides != null)
            {
                foreach (EventReminder r in _event.Reminders.Overrides)
                {
                    listBoxReminders.Items.Add(r.ToString());
                }
            }
            listBoxGuests.Items.Clear();
            if (_event.Attendees != null)
            {
                foreach (EventAttendee r in _event.Attendees)
                {
                    listBoxGuests.Items.Add(r.Email);
                }
            }
            listBoxGuestsCan.Items.Clear();
            if (_event.GuestsCanInviteOthers != null && _event.GuestsCanInviteOthers.Value)
                listBoxGuestsCan.Items.Add("invite others");
            if (_event.GuestsCanModify !=  null && _event.GuestsCanModify.Value)
                listBoxGuestsCan.Items.Add("modify event");
            if (_event.GuestsCanSeeOtherGuests != null && _event.GuestsCanSeeOtherGuests.Value)
                listBoxGuestsCan.Items.Add("see others guests");
            if (_event.ColorId != null)
            {
                buttonEventColor.Background = new SolidColorBrush(Tools.HexColor(_calendarComponent.getEventColorById(_event.ColorId).Background));
            }
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            _idEvent = this.NavigationContext.QueryString["id"];
            _event = _calendarComponent.getEventById(_idEvent);
        }

        private void ButtonAdd_Click(object sender, EventArgs e)
        {
            string uri = string.Format("/Page/CreateEvent.xaml?parameter={0}", CreateEvent.TYPE_CREATE_FROM_NOTHING.ToString(), _idEvent);
            NavigationService.Navigate(new Uri(uri, UriKind.Relative));
        }

        private void ButtonEdit_Click(object sender, EventArgs e)
        {
            string uri = string.Format("/Page/CreateEvent.xaml?parameter={0}&id={1}", CreateEvent.TYPE_EDIT.ToString(), _idEvent);
            NavigationService.Navigate(new Uri(uri, UriKind.Relative));
        }

        private void ButtonDelete_Click(object sender, EventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Are you sure you want to delete this event?", "Confirmation", MessageBoxButton.OKCancel);
            if (result == MessageBoxResult.OK)
            {
                _calendarComponent.DeleteEvent(DeletedEvent, _calendarComponent.getEventById(_idEvent));
            }
        }

        private void ApplicationTitle_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Page/Home.xaml?tab=" + Home.ONGLET_TO_DO, UriKind.Relative));
        }

        #endregion

        #region Callback

        private void DeletedEvent(IRestResponse<Error> response)
        {
            if (response.Data != null)
            {
                MessageBox.Show("An error occured, please try again", "Event", MessageBoxButton.OK);
                return;
            }
            NavigationService.GoBack();
        }

        #endregion
    }
}