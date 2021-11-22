using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Policy;
using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;
using FastReport;
using FastReport.Export.PdfSimple;
using MongoDB.Driver;
using static System.Console;

CsvConfiguration csvConfiguration = new(CultureInfo.InvariantCulture)
{
    DetectDelimiter = true,
};
using var Base = File.OpenText(@"Temp\sample-csv-file-for-testing.csv");
using CsvReader reader = new(Base, csvConfiguration);
reader.Context.RegisterClassMap<RawProductInfoMap>();
var readProcess = reader.EnumerateRecordsAsync(new RawProductInfo());

//var writeConfig = new CsvConfiguration(CultureInfo.InvariantCulture)
//{
//    Delimiter = ";"
//};
//using var Next = File.CreateText("Temp\\sampleResult.csv");
//await using CsvWriter writer = new(Next, writeConfig);
//await writer.WriteRecordsAsync(records);

MongoClient client = new("mongodb://root:pass12345@localhost:27017");
var database = client.GetDatabase("SampleReports");

var collectionOfAllData = database.GetCollection<RawProductInfo>("RawData");

const byte bufferSize = 20;
var buffer = Enumerable.Range(0, bufferSize).Select(_ => new RawProductInfo()).ToArray();


await foreach (var rawRecord in readProcess)
{
    for (var i = 0; i < bufferSize; i++)
    {
        buffer[i].Copy(rawRecord);
    }
    await collectionOfAllData.InsertManyAsync(buffer);
}


//var directory = @"D:\demo\test";

//Report report = new();
//report.Load(directory.MergePath("Simple List.frx"));

//DataSet data = new();
//data.ReadXml(directory.MergePath("nwind.xml"));

//report.RegisterData(data, "NorthWind");

//report.Prepare();

//PDFSimpleExport pdfExport = new();

//pdfExport.Export(report, directory.MergePath("Simple List.pdf"));

//==End Work=======================

WriteLine("Work done");
_ = ReadKey();

//==Extensions=====================

static class Extensions
{
    public static string MergePath(this string self, string endPath) => Path.Combine(self, endPath);
    public static string MergePath(this string self, string middlePath, string endPath) => Path.Combine(self, middlePath, endPath);
}

sealed class StringPriceToDecimalConverter : DefaultTypeConverter
{
    public override object ConvertFromString(string text, IReaderRow row, MemberMapData memberMapData)
    {
        if (string.IsNullOrEmpty(text) || text == "$-") return default(decimal);
        var value = text.Trim('\"', ' ');
        if (value.Contains('(')) value = text[(text.IndexOf('(')+1)..(text.IndexOf(')')-1)];
        if (decimal.TryParse(value, NumberStyles.Currency, CultureInfo.GetCultureInfo("en-US"), out var result)) return result;
        return base.ConvertFromString(text, row, memberMapData);
    }
}

record ProductInfo
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
    public decimal Profit { get; set; }
    public DateTime Date { get; set; }
}

// Segment,Country,Product,Discount Band,Units Sold,Manufacturing Price,Sale Price,Gross Sales,Discounts,Sales,COGS,Profit,Date,Month Number,Month Name,Year
record RawProductInfo
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

    public void Copy(RawProductInfo original)
    {
        Segment = original.Segment;
        Country = original.Country;
        Product = original.Product;
        DiscountBand = original.DiscountBand;
        UnitsSold = original.UnitsSold;
        ManufacturingPrice = original.ManufacturingPrice;
        SalePrice = original.SalePrice;
        GrossSales = original.GrossSales;
        Discounts = original.Discounts;
        Sales = original.Sales;
        COGS = original.COGS;
        Profit = original.Profit;
        Date = original.Date;
        MonthNumber = original.MonthNumber;
        MonthName = original.MonthName;
        Year = original.Year;
    }

}

sealed class RawProductInfoMap : ClassMap<RawProductInfo>
{
    public RawProductInfoMap()
    {
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