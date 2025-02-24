using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talbat.Core.Entites;

namespace Talbat.Core.Specification
{
   public class ProWithFilterForCountAsyns:BaseSpecification<Product>
    {
        public ProWithFilterForCountAsyns(ProSpecParams Params):base(P =>
            (string.IsNullOrEmpty(Params.Search) || P.Name.ToLower().Contains(Params.Search))&&
              (!Params.BrandId.HasValue || P.ProductBrandId == Params.BrandId) &&
            (!Params.TypeId.HasValue || P.ProductTypeId == Params.TypeId)
            )
        {
            
        }
    }
}
