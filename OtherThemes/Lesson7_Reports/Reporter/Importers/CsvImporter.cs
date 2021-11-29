using System.Collections.Generic;
using System.Globalization;
using System.IO;
using CsvHelper;
using CsvHelper.Configuration;

namespace Reporter.Importers
{
    public class CsvImporter
    {
        private readonly CsvConfiguration _csvConfiguration;

        public CsvImporter()
        {
            _csvConfiguration = new(CultureInfo.InvariantCulture)
            {
                DetectDelimiter = true,
            };
        }

        public IAsyncEnumerable<TImportModel> ReadImportData<TImportModel, TImportModelMap>(string filePath)
            where TImportModelMap : ClassMap
            where TImportModel : new()
        {
            using var stream = File.OpenText(filePath);
            using CsvReader reader = new(stream, _csvConfiguration);

            reader.Context.RegisterClassMap<TImportModelMap>();

            return reader.EnumerateRecordsAsync(new TImportModel());
        }
    }
}