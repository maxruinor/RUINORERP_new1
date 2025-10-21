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
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using Netron.GraphLib.Attributes;
using Netron.GraphLib.UI;
using Netron.GraphLib.Interfaces;
using System.Data;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Collections.Generic;
using System.Linq;
using Netron.GraphLib;
using RUINORERP.WF;
using RUINORERP.UI.WorkFlowDesigner.Entities;
using RUINORERP.UI.WorkFlowDesigner.UI;

namespace RUINORERP.UI.WorkFlowDesigner.Nodes
{
    /// <summary>
    /// 或签节点 - 只需要任意一个指定审批人通过即可继续流程
    /// </summary>
    [Serializable]
    [Description("或签")]
    [JsonObject(MemberSerialization.OptIn)]
    [NetronGraphShape("或签", "8ED1469D-90B2-43ab-B000-4FF5C682F538", "工作流", "RUINORERP.UI.WorkFlowDesigner.Nodes.OrSignNode",
         "只需要任意一个指定审批人通过即可继续流程")]
    public class OrSignNode : BaseNode
    {
        #region Fields

        #region the connectors
        private Connector TopNode;
        private Connector BottomNode;
        private Connector LeftNode;
        private Connector RightNode;
        #endregion
        
        // 或签节点特定属性
        private OrSignStep _orSignStep;
        
        #endregion

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public OrSignNode() : base()
        {
            NodeType = WFNodeType.Step;
            _orSignStep = new OrSignStep();
            NodeStepPropertyValue = _orSignStep;

            Rectangle = new RectangleF(0, 0, 70, 40);

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

            this.Text = "或签";
             
            IsResizable = true;
        }
        
        /// <summary>
        /// This is the default constructor of the class.
        /// </summary>
        public OrSignNode(IGraphSite site) : base(site)
        {
            //set the default size
            Rectangle = new RectangleF(0, 0, 70, 20);
            _orSignStep = new OrSignStep();
            NodeStepPropertyValue = _orSignStep;
            
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
        protected OrSignNode(SerializationInfo info, StreamingContext context) : base(info, context)
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
            
            _orSignStep = (OrSignStep)info.GetValue("OrSignStep", typeof(OrSignStep));
        }
        #endregion

        #region Properties   
        
        [Browsable(false)]
        public OrSignStep OrSignStep
        {
            get { return _orSignStep; }
            set { _orSignStep = value; }
        }

        public override void AddProperties()
        {
            base.AddProperties();
            
            // 添加或签节点特定属性
            Bag.Properties.Add(new PropertySpec("审批人员", typeof(List<ApprovalUser>), "或签属性", 
                "或签节点的审批人员列表", new List<ApprovalUser>(), typeof(ApprovalUserListEditor)));
            Bag.Properties.Add(new PropertySpec("已审批人数", typeof(int), "或签属性", 
                "已审批通过的人数", 0));
            Bag.Properties.Add(new PropertySpec("状态", typeof(string), "或签属性", 
                "或签节点的当前状态", "Pending"));
        }
        
        #region PropertyBag
        protected override void GetPropertyBagValue(object sender, PropertySpecEventArgs e)
        {
            base.GetPropertyBagValue(sender, e);
            
            switch (e.Property.Name)
            {
                case "审批人员":
                    e.Value = _orSignStep.Approvers;
                    break;
                case "已审批人数":
                    e.Value = _orSignStep.ApprovedCount;
                    break;
                case "状态":
                    e.Value = _orSignStep.Status;
                    break;
            }
        }

        protected override void SetPropertyBagValue(object sender, PropertySpecEventArgs e)
        {
            base.SetPropertyBagValue(sender, e);
            
            switch (e.Property.Name)
            {
                case "审批人员":
                    _orSignStep.Approvers = (List<ApprovalUser>)e.Value;
                    break;
                case "已审批人数":
                    _orSignStep.ApprovedCount = (int)e.Value;
                    break;
                case "状态":
                    _orSignStep.Status = e.Value.ToString();
                    break;
            }
        }
        #endregion
        
        #endregion

        #region Methods
        /// <summary>
        /// Overrides the default bitmap used in the shape viewer
        /// </summary>
        /// <returns></returns>
        public override Bitmap GetThumbnail()
        {
            Bitmap bmp = null;
            try
            {
                Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Netron.GraphLib.BasicShapes.Resources.BasicNode.gif");

                bmp = Bitmap.FromStream(stream) as Bitmap;
                stream.Close();
                stream = null;
            }
            catch (Exception exc)
            {
                Trace.WriteLine(exc.Message, "BasicNode.GetThumbnail");
            }
            return bmp;
        }

        /// <summary>
        /// Paints the shape of the person object in the plex. Here you can let your imagination go.
        /// MAKE IT PERFORMANT, this is a killer method called 200.000 times a minute!
        /// </summary>
        /// <param name="g">The graphics canvas onto which to paint</param>
        public override void Paint(Graphics g)
        {
            base.Paint(g);
            if (RecalculateSize)
            {
                Rectangle = new RectangleF(new PointF(Rectangle.X, Rectangle.Y),
                    g.MeasureString(this.Text, Font));
                Rectangle = System.Drawing.RectangleF.Inflate(Rectangle, 10, 10);
                RecalculateSize = false; //very important!
            }
            if (ShapeColor != Color.Transparent)
            {
                GraphicsPath path = new GraphicsPath();
                path.AddLine(Rectangle.X, Rectangle.Y, Rectangle.Right - 10, Rectangle.Y);
                path.AddArc(Rectangle.X + Rectangle.Width - 20, Rectangle.Y, 20, 20, -90, 90);
                path.AddLine(Rectangle.Right, Rectangle.Y + 10, Rectangle.Right, Rectangle.Bottom);
                path.AddLine(Rectangle.Right, Rectangle.Bottom, Rectangle.Left + 10, Rectangle.Bottom);
                path.AddArc(Rectangle.X, Rectangle.Y + Rectangle.Height - 20, 20, 20, 90, 90);
                path.AddLine(Rectangle.X, Rectangle.Y + Rectangle.Height - 10, Rectangle.X, Rectangle.Y);
                //shadow
                Region darkRegion = new Region(path);
                darkRegion.Translate(5, 5);

                g.FillRegion(new SolidBrush(Color.FromArgb(20, Color.Black)), darkRegion);

                // 或签节点使用特殊颜色以区分
                g.FillPath(new SolidBrush(Color.Orange), path);
            }
            if (ShowLabel)
            {
                StringFormat sf = new StringFormat();
                sf.Alignment = StringAlignment.Center;
                g.DrawString(Text, Font, TextBrush, Rectangle.X + (Rectangle.Width / 2), Rectangle.Y + 3, sf);
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
            
            info.AddValue("OrSignStep", _orSignStep, typeof(OrSignStep));
        }

        #endregion
    }
}