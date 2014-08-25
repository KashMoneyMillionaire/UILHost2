using System.Globalization;
using System.Linq;
using System.Security.Authentication;
using System.Security.Claims;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using UILHost.Infrastructure.Domain;
using UILHost.Infrastructure.Services.Interface;

namespace UILHost.Web.Service
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly SessionFacade _session;
        private readonly ITeacherService _teacherSvc;

        private readonly IAuthenticationManager _authManager = null;

        public IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.Current != null
                    ? HttpContext.Current.GetOwinContext().Authentication
                    : _authManager;
            }
        }

        public AuthenticationService(
            SessionFacade session,
            ITeacherService teacherSvc)
            : this(session, null, teacherSvc) { }

        public AuthenticationService(
            SessionFacade session,
            IAuthenticationManager authManager,
            ITeacherService teacherSvc)
        {
            _authManager = authManager;
            _teacherSvc = teacherSvc;
            _session = session;
        }

        public string GetCurrentUserUsername()
        {
            var userUsernameClaim = AuthenticationManager.User.Claims
                .FirstOrDefault(c => c.Type == ClaimTypes.Name);

            if (userUsernameClaim == null)
                return "";

            return userUsernameClaim.Value;
        }

        public long GetCurrentUserId()
        {
            var claim = AuthenticationManager.User.Claims
                .FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            if (claim != null)
                return long.Parse(claim.Value);
            return -1;
        }

        public string GetCurrentUserFirstName()
        {
            if (_session.CurrentTeacher == null)
            {
                _session.CurrentTeacher = GetCurrentTeacher();
            }
            return _session.CurrentTeacher.FirstName;
        }

        public Teacher GetCurrentTeacher(long id = 0)
        {
            var teacher = id == 0 ? _teacherSvc.Read(GetCurrentUserId()) : _teacherSvc.Read(id);
            if(teacher == null)
                throw new AuthenticationException("User is not signed in.");
            return teacher;
        }

        //public bool CurrentUserProfileHasPermission(SecurityModuleKey permissionKey)
        //{
        //    var userProfile = _userProfileSvc.Find(GetCurrentUserId());
        //    var cup = _clientUserProfileSvc.ReadById(userProfile.Id);

        //    return cup.SecurityRoles.Any(s => s.SecurityModules.Select(p => p.Key).Contains(permissionKey));
        //}

        public void SignIn(Teacher userProfile, bool persist)
        {
            SignOut();

            var identity =
                new ClaimsIdentity(
                new[] 
                            { 
                                new Claim(ClaimTypes.Name, userProfile.Email),
                                new Claim(ClaimTypes.NameIdentifier, userProfile.Id.ToString(CultureInfo.InvariantCulture))
                            },
                DefaultAuthenticationTypes.ApplicationCookie,
                ClaimTypes.Name,
                ClaimTypes.Role);

            AuthenticationManager.SignIn(new AuthenticationProperties() { IsPersistent = persist }, identity);

            _session.CurrentTeacher = userProfile;
        }

        public void SignOut()
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
        }
    }
}