using Microsoft.Msagl.Drawing;
using Microsoft.Msagl.GraphViewerGdi;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SegmentTreeVisualizer
{
    public partial class Form1 : Form
    {
        Func<int, int, int> max = (x, y) => Math.Max(x, y);
        Func<int, int, int> sum = (x, y) => x + y;

        GViewer viewer = new GViewer();
        public Form1()
        {
            InitializeComponent();

           
        }

        private async Task Init()
        {
            Controls.Add(viewer);
        
            Graph graph = new Graph();

            int[] input = new int[] { 1, -2, 3, 41, 9, 6, 4, 18, 100 };

            Func<int, int, int> selectedFunction = max;
            ArrayBased.SegmentTree<int> s = new ArrayBased.SegmentTree<int>(input, int.MinValue, selectedFunction);

            await Traverse(graph, 0, input.Length - 1, input, 0, selectedFunction);

            viewer.Dock = DockStyle.Fill;
        }

        private async Task<Node> Traverse(Graph graph, int l, int r, int[] data, int id, Func<int, int, int> combine)
        {
            if (l == r)
            {
                Node n = new Node($"{id}")
                {
                    LabelText = $"{data[l]}, [{l}, {l}]",
                    UserData = data[l]
                };
                graph.AddNode(n);
                return n;
            }

            int mid = (l + r) / 2;
            Node left = await Traverse(graph, l, mid, data, id * 2 + 1, combine);
            Node right = await Traverse(graph, mid + 1, r, data, id * 2 + 2, combine);
            
            Node node = new Node($"{id}")
            {
                LabelText = $"{combine((int)left.UserData, (int)right.UserData)}, [{l}, {r}]",
                UserData = combine((int)left.UserData, (int)right.UserData)
            };
            node.AddOutEdge(new Edge(node, right, ConnectionToGraph.Connected));
            node.AddOutEdge(new Edge(node, left, ConnectionToGraph.Connected));
            graph.AddNode(node);

            viewer.Graph = graph;
        //    viewer.Invalidate();
         //   await Task.Delay(2000);

            return node;
        }

        private async void Form1_Load(object sender, EventArgs e)
        {
            await Init();
        }
    }
}
