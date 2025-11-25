using System;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Runtime.Serialization;
using System.Reflection;
using System.Windows.Forms;
using Netron.GraphLib.Attributes;
using Netron.GraphLib.Interfaces;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using Netron.GraphLib;
using RUINORERP.WF;
using RUINORERP.Model;

namespace RUINORERP.UI.WorkFlowDesigner.Nodes
{
    /// <summary>
    /// 流程导航节点
    /// </summary>
    [Serializable]
    [Description("流程导航")]
    [JsonObject(MemberSerialization.OptIn)]
    [NetronGraphShape("流程导航", "8ED1469D-90B2-43ab-B000-4FF5C682F540", "工作流", "RUINORERP.UI.WorkFlowDesigner.Nodes.ProcessNavigationNode",
         "流程导航的节点图形")]
    public class ProcessNavigationNode : BaseNode
    {
        #region Fields

        private string mNodeId;
        private string mProcessName;
        private string mDescription;
        private string mMenuID;
        private string mFormName;
        private string mClassPath;
        private int mBusinessType = 0;
        private Color mNodeColor = Color.FromArgb(79, 129, 189);
        private Font mTextFont = new Font("Microsoft YaHei", 10, FontStyle.Regular);
        private string mCustomText = "";
        private bool mShowCustomText = false;
        private float mFontSize = 10;
        private FontStyle mFontStyle = FontStyle.Regular;
        private float mOpacity = 1.0f;
        private Image mBackgroundImage = null;
        private string mBackgroundImagePath = "";
        private bool mTextWrap = true;
        private ContentAlignment mTextAlignment = ContentAlignment.MiddleCenter;
        
        #region 特定属性
        private string mProcessId = string.Empty;
        private string mNavigationUrl = string.Empty;
        #endregion

        #region the connectors
        private Connector TopNode;
        private Connector BottomNode;
        private Connector LeftNode;
        private Connector RightNode;
        #endregion

        #endregion

        #region Properties

        [JsonProperty]
        [Description("节点ID")]
        public string NodeId
        {
            get { return mNodeId; }
            set
            { 
                if (mNodeId != value)
                {
                    mNodeId = value;
                    Invalidate();
                }
            }
        }

        [JsonProperty]
        [Description("流程名称")]
        public string ProcessName
        {
            get { return mProcessName; }
            set
            {
                if (mProcessName != value)
                {
                    mProcessName = value;
                    this.Text = value;
                    Invalidate();
                }
            }
        }

        [JsonProperty]
        [Description("流程描述")]
        public string Description
        {
            get { return mDescription; }
            set
            {
                if (mDescription != value)
                {
                    mDescription = value;
                    Invalidate();
                }
            }
        }

        [JsonProperty]
        [Description("关联菜单ID")]
        public string MenuID
        {
            get { return mMenuID; }
            set
            {
                if (mMenuID != value)
                {
                    mMenuID = value;
                    Invalidate();
                }
            }
        }

        [JsonProperty]
        [Description("关联窗体名称")]
        public string FormName
        {
            get { return mFormName; }
            set
            {
                if (mFormName != value)
                {
                    mFormName = value;
                    Invalidate();
                }
            }
        }

        [JsonProperty]
        [Description("关联类路径")]
        public string ClassPath
        {
            get { return mClassPath; }
            set
            {
                if (mClassPath != value)
                {
                    mClassPath = value;
                    Invalidate();
                }
            }
        }

        [JsonProperty]
        [Description("业务类型")]
        public ProcessNavigationNodeBusinessType BusinessType
        {
            get { return (ProcessNavigationNodeBusinessType)mBusinessType; }
            set
            {
                if (mBusinessType != (int)value)
                {
                    mBusinessType = (int)value;
                    Invalidate();
                }
            }
        }

        [JsonProperty]
        [Description("模块ID")]
        public long? ModuleID { get; set; }

        [JsonProperty]
        [Description("节点颜色")]
        public Color NodeColor
        {
            get { return mNodeColor; }
            set
            {
                if (mNodeColor != value)
                {
                    mNodeColor = value;
                    Invalidate();
                }
            }
        }

        [JsonProperty]
        [Description("填充颜色")]
        public Color FillColor
        {
            get { return mNodeColor; }
            set
            {
                if (mNodeColor != value)
                {
                    mNodeColor = value;
                    Invalidate();
                }
            }
        }

        private Color _borderColor = Color.FromArgb(255, 255, 255);
        [JsonProperty]
        [Description("边框颜色")]
        public Color BorderColor
        {
            get { return _borderColor; }
            set
            { 
                _borderColor = value;
                Invalidate();
            }
        }

        private Color _fontColor = Color.White;
        [JsonProperty]
        [Description("字体颜色")]
        public Color FontColor
        {
            get { return _fontColor; }
            set
            { 
                _fontColor = value;
                Invalidate();
            }
        }

        [Description("文本字体")]
        public Font TextFont
        {
            get { return mTextFont; }
            set
            {
                mTextFont = value;
                if (value != null)
                {
                    mFontSize = value.Size;
                    mFontStyle = value.Style;
                }
                Invalidate();
            }
        }

        [JsonProperty]
        [Description("自定义文本内容")]
        public string CustomText
        {
            get { return mCustomText; }
            set
            { 
                mCustomText = value;
                Invalidate();
            }
        }

        [JsonProperty]
        [Description("是否显示自定义文本")]
        public bool ShowCustomText
        {
            get { return mShowCustomText; }
            set
            { 
                mShowCustomText = value;
                Invalidate();
            }
        }

        [JsonProperty]
        [Description("字体大小")]
        public float FontSize
        {
            get { return mFontSize; }
            set
            { 
                mFontSize = value;
                UpdateFont();
                Invalidate();
            }
        }

        [JsonProperty]
        [Description("字体样式")]
        public FontStyle TextStyle
        {
            get { return mFontStyle; }
            set
            { 
                mFontStyle = value;
                UpdateFont();
                Invalidate();
            }
        }

        [JsonProperty]
        [Description("节点透明度")]
        public float Opacity
        {
            get { return mOpacity; }
            set
            { 
                mOpacity = Math.Max(0.1f, Math.Min(1.0f, value)); // 限制在0.1到1.0之间
                Invalidate();
            }
        }

        [JsonProperty]
        [Description("背景图像路径")]
        public string BackgroundImagePath
        {
            get { return mBackgroundImagePath; }
            set
            { 
                mBackgroundImagePath = value;
                // 尝试加载背景图像
                if (!string.IsNullOrEmpty(value) && File.Exists(value))
                {
                    try
                    {
                        mBackgroundImage = Image.FromFile(value);
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine("加载背景图像失败: " + ex.Message);
                        mBackgroundImage = null;
                    }
                }
                else
                {
                    mBackgroundImage = null;
                }
                Invalidate();
            }
        }

        [Description("背景图像")]
        public Image BackgroundImage
        {
            get { return mBackgroundImage; }
            set
            {
                mBackgroundImage = value;
                // 如果设置了图像但没有路径，则重置路径
                if (value != null && string.IsNullOrEmpty(mBackgroundImagePath))
                {
                    mBackgroundImagePath = "";
                }
                Invalidate();
            }
        }

        [JsonProperty]
        [Description("文字是否自动换行")]
        public bool TextWrap
        {
            get { return mTextWrap; }
            set
            { 
                mTextWrap = value;
                Invalidate();
            }
        }

        [JsonProperty]
        [Description("文字对齐方式")]
        public ContentAlignment TextAlignment
        {
            get { return mTextAlignment; }
            set
            { 
                mTextAlignment = value;
                Invalidate();
            }
        }

        #region 流程导航特定属性
        [JsonProperty]
        [Description("流程ID"), Category("流程导航")]
        public string ProcessId
        {
            get { return mProcessId; }
            set { mProcessId = value; Invalidate(); }
        }

        [JsonProperty]
        [Description("导航URL"), Category("流程导航")]
        public string NavigationUrl
        {
            get { return mNavigationUrl; }
            set { mNavigationUrl = value; Invalidate(); }
        }
        #endregion

        #endregion

        #region Constructor

        public ProcessNavigationNode() : base()
        {
            Initialize();
        }

        // 修复：不要尝试重写事件作为方法
        // 而是应该在构造函数中为事件添加处理程序
        public ProcessNavigationNode(IGraphSite site) : base(site)
        {
            Initialize();
        }

        public ProcessNavigationNode(IGraphSite site, WFNodeType nodeType) : base(site, nodeType)
        {
            Initialize();
        }

        public ProcessNavigationNode(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            if (info != null)
            {
                mNodeId = info.GetString("NodeId");
                mProcessName = info.GetString("ProcessName");
                mDescription = info.GetString("Description");
                // 添加流程导航特定属性的反序列化
                try { mProcessId = info.GetString("ProcessId"); } catch { }
                try { mNavigationUrl = info.GetString("NavigationUrl"); } catch { }
                mMenuID = info.GetString("MenuID");
                mFormName = info.GetString("FormName");
                mClassPath = info.GetString("ClassPath");
                mNodeColor = (Color)info.GetValue("NodeColor", typeof(Color));
                
                // 加载颜色属性
                try { _fontColor = (Color)info.GetValue("FontColor", typeof(Color)); } catch { }
                
                try { _borderColor = (Color)info.GetValue("BorderColor", typeof(Color)); } catch { }
                
                // 加载新的视觉属性
                try { mCustomText = info.GetString("CustomText"); } catch { }
                
                try { mShowCustomText = info.GetBoolean("ShowCustomText"); } catch { }
                
                try { mFontSize = info.GetSingle("FontSize"); } catch { }
                
                try { mFontStyle = (FontStyle)info.GetInt32("FontStyle"); } catch { }
                
                try { mOpacity = info.GetSingle("Opacity"); } catch { }
                
                try { mBackgroundImagePath = info.GetString("BackgroundImagePath"); } catch { }
                
                try { mTextWrap = info.GetBoolean("TextWrap"); } catch { }
                
                try { mTextAlignment = (ContentAlignment)info.GetInt32("TextAlignment"); } catch { }
                
                // 尝试加载背景图像
                if (!string.IsNullOrEmpty(mBackgroundImagePath) && File.Exists(mBackgroundImagePath))
                {
                    try
                    {
                        mBackgroundImage = Image.FromFile(mBackgroundImagePath);
                    }
                    catch { }
                }
                
                // 更新字体设置
                UpdateFont();
            }
            Initialize();
        }

        private void Initialize()
        {
            if (string.IsNullOrEmpty(mNodeId))
            {
                mNodeId = Guid.NewGuid().ToString();
            }

            if (string.IsNullOrEmpty(mProcessName))
            {
                mProcessName = "流程节点";
                this.Text = mProcessName;
            }

            // 设置节点大小
            Rectangle = new RectangleF(0, 0, 70, 40);

            // 添加连接器
            TopNode = new Connector(this, "Top", true);
            TopNode.ConnectorLocation = ConnectorLocation.North;
            TopNode.AllowNewConnectionsFrom = true;
            TopNode.AllowNewConnectionsTo = true;
            Connectors.Add(TopNode);

            BottomNode = new Connector(this, "Bottom", true);
            BottomNode.ConnectorLocation = ConnectorLocation.South;
            BottomNode.AllowNewConnectionsFrom = true;
            BottomNode.AllowNewConnectionsTo = true;
            Connectors.Add(BottomNode);

            LeftNode = new Connector(this, "Left", true);
            LeftNode.ConnectorLocation = ConnectorLocation.West;
            LeftNode.AllowNewConnectionsFrom = true;
            LeftNode.AllowNewConnectionsTo = true;
            Connectors.Add(LeftNode);

            RightNode = new Connector(this, "Right", true);
            RightNode.ConnectorLocation = ConnectorLocation.East;
            RightNode.AllowNewConnectionsFrom = true;
            RightNode.AllowNewConnectionsTo = true;
            Connectors.Add(RightNode);

            IsResizable = true;
            
            // 初始化NodeStepPropertyValue属性，避免双击节点或连接线时出现空引用错误
            NodeStepPropertyValue = this;

            // 为事件添加处理程序
            OnMouseDown += ProcessNavigationNode_OnMouseDown;
            OnMouseUp += ProcessNavigationNode_OnMouseUp;
        }

        /// <summary>
        /// 更新字体设置
        /// </summary>
        private void UpdateFont()
        {
            try
            {
                string fontFamilyName = mTextFont?.FontFamily.Name ?? "Microsoft YaHei";
                mTextFont = new Font(fontFamilyName, mFontSize, mFontStyle);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("更新字体失败: " + ex.Message);
                // 回退到默认字体
                mTextFont = new Font("Microsoft YaHei", 10, FontStyle.Regular);
            }
        }

        // 连接器已在Initialize方法中直接添加，移除独立方法
        
        /// <summary>
        /// 更新连接器位置
        /// 确保连接器总是显示在节点的正确位置
        /// </summary>
        public override void Invalidate()
        {
            base.Invalidate();
            UpdateConnectorsPosition();
        }
        
        /// <summary>
        /// 更新连接器的位置
        /// </summary>
        private void UpdateConnectorsPosition()
        {
            // Connector类没有Point属性，使用ConnectionPoint方法获取坐标
            // 这里只更新节点自身的位置信息，连接器的实际位置由ConnectionPoint方法提供
            // 不需要直接设置连接器的Point属性
            Invalidate(); // 触发重绘，确保连接器位置正确显示
        }
        
        /// <summary>
        /// 确保在节点移动时更新连接器位置
        /// </summary>
        public void Move(float x, float y)
        {
            // 直接更新矩形位置而不是调用不存在的基类方法
            Rectangle = new RectangleF(x, y, Rectangle.Width, Rectangle.Height);
            UpdateConnectorsPosition();
        }
        
        /// <summary>
        /// 确保在调整节点大小时更新连接器位置
        /// </summary>
        public void Resize(float width, float height)
        {
            // 直接更新矩形大小而不是调用不存在的基类方法
            Rectangle = new RectangleF(Rectangle.X, Rectangle.Y, width, height);
            UpdateConnectorsPosition();
        }

        #endregion

        #region Serialization

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue("NodeId", mNodeId);
            info.AddValue("ProcessName", mProcessName);
            info.AddValue("Description", mDescription);
            info.AddValue("MenuID", mMenuID);
            info.AddValue("FormName", mFormName);
            info.AddValue("ClassPath", mClassPath);
            info.AddValue("NodeColor", mNodeColor);
            info.AddValue("FontColor", _fontColor);
            info.AddValue("BorderColor", _borderColor);
            
            // 添加新的视觉属性
            info.AddValue("CustomText", mCustomText);
            info.AddValue("ShowCustomText", mShowCustomText);
            info.AddValue("FontSize", mFontSize);
            info.AddValue("FontStyle", (int)mFontStyle);
            info.AddValue("Opacity", mOpacity);
            info.AddValue("BackgroundImagePath", mBackgroundImagePath);
            info.AddValue("TextWrap", mTextWrap);
            info.AddValue("TextAlignment", (int)mTextAlignment);
            
            // 序列化流程导航特定属性
            info.AddValue("ProcessId", mProcessId);
            info.AddValue("NavigationUrl", mNavigationUrl);
        }

        #endregion

        #region Painting

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
                // Draw the node shape with rounded corners
                using (GraphicsPath path = new GraphicsPath())
                {
                    path.AddLine(Rectangle.X, Rectangle.Y, Rectangle.Right - 10, Rectangle.Y);
                    path.AddArc(Rectangle.X + Rectangle.Width - 20, Rectangle.Y, 20, 20, -90, 90);
                    path.AddLine(Rectangle.Right, Rectangle.Y + 10, Rectangle.Right, Rectangle.Bottom);
                    path.AddLine(Rectangle.Right, Rectangle.Bottom, Rectangle.Left + 10, Rectangle.Bottom);
                    path.AddArc(Rectangle.X, Rectangle.Y + Rectangle.Height - 20, 20, 20, 90, 90);
                    path.AddLine(Rectangle.X, Rectangle.Y + Rectangle.Height - 10, Rectangle.X, Rectangle.Y);
                    
                    //shadow
                    using (Region darkRegion = new Region(path))
                    {
                        darkRegion.Translate(5, 5);
                        g.FillRegion(new SolidBrush(Color.FromArgb(20, Color.Black)), darkRegion);
                    }

                    // 填充节点
                    g.FillPath(new SolidBrush(ShapeColor), path);
                    
                    // 绘制边框
                    using (Pen pen = new Pen(BorderColor, 1))
                    {
                        g.DrawPath(pen, path);
                    }
                }
            }
            if (ShowLabel)
            {
                using (StringFormat sf = new StringFormat())
                {
                    sf.Alignment = StringAlignment.Center;
                    sf.LineAlignment = StringAlignment.Center;
                    g.DrawString(Text, Font, new SolidBrush(_fontColor), Rectangle, sf);
                }
            }
        }
        
        /// <summary>
        /// Returns a floating-point point coordinates for a given connector
        /// </summary>
        /// <param name="c">A connector object</param>
        /// <returns>A floating-point pointF</returns>
        public override PointF ConnectionPoint(Connector c)
        {
            if (c == TopNode) return new PointF(Rectangle.Left + (Rectangle.Width / 2), Rectangle.Top);
            if (c == BottomNode) return new PointF(Rectangle.Left + (Rectangle.Width / 2), Rectangle.Bottom);
            if (c == LeftNode) return new PointF(Rectangle.Left, Rectangle.Top + (Rectangle.Height / 2));
            if (c == RightNode) return new PointF(Rectangle.Right, Rectangle.Top + (Rectangle.Height / 2));
            return new PointF(0, 0);
        }

        /// <summary>
        /// Overrides the default bitmap used in the shape viewer
        /// </summary>
        /// <returns></returns>
        public override Bitmap GetThumbnail()
        {            
            try
            {
                // 创建一个小的位图作为缩略图
                Bitmap bmp = new Bitmap(32, 32);
                using (Graphics g = Graphics.FromImage(bmp))
                {
                    // 设置高质量绘制
                    g.SmoothingMode = SmoothingMode.AntiAlias;
                    
                    // 绘制节点形状
                    Rectangle rect = new Rectangle(2, 2, 28, 28);
                    using (GraphicsPath path = new GraphicsPath())
                    {
                        path.AddLine(rect.X, rect.Y, rect.Right - 5, rect.Y);
                        path.AddArc(rect.X + rect.Width - 10, rect.Y, 10, 10, -90, 90);
                        path.AddLine(rect.Right, rect.Y + 5, rect.Right, rect.Bottom);
                        path.AddLine(rect.Right, rect.Bottom, rect.Left + 5, rect.Bottom);
                        path.AddArc(rect.X, rect.Y + rect.Height - 10, 10, 10, 90, 90);
                        path.AddLine(rect.X, rect.Y + rect.Height - 5, rect.X, rect.Y);
                        
                        // 填充和描边
                        g.FillPath(new SolidBrush(mNodeColor), path);
                        g.DrawPath(new Pen(Color.Black, 1), path);
                    }
                    
                    // 绘制文本
                    using (StringFormat sf = new StringFormat())
                    {
                        sf.Alignment = StringAlignment.Center;
                        sf.LineAlignment = StringAlignment.Center;
                        g.DrawString("导", new Font("Microsoft YaHei", 8), Brushes.White, rect, sf);
                    }
                }
                return bmp;
            }
            catch (Exception exc)
            {
                Trace.WriteLine(exc.Message, "ProcessNavigationNode.GetThumbnail");
                // 返回基本位图
                return new Bitmap(32, 32);
            }
        }

        #endregion

        #region Mouse Events

        private void ProcessNavigationNode_OnMouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                // 右键显示上下文菜单
                ShowContextMenu(new Point(e.X, e.Y));
            }
        }

        private void ProcessNavigationNode_OnMouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && e.Clicks == 2)
            {
                // 设计模式：显示属性对话框
                // 在设计模式下，双击节点应该显示属性对话框
                ShowPropertiesDialog();
            }
        }

        /// <summary>
        /// 显示右键菜单
        /// </summary>
        /// <param name="location">鼠标位置</param>
        private void ShowContextMenu(Point location)
        {
            try
            {
                ContextMenuStrip contextMenu = new ContextMenuStrip();

                // 属性菜单项
                ToolStripMenuItem propertiesItem = new ToolStripMenuItem("属性");
                propertiesItem.Click += (s, e) => ShowPropertiesDialog();

                // 测试连接菜单项
                ToolStripMenuItem testConnectionItem = new ToolStripMenuItem("测试连接");
                testConnectionItem.Click += (s, e) => TestConnection();

                contextMenu.Items.AddRange(new ToolStripItem[] {
                    propertiesItem,
                    new ToolStripSeparator(),
                    testConnectionItem
                });

                // 显示菜单
                // 使用Site作为IGraphSite，转换为Control来显示菜单
                if (Site is Control control)
                {
                    contextMenu.Show(control, location);
                }
                else
                {
                    // 如果Site不是Control，尝试获取父窗体  先让编译通过但不推荐使用
                    //var parentForm = this.FindForm();
                    //if (parentForm != null)
                    //{
                    //    contextMenu.Show(parentForm, location);
                    //}
                }
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show($"显示右键菜单失败：{ex.Message}", "错误",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 显示属性对话框
        /// </summary>
        private void ShowPropertiesDialog()
        {
            try
            {
                using (var dialog = new Dialogs.ProcessNavigationNodePropertiesDialog(this))
                {
                    if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        // 属性已更新，重绘节点
                        Invalidate();
                    }
                }
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show($"显示属性对话框失败：{ex.Message}", "错误",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 测试连接
        /// </summary>
        private void TestConnection()
        {
            try
            {
                if (!string.IsNullOrEmpty(mMenuID))
                {
                    // 通过菜单ID查找菜单信息


                    //var menuService = Startup.GetFromFac<Itb_MenuInfoServices>();
                    //var menuInfo = await menuService.QueryableByIDAsync(long.Parse(mMenuID));

                    var menuInfo = new tb_MenuInfo();

                    throw new Exception("先编译通过，调试时完善");

                    if (menuInfo != null)
                    {
                        System.Windows.Forms.MessageBox.Show($"菜单 '{menuInfo.CaptionCN}' 连接测试成功！", "提示",
                            System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information);
                    }
                    else
                    {
                        System.Windows.Forms.MessageBox.Show($"未找到菜单ID为 {mMenuID} 的菜单信息", "提示",
                            System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
                    }
                }
                else
                {
                    System.Windows.Forms.MessageBox.Show("该节点未关联任何菜单，请先设置属性", "提示",
                        System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show($"测试连接失败：{ex.Message}", "错误",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
        }

        private void OpenRelatedForm()
        {
            try
            {
                if (!string.IsNullOrEmpty(mMenuID))
                {
                    var menuInfo = new tb_MenuInfo();

                    throw new Exception("先编译通过，调试时完善");

                    // 通过菜单ID查找菜单信息
                    //var menuService = Startup.GetFromFac<Itb_MenuInfoServices>();
                    //var menuInfo = await menuService.QueryableByIDAsync(long.Parse(mMenuID));

                    //if (menuInfo != null)
                    //{
                    //    // 使用现有的菜单打开机制
                    //    await MenuHelperExtensions.OpenMenuAsync(menuInfo.MenuID);
                    //}
                    //else
                    //{
                    //    System.Windows.Forms.MessageBox.Show($"未找到菜单ID为 {mMenuID} 的菜单信息", "提示",
                    //        System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information);
                    //}
                }
                else if (!string.IsNullOrEmpty(mFormName) && !string.IsNullOrEmpty(mClassPath))
                {
                    // 直接通过反射创建窗体
                    var assembly = System.Reflection.Assembly.LoadFrom("RUINORERP.UI.exe");
                    var formType = assembly.GetType(mClassPath + "." + mFormName);

                    if (formType != null)
                    {
                        var form = Activator.CreateInstance(formType) as System.Windows.Forms.Form;
                        form?.Show();
                    }
                }
                else
                {
                    System.Windows.Forms.MessageBox.Show("该节点未关联任何业务单据", "提示",
                        System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show($"打开业务单据时发生错误：{ex.Message}", "错误",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
        }

        #endregion


    }
}