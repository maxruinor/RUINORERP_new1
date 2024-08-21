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
using static Netron.GraphLib.Entitology.TaskEvent;
using System.Data;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Collections.Generic;
using System.Linq;
using Netron.GraphLib;
using MathNet.Numerics.Distributions;
using System.Drawing.Design;
using RUINORERP.UI.WorkFlowDesigner.TypeTransfer;
using RUINORERP.UI.WorkFlowDesigner.Entities;
using SqlSugar;
namespace RUINORERP.UI.WorkFlowDesigner.Nodes
{
    /// <summary>
    /// A simple rectangular shape with four connectors.
    /// </summary>
    [Serializable]
    [Description("条件")]
    [JsonObject(MemberSerialization.OptIn)]
    [NetronGraphShape("条件", "8ED1469D-90B2-43ab-B000-4FF5C682F535", "工作流", "RUINORERP.UI.WorkFlowDesigner.Nodes.WFConditionStep",
         "流程步骤的条件图形")]
    public class WFConditionStep : BaseNode
    {
        #region Fields

        #region the connectors
        private Connector TopNode;
        private Connector BottomNode;
        private Connector LeftNode;
        private Connector RightNode;
        #endregion
        #endregion

        #region Constructor


        /// <summary>
        /// Constructor
        /// </summary>
        public WFConditionStep() : base()
        {
            NodeType = WFNodeType.Step;


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

            // 创建一个画笔对象
            Pen pen = new Pen(Color.Black, 2);

            // 创建一个画刷对象
            Brush brush = new SolidBrush(Color.White);
            /*
            // 绘制开始节点的形状
            g.FillEllipse(brush, startPoint.X - 20, startPoint.Y - 20, 40, 40);

            // 绘制开始节点的文本
            Font font = new Font("Arial", 12);
            StringFormat format = new StringFormat();
            format.Alignment = StringAlignment.Center;
            format.LineAlignment = StringAlignment.Center;
            g.DrawString("开始", font, brush, startPoint, format);

            // 绘制开始节点的连接线
            Point endPoint = new Point(startPoint.X + 40, startPoint.Y);
            g.DrawLine(pen, startPoint, endPoint);

          */

            this.Text = "暂时不使用的条件";

            IsResizable = true;
        }
        /// <summary>
        /// This is the default constructor of the class.
        /// </summary>
        public WFConditionStep(IGraphSite site) : base(site)
        {
            //set the default size
            Rectangle = new RectangleF(0, 0, 70, 20);
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
        protected WFConditionStep(SerializationInfo info, StreamingContext context) : base(info, context)
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

        #region Properties	

        //private string mNodeId;


        //[JsonIgnore]
        //public string NodeId
        //{
        //    get { return mNodeId; }
        //    set { mNodeId = value; }
        //}



        //private List<User> _users = new List<User>();
        //[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        //public List<User> Users
        //{
        //    get { return _users; }
        //    set { _users = value; }
        //}

        //private List<WFCondition> _Conditions;

        //[JsonProperty("Conditions", NullValueHandling = NullValueHandling.Ignore)]
        //[Editor(typeof(ListConditionEditor), typeof(UITypeEditor)), LocalizableAttribute(true)]//属性编辑器接口
        //[TypeConverter(typeof(ListConditionTypeConverter))]           //值转化器
        //[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]   //序列化内容保存设置的
        //public List<WFCondition> Conditions
        //{
        //    get { return _Conditions; }
        //    set { _Conditions = value; }
        //}

        //private WFCondition _Condition;

        //[JsonProperty("Condition", NullValueHandling = NullValueHandling.Ignore)]
        //[Editor(typeof(ConditionEditor), typeof(UITypeEditor)), LocalizableAttribute(true)]//属性编辑器接口
        //[TypeConverter(typeof(ConditionTypeConverter<WFCondition>))]           //值转化器
        //[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]   //序列化内容保存设置的
        //public WFCondition Condition
        //{
        //    get { return _Condition; }
        //    set { _Condition = value; }
        //}

        //public string _NextStepId;

        //[JsonProperty("NextStepId", NullValueHandling = NullValueHandling.Ignore)]
        //public string NextStepId
        //{
        //    get { return _NextStepId; }
        //    set { _NextStepId = value; }
        //}

        //public string _StepType;

        //[JsonProperty("StepType", NullValueHandling = NullValueHandling.Include)]
        //public string StepType
        //{
        //    get { return _StepType; }
        //    set { _StepType = value; }
        //}

        //public string _ConditionType;

        //[Description("条件类型")]
        //[JsonProperty("ConditionType", NullValueHandling = NullValueHandling.Include)]
        //public string ConditionType
        //{
        //    get { return _ConditionType; }
        //    set { _ConditionType = value; }
        //}


        public override void AddProperties()
        {
            base.AddProperties();
            /*
            Bag.Properties.Add(new PropertySpec("Id", typeof(string)));
            Bag.Properties.Add(new PropertySpec("NextStepId", typeof(string)));
            Bag.Properties.Add(new PropertySpec("StepType", typeof(string)));
            // Bag.Properties.Add(new PropertySpec("Conditions", typeof(string)));
            //条件设置后面这个参数为默认值
            PropertySpec specConditions = new PropertySpec("Condition", typeof(string), "特殊设置", "条件设置.", null, typeof(ConditionEditor), typeof(UITypeEditor));

            Bag.Properties.Add(specConditions);
            Bag.Properties.Add(new PropertySpec("Users", typeof(List<User>)));

            PropertySpec specConditionType = new PropertySpec("ConditionType", typeof(string), "特殊设置", "设置条件类型.", null, typeof(ReflectedEnumStyleEditor), typeof(System.ComponentModel.TypeConverter));
            ArrayList list = new ArrayList();
            string[] names = Enum.GetNames(typeof(ConditionType));
            for (int k = 0; k < names.Length; k++)
                list.Add(names[k]);
            specConditionType.Attributes = new Attribute[] { new ReflectedEnumAttribute(list) };

            Bag.Properties.Add(specConditionType);
            */
            #region 子类型是一个反射的枚举类型，因此此构造.
            /*
            PropertySpec specSubType = new PropertySpec("ConditionType", typeof(string), "Appearance", "Gets or sets the sub-type of the shape.", "Task", typeof(ReflectedEnumStyleEditor), typeof(System.ComponentModel.TypeConverter));
            ArrayList list = new ArrayList();
            string[] names = Enum.GetNames(typeof(SubTypes));
            for (int k = 0; k < names.Length; k++)
                list.Add(names[k]);
            specSubType.Attributes = new Attribute[] { new ReflectedEnumAttribute(list) };
            Bag.Properties.Add(specSubType);
            */
            //bag.Properties.Add(new PropertySpec("Type", typeof(string),"Test","The shape's sub-type","Task",typeof(ReflectedEnumStyleEditor),typeof(TypeConverter))  );
            #endregion
        }
        #region PropertyBag
        protected override void GetPropertyBagValue(object sender, PropertySpecEventArgs e)
        {
            base.GetPropertyBagValue(sender, e);
            /*
            switch (e.Property.Name)
            {
                case "Id":

                    e.Value = mId;
                    break;
                case "NextStepId":
                    e.Value = _NextStepId;
                    break;
                case "Users":
                    e.Value = _users;
                    break;
                case "Condition":
                    e.Value = _Condition;
                    break;
                case "ConditionType":
                    e.Value = _Conditions;
                    break;
                case "Conditions":
                    e.Value = _Conditions;
                    break;

            }*/
        }

        protected override void SetPropertyBagValue(object sender, PropertySpecEventArgs e)
        {
            base.SetPropertyBagValue(sender, e);
            /*
            switch (e.Property.Name)
            {

                case "Id":

                    //use the logic and the constraint of the object that is being reflected
                    if (!(e.Value.ToString() == null))
                    {
                        mId = System.Convert.ToString(e.Value);
                    }
                    else
                    {
                        //MessageBox.Show("Not a valid label", "Invalid label", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                        this.Invalidate();
                    }
                    break;

                case "NextStepId":

                    //use the logic and the constraint of the object that is being reflected
                    if (!(e.Value.ToString() == null))
                    {
                        _NextStepId = System.Convert.ToString(e.Value);
                    }
                    else
                    {
                        //MessageBox.Show("Not a valid label", "Invalid label", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                        this.Invalidate();
                    }
                    break;
                case "StepType":

                    //use the logic and the constraint of the object that is being reflected
                    if (!(e.Value.ToString() == null))
                    {
                        _StepType = System.Convert.ToString(e.Value);
                    }
                    else
                    {
                        //MessageBox.Show("Not a valid label", "Invalid label", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                        this.Invalidate();
                    }
                    break;
                case "Condition":

                    //use the logic and the constraint of the object that is being reflected
                    if (!(e.Value.ToString() == null) && e.Value is WFCondition)
                    {
                        _Condition = e.Value as WFCondition;
                    }
                    else
                    {
                        //MessageBox.Show("Not a valid label", "Invalid label", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                        this.Invalidate();
                    }
                    break;
                case "Conditions":

                    //use the logic and the constraint of the object that is being reflected
                    if (!(e.Value.ToString() == null) && e.Value is WFCondition)
                    {
                        _Conditions = e.Value as List<WFCondition>;
                    }
                    else
                    {
                        //MessageBox.Show("Not a valid label", "Invalid label", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                        this.Invalidate();
                    }
                    break;
                case "Users":

                    //use the logic and the constraint of the object that is being reflected
                    if (!(e.Value.ToString() == null))
                    {
                        // _users = e.Value.ToList();
                    }
                    else
                    {
                        //MessageBox.Show("Not a valid label", "Invalid label", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                        this.Invalidate();
                    }
                    break;
                case "ConditionType":

                    //use the logic and the constraint of the object that is being reflected
                    if (!(e.Value.ToString() == null))
                    {
                        // _users = e.Value.ToList();
                    }
                    else
                    {
                        //MessageBox.Show("Not a valid label", "Invalid label", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                        this.Invalidate();
                    }
                    break;
            }*/
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

                g.FillPath(new SolidBrush(ShapeColor), path);
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
        }





        #endregion
    }

}
