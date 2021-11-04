using System.Collections.Generic;

namespace DataToParse.Serializers
{
    public interface ISerializer
    {
        string Serialize<T>(List<T> data);
    }
}