using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OMS.Domain.Orders.Repositories
{
    public interface IOrderQueryRepository
    {
        void GetById(int id);
    }
}
