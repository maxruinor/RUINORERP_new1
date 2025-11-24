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
using RUINORERP.UI.Common;
using System.Linq;

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
            set { mNodeId = value; }
        }

        [JsonProperty]
        [Description("流程名称")]
        public string ProcessName
        {
            get { return mProcessName; }
            set
            {
                mProcessName = value;
                this.Text = value;
                Invalidate();
            }
        }

        [JsonProperty]
        [Description("流程描述")]
        public string Description
        {
            get { return mDescription; }
            set
            {
                mDescription = value;
                Invalidate();
            }
        }

        [JsonProperty]
        [Description("关联菜单ID")]
        public string MenuID
        {
            get { return mMenuID; }
            set { mMenuID = value; }
        }

        [JsonProperty]
        [Description("关联窗体名称")]
        public string FormName
        {
            get { return mFormName; }
            set { mFormName = value; }
        }

        [JsonProperty]
        [Description("关联类路径")]
        public string ClassPath
        {
            get { return mClassPath; }
            set { mClassPath = value; }
        }

        [JsonProperty]
        [Description("业务类型")]
        public int BusinessType
        {
            get { return mBusinessType; }
            set { mBusinessType = value; }
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
                mNodeColor = value;
                Invalidate();
            }
        }

        [JsonProperty]
        [Description("填充颜色")]
        public Color FillColor
        {
            get { return mNodeColor; }
            set
            {
                mNodeColor = value;
                Invalidate();
            }
        }

        [JsonProperty]
        [Description("边框颜色")]
        public Color BorderColor
        {
            get { return Color.FromArgb(255, 255, 255); }
            set
            {
                Invalidate();
            }
        }

        [JsonProperty]
        [Description("字体颜色")]
        public Color FontColor
        {
            get { return Color.White; }
            set
            {
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

            // 为事件添加处理程序
            OnMouseDown += ProcessNavigationNode_OnMouseDown;
            OnMouseUp += ProcessNavigationNode_OnMouseUp;
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
        }

        #endregion

        #region Painting

        public override void Paint(Graphics g)
        {
            if (g == null) return;

            // 抗锯齿
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

            // 绘制阴影
            RectangleF shadowRect = Rectangle;
            shadowRect.Offset(3, 3);
            using (SolidBrush shadowBrush = new SolidBrush(Color.FromArgb(50, Color.Black)))
            {
                g.FillRectangle(shadowBrush, shadowRect);
            }

            // 绘制圆角矩形背景
            using (GraphicsPath path = CreateRoundedRectanglePath(Rectangle, 10))
            {
                using (SolidBrush brush = new SolidBrush(mNodeColor))
                {
                    g.FillPath(brush, path);
                }

                using (Pen pen = new Pen(Color.FromArgb(255, 255, 255), 2))
                {
                    g.DrawPath(pen, path);
                }
            }

            // 绘制文本
            if (!string.IsNullOrEmpty(mProcessName))
            {
                RectangleF textRect = Rectangle;
                textRect.Inflate(-10, -10);

                using (SolidBrush textBrush = new SolidBrush(Color.White))
                {
                    g.DrawString(mProcessName, mTextFont, textBrush, textRect,
                        new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
                }
            }

            // 绘制描述文本（如果有）
            if (!string.IsNullOrEmpty(mDescription))
            {
                RectangleF descRect = Rectangle;
                descRect.Y += Rectangle.Height / 2;
                descRect.Height = Rectangle.Height / 2;
                descRect.Inflate(-10, -5);

                using (Font descFont = new Font("Microsoft YaHei", 8))
                using (SolidBrush descBrush = new SolidBrush(Color.FromArgb(200, 255, 255, 255)))
                {
                    g.DrawString(mDescription, descFont, descBrush, descRect,
                        new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
                }
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
                // 双击时打开关联的业务单据
                OpenRelatedForm();
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