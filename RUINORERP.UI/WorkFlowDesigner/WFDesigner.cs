using EnumsNET;
using log4net.Plugin;
using Netron.GraphLib;
using Netron.NetronLight;
using RUINORERP.Common.Helper;
using RUINORERP.Global;
using RUINORERP.Model;
using RUINORERP.UI.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RUINORERP.UI.WorkFlowDesigner
{

    [MenuAttrAssemblyInfo("工作流设计器New", ModuleMenuDefine.模块定义.基础资料, ModuleMenuDefine.基础资料.系统设置)]
    public partial class WFDesigner : UserControl
    {
        public WFDesigner()
        {
            InitializeComponent();
            this.BaseToolStrip.ItemClicked += BaseToolStrip_ItemClicked;
            this.drawingStrip.ItemClicked += BaseToolStrip_ItemClicked;
            this.actionsStrip.ItemClicked += BaseToolStrip_ItemClicked;
            shapesListView.MouseDown += shapesListView_MouseDown;
            

        }


        private void BaseToolStrip_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            MainForm.Instance.AppContext.log.ActionName = e.ClickedItem.Text;
            if (e.ClickedItem.Text.Length > 0)
            {
                DoButtonClick(EnumHelper.GetEnumByString<WFDesignerEnum>(e.ClickedItem.Text));
            }
        }


        /// <summary>
        /// 控制功能按钮
        /// </summary>
        /// <param name="p_Text"></param>
        protected virtual void DoButtonClick(WFDesignerEnum menuItem)
        {
            //操作前将数据收集
            this.ValidateChildren(System.Windows.Forms.ValidationConstraints.None);
            switch (menuItem)
            {
                case WFDesignerEnum.新建:
                    NewWF();
                    break;
                case WFDesignerEnum.保存:
                    break;
                case WFDesignerEnum.打开:

                    break;
                case WFDesignerEnum.属性:
                    this.leftTabControl.SelectedTab = tabProperties;
                    
                    break;
                case WFDesignerEnum.撤销:
                   
                    break;
                case WFDesignerEnum.重做:
                   
                    break;
                case WFDesignerEnum.绘制矩形:
                 
                    break;
                case WFDesignerEnum.绘制椭圆:
                   
                    break;
                case WFDesignerEnum.组合:
                 
                    break;
                case WFDesignerEnum.取消分组:
                   
                    break;
                case WFDesignerEnum.文本工具:
                   
                    break;
                case WFDesignerEnum.绘图:
                    break;
                case WFDesignerEnum.连接:
                  
                    break;
                case WFDesignerEnum.指针:
                    break;
                case WFDesignerEnum.移动连接点:
                  
                    break;
                case WFDesignerEnum.置于底层:
                   
                    break;
                case WFDesignerEnum.置于顶层:
                    
                    break;
                case WFDesignerEnum.向上:
                 
                    break;
                case WFDesignerEnum.向下:
                    
                    break;
                case WFDesignerEnum.复制:
                    break;
                case WFDesignerEnum.粘贴:
                    break;
                default:
                    break;
            }
        }

        private Bitmap GetSampleImage()
        {

            Stream stream = System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream("RUINORERP.Resources.WF_Talking.gif");
            Bitmap bmp = null;
            if (stream != null)
            {
                bmp = Bitmap.FromStream(stream) as Bitmap;
                stream.Close();
            }
            return bmp;
        }

        #region 
        private void NewWF()
        {
            DialogResult res = MessageBox.Show("你确定吗？", "新建设计图", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            //if (res == DialogResult.Yes)
            //    this.diagramControl1.NewDiagram();
            //this.diagramControl1.Dock = DockStyle.Fill;
        }
        #endregion

        #region Undo/redo tools

        /// <summary>
        /// Updates the undo/redo buttons to reflect the history changes
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="T:Netron.NetronLight.HistoryChangeEventArgs"/> instance containing the event data.</param>
        private void diagramControl1_OnHistoryChange(object sender, HistoryChangeEventArgs e)
        {
            if (e.UndoText.Length == 0)
            {
                this.undoButton.Enabled = false;

            }
            else
            {
                this.undoButton.Enabled = true;

                this.undoButton.ToolTipText = "撤销 " + e.UndoText;

            }

            if (e.RedoText.Length == 0)
            {
                this.redoButton.Enabled = false;

            }
            else
            {
                this.redoButton.Enabled = true;

                this.redoButton.ToolTipText = "重做 " + e.RedoText;

            }
        }
        #endregion









        #region Diagram control handler links
        private void diagramControl1_OnShowSelectionProperties(object sender, SelectionEventArgs e)
        {
            this.propertyGrid.SelectedObjects = e.SelectedObjects;
        }
        #endregion



        #region Drawing tools


        /// <summary>
        /// Handles the MouseDown event of the shapesListView control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="T:System.Windows.Forms.MouseEventArgs"/> instance containing the event data.</param>
        private void shapesListView_MouseDown(object sender, MouseEventArgs e)
        {
            ListViewItem item = this.shapesListView.GetItemAt(e.X, e.Y);
            if (item != null)
            {
                if (item.Tag != null)
                {
                   // this.diagramControl1.DoDragDrop(item.Tag.ToString(), DragDropEffects.All);
                }
                else
                {

                }
       
            }

        }

        #endregion


        private void diagramControl1_OnEntityAdded(object sender, EntityEventArgs e)
        {
            string msg = string.Empty;
            if (e.Entity is IShape)
                msg = "New shape '" + e.Entity.EntityName + "' added.";
            else if (e.Entity is IConnection)
                msg = "New connection added.";

            if (msg.Length > 0)
                ShowStatusText(msg);
            if (e.Entity is ComplexRectangle)
            {
                ComplexRectangle shape = e.Entity as ComplexRectangle;

                try
                {
                    shape.Services[typeof(IMouseListener)] = new MyPlugin(shape);
                }
                catch (ArgumentException exc)
                {
                    ShowStatusText(exc.Message);

                }


            }
            else if (e.Entity is ImageShape)
            {
                Bitmap bmp = GetSampleImage();
                if (bmp != null)
                    (e.Entity as ImageShape).Image = bmp;
            }
        }

        private void diagramControl1_OnEntityRemoved(object sender, EntityEventArgs e)
        {
            ShowStatusText("Entity '" + e.Entity.EntityName + "' removed.");
        }

        private void ShowStatusText(string text)
        {
            this.statusLabel1.Text = text;
            this.statusLabel1.Visible = true;
            statusTimer.Start();
        }

        private void statusTimer_Tick(object sender, EventArgs e)
        {
            statusTimer.Stop();
            this.statusLabel1.Visible = false;
        }


        private void shapesListView_GiveFeedback(object sender, GiveFeedbackEventArgs e)
        {
            Cursor.Current = Cursors.HSplit;
        }

       
      

        private void NCWFDesigner_Load(object sender, EventArgs e)
        {
            // 获取 ShapeTypes 枚举的所有值
            foreach (ShapeTypes shapeType in Enum.GetValues(typeof(ShapeTypes)))
            {
                ListViewItem listViewItem = new ListViewItem(shapeType.GetName<ShapeTypes>());
                listViewItem.Tag = shapeType.ToString();
                shapesListView.Items.Add(listViewItem);
            }

        }
    }
 
}
