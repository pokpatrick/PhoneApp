using System;
using System.Net;
using System.Diagnostics;

using RestSharp;

namespace PhoneApp.Model
{
    public class AuthenticationProcess
    {
        RestClient client;

        public AuthenticationProcess()
        {
            BaseUri = "https://accounts.google.com";
            ApprovalEndPoint = "/o/oauth2/approval";
            TokenEndPoint = "/o/oauth2/token";
            AuthEndPoint = "/o/oauth2/auth";

            ClientId = "";
            Secret = "";
            Scope = "";

            client = new RestClient(this.BaseUri);
        }

        public bool HasAuthenticated
        {
            get
            {
                return AuthResult != null && !string.IsNullOrEmpty(AuthResult.access_token);
            }
        }

        public void Revoke()
        {
            AuthResult = null;
        }

        public Uri AuthUri
        {
            get
            {
                UriBuilder builder = new UriBuilder(BaseUri);
                builder.Path = AuthEndPoint;
                builder.Query = string.Format("response_type=code&redirect_uri=http://localhost&scope={0}&client_id={1}", Scope, ClientId);
                return builder.Uri;
            }
        }

        public void ExchangeCodeForToken(string code)
        {
            if (string.IsNullOrEmpty(code))
            {
                OnAuthenticationFailed();
            }
            else
            {
                var request = new RestRequest(this.TokenEndPoint, Method.POST);
                request.AddParameter("code", code);
                request.AddParameter("client_id", this.ClientId);
                request.AddParameter("client_secret", this.Secret);
                request.AddParameter("redirect_uri", "http://localhost");
                request.AddParameter("grant_type", "authorization_code");

                client.ExecuteAsync<AuthResult>(request, GetAccessToken);
            }
        }

        void GetAccessToken(IRestResponse<AuthResult> response)
        {
            if (response == null || response.StatusCode != HttpStatusCode.OK
                || response.Data == null || string.IsNullOrEmpty(response.Data.access_token))
            {
                OnAuthenticationFailed();
            }
            else
            {
                Debug.Assert(response.Data != null);
                AuthResult = response.Data;
                OnAuthenticated();
            }
        }

        public void RefreshAccessToken()
        {
            Debug.Assert(HasAuthenticated);

            var authorize = new RestRequest(this.TokenEndPoint, Method.POST);
            authorize.AddParameter("refresh_token", AuthResult.refresh_token);
            authorize.AddParameter("client_id", this.ClientId);
            authorize.AddParameter("client_secret", this.Secret);
            authorize.AddParameter("grant_type", "refresh_token");

            client.ExecuteAsync<AuthResult>(authorize, RefreshAccessToken);
        }

        void RefreshAccessToken(IRestResponse<AuthResult> response)
        {
            if (response == null || response.StatusCode != HttpStatusCode.OK
                || response.Data == null || string.IsNullOrEmpty(response.Data.access_token))
            {
                OnAuthenticationFailed();
            }
            else
            {
                Debug.Assert(response.Data != null);
                Debug.Assert(AuthResult != null);

                AuthResult r = response.Data;
                r.refresh_token = AuthResult.refresh_token;
                AuthResult = r;
                OnAuthenticated();
            }
        }

        public event EventHandler Authenticated;
        protected virtual void OnAuthenticated()
        {
            var e = Authenticated;
            if (e != null)
                e(this, EventArgs.Empty);
        }

        public event EventHandler AuthenticationFailed;
        public virtual void OnAuthenticationFailed()
        {
            var e = AuthenticationFailed;
            if (e != null)
                e(this, EventArgs.Empty);
        }

        /// <summary>
        /// Gets or sets the value of the <see cref="AuthResult" />
        /// property. This is a dependency property.
        /// </summary>
        public AuthResult AuthResult { get; set; }

        /// <summary>
        /// Gets or sets the value of the <see cref="ApprovalEndPoint" />
        /// property. This is a dependency property.
        /// </summary>
        public string ApprovalEndPoint { get; set; }

        /// <summary>
        /// Gets or sets the value of the <see cref="TokenEndPoint" />
        /// property. This is a dependency property.
        /// </summary>
        public string TokenEndPoint { get; set; }

        /// <summary>
        /// Gets or sets the value of the <see cref="Scope" />
        /// property. This is a dependency property.
        /// </summary>
        public string Scope { get; set; }

        /// <summary>
        /// Gets or sets the value of the <see cref="AuthEndPoint" />
        /// property. This is a dependency property.
        /// </summary>
        public string AuthEndPoint { get; set; }

        /// <summary>
        /// Gets or sets the value of the <see cref="BaseUri" />
        /// property. This is a dependency property.
        /// </summary>
        public String BaseUri { get; set; }

        /// <summary>
        /// Gets or sets the value of the <see cref="Secret" />
        /// property. This is a dependency property.
        /// </summary>
        public string Secret { get; set; }

        /// <summary>
        /// Gets or sets the value of the <see cref="ClientId" />
        /// property. This is a dependency property.
        /// </summary>
        public string ClientId { get; set; }
    }
}
