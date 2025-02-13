namespace OMS.Domain.Contracts
{
    public abstract class AggregateRoot<Tid> where Tid : struct
    {
        public Tid Id { get; private set; }
        public DateTime CreationDate { get; private set; }
        public bool IsDeleted { get; set; }

        public void Remove() => this.IsDeleted = true;
    }
}
