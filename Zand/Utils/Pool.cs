using System.Collections.Generic;

namespace Zand.Utils
{
    public class Pool<T> where T: IPoolable, new()
    {
        private Queue<T> _items;
        public int Capacity { get; }

        public Pool(int capacity)
        {
            Capacity = capacity;
            _items = new Queue<T>();

            for (int i = 0; i < Capacity; i++)
            {
                _items.Enqueue(new T());
            }
        }

        public T ObtainItem()
        {
            return _items.Dequeue();
        }

        public void Release(T item)
        {
            _items.Enqueue(item);

            if (item is IPoolable)
            {
                item.Reset();
            }
        }

    }

    public interface IPoolable
    {
        public void Reset() { }
    }
}
