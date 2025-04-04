using Netron.GraphLib;
using Netron.GraphLib.Attributes;
using Netron.GraphLib.Configuration;
using Netron.GraphLib.Interfaces;
using System;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using Newtonsoft.Json;

namespace RUINORERP.UI.WorkFlowDesigner.UI
{
    /// <summary>
    /// A multi-line connection painter, this code is part of an online tutorial: see the Netron site for more information
    /// http://netron.sf.net
    /// </summary>
    [Netron.GraphLib.Attributes.NetronGraphConnection("Workflow", "B57446B1-0D6B-473d-8EFC-7597822D9469", "RUINORERP.UI.WorkFlowDesigner.UI.WorkflowConnection")]
    public class WorkflowConnection : Connection
    {

        private string _DataType;

        [JsonProperty("DataType", NullValueHandling = NullValueHandling.Ignore)]
        public string DataType
        {
            get { return _DataType; }
            set { _DataType = value; }
        }

        #region Fields

        private bool mBoxedLabel;
        /// <summary>
        /// Holds the polyline data
        /// </summary>
        private ArrayList mInsertionPoints;
        /// <summary>
        /// default line width
        /// </summary>
        private float mLineWidth = 1F;
        /// <summary>
        /// default rest length of the connection
        /// </summary>
        private int mRestLength = 100;
        /// <summary>
        /// This is a public floating point assigned by the canvascontrol in the MouseMove and 
        /// MouseDown events. It makes it possible mTo show a drawn line before there is an actual link between
        /// two connectors.
        /// </summary>
        private PointF mToPoint;
        /// <summary>
        /// The starting connector
        /// </summary>
        private Connector mFrom;
        /// <summary>
        /// the destination connector
        /// </summary>
        private Connector mTo;
        /// <summary>
        /// The line color
        /// </summary>
        private Color mLineColor = Color.Black;
        /// <summary>
        /// The line style (Solid, Dashed...)
        /// </summary>
        private DashStyle mLineStyle = DashStyle.Solid;
        /// <summary>
        /// The line weight; thin, medium or fat. Could be set mTo arbitrary size.
        /// </summary>
        private ConnectionWeight mLineWeight = ConnectionWeight.Thin;
        /// <summary>
        /// The type of arrow or line end
        /// </summary>
        private ConnectionEnd mLineEnd = ConnectionEnd.NoEnds;
        /// <summary>
        /// the shape of the connection
        /// </summary>
        private string mLinePath = "Workflow";
        /// <summary>
        /// the painter class used to paint the connection
        /// </summary>
        private WorkflowConnectionPainter mPainter;
        /// <summary>
        /// Tracker used for the connection
        /// </summary>
        private Tracker mTracker;

        /// <summary>
        /// the z-order of the connection
        /// </summary>
        private int mZOrder;
        #endregion

        #region Properties
 
        /// <summary>
        /// Gets the connection painter
        /// </summary>
        protected new internal WorkflowConnectionPainter ConnectionPainter
        {
            get { return mPainter; }
        }

        /// <summary>
        /// Gets or sets the additional set of points along the connection
        /// </summary>
        internal ArrayList InsertionPoints
        {
            get { return mInsertionPoints; }
            set { mInsertionPoints = value; }
        }

        /// <summary>
        /// Gets or sets the font to be used for drawing text
        /// 
        /// </summary>
        /// <remarks>Redefines the Font property of the Entity class as public <see cref="Netron.GraphLib.Entity"/></remarks>
        public new Font Font
        {
            get { return base.Font; }
            set { base.Font = value; }
        }

        /// <summary>
        /// Gets or sets whether the label is shown as a tooltip
        /// </summary>
        public bool BoxedLabel
        {
            get { return mBoxedLabel; }
            set { mBoxedLabel = value; }
        }

        /// <summary>
        /// Puts the connection in a layer
        /// </summary>
        /// <param name="layer"></param>
        protected override void SetLayer(GraphLayer layer)
        {
            base.SetLayer(layer);
            if (layer == GraphAbstract.DefaultLayer)
            {
                LineColor = Color.FromArgb(255, mLineColor);
            }
            else
            {
                if (Layer.UseColor)
                    LineColor = Color.FromArgb((int)(Layer.Opacity * 255f / 100), Layer.LayerColor);
                else
                    LineColor = Color.FromArgb((int)(Layer.Opacity * 255f / 100), mLineColor);
            }
        }

        /// <summary>
        /// Gets the ConnectionPainter object for this connection
        /// </summary>
        protected internal ConnectionPainter Painter
        {
            get { return mPainter; }
        }

        /// <summary>
        /// Gets or sets whether the connection is selected
        /// </summary>
        public override bool IsSelected
        {
            get { return base.IsSelected; }
            set
            {
                base.IsSelected = value;
                mTracker = new ConnectionTracker(mInsertionPoints, true);

                Invalidate();
            }
        }

        /// <summary>
        /// Gets or set the tracker associated with the connection
        /// </summary>
        public override Tracker Tracker
        {
            get { return mTracker; }
            set { mTracker = value; }
        }

        /// <summary>
        /// Gets or sets the type of connection or shape of the path.
        /// </summary>
        [GraphMLData]
        public string LinePath
        {
            get { return mLinePath; }
            [Browsable(false)]
            set
            {
                mLinePath = value;
                switch (value)
                {
                    case "Workflow":
                        // mPainter = new DefaultPainter(this);
                        mPainter = new WorkflowConnectionPainter(this);
                        mTracker = new ConnectionTracker(mInsertionPoints, true);
                        break;

                    default:
                        mTracker = new ConnectionTracker(mInsertionPoints, true);
                        //we have a custom connection
                        ConnectionSummary consum = this.Site.Libraries.GetConnectionSummary(value);
                        if (consum == null)
                        {
                            Trace.WriteLine("Couldn't find the custom connection called '" + value + "'", "Connection.LinePath");
                        }
                        else
                        {
                            try
                            {

                                //TODO: add some .Net security checks here to make sure no malicious code will harm your computer
                                Directory.SetCurrentDirectory(Path.GetDirectoryName(Application.ExecutablePath));//reset the dir since the Dialogs can change the CurrentDirectory!
                                Assembly ass = Assembly.LoadFrom(consum.LibPath);

                                //Directory.SetCurrentDirectory(Path.GetDirectoryName(Application.ExecutablePath));

                                //handle = ass.CreateInstance(consum.ReflectionName,true,BindingFlags.Public | BindingFlags.Instance | BindingFlags.CreateInstance , null, new object[]{this}, null, new object[]{}) as ObjectHandle;
                                mPainter = ass.CreateInstance(consum.ReflectionName, true, BindingFlags.Public | BindingFlags.Instance | BindingFlags.CreateInstance, null, new object[] { this }, null, new object[] { }) as WorkflowConnectionPainter;
                                if (mPainter == null)
                                {
                                    mPainter = new WorkflowConnectionPainter(this);
                                    Trace.WriteLine("Couldn't instantiate the painter called '" + value + "'", "Connection.LinePath");
                                }
                                //Trace.WriteLine(o.ToString());
                                //handle = Activator.CreateInstance(consum.LibPath,consum.ReflectionName,ins);
                                //mPainter = handle.Unwrap() as ConnectionPainter;
                            }
                            catch (Exception exc)
                            {
                                //TODO: catch a more specific exception here
                                Trace.WriteLine(exc.Message, "Connection.LinePath");
                            }
                            //shape.Site = this;											   
                            //mTracker = new ConnectionTracker(mInsertionPoints,true);

                        }


                        break;
                }

                Invalidate();
            }
        }
        /// <summary>
        /// Gets or sets the rest length of the connection (used by the layour algorithms)
        /// </summary>
        public int RestLength
        {
            get { return mRestLength; }
            [Browsable(false)]
            set { mRestLength = value; }
        }
        /// <summary>
        /// Returns the length of the connection
        /// </summary>
        public double Length
        {
            get
            {
                PointF s = From.BelongsTo.ConnectionPoint(From); //start point
                PointF e = (To != null) ? To.BelongsTo.ConnectionPoint(To) : mToPoint; //endpoint
                return Math.Sqrt((s.X - e.X) * (s.X - e.X) + (s.Y - e.Y) * (s.Y - e.Y));
            }
        }
        /// <summary>
        /// Gets the rectangle corresponding mTo or embedding the connection
        /// </summary>
        public SizeF ConnectionSize
        {

            get
            {
                SizeF ret = new SizeF(0, 0);
                try
                {
                    PointF s = From.BelongsTo.ConnectionPoint(From); //start point
                    PointF e = (To != null) ? To.BelongsTo.ConnectionPoint(To) : mToPoint; //endpoint
                    RectangleF sr = new RectangleF(s, new SizeF(0, 0));
                    RectangleF er = new RectangleF(e, new SizeF(0, 0));
                    RectangleF rec = RectangleF.Union(sr, er);
                    ret = rec.Size;
                }
                catch (Exception exc)
                {
                    //TODO: catch a more specific excpetion here
                    Trace.WriteLine(exc.Message, "Connection.ConnectionSize");
                }
                catch
                {
                    Trace.WriteLine("Non-CLS exception caught.", "Connection.ConnectionSize");
                }
                return ret;
            }
        }


        /// <summary>
        /// Gets or sets the line style
        /// </summary>
        public DashStyle LineStyle
        {
            get { return mLineStyle; }
            [Browsable(false)]
            set
            {
                mLineStyle = value;
                Pen.Color = mLineColor;
                Pen.Width = mLineWidth;
                Pen.DashStyle = mLineStyle;
                this.Invalidate();
            }
        }
        /// <summary>
        /// Gets or sets the line end
        /// </summary>
        public ConnectionEnd LineEnd
        {
            get { return mLineEnd; }
            set { mLineEnd = value; }
        }
        /// <summary>
        /// Gets or sets the temporary To point when drawing and connecting mTo a To connector.
        /// Holds normally the mouse coordinate.
        /// </summary>
        public PointF ToPoint
        {
            get { return mToPoint; }
            set { mToPoint = value; }
        }


        /// <summary>
        /// Gets or sets the line color
        /// </summary>
        [GraphMLData]
        public Color LineColor
        {
            get { return mLineColor; }
            set
            {
                mLineColor = value;
                Pen.Color = value;
                mPainter.Pen = Pen;
                this.Invalidate();

            }
        }

        /// <summary>
        /// Gets or sets the line weight
        /// </summary>
        [GraphMLData]
        public ConnectionWeight LineWeight
        {
            get { return mLineWeight; }
            set
            {
                mLineWeight = value;
                switch (mLineWeight)
                {
                    case ConnectionWeight.Thin:
                        mLineWidth = 1F; break;
                    case ConnectionWeight.Medium:
                        mLineWidth = 2F; break;
                    case ConnectionWeight.Fat:
                        mLineWidth = 3F; break;
                }
                Pen.Width = mLineWidth;
                this.Invalidate();

            }
        }

        /// <summary>
        /// Gets or sets where the connection originates
        /// </summary>
        public Connector From
        {
            get { return mFrom; }
            set { mFrom = value; }
        }
        /// <summary>
        /// Gets or sets where the connection ends
        /// </summary>
        public Connector To
        {
            get { return mTo; }
            set
            {
                mTo = value;

            }
        }

        #endregion

        #region Constructors
        /// <summary>
        /// Default constructor, assigns null connectors and so a null connection
        /// </summary>
        public WorkflowConnection()
        {
            InitConnection();
        }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="site"></param>
        public WorkflowConnection(IGraphSite site) : base(site)
        {
            InitConnection();
        }

        /// <summary>
        /// Deserialization constructor
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        protected WorkflowConnection(SerializationInfo info, StreamingContext context) : base(info, context)
        {

            this.InitConnection();
            //attaching the connection to shape connector occurs in the BinarySerializer class;
            //instantiate the connectors to keep the UID references until then

            this.mTo = new Connector(info.GetString("mTo.UID"));

            this.mFrom = new Connector(info.GetString("mFrom.UID"));

            this.mLineColor = (Color)info.GetValue("mLineColor", typeof(Color));

            this.mLineEnd = (ConnectionEnd)info.GetValue("mLineEnd", typeof(ConnectionEnd));

            this.mLineWidth = info.GetInt32("mLineWidth");

            mLinePath = info.GetString("mLinePath");

             

            mInsertionPoints = (ArrayList)info.GetValue("mInsertionPoints", typeof(ArrayList));

            this.Tag = info.GetString("mLayer"); //layer is set in the post-deserialization

            try
            {
                this.mZOrder = info.GetInt32("mZOrder");
            }
            catch
            {
                this.mZOrder = 0;
            }

        }
        /// <summary>
        /// Additional actions after deserialization
        /// </summary>
        public override void PostDeserialization()
        {
            base.PostDeserialization();
            /* not the easiest thing to do, but anyway; here's the (de)serialization of Bezier connections.
             *This is different than the other connections because the this connection depends on additional objects for its shape and drawing:
             * handles and tangent to manipulate the curvature. 
             */
            
                LinePath = mLinePath;//weird, but that's how to combine the ISerializable mechanism with custom conections

            if (Tag != null && typeof(string).IsInstanceOfType(Tag))
            {
                SetLayer((string)Tag);
                Tag = null; //be nice to the host/user
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Adds an intermediate connection point to the connection
        /// </summary>
        /// <param name="point"></param>
        public void AddConnectionPoint(PointF point)
        {
            this.mInsertionPoints.Add(point);
            //the test is necessary if the developer doesn't implement a separate painter but paints in this class (which is bad bad)
            if (mPainter != null) mPainter.AddConnectionPoint(point);
        }

        /// <summary>
        /// Removes a connection point from the connection
        /// </summary>
        /// <param name="point"></param>
        public void RemoveConnectionPoint(PointF point)
        {
            RectangleF r = new RectangleF(point.X, point.Y, 0, 0);
            RectangleF s;

            for (int m = 0; m < this.mInsertionPoints.Count; m++)
            {
                s = new RectangleF((PointF)mInsertionPoints[m], new SizeF(50, 50));
                s.Offset(-25, -25);//center around the point
                if (s.Contains(r))
                {
                    mInsertionPoints.RemoveAt(m);
                    mPainter.RemoveConnectionPoint(point);
                    return;
                }
            }
        }

        /// <summary>
        /// Common constructors initialization
        /// </summary>
        public void InitConnection()
        {
            //From = null;
            //To = null;
            this.ShowLabel = false;
            Pen = BlackPen;
            mPainter = new WorkflowConnectionPainter(this);
            mTracker = new ConnectionTracker(mInsertionPoints, true);
            mInsertionPoints = new ArrayList();
        }



        /// <summary>
        /// Returns wether or not the given rectangle is contained in the object
        /// </summary>
        /// <param name="r"></param>
        /// <returns></returns>
        public override bool Hit(RectangleF r)
        {
            if ((From == null) || (To == null)) return false;

            PointF p1 = From.AdjacentPoint;
            PointF p2 = To.AdjacentPoint;




            if ((r.Width == 0) && (r.Height == 0))
            {
                PointF p = r.Location;

                if (mPainter.Selected)
                {
                    return mTracker.Hit(p) != Point.Empty || mPainter.Hit(p);
                }
                return mPainter.Hit(p);
            }

            return (r.Contains(p1) && r.Contains(p2));
        }

        /// <summary>
        /// Returns the points of the connection
        /// </summary>
        /// <returns>An array of PointF structs</returns>
        public PointF[] GetConnectionPoints()
        {
            if (From == null) return null;
            PointF[] points = new PointF[4 + mInsertionPoints.Count];

            PointF s = From.Location;
            PointF e = (To != null) ? To.Location : mToPoint;

            points[0] = s; //the From connector

            //if there an adjacent point, use it
            if (mFrom.ConnectorLocation == ConnectorLocation.Unknown)
                points[1] = s;
            else
                points[1] = mFrom.AdjacentPoint;

            //loop over the intermediate points
            for (int k = 0; k < mInsertionPoints.Count; k++)
            {
                points[2 + k] = (PointF)mInsertionPoints[k];
            }

            //use the adjacent point of the To connector
            if (mTo == null || mTo.ConnectorLocation == ConnectorLocation.Unknown)
                points[4 + mInsertionPoints.Count - 2] = e;
            else
                points[4 + mInsertionPoints.Count - 2] = mTo.AdjacentPoint;
            //the To connector
            points[4 + mInsertionPoints.Count - 1] = e;

            return points;
        }
  

        /// <summary>
        /// Paints the label
        /// </summary>
        /// <param name="g"></param>
        protected new void PaintLabel(Graphics g)
        {
            try
            {
                RectangleF r = RectangleF.Union(this.mFrom.ConnectionGrip(), this.mTo.ConnectionGrip());
                Size s = g.MeasureString(this.Text, Font).ToSize();
                RectangleF a = RectangleF.Empty;
                switch (this.mLinePath)
                {
                    case "Workflow":
                        a = new RectangleF(r.X + r.Width / 2, r.Y + r.Height / 2 + 6, s.Width, s.Height + 1);
                        break;
                    case "Default":
                        a = new RectangleF(r.X + r.Width / 2, r.Y + r.Height / 2 + 6, s.Width, s.Height + 1);
                        break;
                    case "Rectangular":
                        switch (this.mFrom.ConnectorLocation)
                        {
                            case ConnectorLocation.South:
                            case ConnectorLocation.North:
                                a = new RectangleF(mFrom.BelongsTo.ConnectionPoint(mFrom).X + 5, mTo.BelongsTo.ConnectionPoint(mTo).Y + 5, s.Width, s.Height + 1);
                                break;
                            case ConnectorLocation.East:
                            case ConnectorLocation.West:
                                a = new RectangleF(mTo.BelongsTo.ConnectionPoint(mTo).X + 5, mFrom.BelongsTo.ConnectionPoint(mFrom).Y + 5, s.Width, s.Height + 1);
                                break;

                        }
                        break;

                }
                RectangleF b = a;
                a.Inflate(+3, +2);
                if (mBoxedLabel)
                {
                    g.FillRectangle(new SolidBrush(Color.FromArgb(255, 255, 231)), a);
                    g.DrawRectangle(new Pen(Color.Black, 1), Rectangle.Round(a));
                }
                g.DrawString(this.Text, Font, new SolidBrush(Color.Black), b.Location);
            }
            catch (Exception exc)
            {
                //TODO: catch a more specific excpetion here
                Trace.WriteLine(exc.Message, "Connection.PaintLabel");
            }
            catch
            {
                Trace.WriteLine("Non-CLS exception caught.", "Connection.PantLabel");
            }
        }
   
        /// <summary>
        /// Draws a line between the To and From connectors
        /// </summary>
        /// <param name="g">The graphics</param>
        protected void PaintPolyLine(Graphics g)
        {


            //style
            if (IsSelected)
                Pen.DashStyle = DashStyle.Dash;
            else
            {
                if (mLineStyle != DashStyle.Custom)
                    Pen.DashStyle = mLineStyle;
                else
                    Pen.DashStyle = DashStyle.Solid;
            }
            if (From == null) return;
            PointF s = From.BelongsTo.ConnectionPoint(From);
            PointF e = (To != null) ? To.BelongsTo.ConnectionPoint(To) : mToPoint;

            //this is for the Omni connector, a central connector
            double len = Math.Sqrt((e.X - s.X) * (e.X - s.X) + (e.Y - s.Y) * (e.Y - s.Y));
            if (len > 0 && To != null && this.To.ConnectorLocation == ConnectorLocation.Omni)
            {
                e.X -= Convert.ToSingle((e.X - s.X) * To.ConnectionShift / len);
                e.Y -= Convert.ToSingle((e.Y - s.Y) * To.ConnectionShift / len);
            }
            if (len > 0 && From != null && this.From.ConnectorLocation == ConnectorLocation.Omni)
            {
                s.X -= Convert.ToSingle((e.X - s.X) * 10 / len);
                s.Y -= Convert.ToSingle((e.Y - s.Y) * 10 / len);
            }
            //now, for the normal connectors
            if ((s.X != e.X) || (s.Y != e.Y))
            {

                mPainter.IsHovered = IsHovered;
                switch (this.mLinePath)
                {
                    case "Workflow":
                        {
                            mPainter.Points = this.GetConnectionPoints(); //update points to reflect the user actions/motions
                            break;
                        }
                    case "Default":
                        {
                            mPainter.Points = this.GetConnectionPoints(); //update points to reflect the user actions/motions
                            break;
                        }
                    
                    case "Rectangular":
                        {
                            mPainter.Points = GetConnectionPoints(); //update points to reflect the user actions/motions						
                            break;
                        }
                    default:
                        mPainter.Points = GetConnectionPoints();
                        break;
                }

                mPainter.Paint(g);
                if (IsHovered || IsSelected) this.mTracker.Paint(g);
            }


            PointF left = PointF.Empty, right = PointF.Empty;



            #region the end arrow
            if (LineEnd == ConnectionEnd.BothFilledArrow ||
                LineEnd == ConnectionEnd.BothOpenArrow ||
                LineEnd == ConnectionEnd.RightFilledArrow ||
                LineEnd == ConnectionEnd.RightOpenArrow)
            {
                switch (this.mTo.ConnectorLocation)
                {
                    case ConnectorLocation.North:
                        left = new PointF(e.X + 4, e.Y - 7);
                        right = new PointF(e.X - 4, e.Y - 7); break;
                    case ConnectorLocation.South:
                        left = new PointF(e.X - 4, e.Y + 7);
                        right = new PointF(e.X + 4, e.Y + 7); break;
                    case ConnectorLocation.West:
                        left = new PointF(e.X - 7, e.Y - 4);
                        right = new PointF(e.X - 7, e.Y + 4); break;
                    case ConnectorLocation.East:
                        left = new PointF(e.X + 7, e.Y + 4);
                        right = new PointF(e.X + 7, e.Y - 4); break;
                    case ConnectorLocation.Unknown:
                        return;
                }
                if (LineEnd == ConnectionEnd.RightFilledArrow || LineEnd == ConnectionEnd.BothFilledArrow)
                    PaintArrow(g, e, left, right, true);
                else
                    PaintArrow(g, e, left, right, false);

                //the omni arrow is a bit more difficult
                if (this.mTo.ConnectorLocation == ConnectorLocation.Omni)
                    if (LineEnd == ConnectionEnd.RightFilledArrow || LineEnd == ConnectionEnd.BothFilledArrow)
                        PaintArrow(g, s, e, mLineColor, true, false);
                    else
                        PaintArrow(g, s, e, mLineColor, false, false);

            }
            #endregion

            #region the start or From arrow
            if (LineEnd == ConnectionEnd.BothFilledArrow ||
                LineEnd == ConnectionEnd.BothOpenArrow ||
                LineEnd == ConnectionEnd.LeftFilledArrow ||
                LineEnd == ConnectionEnd.LeftOpenArrow)
            {

                switch (this.mFrom.ConnectorLocation)
                {
                    case ConnectorLocation.North:
                        left = new PointF(s.X + 4, s.Y - 7);
                        right = new PointF(s.X - 4, s.Y - 7); break;
                    case ConnectorLocation.South:
                        left = new PointF(s.X - 4, s.Y + 7);
                        right = new PointF(s.X + 4, s.Y + 7); break;
                    case ConnectorLocation.West:
                        left = new PointF(s.X - 7, s.Y - 4);
                        right = new PointF(s.X - 7, s.Y + 4); break;
                    case ConnectorLocation.East:
                        left = new PointF(s.X + 7, s.Y + 4);
                        right = new PointF(s.X + 7, s.Y - 4); break;
                    case ConnectorLocation.Unknown:
                        return;

                }

                if (LineEnd == ConnectionEnd.LeftFilledArrow || LineEnd == ConnectionEnd.BothFilledArrow)
                    PaintArrow(g, s, left, right, true);
                else
                    PaintArrow(g, s, left, right, false);
                //the omni arrow is a bit more difficult
                if (this.mTo.ConnectorLocation == ConnectorLocation.Omni)
                    if (LineEnd == ConnectionEnd.LeftFilledArrow || LineEnd == ConnectionEnd.BothFilledArrow)
                        PaintArrow(g, e, s, mLineColor, true, false);
                    else
                        PaintArrow(g, e, s, mLineColor, false, false);
            }
            #endregion
        }

   

  

        #endregion

 

        #region PropertyGrid related
        /// <summary>
        /// Adds property grid accessible properties mTo the connection
        /// </summary>
        public override void AddProperties()
        {
            base.AddProperties();
            Bag.Properties.Add(new PropertySpec("LineColor", typeof(Color), "Appearance", "Gets or sets the backcolor of the label."));
            Bag.Properties.Add(new PropertySpec("LineWeight", typeof(ConnectionWeight), "Appearance", "Gets or sets the line weight."));
            Bag.Properties.Add(new PropertySpec("LineStyle", typeof(DashStyle), "Appearance", "Gets or sets the line style."));
            Bag.Properties.Add(new PropertySpec("LineEnd", typeof(ConnectionEnd), "Appearance", "Gets or sets the line end type."));
            Bag.Properties.Add(new PropertySpec("BoxedLabel", typeof(bool), "Appearance", "Gets or sets whether the label is shown as a tooltip."));
            //the z-order
            Bag.Properties.Add(new PropertySpec("Z-order", typeof(int), "Layout", "The z-order of the shape", 0));

            Bag.Properties.Add(new PropertySpec("DataType", typeof(string)));//�ҵ��޸�


            #region Variable linepath collection
            PropertySpec specLinePath = new PropertySpec("LinePath", typeof(string), "Appearance", "Gets or sets the line shape.", "Workflow", typeof(ConnectionStyleEditor), typeof(System.ComponentModel.TypeConverter));
            ArrayList list = new ArrayList();
            for (int k = 0; k < this.Site.Libraries.Count; k++)
            {
                for (int m = 0; m < this.Site.Libraries[k].ConnectionSummaries.Count; m++)
                    list.Add(Site.Libraries[k].ConnectionSummaries[m].Name);
            }

            specLinePath.Attributes = new Attribute[] { new ConnectionStyleAttribute(list) };
            Bag.Properties.Add(specLinePath);
            #endregion

            #region Layers
            //the layer			
            PropertySpec specLayer = new PropertySpec("Layer", typeof(string), "Appearance", "Gets or sets the line shape.", "Default", typeof(LayerUITypeEditor), typeof(System.ComponentModel.TypeConverter));
            specLayer.Attributes = Site.GetLayerAttributes();
            Bag.Properties.Add(specLayer);
            #endregion

        }
        /// <summary>
        /// Gets the value of the requested property
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void GetPropertyBagValue(object sender, PropertySpecEventArgs e)
        {
            base.GetPropertyBagValue(sender, e);
            switch (e.Property.Name)
            {
                case "Text": e.Value = this.Text; break;
                case "LineColor": e.Value = mLineColor; break;

                case "LineWeight": e.Value = this.mLineWeight; break;
                case "LineStyle": e.Value = this.mLineStyle; break;
                case "LineEnd": e.Value = this.mLineEnd; break;
                case "LinePath": e.Value = this.mLinePath; break;
                case "Layer":
                    if (Layer == null)
                        e.Value = GraphAbstract.DefaultLayer;
                    else
                        e.Value = this.Layer;
                    break;
                case "BoxedLabel":
                    e.Value = this.mBoxedLabel;
                    break;
                case "Z-order":
                    e.Value = this.ZOrder;
                    break;

                case "DataType":
                    e.Value = this.DataType;
                    break;
            }
        }

        /// <summary>
        /// Sets the value of the given property
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void SetPropertyBagValue(object sender, PropertySpecEventArgs e)
        {
            base.SetPropertyBagValue(sender, e);
            switch (e.Property.Name)
            {
                case "LineColor": this.LineColor = (Color)e.Value; break;

                case "LineWeight": this.LineWeight = (ConnectionWeight)e.Value; break;
                case "LineStyle": this.LineStyle = (DashStyle)e.Value; break;
                case "LineEnd": this.LineEnd = (ConnectionEnd)e.Value; break;
                case "LinePath": this.LinePath = (string)e.Value; break;
                case "Layer":
                    this.SetLayer(e.Value as GraphLayer);
                    this.Invalidate();
                    break;
                case "BoxedLabel":
                    this.mBoxedLabel = (bool)e.Value;
                    break;
                case "Z-order":
                    this.ZOrder = (int)e.Value;

                    break;
                case "DataType":
                    this.DataType = (string)e.Value;
                    break;
            }
        }

        #endregion

        #region ISerializable Members

        /// <summary>
        /// ISerializable implementation
        /// </summary>
        /// <param name="info">the serialization info</param>
        /// <param name="context">the streaming context</param>
        [SecurityPermissionAttribute(SecurityAction.Demand, SerializationFormatter = true)]
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);

            info.AddValue("mLinewidth", this.mLineWidth);

            info.AddValue("mFrom.UID", this.mFrom.UID.ToString());

            info.AddValue("mTo.UID", this.mTo.UID.ToString());

            info.AddValue("mLineColor", this.mLineColor, typeof(Color));

            info.AddValue("mLineEnd", this.mLineEnd, typeof(ConnectionEnd));

            info.AddValue("mLineWidth", this.mLineWidth);

            info.AddValue("mLinePath", this.LinePath);

            info.AddValue("mInsertionPoints", mInsertionPoints, typeof(ArrayList));

            info.AddValue("mZOrder", this.mZOrder);

            if (this.mLinePath == "Bezier")
            {
                info.AddValue("mPainter", this.mPainter, typeof(BezierPainter));
            }

 

      
        }

        #endregion
    }


}


