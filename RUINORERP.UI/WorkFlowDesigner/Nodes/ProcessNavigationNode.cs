using Netron.GraphLib;
using Netron.GraphLib.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.Serialization;
using RUINORERP.WF;
using RUINORERP.Model;
using Netron.GraphLib.Attributes;
using System.Windows.Forms;
using RUINORERP.UI.WorkFlowDesigner.Dialogs;
using RUINORERP.UI.Common;
using System.Linq;
using System.IO;
using System.Drawing.Imaging;

namespace RUINORERP.UI.WorkFlowDesigner.Nodes
{
    /// <summary>
    /// 流程导航图节点
    /// 用于创建美观的业务流程示意图，支持点击打开对应业务单据窗体
    /// </summary>
    [Serializable]
    [Description("流程导航节点")]
    [JsonObject(MemberSerialization.OptIn)]
    [NetronGraphShape("流程导航节点", "9ED1469D-90B2-43ab-B000-4FF5C682F503", "流程导航", "RUINORERP.UI.WorkFlowDesigner.Nodes.ProcessNavigationNode",
         "用于业务流程导航的节点图形")]
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
            Rectangle = new RectangleF(0, 0, 140, 80);

            // 添加连接器
            AddConnectors();

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

        private void AddConnectors()
        {
            // 创建连接器
            TopNode = new Connector(this, "Top", true);
            TopNode.ConnectorLocation = ConnectorLocation.North;

            BottomNode = new Connector(this, "Bottom", true);
            BottomNode.ConnectorLocation = ConnectorLocation.South;

            LeftNode = new Connector(this, "Left", true);
            LeftNode.ConnectorLocation = ConnectorLocation.West;

            RightNode = new Connector(this, "Right", true);
            RightNode.ConnectorLocation = ConnectorLocation.East;

            // 添加连接器到集合
            Connectors.Add(TopNode);
            Connectors.Add(BottomNode);
            Connectors.Add(LeftNode);
            Connectors.Add(RightNode);
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
        }

        #endregion

        #region Painting

        public override void Paint(Graphics g)
        {
            if (g == null) return;

            // 设置高质量渲染
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;

            // 创建带有透明度的Graphics容器
            GraphicsContainer container = g.BeginContainer();
            
            try
            {
                // 绘制阴影
                RectangleF shadowRect = Rectangle;
                shadowRect.Offset(3, 3);
                using (SolidBrush shadowBrush = new SolidBrush(Color.FromArgb(50, Color.Black)))
                {
                    g.FillRectangle(shadowBrush, shadowRect);
                }

                // 绘制圆角矩形背景或背景图像
                using (GraphicsPath path = CreateRoundedRectanglePath(Rectangle, 10))
                {
                    // 先绘制背景图像（如果有）
                    if (!string.IsNullOrEmpty(mBackgroundImagePath) && File.Exists(mBackgroundImagePath))
                    {
                        try
                        {
                            using (Image backgroundImage = Image.FromFile(mBackgroundImagePath))
                            {
                                // 创建透明度调整的图像
                                using (Image transparentImage = new Bitmap(backgroundImage.Width, backgroundImage.Height))
                                using (Graphics imageG = Graphics.FromImage(transparentImage))
                                {
                                    ColorMatrix colorMatrix = new ColorMatrix();
                                    colorMatrix.Matrix33 = mOpacity; // 设置透明度
                                    ImageAttributes imageAttributes = new ImageAttributes();
                                    imageAttributes.SetColorMatrix(colorMatrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);
                                    
                                    imageG.DrawImage(backgroundImage, 
                                        new Rectangle(0, 0, transparentImage.Width, transparentImage.Height),
                                        0, 0, backgroundImage.Width, backgroundImage.Height,
                                        GraphicsUnit.Pixel, imageAttributes);
                                    
                                    // 拉伸绘制到节点区域
                                    g.DrawImage(transparentImage, Rectangle);
                                }
                            }
                        }
                        catch
                        {
                            // 图像加载失败时，使用默认背景
                            using (SolidBrush brush = new SolidBrush(Color.FromArgb((int)(mOpacity * 255), mNodeColor)))
                            {
                                g.FillPath(brush, path);
                            }
                        }
                    }
                    else
                    {
                        // 使用节点颜色作为背景
                        using (SolidBrush brush = new SolidBrush(Color.FromArgb((int)(mOpacity * 255), mNodeColor)))
                        {
                            g.FillPath(brush, path);
                        }
                    }

                    // 绘制边框
                    using (Pen pen = new Pen(_borderColor, 2))
                    {
                        g.DrawPath(pen, path);
                    }
                }

                // 绘制文本
                RectangleF textRect = Rectangle;
                textRect.Inflate(-10, -10);
                
                // 确定要显示的文本
                string displayText = mShowCustomText && !string.IsNullOrEmpty(mCustomText) ? mCustomText : mProcessName;
                
                // 设置文本格式
                StringFormat stringFormat = new StringFormat();
                
                // 根据TextAlignment设置对齐方式
                switch (mTextAlignment)
                {
                    case ContentAlignment.TopLeft:
                        stringFormat.Alignment = StringAlignment.Near;
                        stringFormat.LineAlignment = StringAlignment.Near;
                        break;
                    case ContentAlignment.TopCenter:
                        stringFormat.Alignment = StringAlignment.Center;
                        stringFormat.LineAlignment = StringAlignment.Near;
                        break;
                    case ContentAlignment.TopRight:
                        stringFormat.Alignment = StringAlignment.Far;
                        stringFormat.LineAlignment = StringAlignment.Near;
                        break;
                    case ContentAlignment.MiddleLeft:
                        stringFormat.Alignment = StringAlignment.Near;
                        stringFormat.LineAlignment = StringAlignment.Center;
                        break;
                    case ContentAlignment.MiddleCenter:
                        stringFormat.Alignment = StringAlignment.Center;
                        stringFormat.LineAlignment = StringAlignment.Center;
                        break;
                    case ContentAlignment.MiddleRight:
                        stringFormat.Alignment = StringAlignment.Far;
                        stringFormat.LineAlignment = StringAlignment.Center;
                        break;
                    case ContentAlignment.BottomLeft:
                        stringFormat.Alignment = StringAlignment.Near;
                        stringFormat.LineAlignment = StringAlignment.Far;
                        break;
                    case ContentAlignment.BottomCenter:
                        stringFormat.Alignment = StringAlignment.Center;
                        stringFormat.LineAlignment = StringAlignment.Far;
                        break;
                    case ContentAlignment.BottomRight:
                        stringFormat.Alignment = StringAlignment.Far;
                        stringFormat.LineAlignment = StringAlignment.Far;
                        break;
                    default:
                        stringFormat.Alignment = StringAlignment.Center;
                        stringFormat.LineAlignment = StringAlignment.Center;
                        break;
                }
                
                // 设置换行
                if (mTextWrap)
                {
                    stringFormat.FormatFlags = StringFormatFlags.LineLimit;
                }

                // 绘制主文本
                using (SolidBrush textBrush = new SolidBrush(_fontColor))
                {
                    g.DrawString(displayText, mTextFont, textBrush, textRect, stringFormat);
                }

                // 如果不使用自定义文本且有描述，绘制描述
                if (!mShowCustomText && !string.IsNullOrEmpty(mDescription))
                {
                    RectangleF descRect = Rectangle;
                    descRect.Y += Rectangle.Height / 2;
                    descRect.Height = Rectangle.Height / 2;
                    descRect.Inflate(-10, -5);

                    using (Font descFont = new Font("Microsoft YaHei", 8))
                    using (SolidBrush descBrush = new SolidBrush(Color.FromArgb(200, _fontColor)))
                    {
                        g.DrawString(mDescription, descFont, descBrush, descRect,
                            new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
                    }
                }
            }
            finally
            {
                g.EndContainer(container);
            }

            // 绘制连接器
            foreach (Connector connector in Connectors)
            {
                connector.Paint(g);
            }
        }

        private GraphicsPath CreateRoundedRectanglePath(RectangleF rect, float radius)
        {
            GraphicsPath path = new GraphicsPath();
            float diameter = radius * 2;

            // 左上角
            path.AddArc(rect.X, rect.Y, diameter, diameter, 180, 90);
            // 右上角
            path.AddArc(rect.Right - diameter, rect.Y, diameter, diameter, 270, 90);
            // 右下角
            path.AddArc(rect.Right - diameter, rect.Bottom - diameter, diameter, diameter, 0, 90);
            // 左下角
            path.AddArc(rect.X, rect.Bottom - diameter, diameter, diameter, 90, 90);

            path.CloseFigure();
            return path;
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