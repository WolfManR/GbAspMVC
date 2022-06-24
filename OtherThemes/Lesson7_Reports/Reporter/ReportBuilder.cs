using FastReport;

using Reporter.Extensions;

using System;
using System.Collections;

namespace Reporter
{
    public class ReportBuilder
    {
        private string _reportFormFile;
        private ReportDataBind _dataBind;
        private int? _startPageNumber;

        private ReportBuilder() { }
        public static ReportBuilder Start() => new();

        public ReportBuilder UseForm(string reportFormFilePath)
        {
            _reportFormFile = reportFormFilePath;
            return this;
        }

        public ReportBuilder WithData(ReportDataBind dataBind)
        {
            _dataBind = dataBind;
            return this;
        }

        public ReportBuilder SetPageNumber(int? startPageNumber)
        {
            _startPageNumber = startPageNumber;
            return this;
        }

        public Report Build()
        {
            Report report = new();

            report.Load(_reportFormFile);

            report.SetDataToReportDataBind(_dataBind.PreloadedData, _dataBind.DataSourceName, _dataBind.DataModuleName);

            if (_startPageNumber is not null) report.InitialPageNumber = _startPageNumber.Value;

            if (!report.Prepare())
            {
                throw new InvalidOperationException("Fail to prepare report, check that data is correct");
            }

            return report;
        }

        public class ReportDataBind
        {
            public ReportDataBind(string dataSourceName, string dataModuleName, IEnumerable preloadedData)
            {
                DataSourceName = dataSourceName;
                DataModuleName = dataModuleName;
                PreloadedData = preloadedData;
            }

            public IEnumerable PreloadedData { get; }
            public string DataSourceName { get; }
            public string DataModuleName { get; }
        }
    }
}