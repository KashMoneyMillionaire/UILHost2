using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(UILHost.Web.Startup))]
namespace UILHost.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
