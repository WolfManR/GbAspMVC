using System;
using System.Data;
using System.IO;
using System.Security.Policy;
using FastReport;
using FastReport.Export.PdfSimple;
using static System.Console;

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