using Mvc.Mailer;

namespace UILHost.Web.Mailers
{ 
    public class UserMailer : MailerBase, IUserMailer 	
	{
		public UserMailer()
		{
			MasterName="_Layout";
		}
		
		public virtual MvcMailMessage Welcome(string email)
		{
			//ViewBag.Data = someObject;
			return Populate(x =>
			{
				x.Subject = "Welcome";
				x.ViewName = "Welcome";
				x.To.Add(email);
			});
		}
 
		public virtual MvcMailMessage PasswordReset(string email)
		{
			//ViewBag.Data = someObject;
			return Populate(x =>
			{
				x.Subject = "Password Reset";
				x.ViewName = "Password Reset";
				x.To.Add(email);
			});
		}
 	}
}