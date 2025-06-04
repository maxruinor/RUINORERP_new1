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
using RUINOR.Core;
using RUINORERP.UI.Common;
using RUINORERP.UI.BI;
using RUINORERP.Common.CustomAttribute;
using System.Linq.Expressions;
using System.Collections.Concurrent;
using RUINORERP.Business;
using RUINORERP.Global.CustomAttribute;
using ObjectsComparer;
using RUINORERP.Common.Extensions;
using RUINORERP.Business.AutoMapper;
using AutoMapper;
using RUINORERP.Model.Base;
using RUINORERP.Global.EnumExt;
using RUINORERP.Global;

namespace RUINORERP.UI.BaseForm
{

    /// <summary>
    /// 过时?
    /// </summary>
    [Obsolete]
    /// <summary>
    /// 基本资料的列表，是否需要加一个标记来表示 在菜单中编辑 ，还是在 其他窗体时 关联编辑。看后面的业务情况。
    /// </summary>
    [PreCheckMustOverrideBaseClass]
    public partial class BaseList : UserControl
    {
        private BaseListRunWay _runway;
        /// <summary>
        /// 窗体运行方式  在关联编辑功能时 这个好像没有起到作用。实际是在frmBaseEditList 这个中实现显示与隐藏。
        /// </summary>
        public BaseListRunWay Runway { get => _runway; set => _runway = value; }


        /// <summary>
        /// 用来保存外键表名与外键主键列名  通过这个打到对应的名称。
        /// </summary>
        public static ConcurrentDictionary<string, string> FKValueColNameTBList = new ConcurrentDictionary<string, string>();


        private Type entityType;
        /// <summary>
        /// 设置关联表名
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="exp">可为空用另一个两名方法无参的</param>
        public void SetBaseValue<T>(Expression<Func<T, string>> exp)
        {
            var mb = exp.GetMemberInfo();
            string key = mb.Name;
            entityType = typeof(T);
            string tableName = typeof(T).Name;
            foreach (var field in typeof(T).GetProperties())
            {
                //获取指定类型的自定义特性
                object[] attrs = field.GetCustomAttributes(false);
                foreach (var attr in attrs)
                {
                    if (attr is FKRelationAttribute)
                    {
                        FKRelationAttribute fkrattr = attr as FKRelationAttribute;
                        FKValueColNameTBList.TryAdd(fkrattr.FK_IDColName, fkrattr.FKTableName);
                    }
                }
            }

            //这里是不是与那个缓存 初始化时的那个字段重复？
            ///显示列表对应的中文
            FieldNameList = UIHelper.GetFieldNameColList(typeof(T));// UIHelper.GetFieldNameList<T>();

            //重构？
            dataGridView1.XmlFileName = tableName;
            Refreshs();



        }

        public void SetBaseValue<T>()
        {
            string tableName = typeof(T).Name;
            //foreach (var field in typeof(T).GetProperties())
            //{
            //    //获取指定类型的自定义特性
            //    object[] attrs = field.GetCustomAttributes(false);
            //    foreach (var attr in attrs)
            //    {
            //        if (attr is FKRelationAttribute)
            //        {
            //            FKRelationAttribute fkrattr = attr as FKRelationAttribute;
            //            FKValueColNameTableNameList.TryAdd(fkrattr.FK_ValueColName, fkrattr.FKTableName);
            //        }
            //    }
            //}

            //这里是不是与那个缓存 初始化时的那个字段重复？
            ///显示列表对应的中文
            FieldNameList = UIHelper.GetFieldNameColList(typeof(T));// UIHelper.GetFieldNameList<T>();

            //重构？
            dataGridView1.XmlFileName = tableName;
            Refreshs();



        }


        //三级 还是两级呢。  反向来 一是 KEY VALUE  然后是列名
        ConcurrentDictionary<string, List<KeyValuePair<object, string>>> _DataDictionary = new ConcurrentDictionary<string, List<KeyValuePair<object, string>>>();

        /// <summary>
        /// 固定的值显示，入库ture 出库false
        /// 每个列表对应的值 ，单独设置
        /// </summary>
        public ConcurrentDictionary<string, List<KeyValuePair<object, string>>> ColNameDataDictionary { get => _DataDictionary; set => _DataDictionary = value; }

     
        public BaseList()
        {
            InitializeComponent();
            this.BaseToolStrip.ItemClicked += ToolStrip1_ItemClicked;
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

        protected Result ComPare<T>(T t, T s)
        {
            Result result = new Result();
            var comparer = new ObjectsComparer.Comparer<T>();
            IEnumerable<ObjectsComparer.Difference> differences;
            bool isEqual = comparer.Compare(t, s, out differences);
            result.IsEqual = isEqual;
            if (!isEqual)
            {
                string differencesMsg = string.Join(Environment.NewLine, differences);
                result.Msg = differencesMsg;
            }
            return result;
        }

        public class Result
        {
            public bool IsEqual { get; set; }
            public string Msg { get; set; }

        }

        private void BindingSourceList_ListChanged(object sender, ListChangedEventArgs e)
        {
            BaseEntity entity = new BaseEntity();
            switch (e.ListChangedType)
            {
                case ListChangedType.Reset:
                    break;
                case ListChangedType.ItemAdded:
                    //如果这里为空出错， 需要先查询一个空的。绑定一下数据源的类型。之前是默认查询了所有
                    entity = bindingSourceList.List[e.NewIndex] as BaseEntity;
                    entity.ActionStatus = ActionStatus.新增;
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

        /// <summary>
        /// 这个的作用在哪里？
        /// </summary>
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
            dataGridView1.FieldNameList = this.FieldNameList;
          
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
                //case MenuItemEnums.高级查询:
                //    toolStripButtonSave.Enabled = false;
                //    toolStripBtnAdvQuery.Visible = true;
                //    break;
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

        /*
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

        */

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
            if (e.ClickedItem.Text.Length>0)
            {
                DoButtonClick(EnumHelper.GetEnumByString<MenuItemEnums>(e.ClickedItem.Text));
            }

        }

        public delegate void AdvQueryShowPageHandler<E>();

        /// <summary>
        /// 实现高级查询 有两步1）实际事件显示窗体，2）完成查询结果的显示 
        /// 事件中调用基类方法 AdvStartQuery AdvStartQuery<tb_UserInfoQueryDto>(dto);
        /// </summary>
        [Browsable(true), Description("外部高级查询事件，为了显示查询窗体页")]
        public event AdvQueryShowPageHandler<BaseEntityDto> AdvQueryShowPageEvent;

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
                //case MenuItemEnums.高级查询:
                //    AdvQuery();
                //    break;
                case MenuItemEnums.关闭:
                    Exit(this);
                    break;
                case MenuItemEnums.刷新:
                    break;
                case MenuItemEnums.导出:
                    break;
                case MenuItemEnums.选中:
                    Selected();
                    break;
                default:
                    break;
            }

            
        }


        #region 定义所有工具栏的方法

        protected void Add()
        {
            object frm = Activator.CreateInstance(EditForm);
            BaseEdit frmadd = frm as BaseEdit;

            //  UCLocationTypeEdit frmadd = new UCLocationTypeEdit();
            frmadd.bindingSourceEdit = bindingSourceList;
            object obj = frmadd.bindingSourceEdit.AddNew();
            frmadd.BindData(obj as BaseEntity);
            //RevertCommand command = new RevertCommand();
            ///*
            //* 使用匿名委托，更加简单，而且匿名委托方法里还可以使用外部变量。
            //*/
            //command.UndoOperation = delegate ()
            //{
            //    //Undo操作会执行到的代码
            //    frmadd.bindingSourceEdit.Remove(obj);
            //};

            if (frmadd.ShowDialog() == DialogResult.OK)
            {
                ToolBarEnabledControl(MenuItemEnums.新增);
            }
            else
            {
                frmadd.bindingSourceEdit.CancelEdit();
                //command.Undo();
            }
        }

        protected virtual void Delete()
        {

        }

        protected virtual void Modify()
        {

        }
        protected virtual void Selected()
        {
            if (bindingSourceList.Current != null)
            {
                //bindingSourceList.Current 
                if (!Edited)
                {
                    //退出
                    Form frm = (this as Control).Parent.Parent as Form;
                    frm.DialogResult = DialogResult.OK;
                    frm.Close();
                }
                else
                {
                    if (MessageBox.Show(this, "有数据没有保存\r\n你确定要退出吗", "询问", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                    {
                        //退出
                        Form frm = (this as Control).Parent.Parent as Form;
                        frm.DialogResult = DialogResult.OK;
                        frm.Close();
                    }
                }
            }
        }

        protected virtual void Modify<T>(BaseEdit frmadd)
        {
            if (bindingSourceList.Current != null)
            {
                RevertCommand command = new RevertCommand();
                frmadd.bindingSourceEdit = bindingSourceList;
                T CurrencyObj = (T)bindingSourceList.Current;
                BaseEntity bty = CurrencyObj as BaseEntity;
                bty.ActionStatus = ActionStatus.加载;
                frmadd.BindData(bty);
                //缓存当前编辑的对象。如果撤销就回原来的值
                T oldobj = CloneHelper.DeepCloneObject<T>((T)bindingSourceList.Current);
                int UpdateIndex = bindingSourceList.Position;
                command.UndoOperation = delegate ()
                {
                    //Undo操作会执行到的代码
                    CloneHelper.SetValues<T>(CurrencyObj, oldobj);
                };
                if (frmadd.ShowDialog() == DialogResult.Cancel)
                {
                    //修改时取消了
                    command.Undo();
                }
                else
                {
                    //if (CurrencyObj.HasChanged)
                    //{
                    //    CurrencyObj.actionStatus = ActionStatus.修改;
                    //    ToolBarEnabledControl(MenuItemEnums.修改);
                    //}

                    //确定了
                    if (oldobj is tb_Prod)
                    {
                        #region
                        var _comparer = new ObjectsComparer.Comparer<T>(
                          new ComparisonSettings
                          {
                              //Null and empty error lists are equal
                              EmptyAndNullEnumerablesEqual = true
                          });

                        _comparer.AddComparerOverride(() => new tb_Prod().tb_Prod_Attr_Relations, DoNotCompareValueComparer.Instance);

                        IEnumerable<Difference> differences;
                        var isEqual = _comparer.Compare(oldobj, CurrencyObj, out differences);
                        tb_Prod be = CurrencyObj as tb_Prod;
                        if (isEqual)
                        {

                            #region 
                            foreach (var item in be.tb_Prod_Attr_Relations)
                            {
                                if (item.ActionStatus == ActionStatus.修改)
                                {
                                    be.ActionStatus = ActionStatus.修改;
                                    ToolBarEnabledControl(MenuItemEnums.修改);
                                    break;
                                }
                            }

                            #endregion

                        }
                        else
                        {
                            if (be.ActionStatus == ActionStatus.无操作 || be.ActionStatus == ActionStatus.加载)
                            {
                                be.ActionStatus = ActionStatus.修改;
                            }
                            if (be.ActionStatus == ActionStatus.修改)
                            {
                                ToolBarEnabledControl(MenuItemEnums.修改);
                            }
                        }
                        #endregion
                    }
                    else
                    {
                        ComPareResult result = UITools.ComPare(oldobj, CurrencyObj);
                        if (!result.IsEqual)
                        {
                            BaseEntity be = CurrencyObj as BaseEntity;
                            be.ActionStatus = ActionStatus.修改;
                            ToolBarEnabledControl(MenuItemEnums.修改);
                        }

                    }

                }

            }
            dataGridView1.Refresh();
        }



        protected void Modify_backup<T>(BaseEdit frmadd)
        {
            if (bindingSourceList.Current != null)
            {
                RevertCommand command = new RevertCommand();
                //UCLocationTypeEdit frmadd = new UCLocationTypeEdit();
                frmadd.bindingSourceEdit = bindingSourceList;
                BaseEntity CurrencyObj = bindingSourceList.Current as BaseEntity;
                CurrencyObj.ActionStatus = ActionStatus.修改;
                frmadd.BindData(CurrencyObj);
                //缓存当前编辑的对象。如果撤销就回原来的值
                // object obj = (base.bindingSourceList.Current as tb_LocationType).Clone();
                //tb_LocationType oldobj = CloneHelper.DeepCloneObject<tb_LocationType>(bindingSourceList.Current as tb_LocationType);
                BaseEntity oldobj = CloneHelper.DeepCloneObject<BaseEntity>(bindingSourceList.Current as BaseEntity);
                int UpdateIndex = bindingSourceList.Position;
                command.UndoOperation = delegate ()
                {
                    //Undo操作会执行到的代码
                    //                    CloneHelper.SetValues<tb_LocationType>(base.bindingSourceList[UpdateIndex], oldobj);
                    CloneHelper.SetValues<T>(CurrencyObj, oldobj);
                };
                if (frmadd.ShowDialog() == DialogResult.Cancel)
                {
                    //修改时取消了
                    command.Undo();
                }
                else
                {
                    //if (CurrencyObj.HasChanged)
                    //{
                    //    CurrencyObj.actionStatus = ActionStatus.修改;
                    //    ToolBarEnabledControl(MenuItemEnums.修改);
                    //}

                    //确定了
                    ComPareResult result = UITools.ComPare(oldobj, CurrencyObj);
                    if (!result.IsEqual)
                    {
                        if (CurrencyObj.ActionStatus == ActionStatus.无操作)
                        {
                            CurrencyObj.ActionStatus = ActionStatus.修改;
                            ToolBarEnabledControl(MenuItemEnums.修改);
                        }
                    }

                }
                dataGridView1.Refresh();
            }
        }


        /// <summary>
        /// 与高级查询执行结果公共使用，如果null时，则执行普通查询？
        /// </summary>
        /// <param name="dto"></param>
        [MustOverride]
        protected virtual void Query()
        {

        }

        public Type GetData<T>(object obj)
        {
            return (T)obj as Type;
        }

        protected  virtual void Save()
        {
            //foreach (var item in bindingSourceList.List)
            //{

            //    System.Reflection.MethodInfo mi = this.GetType().GetMethod("GetData").MakeGenericMethod(new Type[] { entityType });
            //    var entity = mi.Invoke(this, null);
            //    //var entity = item as tb_Location;

            //    //switch (entity.actionStatus)
            //    //{
            //    //    case ActionStatus.无操作:
            //    //        break;
            //    //    case ActionStatus.新增:
            //    //    case ActionStatus.修改:
            //    //        //await Startup.GetFromFac<tb_LocationController>().SaveOrUpdate < entityType.GetType() > (entity);
            //    //        break;
            //    //    case ActionStatus.删除:
            //    //        break;
            //    //    default:
            //    //        break;
            //    //}

            //}
            ////base.ToolBarEnabledControl(MenuItemEnums.保存);
        }


      


      

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
            return base.ProcessCmdKey(ref msg, keyData);

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
                if (MessageBox.Show(this, "有数据没有保存\r\n你确定要退出吗?   这里是不是可以进一步提示 哪些内容没有保存？", "询问", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
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


        private void CloseTheForm(object thisform)
        {
            KryptonWorkspaceCell cell = MainForm.Instance.kryptonDockableWorkspace1.ActiveCell;
            if (cell == null)
            {
                cell = new KryptonWorkspaceCell();
                MainForm.Instance.kryptonDockableWorkspace1.Root.Children.Add(cell);
            }
            if ((thisform as Control).Parent is KryptonPage)
            {
                KryptonPage page = (thisform as Control).Parent as KryptonPage;
                MainForm.Instance.kryptonDockingManager1.RemovePage(page.UniqueName, true);
                page.Dispose();
            }
            else
            {
                Form frm = (thisform as Control).Parent.Parent as Form;
                frm.Close();
            }
          

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

            if (Runway == BaseListRunWay.选中模式)
            {
                tsbtnSelected.Visible = true;
                Selected();
            }
            else
            {
                tsbtnSelected.Visible = false;
                Modify();
            }

        }

  

        private void BaseList_Load(object sender, EventArgs e)
        {
            if (Runway == BaseListRunWay.选中模式)
            {
                tsbtnSelected.Visible = true;
            }
            else
            {
                tsbtnSelected.Visible = false;
            }
        }
    }
}
