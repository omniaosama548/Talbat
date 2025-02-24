using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talbat.Core.Entites;

namespace Talbat.Core
{
    public interface IPaymentService
    {
        //create or update paymentintint
        Task<CustomerBasket?> CreateOrUpdatePaymentIntent(string BasketId);
    }
}
