// Copyright 2011 Chris Patterson
// 
// Licensed under the Apache License, Version 2.0 (the "License"); you may not use 
// this file except in compliance with the License. You may obtain a copy of the 
// License at 
// 
//     http://www.apache.org/licenses/LICENSE-2.0 
// 
// Unless required by applicable law or agreed to in writing, software distributed 
// under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR 
// CONDITIONS OF ANY KIND, either express or implied. See the License for the 
// specific language governing permissions and limitations under the License.
namespace OdoyuleRules.Visualizer
{
    using System;
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.Linq;
    using Graphing;
    using Microsoft.Glee.Drawing;
    using Microsoft.Glee.GraphViewerGdi;
    using Models.RuntimeModel;
    using QuickGraph;
    using QuickGraph.Glee;
    using Util.Caching;
    using Color = Microsoft.Glee.Drawing.Color;

    public class RulesEngineGraphGenerator
    {
        static readonly Cache<Type, Color> _bg;
        static readonly Cache<Type, Color> _fg;
        static readonly Cache<Type, Shape> _shape;

        static RulesEngineGraphGenerator()
        {
            Color defaultBg = Color.Black;
            _bg = new DictionaryCache<Type, Color>(key => defaultBg)
                {
                    {typeof (AlphaNode<>), Color.Yellow},
                    {typeof (PropertyNode<,>), Color.Red},
                    {typeof (EqualNode<,>), Color.Blue},
                    {typeof (ValueNode<,>), Color.Blue},
                    {typeof (CompareNode<,>), Color.Blue},
                    {typeof (NotNullNode<,>), Color.Blue},
                    {typeof (JoinNode<>), Color.Green},
                    {typeof (LeftJoinNode<,>), Color.LightGreen},
                    {typeof (ConditionNode<>), Color.Blue},
                    {typeof (DelegateProductionNode<>), Color.LightGray},
                    {typeof (ConstantNode<>), Color.Magenta},
                };

            Color defaultFg = Color.White;
            _fg = new DictionaryCache<Type, Color>(key => defaultFg)
                {
                    {typeof (AlphaNode<>), Color.Black},
                    {typeof (PropertyNode<,>), Color.White},
                    {typeof (EqualNode<,>), Color.White},
                    {typeof (ValueNode<,>), Color.White},
                    {typeof (CompareNode<,>), Color.White},
                    {typeof (NotNullNode<,>), Color.White},
                    {typeof (JoinNode<>), Color.White},
                    {typeof (LeftJoinNode<,>), Color.Black},
                    {typeof (ConditionNode<>), Color.White},
                    {typeof (DelegateProductionNode<>), Color.Black},
                    {typeof (ConstantNode<>), Color.Black},
                };

            Shape defaultShape = Shape.Ellipse;
            _shape = new DictionaryCache<Type, Shape>(key => defaultShape)
                {
                    {typeof (RulesEngine), Shape.Ellipse},
                    {typeof (AlphaNode<>), Shape.Ellipse},
                    {typeof (ConstantNode<>), Shape.Circle},
                    {typeof (JoinNode<>), Shape.Ellipse},
                    {typeof (LeftJoinNode<,>), Shape.Ellipse},
                    {typeof (DelegateProductionNode<>), Shape.DoubleCircle},
                };
        }

        public Graph CreateGraph(RulesEngineGraph data)
        {
            var graph = new AdjacencyGraph<Vertex, Edge<Vertex>>();

            graph.AddVertexRange(data.Vertices);
            graph.AddEdgeRange(data.Edges.Select(x => new Edge<Vertex>(x.From, x.To)));

            GleeGraphPopulator<Vertex, Edge<Vertex>> glee = graph.CreateGleePopulator();

            glee.NodeAdded += NodeStyler;
            glee.EdgeAdded += EdgeStyler;
            glee.Compute();

            Graph gleeGraph = glee.GleeGraph;

            return gleeGraph;
        }

        public void SaveGraphToFile(RulesEngineGraph data, int width, int height, string filename)
        {
            Graph gleeGraph = CreateGraph(data);

            var renderer = new GraphRenderer(gleeGraph);
            renderer.CalculateLayout();

            var bitmap = new Bitmap(width, height, PixelFormat.Format32bppArgb);
            renderer.Render(bitmap);

            bitmap.Save(filename, ImageFormat.Png);
        }

        void NodeStyler(object sender, GleeVertexEventArgs<Vertex> args)
        {
            Color color = _bg[args.Vertex.VertexType];

            args.Node.Attr.Fillcolor = color;

            args.Node.Attr.Shape = _shape[args.Vertex.VertexType];
            args.Node.Attr.Fontcolor = _fg[args.Vertex.VertexType];
            args.Node.Attr.Fontsize = 8;
            args.Node.Attr.FontName = "Arial";
            args.Node.Attr.Label = args.Vertex.Title;
            args.Node.Attr.Padding = 1.1;
        }

        static void EdgeStyler(object sender, GleeEdgeEventArgs<Vertex, Edge<Vertex>> e)
        {
            //e.GEdge.EdgeAttr.Label = e.Edge.Source.TargetType.Name;
            //e.GEdge.EdgeAttr.FontName = "Tahoma";
            //e.GEdge.EdgeAttr.Fontsize = 6;
        }
    }
}