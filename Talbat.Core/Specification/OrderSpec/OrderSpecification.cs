using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talbat.Core.Entites.Order_Agg;

namespace Talbat.Core.Specification.OrderSpec
{
    public class OrderSpecification:BaseSpecification<Order>
    {
        public OrderSpecification(string email):base(O=>O.BuyerEmail==email)
        {
            Includes.Add(O => O.DeliveryMethod);
            Includes.Add(O=>O.Items);
            SetOrderByDesc(O => O.OrderDate);
        }
        public OrderSpecification(string email,int orderId):base(O=>O.BuyerEmail==email && O.Id==orderId)
        {
            Includes.Add(O => O.DeliveryMethod);
            Includes.Add(O => O.Items);
        }
    }
}
