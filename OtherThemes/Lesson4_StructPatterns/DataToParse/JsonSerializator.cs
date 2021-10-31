using System.Collections.Generic;
using System.Text.Json;

namespace DataToParse
{
    public class JsonSerializator : ISerializer
    {
        public string Serialize<T>(IEnumerable<T> data)
        {
            return JsonSerializer.Serialize(data);
        }
    }
}