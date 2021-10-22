namespace Infrastructure.Abstractions
{
    public abstract class Machine
    {
        public Engine Engine { get; set; }
        public Frame Frame { get; set; }
    }
}