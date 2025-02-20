using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Autofac;
using Krypton.Toolkit;
using RUINORERP.Common.Helper;
using Krypton.Workspace;
using Krypton.Navigator;
using RUINORERP.Model;
using RUINORERP.UI.Common;
using System.Collections.Concurrent;
using System.Windows.Documents;
using Fireasy.Common.Extensions;

namespace RUINORERP.UI.BaseForm
{

    /// <summary>
    /// 暂时只用于产品类别 如果用于其他基类<T>写法
    /// </summary>
    public partial class BaseListWithTree : BaseUControl
    {
        public BaseListWithTree()
        {
            InitializeComponent();
            this.toolStrip1.ItemClicked += ToolStrip1_ItemClicked;
            this.bindingSourceList.ListChanged += BindingSourceList_ListChanged;

            InitListData();
            ShowToolBarOfList();

            #region 后加by watson 参考自其他dg
            /*
            DataGridViewCellStyle c = new DataGridViewCellStyle();
            c.BackColor = Color.Yellow;
            dataGridView1.AlternatingRowsDefaultCellStyle = c;
            dataGridView1.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridView1.GridColor = Color.SkyBlue;
            dataGridView1.AlternatingRowsDefaultCellStyle.BackColor = Color.Beige;
            dataGridView1.DefaultCellStyle.SelectionBackColor = Color.MistyRose;
            dataGridView1.DefaultCellStyle.SelectionForeColor = Color.Blue;
            dataGridView1.RowHeadersDefaultCellStyle.SelectionBackColor = Color.Gold;
            dataGridView1.RowHeadersDefaultCellStyle.SelectionForeColor = Color.Green;
         

            // this.FirstDisplayedScrollingRowIndex = this.Rows[this.Rows.Count - 1].Index;
            dataGridView1.RowHeadersWidth = 59;
            //this.this.Rows[this.Rows.Count + 1].Selected = true;

            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.AllowUserToDeleteRows = false;
            */
            #endregion

        }

        private void BindingSourceList_ListChanged(object sender, ListChangedEventArgs e)
        {
            BaseEntity entity = new BaseEntity();
            switch (e.ListChangedType)
            {
                case ListChangedType.Reset:
                    break;
                case ListChangedType.ItemAdded:
                    entity = bindingSourceList.List[e.NewIndex] as BaseEntity;
                    if (entity != null)
                    {
                        entity.ActionStatus = ActionStatus.新增;
                    }

                    break;
                case ListChangedType.ItemDeleted:
                    if (e.NewIndex < bindingSourceList.Count)
                    {
                        entity = bindingSourceList.List[e.NewIndex] as BaseEntity;
                        entity.ActionStatus = ActionStatus.删除;
                    }
                    break;
                case ListChangedType.ItemMoved:
                    break;
                case ListChangedType.ItemChanged:
                    entity = bindingSourceList.List[e.NewIndex] as BaseEntity;
                    if (entity.ActionStatus == ActionStatus.无操作)
                    {
                        entity.ActionStatus = ActionStatus.修改;
                    }
                    break;
                case ListChangedType.PropertyDescriptorAdded:
                    break;
                case ListChangedType.PropertyDescriptorDeleted:
                    break;
                case ListChangedType.PropertyDescriptorChanged:
                    break;
                default:
                    break;
            }
        }


        private Type _EditForm;
        public Type EditForm { get => _EditForm; set => _EditForm = value; }

        /// <summary>
        /// 控制列表的菜单的显示
        /// </summary>
        internal void ShowToolBarOfList()
        {
            /*
            string temp = "高级查询新增修改退出保存删除刷新查询导出Excel";
            foreach (ToolStripItem tb in toolStrip1.Items)
            {
                if (temp.Contains(tb.Text))
                {
                    tb.Enabled = true;
                }
                else
                {
                    tb.Enabled = false;
                }
            }
            */

            //foreach (DataGridViewColumn var in this.dataGridView1.Columns)
            //{
            //    var.HeaderText = this.FieldNameList.Find(delegate (KeyValuePair<string, string> kv) { return kv.Key == var.Name; }).Value;
            //    if (var.HeaderText.Trim().Length == 0)
            //    {
            //        var.Visible = false;
            //    }
            //}

        }


        /// <summary>
        /// 不同情况，显示不同的可用情况
        /// </summary>
        internal void ToolBarEnabledControl(MenuItemEnums menu)
        {
            switch (menu)
            {
                case MenuItemEnums.新增:
                case MenuItemEnums.删除:
                case MenuItemEnums.修改:
                    toolStripButtonSave.Enabled = true;
                    break;
                case MenuItemEnums.查询:
                    toolStripButtonSave.Enabled = false;
                    break;
                case MenuItemEnums.保存:
                    toolStripButtonSave.Enabled = false;
                    break;

                case MenuItemEnums.关闭:
                    break;
                case MenuItemEnums.刷新:
                    break;
                case MenuItemEnums.打印:
                    break;
                case MenuItemEnums.导出:
                    break;
                default:
                    break;
            }
            Edited = toolStripButtonSave.Enabled;
        }


        #region 画行号

        private void dataGridView1_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {

            if (e.ColumnIndex >= 0 && e.RowIndex >= 0)
            {
                DataGridViewPaintParts paintParts =
                    e.PaintParts & ~DataGridViewPaintParts.Focus;

                e.Paint(e.ClipBounds, paintParts);
                e.Handled = true;
            }

            if (e.ColumnIndex < 0 && e.RowIndex >= 0)
            {
                e.Paint(e.ClipBounds, DataGridViewPaintParts.All);
                Rectangle indexRect = e.CellBounds;
                indexRect.Inflate(-2, -2);

                TextRenderer.DrawText(e.Graphics,
                    (e.RowIndex + 1).ToString(),
                    e.CellStyle.Font,
                    indexRect,
                    e.CellStyle.ForeColor,
                    TextFormatFlags.Right | TextFormatFlags.VerticalCenter);
                e.Handled = true;
            }
        }

        #endregion

        private ConcurrentDictionary<string, KeyValuePair<string, bool>> fieldNameList;

        /// <summary>
        /// 表列名的中文描述集合
        /// </summary>
        [Description("表列名的中文描述集合"), Category("自定属性"), Browsable(true)]
        public ConcurrentDictionary<string, KeyValuePair<string, bool>> FieldNameList
        {
            get
            {
                return fieldNameList;
            }
            set
            {
                fieldNameList = value;
            }

        }



        public System.Windows.Forms.BindingSource _ListDataSoure = null;

        [Description("列表中的要显示的数据来源[BindingSource]"), Category("自定属性"), Browsable(true)]
        /// <summary>
        /// 列表的数据源(实际要显示的)
        /// </summary>
        public System.Windows.Forms.BindingSource ListDataSoure
        {
            get { return _ListDataSoure; }
            set { _ListDataSoure = value; }
        }
        private bool editflag;

        /// <summary>
        /// 是否为编辑 如果为是则true
        /// </summary>
        public bool Edited
        {
            get { return editflag; }
            set { editflag = value; }
        }



        /// <summary>
        /// 初始化列表数据
        /// </summary>
        internal void InitListData()
        {
            this.dataGridView1.DataSource = null;
            toolStripButtonSave.Enabled = false;
            ListDataSoure = bindingSourceList;
            //绑定导航
            this.bindingNavigatorList.BindingSource = ListDataSoure;

            this.dataGridView1.DataSource = ListDataSoure.DataSource;

        }

        protected virtual void ToolStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            MainForm.Instance.AppContext.log.ActionName = e.ClickedItem.Text;
            if (e.ClickedItem.Text.Length > 0)
            {
                DoButtonClick(EnumHelper.GetEnumByString<MenuItemEnums>(e.ClickedItem.Text));
            }
        }

        /// <summary>
        /// 控制功能按钮
        /// </summary>
        /// <param name="p_Text"></param>
        protected virtual void DoButtonClick(MenuItemEnums menuItem)
        {
            //操作前将数据收集
            this.ValidateChildren(System.Windows.Forms.ValidationConstraints.None);
            switch (menuItem)
            {
                case MenuItemEnums.新增:
                    Add();
                    break;
                case MenuItemEnums.删除:
                    if (bindingSourceList.Current == null)
                    {

                        return;
                    }
                    Delete();
                    break;
                case MenuItemEnums.修改:
                    Modify();
                    break;
                case MenuItemEnums.查询:
                    Query();
                    break;
                case MenuItemEnums.保存:
                    //操作前将数据收集
                    this.ValidateChildren(System.Windows.Forms.ValidationConstraints.None);
                    Save();
                    break;

                case MenuItemEnums.关闭:
                    Exit(this);
                    break;
                case MenuItemEnums.刷新:
                    break;
                case MenuItemEnums.导出:
                    break;
                default:
                    break;
            }


        }


        #region 定义所有工具栏的方法

        protected virtual void Add()
        {

        }

        protected virtual void Delete()
        {

        }

        protected virtual void Modify()
        {

        }

        protected virtual void Query()
        {

        }
        protected virtual void Save()
        {

        }

        protected virtual void Exit(object thisform)
        {
            if (!Edited)
            {
                //退出
                CloseTheForm(thisform);
            }
            else
            {
                if (MessageBox.Show(this, "有数据没有保存\r\n你确定要退出吗", "询问", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                {
                    //退出
                    CloseTheForm(thisform);
                }
            }
        }

        protected virtual void Refreshs()
        {
            this.dataGridView1.FieldNameList = this.FieldNameList;
            if (!Edited)
            {
                Query();
            }
            else
            {
                if (MessageBox.Show(this, "有数据没有保存\r\n你确定要重新加载吗", "询问", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                {
                    Query();
                }
            }

        }
        #endregion

        /// <summary>
        /// esc退出窗体
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="keyData"></param>
        /// <returns></returns>
        protected override bool ProcessCmdKey(ref System.Windows.Forms.Message msg, System.Windows.Forms.Keys keyData) //激活回车键
        {
            int WM_KEYDOWN = 256;
            int WM_SYSKEYDOWN = 260;

            if (msg.Msg == WM_KEYDOWN | msg.Msg == WM_SYSKEYDOWN)
            {
                switch (keyData)
                {
                    case Keys.Escape:
                        Exit(this);//csc关闭窗体
                        break;
                }

            }
            //return false;
            var key = keyData & Keys.KeyCode;
            var modeifierKey = keyData & Keys.Modifiers;
            if (modeifierKey == Keys.Control && key == Keys.F)
            {
                // MessageBox.Show("Control+F is pressed");
                return true;

            }

            var otherkey = keyData & Keys.KeyCode;
            var othermodeifierKey = keyData & Keys.Modifiers;
            if (othermodeifierKey == Keys.Control && otherkey == Keys.F)
            {
                MessageBox.Show("Control+F is pressed");
                return true;
            }

            return base.ProcessCmdKey(ref msg, keyData);

        }

        private void CloseTheForm(object thisform)
        {
            KryptonWorkspaceCell cell = MainForm.Instance.kryptonDockableWorkspace1.ActiveCell;
            if (cell == null)
            {
                cell = new KryptonWorkspaceCell();
                MainForm.Instance.kryptonDockableWorkspace1.Root.Children.Add(cell);
            }

            KryptonPage page = FindPage(thisform as Control);
            if (page != null)
            {
                MainForm.Instance.kryptonDockingManager1.RemovePage(page.UniqueName, true);
                page.Dispose();
            }
            else
            {
                //浮动窗体关闭

                KryptonForm form = FindKryptonForm(thisform as Control);
                if (form != null)
                {
                    form.Close();
                }
                //thisform is KryptonForm ? (thisform as KryptonForm).Close() : this.Close();
            }

            //KryptonPage page = (thisform as Control).Parent as KryptonPage;
            //if (page != null)
            //{
            //    MainForm.Instance.kryptonDockingManager1.RemovePage(page.UniqueName, true);
            //    page.Dispose();
            //}


            /*
            if (page == null)
            {
                //浮动
               
            }
            else
            {
                //活动内
                if (cell.Pages.Contains(page))
                {
                    cell.Pages.Remove(page);
                    page.Dispose();
                }
            }
            */
        }

        private KryptonPage FindPage(Control control)
        {
            KryptonPage OutPutPage = null;
            KryptonPage page = control as KryptonPage;
            if (page != null)
            {
                OutPutPage = page;
            }
            else
            {
                if (control.Parent != null)
                {
                    return FindPage(control.Parent as Control);
                }
            }
            return OutPutPage;
        }

        private KryptonForm FindKryptonForm(Control control)
        {
            KryptonForm OutPutForm = null;
            KryptonForm form = control as KryptonForm;
            if (form != null)
            {
                OutPutForm = form;
            }
            else
            {
                if (control.Parent != null)
                {
                    return FindKryptonForm(control.Parent as Control);
                }
            }
            return OutPutForm;
        }

        private void bindingNavigatorMovePreviousItem_Click(object sender, EventArgs e)
        {
            ListDataSoure.MovePrevious();
        }

        private void bindingNavigatorMoveNextItem_Click(object sender, EventArgs e)
        {
            ListDataSoure.MoveNext();
        }

        private void bindingNavigatorMoveLastItem_Click(object sender, EventArgs e)
        {
            ListDataSoure.MoveLast();
        }

        private void bindingNavigatorMoveFirstItem_Click(object sender, EventArgs e)
        {
            ListDataSoure.MoveFirst();
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            Modify();
        }

        private void dataGridView1_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {

        }


        //三级 还是两级呢。  反向来 一是 KEY VALUE  然后是列名
        ConcurrentDictionary<string, List<KeyValuePair<object, string>>> _DataDictionary = new ConcurrentDictionary<string, List<KeyValuePair<object, string>>>();

        /// <summary>
        /// 固定的值显示，入库ture 出库false
        /// 每个列表对应的值 ，单独设置
        /// </summary>
        public ConcurrentDictionary<string, List<KeyValuePair<object, string>>> ColNameDataDictionary { get => _DataDictionary; set => _DataDictionary = value; }


        private void dataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            //如果列是隐藏的是不是可以不需要控制显示了呢? 后面看是否是导出这块需要不需要 不然可以隐藏的直接跳过
            if (!dataGridView1.Columns[e.ColumnIndex].Visible)
            {
                return;
            }

            if (e.Value == null)
            {
                e.Value = "";
                return;
            }
            //固定字典值显示
            string colDbName = dataGridView1.Columns[e.ColumnIndex].Name;

            if (colDbName == "Parent_id")
            {
                if (e.Value == null || e.Value.ToString() == "0")
                {
                    e.Value = "类目根节点";
                    return;
                }
                else
                {
                    ////这里要用缓存
                    //BindingSortCollection<tb_ProdCategories> list = ListDataSoure.DataSource as BindingSortCollection<tb_ProdCategories>;
                    //List<tb_ProdCategories> clist = list.ToList<tb_ProdCategories>();
                    //tb_ProdCategories pc = clist.Find(t => t.Category_ID == int.Parse(e.Value.ToString()));
                    //if (pc != null)
                    //{
                    //    e.Value = pc.Category_name;
                    //    return;
                    //}

                }

            }


            if (ColNameDataDictionary.ContainsKey(colDbName))
            {
                List<KeyValuePair<object, string>> kvlist = new List<KeyValuePair<object, string>>();
                //意思是通过列名找，再通过值找到对应的文本
                ColNameDataDictionary.TryGetValue(colDbName, out kvlist);
                if (kvlist != null)
                {
                    KeyValuePair<object, string> kv = kvlist.FirstOrDefault(t => t.Key.ToString().ToLower() == e.Value.ToString().ToLower());
                    if (kv.Value != null)
                    {
                        e.Value = kv.Value;
                        return;
                    }

                }
            }

            //动态字典值显示
            string colName = UIHelper.ShowGridColumnsNameValue<tb_ProdCategories>(colDbName, e.Value);
            if (!string.IsNullOrEmpty(colName))
            {
                e.Value = colName;
                return;
            }

            //图片特殊处理
            if (colDbName == "Image")
            {
                if (e.Value != null)
                {
                    System.IO.MemoryStream buf = new System.IO.MemoryStream((byte[])e.Value);
                    Image image = Image.FromStream(buf, true);
                    e.Value = image;
                    return;
                    //这里用缓存
                }
            }

        }
    }
}
