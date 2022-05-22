using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SegmentTreeVisualizer.ArrayBased
{
    public class SegmentTree<T>
    {
        public T[] Data { get; }
        private IList<T> collection;

        private int count;
        T defaultValue;
        Func<T, T, T> combinator;
        public SegmentTree(IList<T> collection, T defaultValue, Func<T, T, T> combiner)
        {
            this.count = collection.Count();
            this.collection = collection;

            int x = (int)(Math.Ceiling(Math.Log(count) / Math.Log(2)));
            int size = 2 * (int)Math.Pow(2, x) - 1;

            Data = new T[size];
            this.defaultValue = defaultValue;
            this.combinator = combiner;

            BuildTree(0, l: 0, r: count - 1, collection);   
        }

        private T BuildTree(int index, int l, int r, IList<T> collection)
        {
            if (l == r)
            {
                return Data[index] = collection[l];
            }

            int mid = (l + r) / 2;
            T left = BuildTree(index * 2 + 1, l, mid, collection);
            T right = BuildTree(index * 2 + 2, mid + 1, r, collection);
            Data[index] = combinator(left, right);

            return Data[index];
        }

        public T Query(int start, int end)
           => Query(0, 0, count - 1, start, end);

        private T Query(int index, int curLeft, int curRight, int start, int end)
        {
            bool full = start <= curLeft && end >= curRight;
            bool none = (curRight < start || curLeft > end);

            if (none) return defaultValue;

            if (full) return Data[index];

            int mid = (curLeft + curRight) / 2;

            var left = Query(index * 2 + 1, curLeft, mid, start, end);
            var right = Query(index * 2 + 2, mid + 1, curRight, start, end);
            return combinator(left, right);
        }

        private void Update(int index, int curLeft, int curRight, int start, int end, T newval)
        {
            if (curRight < start || curLeft > end) return;

            Data[index] = combinator(Data[index], newval);
            if (curLeft == curRight) collection[curLeft] = newval;

            if (curLeft != curRight)
            {
                int mid = (curLeft + curRight) / 2;
                Update(index * 2 + 1, curLeft, mid, start, end, newval);
                Update(index * 2 + 2, mid + 1, curRight, start, end, newval);
            }
        }
        public void Update(int start, int end, T newval)
        {
            Update(0, 0, count - 1, start, end, newval);
        }
    }
}
