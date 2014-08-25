﻿using System.Threading;
using System.Threading.Tasks;
using Repository.Pattern.Infrastructure;
using UILHost.Repository.Pattern.Repositories;

namespace UILHost.Repository.Pattern.UnitOfWork
{
    public interface IUnitOfWorkAsync : IUnitOfWork
    {
        Task<int> SaveChangesAsync();
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
        IRepositoryAsync<TEntity> RepositoryAsync<TEntity>() where TEntity : class, IObjectState;
    }
}