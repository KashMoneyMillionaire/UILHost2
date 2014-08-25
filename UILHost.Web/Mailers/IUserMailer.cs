using Mvc.Mailer;

namespace UILHost.Web.Mailers
{ 
    public interface IUserMailer
    {
        MvcMailMessage Welcome(string email);
        MvcMailMessage PasswordReset(string email);
    }
}