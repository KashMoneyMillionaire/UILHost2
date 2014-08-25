using System.Web;
using Microsoft.Owin.Security;
using UILHost.Infrastructure.IoC;
using UILHost.Web.Service;

namespace UILHost.Web
{ 
    public class HttpEvironmentModule : ModuleBase
    {
        private readonly HttpSessionStateBase _sessionState;
        private readonly IAuthenticationManager _authManager;

        public HttpEvironmentModule(
            HttpSessionStateBase sessionState = null,
            IAuthenticationManager authManager = null)
        {
            this._sessionState = sessionState;
            this._authManager = authManager;
        }

        public override void Load()
        {
            BindToSelfForTransientScope<SessionFacade>(null, binding =>
            {
                if (_sessionState != null)
                    binding.WithConstructorArgument("sessionState", _sessionState);
            });

            BindForTransientScope<IAuthenticationService, AuthenticationService>();
            if (_authManager != null)
                Bind<IAuthenticationManager>().ToConstant(_authManager);

            // FILTER BINDINGS

            //Kernel.BindFilter<ContextFilter>(FilterScope.Global, 0);

            //Kernel.BindFilter<PageViewAuditFilter>(FilterScope.Global, 1);

            //Kernel.BindFilter<RoleAuthenticationFilter>(FilterScope.Action, 2)
            //    .WhenActionMethodHas<RoleAttribute>();
            //.WithConstructorArgumentFromActionAttribute<RoleAttribute>("permissionsAllowed", o => o.permissionsAllowed)
            //.WithConstructorArgumentFromActionAttribute<RoleAttribute>("unauthorizedMessage", o => o.UnauthorizedMessage);
        }
    }
}