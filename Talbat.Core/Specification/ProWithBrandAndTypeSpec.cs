using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talbat.Core.Entites;

namespace Talbat.Core.Specification
{
    public class ProWithBrandAndTypeSpec:BaseSpecification<Product>
    {
        public ProWithBrandAndTypeSpec(ProSpecParams Params) :base(P=>
            (string.IsNullOrEmpty(Params.Search)||P.Name.ToLower().Contains(Params.Search))&&
           (!Params.BrandId.HasValue||P.ProductBrandId==Params. BrandId)&&
            (!Params.TypeId.HasValue || P.ProductTypeId == Params.TypeId)
            )
        {
            Includes.Add(P=>P.ProductType);
            Includes.Add(P => P.ProductBrand);
            if (!string.IsNullOrEmpty(Params.Sort))
            {
                switch(Params.Sort)
                {
                    case "PriceAsc":
                        SetOrderBy(P=>P.Price);
                        break;
                    case "PriceDesc":
                        SetOrderByDesc(P => P.Price);
                        break;
                    default:
                        SetOrderBy(P=>P.Name);
                        break;
                }
            }
             
            ApplyPagination(Params.PageSize*(Params.PageIndex-1),Params.PageSize); ;
        }
        public ProWithBrandAndTypeSpec(int id):base(P=>P.Id==id)
        {
            Includes.Add(P => P.ProductType);
            Includes.Add(P => P.ProductBrand);
        }
    }
}
