using System;
using System.Diagnostics;
using System.Net;
using System.Net.NetworkInformation;
using System.Collections.Generic;
using System.Windows;
using System.Threading;
using System.IO.IsolatedStorage;
using System.Xml.Serialization;
using Microsoft.Phone.Controls;

namespace PhoneApp.Model
{
    public class OAuth
    {
        private object _sync = new object();
        private WebBrowser _webBrowser = new WebBrowser();
        private AuthenticationProcess _process = new AuthenticationProcess()
        {
            ClientId = "737021515087.apps.googleusercontent.com",
            Secret = "aLsTdiHw11X-zZjArucY_yUi",

            // this specifies which Google APIs your app intends to use and needs permission for
            Scope = "https://www.googleapis.com/auth/userinfo.profile https://www.googleapis.com/auth/tasks https://www.googleapis.com/auth/calendar"
        };

        #region Consructor
        private OAuth()
        {
            _process.Authenticated += new EventHandler(_process_Authenticated);
            _process.AuthenticationFailed += new EventHandler(_process_AuthenticationFailed);
            _webBrowser.Visibility = Visibility.Collapsed;
            _webBrowser.IsScriptEnabled = true;
            _webBrowser.Navigating += webBrowser_Navigating;
            _webBrowser.LoadCompleted += webBrowser_LoadCompleted;
        }

        private OAuth(AuthResult authResult) : this()
        {
            _process.AuthResult = authResult;
        }

        private OAuth(string email, string passwd): this()
        {
            _email = email;
            _passwd = passwd;
        }
        #endregion

        #region Singleton
        private static OAuth _instance;
        public static OAuth Instance
        {
            get
            {
                if (_instance == null)
                {
                    AuthResult authResult = null;
                    IsolatedStorageSettings.ApplicationSettings.TryGetValue<AuthResult>("AuthResult", out authResult);
                    _instance = authResult != null ? new OAuth(authResult) : new OAuth();
                    bool autoLogin = false;
                    IsolatedStorageSettings.ApplicationSettings.TryGetValue<bool>("AutoLogin", out autoLogin);
                    if (autoLogin)
                    {
                        string email, passwd;
                        IsolatedStorageSettings.ApplicationSettings.TryGetValue<string>("Email", out email);
                        IsolatedStorageSettings.ApplicationSettings.TryGetValue<string>("Passwd", out passwd);
                        _instance.Email = email;
                        _instance.Passwd = passwd;
                    }
                }
                return _instance;
            }
        }
        #endregion

        #region Properties

        private string _email = String.Empty;
        public string Email
        {
            get { return _email; }
            set { _email = value; }
        }

        private string _passwd = String.Empty;
        public string Passwd
        {
            get { return _passwd; }
            set { _passwd = value; }
        }

        private bool _autoLogin = false;
        public bool AutoLogin
        {
            get { return !_email.Equals(String.Empty) && !_passwd.Equals(String.Empty); }
            set { _autoLogin = value; }
        }

        public bool HasAuthenticated
        {
            get { lock (_sync) return _process.HasAuthenticated; }
        }

        public DateTime TokenExpiresAt
        {
            get
            {
                lock (_sync)
                {
                    if (HasAuthenticated)
                        return _process.AuthResult.ExpiresAt;
                }
                return DateTime.MinValue;
            }
        }

        private Uri _authUri = new Uri("about:blank");
        public Uri AuthUri
        {
            get { return _authUri; }
            set { if (_authUri != value) { _authUri = value; } }
        }

        public string ApprovalHost
        {
            get { return "localhost"; }
        }

        private string _code;
        public string Code
        {
            get { return _code; }
            set {
                _code = value;
                if (!isNetworkAvailable())
                    return;
                _process.ExchangeCodeForToken(Code);
            }
        }
        #endregion

        #region Events
        public event EventHandler ConnectionTimedOut;
        private void OnConnectionTimedOut()
        {
            var e = ConnectionTimedOut;
            if (e != null)
                e(this, EventArgs.Empty);
        }

        public event EventHandler Authenticated
        {
            add { lock (_sync) _process.Authenticated += value; }
            remove { lock (_sync) _process.Authenticated -= value; }
        }

        public event EventHandler AuthenticationFailed
        {
            add { lock (_sync) _process.AuthenticationFailed += value; }
            remove { lock (_sync) _process.AuthenticationFailed -= value; }
        }
        #endregion

        public void Logout()
        {
            lock (_sync) { _process.Revoke(); _isAuthenticating = false; }
        }

        private bool _isAuthenticating;
        private Queue<Action<string>> _queuedRequests = new Queue<Action<string>>();
        public void GetAccessCode(Action<string> callback)
        {
            if (!isNetworkAvailable())
                return;

            lock (_sync)
            {
                if (_isAuthenticating)
                {
                    _queuedRequests.Enqueue(callback);
                }
                else if (HasAuthenticated)
                {
                    if (!_process.AuthResult.IsExpired)
                    {
                        callback(_process.AuthResult.access_token);
                    }
                    else
                    {
                        _isAuthenticating = true;
                        _queuedRequests.Enqueue(callback);
                        LaunchTimer();
                        _process.RefreshAccessToken();
                    }
                }
                else
                {
                    _isAuthenticating = true;
                    _queuedRequests.Enqueue(callback);

                    LaunchTimer();

                    _webBrowser.Navigate(_process.AuthUri);

                    AuthUri = _process.AuthUri;
                }
            }
        }

        private void LaunchTimer()
        {
            new Timer(new TimerCallback(OnTimeOut), null, Tools.requestTimeOut, 0);
        }

        private void OnTimeOut(object state)
        {
            if (HasAuthenticated) return;
            Deployment.Current.Dispatcher.BeginInvoke(() =>
            {
                _isAuthenticating = false;
                OnConnectionTimedOut();
                MessageBox.Show("Connection timed out.", "Network", MessageBoxButton.OK);
            });
        }

        private Boolean isNetworkAvailable()
        {
            if (!NetworkInterface.GetIsNetworkAvailable())
            {
                MessageBox.Show("Internet Unavailable.", "Network", MessageBoxButton.OK);
                return false;
            }
            return true;
        }

        private void _process_Authenticated(object sender, EventArgs e)
        {
            lock (_sync)
            {
                _isAuthenticating = false;

                while (_queuedRequests.Count > 0)
                    _queuedRequests.Dequeue()(_process.AuthResult.access_token);

                IsolatedStorageSettings.ApplicationSettings["AuthResult"] = _process.AuthResult;
                if (_autoLogin && AutoLogin)
                {
                    IsolatedStorageSettings.ApplicationSettings["AutoLogin"] = true;
                    IsolatedStorageSettings.ApplicationSettings["Email"] = _email;
                    IsolatedStorageSettings.ApplicationSettings["Passwd"] = _passwd;
                }
            }
        }

        private void _process_AuthenticationFailed(object sender, EventArgs e)
        {
            lock (_sync)
            {
                _isAuthenticating = false;
                _process.Revoke();

                AuthUri = new Uri("about:blank");
                AuthUri = _process.AuthUri;
                MessageBox.Show("Please try again", "Login failed", MessageBoxButton.OK);
                ((PhoneApplicationFrame)App.Current.RootVisual).Navigate(new Uri("/Page/Authentication.xaml", UriKind.Relative));
            }
        }

        private void webBrowser_Navigating(object sender, NavigatingEventArgs e)
        {
            if (e.Uri.Host.Equals("localhost"))
            {
                e.Cancel = true;
                int pos = e.Uri.Query.IndexOf("=");
                // setting this text will bind it back to the view
                Code = pos > -1 ? e.Uri.Query.Substring(pos + 1) : null;
            }
            else if (!_isAuthenticating)
            {
                e.Cancel = true;
                Code = String.Empty;
            }
        }

        private void webBrowser_LoadCompleted(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {

            if (e.Uri.AbsolutePath.Equals("/ServiceLogin") && e.Uri.Query.Contains("service=lso"))
            {
                try
                {
                    _webBrowser.InvokeScript("eval", "document.getElementById('Email').value = '" + _email + "';");
                    _webBrowser.InvokeScript("eval", "document.getElementById('Passwd').value = '" + _passwd + "';");
                    _webBrowser.InvokeScript("eval", "document.getElementById('signIn').click()");
                }
                catch { }
            }
            else if (e.Uri.AbsolutePath.Equals("/ServiceLoginAuth"))
            {
                bool error = false;
                try
                {
                    _webBrowser.InvokeScript("eval", "document.getElementById('errormsg_0_Passwd').innerHTML = ''");
                    error = true;
                }
                catch { }
                if (!error)
                {
                    try
                    {
                        _webBrowser.InvokeScript("eval", "document.getElementById('errormsg_0_Email').innerHTML = ''");
                        error = true;
                    }
                    catch { }
                }
                if (error)
                {
                    Code = String.Empty;
                }
            }
            else if (e.Uri.AbsolutePath.Equals(_process.AuthEndPoint) && !e.Uri.Query.Contains("approval"))
            {
                try
                {
                    _webBrowser.InvokeScript("eval", "setTimeout(\"document.getElementById('submit_approve_access').click()\", 2500)");
                }
                catch { }
            }
        }
    }
}
