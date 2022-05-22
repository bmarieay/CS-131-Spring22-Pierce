using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SegmentTreeVisualizer.NodeBased
{
    public class MaxSegmentTree : SegmentTree<int>
    {
        public MaxSegmentTree(IList<int> collection)
            : base(collection, int.MinValue, Math.Max)
        {
        }
    }
}
