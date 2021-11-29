using System;
using System.Collections.Generic;
using FastReport;
using Reporter.Extensions;

namespace Reporter
{
    public class ReportBuilder<TDataModel>
    {
        private string _reportFormFile;
        private IReadOnlyCollection<TDataModel> _data;
        private int? _startPageNumber;

        private ReportBuilder(){}
        public static ReportBuilder<TDataModel> Start() => new ReportBuilder<TDataModel>();

        public ReportBuilder<TDataModel> UseForm(string reportFormFilePath)
        {
            _reportFormFile = reportFormFilePath;
            return this;
        }

        public ReportBuilder<TDataModel> WithData(IReadOnlyCollection<TDataModel> data)
        {
            _data = data;
            return this;
        }

        public ReportBuilder<TDataModel> SetPageNumber(int? startPageNumber)
        {
            _startPageNumber = startPageNumber;
            return this;
        }

        public Report Build()
        {
            Report report = new();

            report.Load(_reportFormFile);

            report.SetDataToReportDataBind(_data, "Products", "Data1");

            if(_startPageNumber is not null) report.InitialPageNumber = _startPageNumber.Value;

            if (!report.Prepare())
            {
                throw new InvalidOperationException("Fail to prepare report, check that data is correct");
            }

            return report;
        }
    }
}