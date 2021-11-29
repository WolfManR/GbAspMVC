using System;
using Reporter;
using Reporter.DbContexts;
using Reporter.Extensions;
using Reporter.Importers;
using Reporter.Models;
using Reporter.Models.Maps;
using Reporter.Repositories;

//=======Configuration======================================

CsvImporter importer = new CsvImporter();
DatabaseConnection dbConnection = new DatabaseConnection(AppConfiguration.MongoDbConnectionString, AppConfiguration.MongoDbDataBase);
Repository repository = new Repository(dbConnection);

//=======Actual Program=====================================

var readCsv = importer.ReadImportData<RawProductInfo, RawProductInfoMap>(AppConfiguration.CsvFile);
var result = await repository.SaveImportDataToTempStorage<RawProductInfo, ProductInfo>(readCsv, AppConfiguration.ProductsCollectionName);
if (!result)
{
    Console.WriteLine("Fail to import raw data");
    return;
}

// import data to report template

var data = await repository.TakeDataForReport<ProductInfo>(0, 20, AppConfiguration.ProductsCollectionName);

var report = ReportBuilder<ProductInfo>.Start()
    .UseForm(AppConfiguration.ReportFormFile)
    .WithData(data)
    .Build();

// export report

report.ExportToPdf(AppConfiguration.OutputReport);

//==End Work=======================

#if Release
    Console.WriteLine("Work done");
    _ = Console.ReadKey();
#endif

namespace Reporter
{
    //==Extensions=====================

    static class AppConfiguration
    {
        public const string MongoDbConnectionString = "mongodb://root:pass12345@localhost:27017";
        public const string MongoDbDataBase = "SampleReports";
        public const string ProductsCollectionName = "RawData";

        const string Directory = @"Temp";
        public static readonly string CsvFile = Directory.MergePath("sample -csv-file-for-testing.csv");
        public static readonly string OutputReport = Directory.MergePath("Simple List.pdf");
        public static readonly string ReportFormFile = Directory.MergePath("ProductsList.frx");
    }
}