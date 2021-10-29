﻿using Bogus;

using System.Text.Json;

namespace DataToParse
{
    public class DataGenerator
    {
        public string GetChairs(int count = 1)
        {
            return JsonSerializer.Serialize(chairFaker.Generate(count));
        }

        public string GetSofas(int count = 1)
        {
            return JsonSerializer.Serialize(sofaFaker.Generate(count));
        }

        Faker<Chair> chairFaker = new Faker<Chair>().Rules((f, c) =>
        {
            c.Id = f.Random.Guid();
            c.Name = f.Commerce.ProductName();
            c.Height = f.Random.Double(10, 40);
            c.Width = f.Random.Double(10, 40);
            c.Description = f.Lorem.Paragraph();
            c.Category = f.Commerce.Categories(4)[f.Random.Int(3)];
            c.Price = f.Random.Decimal(30, 60);
        });

        Faker<Sofa> sofaFaker = new Faker<Sofa>().Rules((f, s) =>
        {
            s.Id = f.Random.Guid();
            s.Name = f.Commerce.ProductName();
            s.Height = f.Random.Double(10, 40);
            s.Width = f.Random.Double(10, 40);
            s.Description = f.Lorem.Paragraph();
            s.Category = f.Commerce.Categories(4)[f.Random.Int(3)];
            s.Price = f.Random.Decimal(30, 60);
        });
    }
}