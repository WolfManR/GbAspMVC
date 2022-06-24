using System.Collections.Generic;
using Bogus;
using UI.DataModels;

namespace UI.Services
{
    public class ModelsGenerator
    {
        private Faker<Book> bookFaker = new Faker<Book>().Rules((f, b) =>
        {
            b.Author = f.Person.FullName;
            b.Title = f.Commerce.ProductName();
            b.Pages = f.Random.Int(1, 1480);
        });

        public IEnumerable<Book> GenerateBooks(int count = 1) => bookFaker.Generate(count);
    }
}