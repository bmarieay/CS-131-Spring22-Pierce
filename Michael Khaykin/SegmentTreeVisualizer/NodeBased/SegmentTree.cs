using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SegmentTreeVisualizer.NodeBased
{
    public class SegmentTree<T>
    {
        private class Node
        {
            public T Value { get; set; }
            public Node Left { get; set; }
            public Node Right { get; set; }
        }

        Node tree;
        T defaultValue;
        Func<T, T, T> combinator;
        public SegmentTree(IList<T> collection, T defaultValue, Func<T, T, T> combiner)
        {
            this.tree = new Node();
            this.defaultValue = defaultValue;
            this.combinator = combiner;

            tree = BuildTree(l: 0, r: collection.Count() - 1, collection);
        }

        private Node BuildTree(int l, int r, IList<T> collection)
        {
            if(l == r)
            {
                return new Node() { Value = collection[l] };
            }

            int mid = (l + r) / 2;
            Node cur = new Node
            {
                Left = BuildTree(l, mid, collection),
                Right = BuildTree(mid + 1, r, collection)
            };
            cur.Value = combinator(cur.Left.Value, cur.Right.Value);
            
            return cur;
        }
    }
}
