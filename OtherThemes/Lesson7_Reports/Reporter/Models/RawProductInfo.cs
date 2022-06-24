using System;

namespace Reporter.Models
{
    public record RawProductInfo
    {
        public string Segment { get; set; }
        public string Country { get; set; }
        public string Product { get; set; }
        public string DiscountBand { get; set; }
        public double UnitsSold { get; set; }

        public decimal ManufacturingPrice { get; set; }
        public decimal SalePrice { get; set; }
        public decimal GrossSales { get; set; }
        public decimal Discounts { get; set; }
        public decimal Sales { get; set; }
        public decimal COGS { get; set; }
        public decimal Profit { get; set; }

        public DateTime Date { get; set; }
        public byte MonthNumber { get; set; }
        public string MonthName { get; set; }
        public short Year { get; set; }
    }
}