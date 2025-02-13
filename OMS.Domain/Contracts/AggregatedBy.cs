using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OMS.Domain.Contracts
{
    public abstract class AggregatedBy<TAggregator, Tid> where TAggregator : AggregateRoot<Tid> where Tid : struct
    {
    }
}
