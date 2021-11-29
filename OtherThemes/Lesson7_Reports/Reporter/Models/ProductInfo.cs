using System;

namespace Reporter.Models
{
    public record ProductInfo : IRewritable<RawProductInfo>
    {
        public string Segment { get; set; }
        public string Country { get; set; }
        public string Product { get; set; }
        public double UnitsSold { get; set; }
        public decimal ManufacturingPrice { get; set; }
        public decimal SalePrice { get; set; }
        public DateTime Date { get; set; }

        public void Rewrite(RawProductInfo importData)
        {
            Segment = importData.Segment;
            Country = importData.Country;
            Product = importData.Product;
            UnitsSold = importData.UnitsSold;
            ManufacturingPrice = importData.ManufacturingPrice;
            SalePrice = importData.SalePrice;
            Date = importData.Date;
        }
    }
}