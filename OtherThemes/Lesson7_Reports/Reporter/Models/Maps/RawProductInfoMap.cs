using CsvHelper.Configuration;
using Reporter.Converters;

namespace Reporter.Models.Maps
{
    public sealed class RawProductInfoMap : ClassMap<RawProductInfo>
    {
        public RawProductInfoMap()
        {
            // Segment,Country,Product,Discount Band,Units Sold,Manufacturing Price,Sale Price,Gross Sales,Discounts,Sales,COGS,Profit,Date,Month Number,Month Name,Year
        
            Map(m => m.Segment);
            Map(m => m.Country);
            Map(m => m.Product);

            Map(m => m.DiscountBand).Name("Discount Band");
            Map(m => m.UnitsSold).Name("Units Sold");

            Map(m => m.ManufacturingPrice).Name("Manufacturing Price").TypeConverter<StringPriceToDecimalConverter>();
            Map(m => m.SalePrice).Name("Sale Price").TypeConverter<StringPriceToDecimalConverter>();
            Map(m => m.GrossSales).Name("Gross Sales").TypeConverter<StringPriceToDecimalConverter>();
            Map(m => m.Discounts).TypeConverter<StringPriceToDecimalConverter>();
            Map(m => m.Sales).TypeConverter<StringPriceToDecimalConverter>();
            Map(m => m.COGS).TypeConverter<StringPriceToDecimalConverter>();
            Map(m => m.Profit).TypeConverter<StringPriceToDecimalConverter>();

            Map(m => m.Date);
            Map(m => m.MonthNumber).Name("Month Number");
            Map(m => m.MonthName).Name("Month Name");
            Map(m => m.Year);
        }
    }
}