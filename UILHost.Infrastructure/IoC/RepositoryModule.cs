using System.Collections.Generic;
using Repository.Pattern.DataContext;
using Repository.Pattern.Ef6;
using Repository.Pattern.Ef6.Factories;
using UILHost.Infrastructure.Data;
using UILHost.Infrastructure.Domain;
using UILHost.Repository.Pattern.Ef6;
using UILHost.Repository.Pattern.Ef6.Factories;
using UILHost.Repository.Pattern.Repositories;
using UILHost.Repository.Pattern.UnitOfWork;

namespace UILHost.Infrastructure.IoC
{
    public class RepositoryModule : ModuleBase
    {
        private readonly string _nameOrConnectionString;

        public RepositoryModule(
            string nameOrConnectionString = null)
        {
            _nameOrConnectionString = nameOrConnectionString;
        }

        public override void Load()
        {
            // UNIT OF WORK BINDINGS

            if (string.IsNullOrEmpty(_nameOrConnectionString))
                BindForTransientScope<IDataContextAsync, OperationalDataContext>(new Dictionary<string, object>()
                    {
                        { "nameOrConnectionString", Constants.DefaultConnectionStringName },
                        { "enableEfAutoDetectChanges", AppConfigFacade.EnableEfAutoDetectChanges }
                    });
            else
                BindForTransientScope<IDataContextAsync, OperationalDataContext>(new Dictionary<string, object>()
                    {
                        { "nameOrConnectionString", _nameOrConnectionString }
                    });

            BindForTransientScope<IRepositoryProvider, RepositoryProvider>();
            BindForTransientScope<IUnitOfWorkAsync, UnitOfWork>();

            // REPOSITORY BINDINGS

            BindForTransientScope<IRepositoryAsync<Teacher>, Repository<Teacher>>();
            BindForTransientScope<IRepositoryAsync<Meet>, Repository<Meet>>();
            BindForTransientScope<IRepositoryAsync<State>, Repository<State>>();

        }

    }
}