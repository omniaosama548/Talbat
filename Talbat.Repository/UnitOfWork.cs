using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talbat.Core;
using Talbat.Core.Entites;
using Talbat.Core.Repositories;
using Talbat.Repository.Data;

namespace Talbat.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly StoreContext _dbContext;
        private Hashtable _repositories;

        public UnitOfWork(StoreContext dbContext)
        {
            _dbContext = dbContext;
            _repositories=new Hashtable();
        }
        public async Task<int>CompleteAsync()
        {
            return await _dbContext.SaveChangesAsync();
        }

        public async ValueTask DisposeAsync()
        {
            await _dbContext.DisposeAsync();
        }

        public IGenericRepository<TEntity> Repository<TEntity>() where TEntity : BaseEntity
        {
           var type=typeof(TEntity).Name;//product ,order
            if (!_repositories.ContainsKey(type))
            {
                var Repository=new GenericRepository<TEntity>(_dbContext);
                _repositories.Add(type, Repository);
            }
            return _repositories[type] as IGenericRepository<TEntity>;
        }
    }
}
