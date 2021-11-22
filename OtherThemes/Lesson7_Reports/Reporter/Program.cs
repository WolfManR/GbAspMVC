using System;
using System.Data;
using System.Globalization;
using System.IO;
using System.Security.Policy;
using CsvHelper;
using CsvHelper.Configuration;
using FastReport;
using FastReport.Export.PdfSimple;
using static System.Console;

CsvConfiguration csvConfiguration = new(CultureInfo.InvariantCulture)
{
    Delimiter = ","
};
CsvReader reader = new(File.OpenText(@"Temp\sample-csv-file-for-testing.csv"), csvConfiguration);
reader.Context.RegisterClassMap<RawProductInfoMap>();
var records = reader.EnumerateRecordsAsync(new RawProductInfo());

int stopRead = 10;
int read = 0;

await foreach (var rawData in records)
{
    if(read == stopRead) break;
    read++;
    Console.WriteLine(rawData);
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
// Segment,Country,Product,Discount Band,Units Sold,Manufacturing Price,Sale Price,Gross Sales,Discounts,Sales,COGS,Profit,Date,Month Number,Month Name,Year
record RawProductInfo
{
    public string Segment { get; set; }
    public string Country { get; set; }
    public string Product { get; set; }
    public string DiscountBand { get; set; }
    public double UnitsSold { get; set; }
    public string ManufacturingPrice { get; set; }
    public string SalePrice { get; set; }
    public string GrossSales { get; set; }
    public string Discounts { get; set; }
    public string Sales { get; set; }
    public string COGS { get; set; }
    public string Profit { get; set; }
    public string Date { get; set; }
    public string MonthNumber { get; set; }
    public string MonthName { get; set; }
    public string Year { get; set; }
}

sealed class RawProductInfoMap : ClassMap<RawProductInfo>
{
    public RawProductInfoMap()
    {
        Map(m => m.DiscountBand).Name("Discount Band");
        Map(m => m.UnitsSold).Name("Units Sold");
        Map(m => m.ManufacturingPrice).Name("Manufacturing Price");
        Map(m => m.SalePrice).Name("Sale Price");
        Map(m => m.GrossSales).Name("Gross Sales");
        Map(m => m.MonthNumber).Name("Month Number");
        Map(m => m.MonthName).Name("Month Name");
    }
}