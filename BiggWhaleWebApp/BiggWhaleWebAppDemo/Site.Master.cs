using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BiggWhaleWebAppDemo
{
    public partial class SiteMaster : MasterPage
    {
        private const string AntiXsrfTokenKey = "__AntiXsrfToken";
        private const string AntiXsrfUserNameKey = "__AntiXsrfUserName";
        private string _antiXsrfTokenValue;

        protected void Page_Init(object sender, EventArgs e)
        {
            // The code below helps to protect against XSRF attacks
            var requestCookie = Request.Cookies[AntiXsrfTokenKey];
            Guid requestCookieGuidValue;
            if (requestCookie != null && Guid.TryParse(requestCookie.Value, out requestCookieGuidValue))
            {
                // Use the Anti-XSRF token from the cookie
                _antiXsrfTokenValue = requestCookie.Value;
                Page.ViewStateUserKey = _antiXsrfTokenValue;
            }
            else
            {
                // Generate a new Anti-XSRF token and save to the cookie
                _antiXsrfTokenValue = Guid.NewGuid().ToString("N");
                Page.ViewStateUserKey = _antiXsrfTokenValue;

                var responseCookie = new HttpCookie(AntiXsrfTokenKey)
                {
                    HttpOnly = true,
                    Value = _antiXsrfTokenValue
                };
                if (FormsAuthentication.RequireSSL && Request.IsSecureConnection)
                {
                    responseCookie.Secure = true;
                }
                Response.Cookies.Set(responseCookie);
            }

            Page.PreLoad += master_Page_PreLoad;
        }

        protected void master_Page_PreLoad(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Set Anti-XSRF token
                ViewState[AntiXsrfTokenKey] = Page.ViewStateUserKey;
                ViewState[AntiXsrfUserNameKey] = Context.User.Identity.Name ?? String.Empty;
            }
            else
            {
                // Validate the Anti-XSRF token
                if ((string)ViewState[AntiXsrfTokenKey] != _antiXsrfTokenValue
                    || (string)ViewState[AntiXsrfUserNameKey] != (Context.User.Identity.Name ?? String.Empty))
                {
                    throw new InvalidOperationException("Validation of Anti-XSRF token failed.");
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!Page.IsPostBack)
                {
                    string page = Path.GetFileNameWithoutExtension(Request.AppRelativeCurrentExecutionFilePath);
                    string pageDirectory = Path.GetDirectoryName(Request.AppRelativeCurrentExecutionFilePath);

                    navMainAnonymous.Visible = !Request.IsAuthenticated;
                    navMainAuthenticated.Visible = Request.IsAuthenticated;

                    string category = Request.QueryString.Count > 0 ? Request.QueryString[0] : string.Empty;
                    if (pageDirectory.Length > 3)
                    {
                        pageDirectory = pageDirectory.Substring(2, pageDirectory.Length - 2);
                    }
                    if (pageDirectory != null && pageDirectory.Length > 0 && page != null && page.Length > 0)
                    {
                        switch (pageDirectory)
                        {
                            case "~":
                                switch (page)
                                {
                                    case "Dashboard":
                                        lnkDashboard.CssClass = "current-menu-item";
                                        break;
                                    case "Search":
                                        lnkSearch.CssClass = "current-menu-item";
                                        break;
                                    case "Contact":
                                        lnkContact.CssClass = "current-menu-item";
                                        break;
                                }
                                break;
                            case "Account":
                                switch (page)
                                {
                                    case "Login":
                                        lnkLogin.CssClass = "current-menu-item";
                                        break;
                                }
                                break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}