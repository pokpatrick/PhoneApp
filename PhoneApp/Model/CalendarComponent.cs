using System;
using System.Net;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Threading;

using RestSharp;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Microsoft.Phone.Controls;
using PhoneApp.Model.Data;

namespace PhoneApp.Model
{
    public class CalendarComponent
    {
        #region Singleton Pattern

        private static CalendarComponent _instance;
        public static CalendarComponent Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new CalendarComponent();
                }
                return _instance;
            }
        }

        #endregion

        #region Attributes

        private OAuth _oAuth = OAuth.Instance;
        private RestClient _client = new RestClient("https://www.googleapis.com");
        private JsonSerializerSettings _serializerSettings = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore };
        private CalendarListEntry _calendar;
        private Events __events;
        private ObservableCollection<Event> _events = new ObservableCollection<Event>();
        private Colors _colors;
        private bool _calendarLoaded = false;
        private object _sync = new Object();
        private int _requesting = 0;
        #endregion

        #region Properties

        public CalendarListEntry Calendar
        {
            get { return _calendar; }
            set { _calendar = value; }
        }

        public ObservableCollection<Event> Events
        {
            get { return _events; }
        }

        public Colors Colors
        {
            get { return _colors; }
        }

        #endregion

        /*
         * Load All events of default calendar
         * 
         * @param: AllLoaded : a callback
         */
        public void LoadAll(Action AllLoaded)
        {
            LoadCalendarList(response =>
            {
                _calendarLoaded = true;
                LoadEvents(res =>
                {
                    LoadColors(r => 
                    {
                        if(AllLoaded != null) AllLoaded();
                    });
                });
            });
        }

        #region Calendar

        /*
         * Load All CalendarList
         * 
         * @param: CalendarListLoaded : a callback
         */
        public void LoadCalendarList(Action<IRestResponse<CalendarList>> CalendarListLoaded)
        {
            _oAuth.GetAccessCode(access_token =>
            {
                lock (_sync)
                {
                    _requesting++;
                    // building request
                    _client.Authenticator = new OAuth2AuthorizationRequestHeaderAuthenticator(access_token);
                    var request = new RestRequest("/calendar/v3/users/me/calendarList", Method.GET);
                    _client.ExecuteAsync<CalendarList>(request, response =>
                    {
                        lock (_sync)
                        {
                            _requesting--;
                            // handle callback
                            _calendarLoaded = true;
                            _calendar = response.Data != null ? response.Data.Items[3] : _calendar;
                            if (CalendarListLoaded != null) CalendarListLoaded(response);
                        }
                    });
                }
            });
        }

        #endregion

        #region Event

        /*
         * Load All Events from default calendar
         * 
         * @param: EventsLoaded : a callback
         */
        public void LoadEvents(Action<IRestResponse<Events>> EventsLoaded)
        {
            if (_calendarLoaded)
                _LoadEvents(EventsLoaded);
            else
                LoadCalendarList((res) =>
                {
                    _LoadEvents(EventsLoaded);
                });
        }

        /*
         * Load All Events from default calendar
         * 
         * @param: EventsLoaded : a callback
         */
        private void _LoadEvents(Action<IRestResponse<Events>> EventsLoaded)
        {
            _oAuth.GetAccessCode(access_token =>
            {
                lock (_sync)
                {
                    _requesting++;
                    // building request
                    _client.Authenticator = new OAuth2AuthorizationRequestHeaderAuthenticator(access_token);
                    var request = new RestRequest("/calendar/v3/calendars/" + _calendar.Id + "/events", Method.GET);
                    _client.ExecuteAsync<Events>(request, response =>
                    {
                        lock (_sync)
                        {
                            _requesting--;
                            // handle request's callback
                            __events = response.Data != null ? response.Data : __events;
                            _events.Clear();
                            if (__events.Items != null)
                                foreach (Event e in __events.Items)
                                    _events.Add(e);
                            if (EventsLoaded != null) EventsLoaded(response);
                        }
                    });
                }
            });
        }

        /*
         * Insert a event from default calendar
         * 
         * @param: InsertedEvent : a callback
         * @param: e : a Event
         */
        public void InsertEvent(Action<IRestResponse<Event>> InsertedEvent, Event e)
        {
            if (_calendarLoaded)
                _InsertEvent(InsertedEvent, e);
            else
                LoadCalendarList((res) =>
                {
                    _InsertEvent(InsertedEvent, e);
                });
        }

        /*
         * Insert a event from default calendar
         * 
         * @param: InsertedEvent : a callback
         * @param: e : a Event
         */
        private void _InsertEvent(Action<IRestResponse<Event>> InsertedEvent, Event e)
        {
            _oAuth.GetAccessCode(access_token =>
            {
                lock (_sync)
                {
                    _requesting++;
                    // building request
                    _client.Authenticator = new OAuth2AuthorizationRequestHeaderAuthenticator(access_token);
                    var request = new RestRequest("/calendar/v3/calendars/" + _calendar.Id + "/events", Method.POST);
                    // serialize Event
                    string eventJson = JsonConvert.SerializeObject(e, Formatting.None, _serializerSettings);
                    request.AddParameter("application/json; charset=utf-8", eventJson, ParameterType.RequestBody);
                    _client.ExecuteAsync<Event>(request, response =>
                    {
                        lock (_sync)
                        {
                            _requesting--;
                            // handle request's callback
                            if (_events != null && response.Data != null)
                                _events.Add(response.Data);
                            if (InsertedEvent != null) InsertedEvent(response);
                        }
                    });
                }
            });
        }

        /*
         * Update a event from default calendar
         * 
         * @param: EventUpdated : a callback
         * @param: e : a Event
         */
        public void UpdateEvent(Action<IRestResponse<Event>> EventUpdated, Event e)
        {
            if (_calendarLoaded)
                _UpdateEvent(EventUpdated, e);
            else
                LoadCalendarList((res) =>
                {
                    _UpdateEvent(EventUpdated, e);
                });
        }

        /*
         * Update a event from default calendar
         * 
         * @param: EventUpdated : a callback
         * @param: e : a Event
         */
        private void _UpdateEvent(Action<IRestResponse<Event>> EventUpdated, Event e)
        {
            _oAuth.GetAccessCode(access_token =>
            {
                lock (_sync)
                {
                    _requesting++;
                    // building request
                    _client.Authenticator = new OAuth2AuthorizationRequestHeaderAuthenticator(access_token);
                    var request = new RestRequest("/calendar/v3/calendars/" + _calendar.Id + "/events/" + e.Id, Method.PUT);
                    // serialize Event
                    string eventJson = JsonConvert.SerializeObject(e, Formatting.None, _serializerSettings);
                    request.AddParameter("application/json; charset=utf-8", eventJson, ParameterType.RequestBody);
                    _client.ExecuteAsync<Event>(request, response =>
                    {
                        lock (_sync)
                        {
                            _requesting--;
                            // handle request's callback
                            if (_events != null && response.Data != null)
                            {
                                foreach (Event _e in _events)
                                {
                                    // Update Events
                                    if (!e.Id.Equals(_e.Id)) continue;
                                    _e.UpdateFrom(response.Data);
                                    break;
                                }
                            }
                            if (EventUpdated != null) EventUpdated(response);
                        }
                    });
                }
            });
        }

        /*
         * Delete a event from default calendar
         * 
         * @param: EventDeleted : a callback
         */
        public void DeleteEvent(Action<IRestResponse<Error>> EventDeleted, Event e)
        {
            if (_calendarLoaded)
                _DeleteEvent(EventDeleted, e);
            else
                LoadCalendarList((res) =>
                {
                    _DeleteEvent(EventDeleted, e);
                });
        }

        /*
         * Delete a event from default calendar
         * 
         * @param: EventDeleted : a callback
         */
        private void _DeleteEvent(Action<IRestResponse<Error>> EventDeleted, Event e)
        {
            _oAuth.GetAccessCode(access_token =>
            {
                lock (_sync)
                {
                    _requesting++;
                    // building request
                    _client.Authenticator = new OAuth2AuthorizationRequestHeaderAuthenticator(access_token);
                    var request = new RestRequest("/calendar/v3/calendars/" + _calendar.Id + "/events/" + e.Id, Method.DELETE);
                    // serialize Event
                    string eventJson = JsonConvert.SerializeObject(e, Formatting.None, _serializerSettings);
                    request.AddParameter("application/json; charset=utf-8", eventJson, ParameterType.RequestBody);
                    _client.ExecuteAsync<Error>(request, response =>
                    {
                        lock (_sync)
                        {
                            _requesting--;
                            // handle request's callback
                            if (_events != null && response.StatusCode == HttpStatusCode.NoContent && response.Data == null)
                            {
                                // Update Events
                                foreach (Event _e in _events)
                                {
                                    if (!e.Id.Equals(_e.Id)) continue;
                                    _events.Remove(_e);
                                    break;
                                }
                            }
                            if (EventDeleted != null) EventDeleted(response);
                        }
                    });
                }
            });
        }

        #endregion

        #region Color

        /*
         * Load All Colors
         * 
         * @param: CalendarListLoaded : a callback
         */
        public void LoadColors(Action<IRestResponse<Colors>> ColorsLoaded)
        {
            _oAuth.GetAccessCode(access_token =>
            {
                // building request
                _client.Authenticator = new OAuth2AuthorizationRequestHeaderAuthenticator(access_token);
                var request = new RestRequest("/calendar/v3/colors", Method.GET);
                _client.ExecuteAsync<Colors>(request, response =>
                {
                    // handle callback
                    _colors = response.Data != null ? response.Data : _colors;
                    if (ColorsLoaded != null) ColorsLoaded(response);
                });
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
         * Return a event
         * 
         * @param: id_event : string
         */
        public Event getEventById(string id)
        {
            foreach (Event e in _events)
            {
                if (!e.Id.Equals(id)) continue;
                return e;
            }
            throw new Exception("Event not found");
        }

        /*
         * If a Event happens on a certain Day
         * 
         * @param e : Event
         * @param date : DateTime
         */
        private bool isToday(Event e, DateTime date)
        {
            String myDate = date.Year.ToString() + "-" + Tools.addZeroFormatStringDate(date.Month) + "-" + Tools.addZeroFormatStringDate(date.Day);

            if (e.Start.DateTime != null)
            {
                if (e.Start.DateTime.Contains(myDate))
                    return true;
                if (Convert.ToDateTime(e.Start.DateTime) < date && e.End.DateTime != null && date < Convert.ToDateTime(e.End.DateTime))
                    return true;
                if (e.Recurrence != null && e.Recurrence.Count > 0)
                {
                    DDay.iCal.RecurrencePattern pattern = new DDay.iCal.RecurrencePattern(e.Recurrence[0]);
                    DDay.iCal.RecurrencePatternEvaluator re = new DDay.iCal.RecurrencePatternEvaluator(pattern);
                    IList<DDay.iCal.IPeriod> list = re.Evaluate(
                        new DDay.iCal.iCalDateTime(e.Start.DateTime), date, date.AddDays(1), false);
                    if (list.Count == 1) return true;
                }
            }
            // Event which takes all day
            if (e.Start.Date != null)
            {
                if (e.Start.Date.Equals(myDate))
                    return true;
                if (e.Start.Date.CompareTo(myDate) == -1 && e.End.Date != null && myDate.CompareTo(e.End.Date) == -1)
                    return true;
                if (e.Recurrence != null && e.Recurrence.Count > 0)
                {
                    DDay.iCal.RecurrencePattern pattern = new DDay.iCal.RecurrencePattern(e.Recurrence[0]);
                    DDay.iCal.RecurrencePatternEvaluator re = new DDay.iCal.RecurrencePatternEvaluator(pattern);
                    IList<DDay.iCal.IPeriod> list = re.Evaluate(
                        new DDay.iCal.iCalDateTime(Convert.ToDateTime(e.Start.Date)), date, date.AddDays(1), false);
                    if (list.Count == 1) return true;
                }
            }
            return false;
        }

        /*
         * Return all Event that happens on a Day
         * 
         * @param: date : DateTime
         */
        public List<Event> getEventByDay(DateTime date)
        {
            List<Event> list = new List<Event>();

            if (_events == null) return list;

            foreach (Event e in _events)
            {
                if (isToday(e, date))
                    list.Add(e);
            }
            return list;
        }

        /*
         * Return a ColorDefinition corresponding to the id
         * 
         * @param: id : idColor
         */
        public ColorDefinition getEventColorById(string id)
        {
            ColorDefinition cd;
            _colors.Event.TryGetValue(id, out cd);
            if (cd == null) throw new KeyNotFoundException();
            return cd;
        }
    }
}
