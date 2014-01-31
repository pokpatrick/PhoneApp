using System;
using System.Net;
using System.Windows;
using System.Threading;
using Microsoft.Phone.Controls;

namespace PhoneApp.Model
{
    public class LoginProcess
    {
        private enum State { TimeOut, LoggedIn, Error, Empty };

        private Timer _timer;
        private Uri _authUri;
        private string _authEndPoint;
        private State _state = State.Empty;
        private string _email = String.Empty;
        private string _passwd = String.Empty;
        private WebBrowser _webBrowser = new WebBrowser();

        public LoginProcess(Uri authUri, string authEndPoint, string email, string passwd)
        {
            _authUri = authUri;
            _authEndPoint = authEndPoint;
            _email = email;
            _passwd = passwd;
            _webBrowser.IsScriptEnabled = true;
            _webBrowser.Visibility = Visibility.Collapsed;
            _webBrowser.Navigating += webBrowser_Navigating;
            _webBrowser.LoadCompleted += webBrowser_LoadCompleted;
        }

        // Méthode boucle du thread
        public void run()
        {
            _timer = new Timer((state) =>
            {
                _timer.Dispose();
                _state = State.TimeOut;

            }, null, 30000, 1000);
            _webBrowser.Navigate(_authUri);
            while (_state == State.Empty) ;
        }

        private void webBrowser_Navigating(object sender, NavigatingEventArgs e)
        {
            if (e.Uri.Host.Equals("localhost"))
            {
                e.Cancel = true;
                int pos = e.Uri.Query.IndexOf("=");

                // setting this text will bind it back to the view
                OAuth.Instance.Code = pos > -1 ? e.Uri.Query.Substring(pos + 1) : null;
                _state = State.LoggedIn;
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
                    //_process.ExchangeCodeForToken(String.Empty);
                    _state = State.Error;
                }
            }
            else if (e.Uri.AbsolutePath.Equals(_authEndPoint) && !e.Uri.Query.Contains("approval"))
            {
                try
                {
                    _webBrowser.InvokeScript("eval", "document.getElementById('submit_approve_access').click()");
                }
                catch { }
            }
        }
    }
}
