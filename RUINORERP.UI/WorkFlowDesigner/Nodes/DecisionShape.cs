using System;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Printing;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
//using System.Runtime.Serialization.Formatters.Soap;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using Netron.GraphLib.Attributes;
using Netron.GraphLib.UI;
using Netron.GraphLib.Interfaces;
using Netron.GraphLib.Entitology;
using Newtonsoft.Json;
using RUINORERP.UI.WorkFlowDesigner.Entities;
using RUINORERP.UI.WorkFlowDesigner.TypeTransfer;
using System.Drawing.Design;
using Netron.GraphLib;
namespace RUINORERP.UI.WorkFlowDesigner.Nodes
{
    /// <summary>
    /// A decision shape with four connectors.
    /// </summary>
    /// <remarks>The EllipseShape is used as a template to make the coding easier.</remarks>
    [Serializable]
    [Description("�ж�")]
    [NetronGraphShape("�ж�", "EE27AB9C-876E-4b70-9116-998D64AB00E2", "������", "RUINORERP.UI.WorkFlowDesigner.Nodes.DecisionShape",
         "�ж�,������������һ���ڵ���߲�����������")]
    public class DecisionShape : BaseNode
    {

        #region Fields

        #region the connectors
        private Connector TopNode;
        private Connector BottomNode;
        private Connector LeftNode;
        private Connector RightNode;

        #endregion
        #endregion


        #region Constructors


        /// <summary>
        /// Constructor
        /// </summary>
        public DecisionShape() : base() 
        {
            NodeType = WFNodeType.Step;
            this.NodeStepPropertyValue = new WF.BizOperation.Steps.DecisionStep();
            Text = "�ж�";
            RecalculateSize = true;

            TopNode = new Connector(this, "Top", true);
            TopNode.ConnectorLocation = ConnectorLocation.North;
            Connectors.Add(TopNode);

            BottomNode = new Connector(this, "Bottom", true);
            BottomNode.ConnectorLocation = ConnectorLocation.South;
            Connectors.Add(BottomNode);

            LeftNode = new Connector(this, "Left", true);
            LeftNode.ConnectorLocation = ConnectorLocation.West;
            Connectors.Add(LeftNode);

            RightNode = new Connector(this, "Right", true);
            RightNode.ConnectorLocation = ConnectorLocation.East;
            Connectors.Add(RightNode);
            IsResizable = true;
        }
        /// <summary>
        /// This is the default constructor of the class.
        /// </summary>
        public DecisionShape(IGraphSite site) : base(site)
        {
            //add the connectors
            TopNode = new Connector(this, "Top", true);
            TopNode.ConnectorLocation = ConnectorLocation.North;
            Connectors.Add(TopNode);

            BottomNode = new Connector(this, "Bottom", true);
            BottomNode.ConnectorLocation = ConnectorLocation.South;
            Connectors.Add(BottomNode);

            LeftNode = new Connector(this, "Left", true);
            LeftNode.ConnectorLocation = ConnectorLocation.West;
            Connectors.Add(LeftNode);

            RightNode = new Connector(this, "Right", true);
            RightNode.ConnectorLocation = ConnectorLocation.East;
            Connectors.Add(RightNode);

            IsResizable = true;
        }
        /// <summary>
        /// Deserialization constructor
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        protected DecisionShape(SerializationInfo info, StreamingContext context) : base(info, context) 
        {
            TopNode = (Connector)info.GetValue("TopNode", typeof(Connector));
            TopNode.BelongsTo = this;
            Connectors.Add(TopNode);

            BottomNode = (Connector)info.GetValue("BottomNode", typeof(Connector));
            BottomNode.BelongsTo = this;
            Connectors.Add(BottomNode);

            LeftNode = (Connector)info.GetValue("LeftNode", typeof(Connector));
            LeftNode.BelongsTo = this;
            Connectors.Add(LeftNode);

            RightNode = (Connector)info.GetValue("RightNode", typeof(Connector));
            RightNode.BelongsTo = this;
            Connectors.Add(RightNode);
        }
        #endregion

        #region Methods
        /// <summary>
        /// Paints the shape of the person object in the plex. Here you can let your imagination go.
        /// MAKE IT PERFORMANT, this is a killer method called 200.000 times a minute!
        /// </summary>
        /// <param name="g">The graphics canvas onto which to paint</param>
        public override void Paint(Graphics g)
        {

            if (RecalculateSize)
            {
                Rectangle = new RectangleF(new PointF(Rectangle.X, Rectangle.Y),
                    g.MeasureString(this.Text, Font));
                Rectangle = System.Drawing.RectangleF.Inflate(Rectangle, 10, 6);
                RecalculateSize = false; //very important!
            }
            GraphicsPath path = new GraphicsPath();
            path.AddLines(new PointF[]{
                                                    new PointF(Rectangle.X+Rectangle.Width/2, Rectangle.Top),
                                                    new PointF(Rectangle.Right, Rectangle.Top +Rectangle.Height/2),
                                                    new PointF(Rectangle.X+Rectangle.Width/2, Rectangle.Bottom),
                                                    new PointF(Rectangle.Left, Rectangle.Top +Rectangle.Height/2),
                                                    new PointF(Rectangle.X+Rectangle.Width/2, Rectangle.Top)
            });
            Region region = new Region(path);
            if (this.ShapeColor != Color.Transparent)
                g.FillRegion(this.BackgroundBrush, region);
            g.DrawPath(this.Pen, path);
            if (ShowLabel)
            {
                StringFormat sf = new StringFormat();
                sf.Alignment = StringAlignment.Center;
                g.DrawString(Text, Font, TextBrush, Rectangle.X + (Rectangle.Width / 2), Rectangle.Y + Rectangle.Height / 2 - 3, sf);
            }

        }

        /// <summary>
        /// Returns a floating-point point coordinates for a given connector
        /// </summary>
        /// <param name="c">A connector object</param>
        /// <returns>A floating-point pointF</returns>
        public override PointF ConnectionPoint(Connector c)
        {

            if (c == TopNode) return new PointF(Rectangle.Left + (Rectangle.Width * 1 / 2), Rectangle.Top);
            if (c == BottomNode) return new PointF(Rectangle.Left + (Rectangle.Width * 1 / 2), Rectangle.Bottom);
            if (c == LeftNode) return new PointF(Rectangle.Left, Rectangle.Top + (Rectangle.Height * 1 / 2));
            if (c == RightNode) return new PointF(Rectangle.Right, Rectangle.Top + (Rectangle.Height * 1 / 2));
            return new PointF(0, 0);

        }


        /// <summary>
        /// Deserialization constructor
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue("TopNode", TopNode, typeof(Connector));

            info.AddValue("BottomNode", BottomNode, typeof(Connector));

            info.AddValue("LeftNode", LeftNode, typeof(Connector));

            info.AddValue("RightNode", RightNode, typeof(Connector));
        }
        protected override Brush BackgroundBrush
        {
            get
            {
                return new LinearGradientBrush(Rectangle, Color.WhiteSmoke, this.ShapeColor, LinearGradientMode.Vertical);
            }
        }
        #endregion
    }

}








