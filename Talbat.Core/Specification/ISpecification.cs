using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Talbat.Core.Entites;

namespace Talbat.Core.Specification
{
    public interface ISpecification<T>where T:BaseEntity
    {
        //dbContext.Products.Where(P=>P.id==id).Include(P => P.ProductBrand).Include(P => P.ProductType)
        //signature for properity Where Condition(Where(P=>P.id==id))
        public Expression<Func<T,bool>> Criteria { get; set; }
        //Signature For Properity for List Of Include[Include(P => P.ProductBrand).Include(P => P.ProductType)]
        public List<Expression<Func<T,object>>> Includes { get; set; }
        //pro for order by (p=>p.name)
        public Expression<Func<T,object>> OrderBy { get; set; }
        public Expression<Func<T, object>> OrderByDesc { get; set; }
        //take(2)
        public int Take { get; set; }
        //skip
        public int Skip { get; set; }
        public bool IsPaginationEnabled { get; set; }
    }
}
