using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talbat.Core.Entites;
using Talbat.Core.Repositories;
using Talbat.Core.Specification;
using Talbat.Repository.Data;

namespace Talbat.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        private readonly StoreContext _dbContext;

        public GenericRepository(StoreContext dbContext)
        {
            _dbContext = dbContext;
        }
        #region Without Specification
        public async Task<IReadOnlyList<T>> GetAllAsync()
        {

            if (typeof(T) == typeof(Product))
            {
                return (IReadOnlyList<T>)await _dbContext.Products.Include(P => P.ProductBrand).Include(P => P.ProductType).ToListAsync();
            }
            else
            {
                return await _dbContext.Set<T>().ToListAsync();
            }
        }
        public async Task<T> GetByIdAsync(int id)
        {
            return await _dbContext.Set<T>().FindAsync(id);
        }


        #endregion
        private IQueryable<T> ApplySpec(ISpecification<T> Spec)
        {
            return SpecificationEvalutor<T>.GetQuery(_dbContext.Set<T>(), Spec);
        }
        public async Task<IReadOnlyList<T>> GetAllWithSpecAsync(ISpecification<T> Spec)
        {
            return await ApplySpec(Spec).ToListAsync();
        }
        public async Task<T> GetEntityWithSpecAsync(ISpecification<T> Spec)
        {
            return await ApplySpec(Spec).FirstOrDefaultAsync();
        }

        public async Task<int> GetCountWithSpecAsync(ISpecification<T> Spec)
        {
           return await ApplySpec(Spec).CountAsync();
        }

        public async Task AddAsync(T item)
        {
            await _dbContext.Set<T>().AddAsync(item);
        }

        public void Delete(T item)
        {
            _dbContext.Set<T>().Remove(item);
        }

        public void Update(T item)
        {
            _dbContext.Set<T>().Update(item);
        }
    }
}
