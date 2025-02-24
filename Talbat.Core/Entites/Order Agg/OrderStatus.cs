using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Talbat.Core.Entites.Order_Agg
{
    public enum OrderStatus
    {
        [EnumMember(Value ="Pending")]
        Pending,
        [EnumMember(Value = "Payment Recieved")]
        PaymentRecieved,
        [EnumMember(Value ="Payment Recieved")]
        PaymentFailed
    }
}
