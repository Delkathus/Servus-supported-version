using System.Collections.Generic;

namespace Servus_v2.Common
{
    public class LimitedQueue<T> : Queue<T>
    {
        public LimitedQueue(int limit)
            : base(limit)
        {
            Limit = limit;
        }

        public int Limit { get; set; }

        public new void Enqueue(T item)
        {
            while (Count >= Limit)
            {
                Dequeue();
            }
            base.Enqueue(item);
        }
    }
}