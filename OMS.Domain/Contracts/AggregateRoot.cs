namespace OMS.Domain.Contracts
{
    public abstract class AggregateRoot
    {
        public DateTime CreationDate { get; private set; }
        public bool IsDeleted { get; set; }

        public void Remove() => this.IsDeleted = true;
    }
    public abstract class AggregateRoot<Tid> : AggregateRoot where Tid : struct 
    {
        public Tid Id { get; private set; }
       
    }
}
