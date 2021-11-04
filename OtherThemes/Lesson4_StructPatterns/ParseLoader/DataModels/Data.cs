namespace ParseLoader.DataModels
{
    public record Data
    {
        public string Name { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }
        public decimal Price { get; set; }
    }
}