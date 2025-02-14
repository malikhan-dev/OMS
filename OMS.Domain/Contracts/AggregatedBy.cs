namespace OMS.Domain.Contracts
{
    public abstract class AggregatedBy<TAggregator, Tid> where TAggregator : AggregateRoot where Tid : struct
    {
        public Tid Id { get; private set; }

    }
}
