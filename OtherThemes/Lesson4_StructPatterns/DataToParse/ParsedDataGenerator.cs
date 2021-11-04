using Bogus;

namespace DataToParse
{
    public class ParsedDataGenerator
    {
        public string GetChairs(ISerializer serializer, int count = 1)
        {
            return serializer.Serialize(chairFaker.Generate(count));
        }

        public string GetSofas(ISerializer serializer, int count = 1)
        {
            return serializer.Serialize(sofaFaker.Generate(count));
        }

        private Faker<Chair> chairFaker = new Faker<Chair>().Rules((f, c) =>
        {
            c.Id = f.Random.Guid();
            c.Name = f.Commerce.ProductName();
            c.Height = f.Random.Double(10, 40);
            c.Width = f.Random.Double(10, 40);
            c.Description = f.Lorem.Paragraph();
            c.Category = f.Commerce.Categories(4)[f.Random.Int(0, 3)];
            c.Price = f.Random.Decimal(30, 60);
        });

        private Faker<Sofa> sofaFaker = new Faker<Sofa>().Rules((f, s) =>
        {
            s.Id = f.Random.Guid();
            s.Name = f.Commerce.ProductName();
            s.Size = new Size()
            {
                Height = f.Random.Double(10, 40),
                Width = f.Random.Double(10, 40)
            };
            s.Description = f.Lorem.Paragraph();
            s.Category = f.Commerce.Categories(4)[f.Random.Int(0, 3)];
            s.Price = f.Random.Decimal(30, 60);
        });
    }
}