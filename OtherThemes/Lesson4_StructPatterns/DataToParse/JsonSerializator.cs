using System.Collections.Generic;
using System.Text.Json;

namespace DataToParse
{
    public class JsonSerializator : ISerializer
    {
        public string Serialize<T>(List<T> data)
        {
            return JsonSerializer.Serialize(data);
        }
    }
}