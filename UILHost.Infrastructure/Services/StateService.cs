using UILHost.Infrastructure.Domain;
using UILHost.Infrastructure.Services.Interface;
using UILHost.Repository.Pattern.Repositories;
using UILHost.Service.Pattern;

namespace UILHost.Infrastructure.Services
{
    public class StateService : Service<State>, IStateService
    {
        public StateService(IRepositoryAsync<State> stateRepo)
            : base(stateRepo) { }
    }
}
