using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talbat.Core.Entites;
using Talbat.Core.Specification;

namespace Talbat.Core.Repositories
{
    public interface IGenericRepository<T>where T : BaseEntity
    {
        #region Without Specification
        Task<IReadOnlyList<T>> GetAllAsync();
        Task<T> GetByIdAsync(int id);
        #endregion
        Task<IReadOnlyList<T>> GetAllWithSpecAsync(ISpecification<T> Spec);
        Task<T> GetEntityWithSpecAsync(ISpecification<T> Spec);
        Task<int> GetCountWithSpecAsync(ISpecification<T> Spec);

        Task AddAsync(T item);
        void Delete(T item);
        void Update(T item);
    }
}
