using System.Collections.Generic;

namespace ParseLoader
{
    class DataStorage
    {
        private List<Data> _storage = new();

        public void AddRange(IEnumerable<Data> data)
        {
            _storage.AddRange(data);
        }

        public IReadOnlyCollection<Data> GetData()
        {
            return _storage;
        }
    }
}