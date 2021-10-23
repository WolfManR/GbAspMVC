namespace Infrastructure.Abstractions
{
    public interface IMachine
    {
        public Engine Engine { get; }
        public Frame Frame { get; }
    }
}