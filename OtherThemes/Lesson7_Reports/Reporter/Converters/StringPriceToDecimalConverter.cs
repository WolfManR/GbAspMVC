using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;

namespace Reporter.Converters
{
    public sealed class StringPriceToDecimalConverter : DefaultTypeConverter
    {
        public override object ConvertFromString(string text, IReaderRow row, MemberMapData memberMapData)
        {
            if (string.IsNullOrEmpty(text) || text == "$-") return default(decimal);
            var value = text.Trim('\"', ' ');
            if (value.Contains('(')) value = text[(text.IndexOf('(') + 1)..(text.IndexOf(')') - 1)];
            if (decimal.TryParse(value, NumberStyles.Currency, CultureInfo.GetCultureInfo("en-US"), out var result)) return result;
            return base.ConvertFromString(text, row, memberMapData);
        }
    }
}