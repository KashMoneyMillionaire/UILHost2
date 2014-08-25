using UILHost.Infrastructure.Domain;

namespace UILHost.Web.Service
{
    public interface IAuthenticationService
    {
        string GetCurrentUserFirstName();
        string GetCurrentUserUsername();
        long GetCurrentUserId();
        //bool CurrentUserProfileHasPermission(SecurityModuleKey permissionKey);
        void SignIn(Teacher profile, bool persist);
        void SignOut();
        Teacher GetCurrentTeacher(long id = 0);
    }
}