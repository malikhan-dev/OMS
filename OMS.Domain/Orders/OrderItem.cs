using OMS.Domain.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OMS.Domain.Orders
{
    public class OrderItem : AggregatedBy<Order,int>
    {
        public int ProductId { get;  set; }
        public int Count { get;  set; }
        public double UnitPrice { get;  set; }
    }
}
