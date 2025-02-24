using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Talbat.Core.Entites;

namespace Talbat.Core.Specification
{
    public class BaseSpecification<T> : ISpecification<T> where T : BaseEntity
    {
        public Expression<Func<T, bool>> Criteria { get ; set ; }
        public List<Expression<Func<T, object>>> Includes { get; set; } = new List<Expression<Func<T, object>>>();
        public Expression<Func<T, object>> OrderBy { get ; set; }
        public Expression<Func<T, object>> OrderByDesc { get ; set ; }
        public int Take { get ; set; }
        public int Skip { get; set; }
        public bool IsPaginationEnabled { get; set; }

        //get all
        public BaseSpecification()
        {
            
        }
        //get by id
        public BaseSpecification(Expression<Func<T, bool>> CriteriaExpression)
        {
            Criteria = CriteriaExpression;
            
        }
        public void SetOrderBy(Expression<Func<T,object>>OrderByExp)
        {
            OrderBy = OrderByExp;
        }
        public void SetOrderByDesc(Expression<Func<T, object>> OrderByExpDesc)
        {
            OrderByDesc = OrderByExpDesc;
        }
        public void ApplyPagination(int skip, int take)
        {
            IsPaginationEnabled = true;
            Skip = skip;
            Take = take;
        }
    }
}
