using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talbat.Core.Entites;
using Talbat.Core.Specification;

namespace Talbat.Repository
{
    public static class SpecificationEvalutor<T>where T : BaseEntity
    {
        //fun to build query
        //dbContext.Products.Where(P=>P.id==id).Include(P => P.ProductBrand).Include(P => P.ProductType)
        public static IQueryable<T>GetQuery(IQueryable<T> inputQuery, ISpecification<T> Spec)
        {
            var Query = inputQuery;  //dbContext.Products
            if (Spec.Criteria != null)
            {
                Query= Query.Where(Spec.Criteria);//dbContext.Products.Where(P=>P.id==id)
            }
            if(Spec.OrderBy != null)
            {
                Query = Query.OrderBy(Spec.OrderBy);
            }
            if(Spec.OrderByDesc != null)
            {
                Query=Query.OrderByDescending(Spec.OrderByDesc);
            }
            if (Spec.IsPaginationEnabled)
            {
                Query=Query.Skip(Spec.Skip).Take(Spec.Take);
            }
            //P => P.ProductBrand ,P => P.ProductType
            Query = Spec.Includes.Aggregate(Query, (CurrentQuery, IncludeExpression) => CurrentQuery.Include(IncludeExpression));
            //dbContext.Products.Where(P=>P.id==id).Include(P => P.ProductBrand)
            //dbContext.Products.Where(P=>P.id==id).Include(P => P.ProductBrand).Include(P => P.ProductType)
            return Query;
        }
    }
}
