using System.Collections.Generic;
using System.Collections.Immutable;

namespace ListWrapperTest
{
    public class ListWrapper<T>
    {
        private readonly List<T> _list = new List<T>();
        
        private readonly object _editLock = new();
        private readonly object _getLock = new();

        public void Add(T item)
        {
            lock (_editLock)
            {
                lock (_getLock)
                {
                    _list.Add(item);
                }
            }
        }

        public void Remove(T item)
        {
            lock (_editLock)
            {
                lock (_getLock)
                {
                    if(_list.Count <= 0) return;
                    _list.Remove(item);
                }
            }
        }

        public IReadOnlyCollection<T> GetData()
        {
            lock (_getLock)
            {
                return _list.ToImmutableArray();
            }
        }

        public T GetLast()
        {
            lock (_getLock)
            {
                return _list[^1];
            }
        }
    }
}