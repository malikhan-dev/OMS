using Automatonymous;
using MassTransit.Saga;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OMS.Application.Services.StateMachine
{
    public class OrderStateInstance : SagaStateMachineInstance, ISagaVersion
    {
        public string CorrelationString { get; set; }
        public Guid CorrelationId { get; set; }
        public string CurrentState { get; set; }
        public int OrderId { get; set; }
        public double TotalPrice { get; set; }
        public DateTime CreatedDate { get; set; }
        public int Version { get; set; }
    }
}
