namespace Infrastructure.Abstractions
{
    public abstract class Frame
    {
        public string Material { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
    }
}