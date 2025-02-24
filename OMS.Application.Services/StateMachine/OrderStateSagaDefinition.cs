using GreenPipes;
using MassTransit;
using MassTransit.ConsumeConfigurators;
using MassTransit.Definition;
using OMS.Application.Services.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OMS.Application.Services.StateMachine
{
    public class OrderStateSagaDefinition : SagaDefinition<OrderStateInstance>
    {
        public OrderStateSagaDefinition()
        {
        }

        public IEndpointDefinition<OrderStateInstance> EndpointDefinition { set => throw new NotImplementedException(); }

        public Type SagaType => typeof(OrderStateMachine);

        public int? ConcurrentMessageLimit => 200;


        public void Configure(IReceiveEndpointConfigurator endpointConfigurator, ISagaConfigurator<OrderStateInstance> sagaConfigurator)
        {
            sagaConfigurator.UseMessageRetry(cfg =>
            {
                cfg.Interval(200, TimeSpan.FromMicroseconds(100));
            });
        }

        public string GetEndpointName(IEndpointNameFormatter formatter)
        {
            return "OmsEndpoint";
        }
    }
}
