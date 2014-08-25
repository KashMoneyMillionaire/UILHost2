using System;
using System.Web;
using UILHost.Infrastructure.Domain;
using UILHost.Infrastructure.IoC;
using UILHost.Infrastructure.Services.Interface;
using UILHost.Web.Service;

namespace UILHost.Web
{
    public class SessionFacade
    {
        private readonly HttpSessionStateBase _sessionState = null;

        public HttpSessionStateBase Session
        {
            get
            {
                return HttpContext.Current != null
                    ? new HttpSessionStateWrapper(HttpContext.Current.Session)
                    : _sessionState;
            }
        }

        private const string KEY_CURRENT_CLIENT = "UILHost.CurrentClient";

        //private const string KEY_LOGIN_SECURITY_QUESTION_VERIFICATION_REQUEST = "UILHost.LoginSecurityQuestionVerificationRequest";
        //private const string KEY_FORGOT_PASSWORD_VERIFICATION_REQUEST = "UILHost.ForgotPasswordVerificationRequest";

        //private const string KEY_CURRENT_STANDARD_REPORTING_QUERY = "UILHost.ReportsFromLastStandardReportingQuery";
        //private const string KEY_CURRENT_TAX_REPORTING_QUERY = "UILHost.ReportsFromLastTaxReturnReportingQuery";

        //public LoginSecurityQuestionVerificationRequest LoginSecurityQuestionVerificationRequest
        //{
        //    get { return (LoginSecurityQuestionVerificationRequest)this.Session[KEY_LOGIN_SECURITY_QUESTION_VERIFICATION_REQUEST]; }
        //    set { this.Session[KEY_LOGIN_SECURITY_QUESTION_VERIFICATION_REQUEST] = value; }
        //}

        //public ForgotPasswordVerificationRequest ForgotPasswordVerificationRequest
        //{
        //    get { return (ForgotPasswordVerificationRequest)this.Session[KEY_FORGOT_PASSWORD_VERIFICATION_REQUEST]; }
        //    set { this.Session[KEY_FORGOT_PASSWORD_VERIFICATION_REQUEST] = value; }
        //}


        public Teacher CurrentTeacher
        {
            get
            {
                return DependencyResolver.GetDependency<IAuthenticationService>()
                    .GetCurrentTeacher();
            }
            set
            {
                if (value == null)
                    Session[KEY_CURRENT_CLIENT] = null;
                else
                {
                    if (value.Id < 1)
                        throw new ArgumentException("The teacher cannot be new - the ID must be larger than one");

                    Session[KEY_CURRENT_CLIENT] = value.Id;
                }


            }
        }

        //public ReportsFromLastStandardReportRequest ReportsFromLastStandardReportingQuery
        //{
        //    get { return (ReportsFromLastStandardReportRequest)this.Session[KEY_CURRENT_STANDARD_REPORTING_QUERY]; }
        //    set { this.Session[KEY_CURRENT_STANDARD_REPORTING_QUERY] = value; }
        //}

        //public ReportsFromLastTaxReturnReportRequest ReportsFromLastTaxReturnReportingQuery
        //{
        //    get { return (ReportsFromLastTaxReturnReportRequest)this.Session[KEY_CURRENT_TAX_REPORTING_QUERY]; }
        //    set { this.Session[KEY_CURRENT_TAX_REPORTING_QUERY] = value; }
        //}

        public SessionFacade() : this(null) { }
        public SessionFacade(HttpSessionStateBase sessionState)
        {
            _sessionState = sessionState;
        }

    }
}