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
using RUINORERP.Common.CollectionExtension;
using RUINORERP.UI.AdvancedUIModule;
using RUINORERP.Business.Processor;
using System.Reflection;
using Microsoft.Extensions.Logging;
using OfficeOpenXml;
using System.IO;
using TransInstruction;
using RUINORERP.UI.SuperSocketClient;
using RUINORERP.Model.Models;
using System.Diagnostics;
using RUINORERP.Global.Model;

namespace RUINORERP.UI.BaseForm
{

    /// <summary>
    /// 基本资料的列表，是否需要加一个标记来表示 在菜单中编辑 ，还是在 其他窗体时 关联编辑。看后面的业务情况。
    /// </summary>
    [PreCheckMustOverrideBaseClass]
    public partial class BaseListGeneric<T> : BaseUControl where T : class
    {
        /// <summary>
        /// 用来保存外键表名与外键主键列名  通过这个打到对应的名称。
        /// </summary>
        public static ConcurrentDictionary<string, string> FKValueColNameTBList = new ConcurrentDictionary<string, string>();


        /// <summary>
        /// 要统计的列
        /// </summary>
        public List<Expression<Func<T, object>>> SummaryCols { get; set; } = new List<Expression<Func<T, object>>>();

        /// <summary>
        /// 保存要总计的列
        /// </summary>
        private List<string> SummaryStrCols { get; set; } = new List<string>();

        /// <summary>
        /// 设置关联表名
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="exp">可为空用另一个两名方法无参的</param>
        private void InitBaseValue()
        {
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



            //这里不这样了，直接用登陆时查出来的。按菜单路径找到菜单 去再搜索 字段。
            //    显示按钮也一样的思路
            this.dataGridView1.FieldNameList = UIHelper.GetFieldNameColList(typeof(T));






            /*
            tb_MenuInfo menuInfo = MainForm.Instance.AppContext.CurUserInfo.UserModList..FirstOrDefault(m => m.ClassPath == this.GetType().FullName);
            if (menuInfo != null)
            {
                var fields = MainForm.Instance.AppContext.CurUserInfo.UserFieldList.Where(f => f.MenuID == menuInfo.MenuID).ToList();
                foreach (var field in fields)
                {
                    //这里权限控制了一下
                    if (!field.IsEnabled)
                    {
                        continue;
                    }
                    KeyValuePair<string, bool> kv;
                    if (this.dataGridView1.FieldNameList.TryGetValue(field.FieldName, out kv) && !field.IsEnabled)
                    {
                        //存在但不可用就是无权限   
                        this.dataGridView1.FieldNameList.TryRemove(field.FieldName, out kv);
                    }

                }
            }
            */


            //重构？
            dataGridView1.XmlFileName = tableName;

            // Refreshs();
        }


        //三级 还是两级呢。  反向来 一是 KEY VALUE  然后是列名
        ConcurrentDictionary<string, List<KeyValuePair<object, string>>> _DataDictionary = new ConcurrentDictionary<string, List<KeyValuePair<object, string>>>();

        /// <summary>
        /// 固定的值显示，入库ture 出库false
        /// 每个列表对应的值 ，单独设置
        /// </summary>
        public ConcurrentDictionary<string, List<KeyValuePair<object, string>>> ColNameDataDictionary { get => _DataDictionary; set => _DataDictionary = value; }


        /// <summary>
        /// 外键关联点，意思是：单位换算表中的两个单位字段不等于原始单位表中的主键字段，这里手动关联指向一下
        /// </summary>
        public ConcurrentDictionary<string, string> ForeignkeyPoints { get; set; } = new ConcurrentDictionary<string, string>();

        /// <summary>
        /// 手动添加外键关联指向
        /// </summary>
        /// <typeparam name="T1">单位表</typeparam>
        /// <typeparam name="T2">单位换算，产品等引用单位的表，字段和主键不一样时使用</typeparam>
        /// <param name="SourceField"></param>
        /// <param name="TargetField"></param>
        public void SetForeignkeyPointsList<T1, T2>(Expression<Func<T1, object>> SourceField, Expression<Func<T2, object>> TargetField)
        {
            MemberInfo Sourceinfo = SourceField.GetMemberInfo();
            string sourceName = Sourceinfo.Name;
            MemberInfo Targetinfo = TargetField.GetMemberInfo();
            string TargetName = Targetinfo.Name;
            if (ForeignkeyPoints == null)
            {
                ForeignkeyPoints = new ConcurrentDictionary<string, string>();
            }
            //以目标为主键，原始的相同的只能为值
            ForeignkeyPoints.TryAdd(TargetName, sourceName);
        }

        /// <summary>
        /// 通过这个类型取到显示的列的中文名
        /// 视图可能来自多个表的内容，所以显示不一样
        /// </summary>
        public List<Type> ColDisplayTypes { get; set; } = new List<Type>();


        private void DataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
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

            string colDbName = dataGridView1.Columns[e.ColumnIndex].Name;
            if (ForeignkeyPoints != null && ForeignkeyPoints.Count > 0)
            {
                if (ForeignkeyPoints.Keys.Contains(colDbName))
                {
                    colDbName = ForeignkeyPoints[colDbName];
                }
            }

            //固定字典值显示
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
            string colName = string.Empty;
            if (ColDisplayTypes != null && ColDisplayTypes.Count > 0)
            {
                colName = UIHelper.ShowGridColumnsNameValue(ColDisplayTypes.ToArray(), colDbName, e.Value);
            }
            else
            {
                colName = UIHelper.ShowGridColumnsNameValue<T>(colDbName, e.Value);
            }
            if (!string.IsNullOrEmpty(colName))
            {
                e.Value = colName;
                return;
            }




            //图片特殊处理
            if (dataGridView1.Columns[e.ColumnIndex].Name == "Image")
            {
                if (e.Value != null)
                {
                    System.IO.MemoryStream buf = new System.IO.MemoryStream((byte[])e.Value);
                    Image image = Image.FromStream(buf, true);
                    e.Value = image;
                    //这里用缓存
                }
            }

            //处理创建人 修改人，因为这两个字段没有做外键。固定的所以可以统一处理

        }




        public BaseListGeneric()
        {
            InitializeComponent();
            if (System.ComponentModel.LicenseManager.UsageMode != System.ComponentModel.LicenseUsageMode.Designtime)
            {
                if (!this.DesignMode)
                {
                    GridRelated = new GridViewRelated();
                    ctr = Startup.GetFromFacByName<BaseController<T>>(typeof(T).Name + "Controller");
                    //权限菜单
                    if (CurMenuInfo == null || CurMenuInfo.ClassPath.IsNullOrEmpty())
                    {
                        CurMenuInfo = MainForm.Instance.MenuList.Where(m => m.IsVisble && m.EntityName == typeof(T).Name && m.ClassPath == this.ToString()).FirstOrDefault();
                        if (CurMenuInfo == null || CurMenuInfo.ClassPath.IsNullOrEmpty())
                        {
                            MessageBox.Show(this.ToString() + "A菜单不能为空，请联系管理员。");
                            return;
                        }
                    }


                    #region 绑定菜单事件

                    foreach (var item in BaseToolStrip.Items)
                    {
                        if (item is ToolStripButton btnItem)
                        {
                            ToolStripButton subItem = item as ToolStripButton;
                            subItem.Click += Item_Click;
                            ControlButton(subItem);

                        }
                        if (item is ToolStripDropDownButton subItemDr)
                        {
                            ControlButton(subItemDr);
                            subItemDr.Click += Item_Click;
                            //下一级
                            if (subItemDr.HasDropDownItems)
                            {
                                foreach (var sub in subItemDr.DropDownItems)
                                {
                                    ToolStripMenuItem subStripMenuItem = sub as ToolStripMenuItem;
                                    ControlButton(subStripMenuItem);
                                    subStripMenuItem.Click += Item_Click;
                                }

                            }
                        }


                    }

                    #endregion

                    InitBaseValue();

                    this.bindingSourceList.ListChanged += BindingSourceList_ListChanged;
                    InitListData();
                    ClassGenericType = typeof(T);
                    Builder();

                }
            }



        }

        public virtual void Builder()
        {
            QueryConditionBuilder();
            //默认展开查询条件
            if (QueryConditionFilter != null && QueryConditionFilter.QueryFields.Count > 0)
            {
                kryptonHeaderGroupTop.Collapsed = false;
            }
            else
            {
                kryptonHeaderGroupTop.Collapsed = true;
            }
            BuildSummaryCols();
            BuildInvisibleCols();
        }

        public virtual void BuildSummaryCols()
        {

        }

        public virtual void BuildInvisibleCols()
        {
        }

        
        public void ControlButton(ToolStripMenuItem btnItem)
        {
            if (!MainForm.Instance.AppContext.IsSuperUser)
            {
                if (CurMenuInfo.tb_P4Buttons == null)
                {
                    btnItem.Visible = false;
                }
                else
                {
                    //如果因为热键 Text改变了。到时再处理
                    tb_P4Button p4b = CurMenuInfo.tb_P4Buttons.Where(b => b.tb_buttoninfo.BtnText == btnItem.Text).FirstOrDefault();
                    if (p4b != null)
                    {
                        btnItem.Visible = p4b.IsVisble;
                        btnItem.Enabled = p4b.IsEnabled;
                    }
                    else
                    {
                        btnItem.Visible = false;
                    }
                }
            }
        }

        public void ControlButton(ToolStripDropDownButton btnItem)
        {
            if (!MainForm.Instance.AppContext.IsSuperUser)
            {
                if (CurMenuInfo.tb_P4Buttons == null)
                {
                    btnItem.Visible = false;
                }
                else
                {
                    //如果因为热键 Text改变了。到时再处理
                    tb_P4Button p4b = CurMenuInfo.tb_P4Buttons.Where(b => b.tb_buttoninfo.BtnText == btnItem.Text).FirstOrDefault();
                    if (p4b != null)
                    {
                        btnItem.Visible = p4b.IsVisble;
                        btnItem.Enabled = p4b.IsEnabled;
                    }
                    else
                    {
                        btnItem.Visible = false;
                    }
                }
            }
        }

        public void ControlButton(ToolStripButton btnItem)
        {
            if (!MainForm.Instance.AppContext.IsSuperUser)
            {
                if (CurMenuInfo.tb_P4Buttons == null)
                {
                    btnItem.Visible = false;
                }
                else
                {
                    //如果因为热键 Text改变了。到时再处理
                    tb_P4Button p4b = CurMenuInfo.tb_P4Buttons.Where(b => b.tb_buttoninfo.BtnText == btnItem.Text).FirstOrDefault();
                    if (p4b != null)
                    {
                        btnItem.Visible = p4b.IsVisble;
                        btnItem.Enabled = p4b.IsEnabled;
                    }
                    else
                    {
                        btnItem.Visible = false;
                    }
                }
            }
        }

 

        protected Result ComPare<C>(C t, C s)
        {
            Result result = new Result();
            var comparer = new ObjectsComparer.Comparer<C>();
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
        /// 不同情况，显示不同的可用情况
        /// </summary>
        internal void ToolBarEnabledControl(MenuItemEnums menu)
        {
            //ToolStripButton

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
                //    ControlButton(toolStripBtnAdvQuery);
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

        private void Item_Click(object sender, EventArgs e)
        {
            MainForm.Instance.AppContext.log.ActionName = sender.ToString();
            if (sender.ToString().Length > 0)
            {
                DoButtonClick(EnumHelper.GetEnumByString<MenuItemEnums>(sender.ToString()));
            }
            else
            {

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
        /// 有些要限制显示内容，如果销售人员看不到供应商。
        /// </summary>
       // public Expression<Func<T, bool>> LimitQueryConditions { get; set; }

        /// <summary>
        /// 如果需要查询条件查询，就要在子类中重写这个方法
        /// </summary>
        public virtual void LimitQueryConditionsBuilder()
        {

        }

        ///// <summary>
        ///// 关联的菜单信息 实际是可以从点击时传入
        ///// </summary>
        //public tb_MenuInfo CurMenuInfo { get; set; }


        /// <summary>
        /// 控制功能按钮
        /// </summary>
        /// <param name="p_Text"></param>
        protected virtual async void DoButtonClick(MenuItemEnums menuItem)
        {
            //操作前将数据收集
            this.ValidateChildren(System.Windows.Forms.ValidationConstraints.None);
            switch (menuItem)
            {
                case MenuItemEnums.新增:
                    Add();
                    break;
                case MenuItemEnums.复制性新增:
                    AddByCopy();
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
                   await Save();
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
                    ExportExcel();
                    break;
                case MenuItemEnums.选中:
                    Selected();
                    break;
                case MenuItemEnums.属性:
                    MenuPersonalizedSettings();
                    break;
                case MenuItemEnums.帮助:
                    SysHelp(CurMenuInfo.CaptionCN);
                    break;
                default:
                    break;
            }

            /*
            if (p_Text == "新增" || p_Text == "F9") Add();
            if (p_Text == "修改" || p_Text == "F10") Modify();
            if (p_Text == "删除" || p_Text == "Del") Delete();
            if (p_Text == "保存" || p_Text == "F12")
            {
                Save();
                //if (ReceiptCheck() == false) { return; }
                //ReceiptSave();
            }
            //if (p_Text == "放弃" || p_Text == "Escape") ReceiptCancel();
            //if (p_Text == "预览" || p_Text == "F6") ReceiptPreview();
            // if (p_Text == "高级查询") AdvancedQuery();
            // if (p_Text == "刷新") ReceiptRefresh();
            if (p_Text == "查询") Query();
            //if (p_Text == "首页" || p_Text == "Home") FirstPage();
            //if (p_Text == "上页" || p_Text == "PageUp") UpPage();
            //if (p_Text == "下页" || p_Text == "PageDown") DownPage();
            //if (p_Text == "末页" || p_Text == "End") LastPage(); 导出Excel
            //if (p_Text == "导出Excel") base.OutFastOfDataGridView(this.dataGridView1);

            if (p_Text == "关闭" || p_Text == "Esc" || p_Text == "退出") CloseTheForm();
            */
        }

        /// <summary>
        /// 帮助
        /// </summary>
        /// <param name="PageName">欢迎.htm</param>
        /// <param name="TagID">html标签a中的id或name</param>
        public virtual void SysHelp(string PageName = null, string TagID = null)
        {
            //hh.exe参数（全）

            /*
             hh.exe	-800	将Help viewer设为800*600
            -title	将chm以窗口800*600显示
            -register	注册hh.exe，将其设为默认的chm文档的shell
            -decompile	反编译chm文件，就是将chm拆散开来，对于破坏狂和翻译人员比较有用，懒人就免了
            -mapid	如果你记住chm中htm、html的id，那么用它定位htm、html文件
            -safe	迫使hh.exe以安全模式打开chm。安全模式？就是所有的快捷键都失效
            原文链接：https://blog.csdn.net/tuwen/article/details/3166696
             */
            // 指定 CHM 文件路径和要定位的页面及段落（这里只是示例，你需要根据实际情况设置）
            string chmFilePath = System.IO.Path.Combine(Application.StartupPath, "help.chm");
            //string targetPage = "基础资料.htm";
            //string targetParagraph1 = "包装信息.htm";
            //string targetParagraph2 = "包装信息\\卡通箱";
            //string targetParagraph3 = "卡通箱.htm";

            // 使用 HH.exe 来打开 CHM 文件并指定定位
            try
            {
                if (PageName.IsNullOrEmpty())
                {
                    Process.Start("hh.exe", $"{chmFilePath}");
                }
                else
                {
                    //Process.Start("hh.exe", $"\"{chmFilePath}\"::{targetPage}#{targetParagraph1}");

                    PageName += ".htm";//必须带.htm后缀
                    if (TagID.IsNullOrEmpty())
                    {
                        Process.Start("hh.exe", $"{chmFilePath}::{PageName}");
                    }
                    else
                    {
                        Process.Start("hh.exe", $"{chmFilePath}::{PageName}#{TagID}");
                    }
                }
                //测试ok
                //Process.Start("hh.exe", "E:\\CodeRepository\\SynologyDrive\\RUINORERP\\RUINORERP.UI\\bin\\Debug\\help.chm::欢迎.htm");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"打开 CHM 文件出错: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        #region 定义所有工具栏的方法
        public virtual void ExportExcel()
        {
            //var EditEntitys = ListDataSoure.DataSource as List<T>;
            //if (EditEntitys == null || EditEntitys.Count == 0)
            //{
            //    return false;
            //}
            UIExcelHelper.ExportExcel(dataGridView1);
        }

        protected void AddByCopy()
        {
            object frm = Activator.CreateInstance(EditForm);
            if (frm.GetType().BaseType.Name.Contains("BaseEditGeneric"))
            {
                T selectItem = null;
                #region 复制性新增就是把当前的值复制到新值中
                //先取当前选中的
                if (bindingSourceList.Current != null)
                {
                    selectItem = (T)bindingSourceList.Current;
                }
                #endregion
                #region 泛型情况

                BaseEditGeneric<T> frmadd = frm as BaseEditGeneric<T>;
                frmadd.bindingSourceEdit = bindingSourceList;
                object NewObj = frmadd.bindingSourceEdit.AddNew();
                frmadd.BindData(NewObj as BaseEntity);

                //复制性 的就是把原有值除主键全部复制过去。
                FastCopy<T, T>.Copy(selectItem as T, NewObj as T, true);
                //设置主键为0才会新增，这里代码顺序不能反
                string PKCol = BaseUIHelper.GetEntityPrimaryKey<T>();
                ReflectionHelper.SetPropertyValue(NewObj, PKCol, 0);

                if (frmadd.ShowDialog() == DialogResult.OK)
                {
                    ToolBarEnabledControl(MenuItemEnums.新增);
                }
                else
                {
                    frmadd.bindingSourceEdit.CancelEdit();
                }
                #endregion
            }
            else
            {
                #region 普通情况
                BaseEdit frmadd = frm as BaseEdit;
                frmadd.bindingSourceEdit = bindingSourceList;
                object obj = frmadd.bindingSourceEdit.AddNew();
                frmadd.BindData(obj as BaseEntity);
                if (frmadd.ShowDialog() == DialogResult.OK)
                {
                    ToolBarEnabledControl(MenuItemEnums.新增);
                }
                else
                {
                    frmadd.bindingSourceEdit.CancelEdit();
                }
                #endregion
            }


        }

        protected virtual void Add()
        {
            object frm = Activator.CreateInstance(EditForm);
            if (frm.GetType().BaseType.Name.Contains("BaseEditGeneric"))
            {
                #region 泛型情况
                BaseEditGeneric<T> frmadd = frm as BaseEditGeneric<T>;
                frmadd.Text = this.CurMenuInfo.CaptionCN + "新增";
                frmadd.bindingSourceEdit = bindingSourceList;
                object obj = frmadd.bindingSourceEdit.AddNew();
                //ctr.InitEntity(obj);
                BusinessHelper.Instance.InitEntity(obj);
                //如果obj转基类为空 ，原因是 载入时没有查询出一个默认的框架出来
                frmadd.BindData(obj as BaseEntity);
                if (frmadd.ShowDialog() == DialogResult.OK)
                {
                    ToolBarEnabledControl(MenuItemEnums.新增);
                }
                else
                {
                    frmadd.bindingSourceEdit.CancelEdit();
                }
                #endregion
            }
            else
            {
                #region 普通情况
                BaseEdit frmadd = frm as BaseEdit;
                frmadd.Text = this.CurMenuInfo.CaptionCN + "新增";
                frmadd.bindingSourceEdit = bindingSourceList;
                object obj = frmadd.bindingSourceEdit.AddNew();
                frmadd.BindData(obj as BaseEntity);
                if (frmadd.ShowDialog() == DialogResult.OK)
                {
                    ToolBarEnabledControl(MenuItemEnums.新增);
                }
                else
                {
                    frmadd.bindingSourceEdit.CancelEdit();
                }
                #endregion
            }


        }


        /// <summary>
        /// 注意这里是物理删除
        /// </summary>
        protected async virtual void Delete()
        {
            if (MessageBox.Show("系统不建议删除基本资料\r\n确定删除吗？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
            {
                T loc = (T)this.bindingSourceList.Current;
                object PKValue = this.bindingSourceList.Current.GetPropertyValue(UIHelper.GetPrimaryKeyColName(typeof(T)));
                this.bindingSourceList.Remove(loc);
                bool rs = await ctr.BaseDeleteAsync(loc);
                if (rs)
                {
                    if (MainForm.Instance.AppContext.SysConfig.IsDebug)
                    {
                        MainForm.Instance.logger.LogInformation($"删除:{typeof(T).Name}，主键值：{PKValue.ToString()} ");
                    }
                    AuditLogHelper.Instance.CreateAuditLog("删除", CurMenuInfo.CaptionCN);
                    //提示服务器开启推送工作流
                    OriginalData beatDataDel = ClientDataBuilder.BaseInfoChangeBuilder(typeof(T).Name);
                    MainForm.Instance.ecs.AddSendData(beatDataDel);
                }
            }

        }

        protected virtual void Modify()
        {
            if (EditForm == null)
            {
                return;
            }

            object frm = Activator.CreateInstance(EditForm);
            if (frm.GetType().BaseType.Name.Contains("BaseEditGeneric"))
            {
                BaseEditGeneric<T> frmaddg = frm as BaseEditGeneric<T>;
                frmaddg.Text = this.CurMenuInfo.CaptionCN + "编辑";
                Modify(frmaddg);
            }
            else
            {
                BaseEdit frmadd = frm as BaseEdit;
                frmadd.Text = this.CurMenuInfo.CaptionCN + "编辑";
                Modify(frmadd);
            }
        }
        protected virtual void Selected()
        {
            if (bindingSourceList.Current != null)
            {
                //将选中的值保存到这里，用在 复杂编辑UI时 编辑外键的其他资料
                base.Tag = bindingSourceList.Current;
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
                        frm.DialogResult = DialogResult.Cancel;
                        frm.Close();
                    }
                }
            }
        }

        protected virtual void MenuPersonalizedSettings()
        {
            UserCenter.frmMenuPersonalization frmMenu = new UserCenter.frmMenuPersonalization();
            frmMenu.MenuPathKey = CurMenuInfo.ClassPath;
            if (frmMenu.ShowDialog() == DialogResult.OK)
            {
                LoadQueryConditionToUI(frmMenu.QueryShowColQty.Value);
            }
        }
        protected virtual void Modify(BaseEdit frmadd)
        {
            if (bindingSourceList.Current != null)
            {
                Command command = new Command();
                frmadd.bindingSourceEdit = bindingSourceList;
                T CurrencyObj = (T)bindingSourceList.Current;
                BaseEntity bty = CurrencyObj as BaseEntity;
                bty.ActionStatus = ActionStatus.加载;
                //ctr.EditEntity(bty);
                BusinessHelper.Instance.EditEntity(bty);
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

        protected virtual void Modify<T>(BaseEditGeneric<T> frmadd) where T : class
        {
            if (bindingSourceList.Current != null)
            {
                Command command = new Command();
                frmadd.bindingSourceEdit = bindingSourceList;
                T CurrencyObj = (T)bindingSourceList.Current;
                BaseEntity bty = CurrencyObj as BaseEntity;
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
                        BaseEntity be = CurrencyObj as BaseEntity;
                        if (!result.IsEqual || be.ActionStatus == ActionStatus.修改)
                        {
                            //基础资料编辑时，只要有修改就变更修改时间和人
                            BusinessHelper.Instance.EditEntity(be);
                            be.ActionStatus = ActionStatus.修改;
                            ToolBarEnabledControl(MenuItemEnums.修改);
                        }

                    }

                }

            }
            dataGridView1.Refresh();
        }

        /*
        protected void Modify_backup<T>(BaseEdit frmadd)
        {
            if (bindingSourceList.Current != null)
            {
                Command command = new Command();
                //UCLocationTypeEdit frmadd = new UCLocationTypeEdit();
                frmadd.bindingSourceEdit = bindingSourceList;
                BaseEntity CurrencyObj = bindingSourceList.Current as BaseEntity;
                CurrencyObj.actionStatus = ActionStatus.修改;
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
                        if (CurrencyObj.actionStatus == ActionStatus.无操作)
                        {
                            CurrencyObj.actionStatus = ActionStatus.修改;
                            ToolBarEnabledControl(MenuItemEnums.修改);
                        }
                    }

                }
                dataGridView1.Refresh();
            }
        }
        */



        KryptonPage AdvPage = null;


        /*
        /// <summary>
        /// 显示查询页过时 作废了
        /// 初步判断 没有使用了。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dto"></param>
        protected virtual void AdvStartQueryPage<T>(BaseEntityDto dto) where T : class
        {
            if (Edited)
            {
                if (MessageBox.Show("你有数据没有保存，当前操作会丢失数据\r\n你确定不保存吗？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.No)
                {
                    return;
                }
            }

            AdvancedUIModule.UCAdvQuery<T> ucAdv = new AdvancedUIModule.UCAdvQuery<T>();
            ucAdv.AdvQueryEvent += UcAdv_QueryEvent;

            int pageW = 750;
            int pageH = 600;

            int left = 0;
            int right = 0;
            if (AdvPage != null)
            {            //计算位于父窗体的中心位置 
                left = this.Location.X + (this.Width / 2) - (AdvPage.Width / 2);
                right = this.Location.Y + (this.Height / 2) - (AdvPage.Height / 2);

                if (MainForm.Instance.kryptonDockingManager1.Pages.Contains(AdvPage))
                {
                    AdvPage.Location = new Point(left, right);
                    AdvPage.Show();
                    return;
                }
            }
            string PageTitle = "高级查询";
            if ((this as Control).Parent is KryptonPage)
            {
                KryptonPage page = (this as Control).Parent as KryptonPage;
                PageTitle = page.Text + "-" + PageTitle;
            }
            AdvPage = MainForm.Instance.NewPage(PageTitle, 1, ucAdv);
            AdvPage.AllowDrop = false;
            left = this.Location.X + (this.Width / 2) - (pageW / 2);
            right = this.Location.Y + (this.Height / 2) - (pageH / 2);
            right += 100;//往下来一点
            //  kp.ClearFlags(KryptonPageFlags.All);
            AdvPage.ClearFlags(KryptonPageFlags.DockingAllowAutoHidden | KryptonPageFlags.DockingAllowClose);

            MainForm.Instance.kryptonDockingManager1.AddFloatingWindow("Floating", new KryptonPage[] { AdvPage },
             new Point(left, right), new Size(pageW, pageH));
        }

        */
        ///// <summary>
        ///// 执行高级查询的结果
        ///// </summary>
        ///// <typeparam name="T"></typeparam>
        ///// <param name="dto"></param>
        //private void UcAdv_QueryEvent<T>(BaseEntityDto dto)
        //{
        //    AdvQueryShowResult(dto);
        //}

        /// <summary>
        /// 执行高级查询的结果
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dto"></param>
        private void UcAdv_QueryEvent(bool useLike, BaseEntityDto dto)
        {
            AdvQueryShowResult(useLike, dto);
        }


        /// <summary>
        /// 设置选中模式
        /// </summary>
        public override void SetSelect()
        {
            tsbtnSelected.Visible = true;
        }



        public virtual void QueryConditionBuilder()
        {
            BaseProcessor baseProcessor = Startup.GetFromFacByName<BaseProcessor>(typeof(T).Name + "Processor");
            QueryConditionFilter = baseProcessor.GetQueryFilter();
        }

        /// <summary>
        /// 扩展带条件查询
        /// </summary>
        protected async virtual void ExtendedQuery()
        {
            if (ValidationHelper.hasValidationErrors(this.Controls))
                return;

            dataGridView1.ReadOnly = true;

            //既然前台指定的查询哪些字段，到时可以配置。这里应该是 除软件删除外的。其他字段不需要

            int pageNum = 1;
            int pageSize = int.Parse(txtMaxRows.Text);

            //提取指定的列名，即条件集合
            // List<string> queryConditions = new List<string>();
            //queryConditions = new List<string>(QueryConditionFilter.QueryFields.Select(t => t.FieldName).ToList());
            //list = await ctr.BaseQueryByAdvancedNavWithConditionsAsync(true, queryConditions, QueryConditionFilter.GetFilterExpression<T>(), QueryDto, pageNum, pageSize) as List<T>;
            if (QueryConditionFilter.FilterLimitExpressions == null)
            {
                QueryConditionFilter.FilterLimitExpressions = new List<LambdaExpression>();
            }
  
            List<T> list = await ctr.BaseQuerySimpleByAdvancedNavWithConditionsAsync(true, QueryConditionFilter, QueryDto, pageNum, pageSize) as List<T>;

            List<string> masterlist = ExpressionHelper.ExpressionListToStringList(SummaryCols);
            if (masterlist.Count > 0)
            {
                dataGridView1.IsShowSumRow = true;
                dataGridView1.SumColumns = masterlist.ToArray();
            }

            ListDataSoure.DataSource = list.ToBindingSortCollection();//这句是否能集成到上一层生成
            dataGridView1.DataSource = ListDataSoure;

            ToolBarEnabledControl(MenuItemEnums.查询);
        }




        private BaseEntity _queryDto = new BaseEntity();

        /// <summary>
        /// 查询条件保存值的对象实体
        /// </summary>
        public BaseEntity QueryDto { get => _queryDto; set => _queryDto = value; }

        private QueryFilter _QueryConditionFilter = new QueryFilter();

        /// <summary>
        /// 查询条件  将来 把querypara优化掉
        /// </summary>
        public QueryFilter QueryConditionFilter { get => _QueryConditionFilter; set => _QueryConditionFilter = value; }

        /// <summary>
        /// 默认不是模糊查询
        /// </summary>
        /// <param name="useLike"></param>
        public void LoadQueryConditionToUI(decimal QueryConditionShowColQty)
        {
            //为了验证设置的属性
            this.AutoValidate = AutoValidate.EnableAllowFocusChange;
            //UIQueryHelper<T> uIQueryHelper = new UIQueryHelper<T>();
            kryptonPanel条件生成容器.GetType().GetProperty("DoubleBuffered", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic).SetValue(kryptonPanel条件生成容器, true, null);
            kryptonPanel条件生成容器.Visible = false;
            kryptonPanel条件生成容器.Controls.Clear();
            kryptonPanel条件生成容器.SuspendLayout();
            //暂时默认了uselike

            //            QueryDto = uIQueryHelper.SetQueryUI(true, kryptonPanel条件生成容器, QueryConditionFilter, QueryConditionShowColQty);
            QueryDto = UIGenerateHelper.CreateQueryUI(typeof(T), true, kryptonPanel条件生成容器, QueryConditionFilter, QueryConditionShowColQty);

            kryptonPanel条件生成容器.ResumeLayout();
            kryptonPanel条件生成容器.Visible = true;
            InvisibleCols = ExpressionHelper.ExpressionListToStringList(InvisibleColsExp);
            ControlMasterColumnsInvisible(InvisibleCols);
            foreach (var item in InvisibleCols)
            {
                KeyValuePair<string, bool> kv = new KeyValuePair<string, bool>();
                dataGridView1.FieldNameList.TryRemove(item, out kv);
            }


            List<T> list = new List<T>();
            bindingSourceList.DataSource = list.ToBindingSortCollection();//这句是否能集成到上一层生成
            dataGridView1.DataSource = bindingSourceList;



        }


        /// <summary>
        /// 保存不可见的列
        /// </summary>
        public List<Expression<Func<T, object>>> InvisibleColsExp { get; set; } = new List<Expression<Func<T, object>>>();

        /// <summary>
        /// 保存不可见的列
        /// </summary>
        public List<string> InvisibleCols { get; set; } = new List<string>();



        /// <summary>
        /// 与高级查询执行结果公共使用，如果null时，则执行普通查询？
        /// </summary>
        /// <param name="dto"></param>
            //[MustOverride]
        public async override void Query()
        {
            if (Edited)
            {
                if (MessageBox.Show("你有数据没有保存，当前操作会丢失数据\r\n你确定不保存吗？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.No)
                {
                    return;
                }
            }


            if (QueryConditionFilter == null || QueryConditionFilter.QueryFields == null || QueryConditionFilter.QueryFields.Count == 0)
            {
                dataGridView1.ReadOnly = true;

                //两种条件组合为一起，一种是process中要处理器中设置好的，另一个是UI中 灵活设置的
                Expression<Func<T, bool>> expression = QueryConditionFilter.GetFilterExpression<T>();
                List<T> list = await MainForm.Instance.AppContext.Db.Queryable<T>().WhereIF(expression != null, expression).ToListAsync();

                List<string> masterlist = ExpressionHelper.ExpressionListToStringList(SummaryCols);
                if (masterlist.Count > 0)
                {
                    dataGridView1.IsShowSumRow = true;
                    dataGridView1.SumColumns = masterlist.ToArray();
                }

                ListDataSoure.DataSource = list.ToBindingSortCollection();
                dataGridView1.DataSource = ListDataSoure;

            }
            else
            {
                ExtendedQuery();
            }
            ToolBarEnabledControl(MenuItemEnums.查询);

            #endregion
        }


        /// <summary>
        /// 控制字段是否显示，添加到里面的是不显示的
        /// </summary>
        /// <param name="InvisibleCols"></param>
        public void ControlMasterColumnsInvisible(List<string> InvisibleCols)
        {
            if (!MainForm.Instance.AppContext.IsSuperUser)
            {
                if (CurMenuInfo.tb_P4Fields != null)
                {
                    foreach (var item in CurMenuInfo.tb_P4Fields)
                    {
                        if (item != null)
                        {
                            if (item.tb_fieldinfo != null)
                            {
                                if (!item.IsVisble && !item.tb_fieldinfo.IsChild && !InvisibleCols.Contains(item.tb_fieldinfo.FieldName))
                                {
                                    InvisibleCols.Add(item.tb_fieldinfo.FieldName);
                                }
                            }
                        }
                    }
                }
            }
        }

        private void UcAdv_OnSelectDataRow(T entity)
        {
            if (entity == null)
            {
                return;
            }
            bindingSourceList.Clear();
        }


        protected async void AdvQueryShowResult(bool useLike, BaseEntityDto dto)
        {
            dataGridView1.ReadOnly = true;
            IMapper mapper = AutoMapperConfig.RegisterMappings().CreateMapper();
            List<T> list = await ctr.BaseQueryByAdvancedNavAsync(useLike, dto);
            //ListDataSoure.DataSource = list.ToBindingSortCollection();//这句是否能集成到上一层生成
            //dataGridView1.DataSource = ListDataSoure;
            bindingSourceList.DataSource = list.ToBindingSortCollection();//这句是否能集成到上一层生成
            dataGridView1.DataSource = bindingSourceList;
            ToolBarEnabledControl(MenuItemEnums.查询);
        }



        protected BaseController<T> ctr;//= Startup.GetFromFacByName<BaseController<T>>(typeof(T).Name + "Controller");
        public async virtual Task<List<T>> Save()
        {
            List<T> list = new List<T>();
            //这里是否要用保存列表来处理
            foreach (var item in bindingSourceList.List)
            {
                var entity = item as BaseEntity;

                switch (entity.ActionStatus)
                {
                    case ActionStatus.无操作:
                        break;
                    case ActionStatus.新增:
                    case ActionStatus.修改:
                        if (MainForm.Instance.AppContext.SysConfig.IsDebug)
                        {
                            //MainForm.Instance.logger.LogInformation($"保存:{typeof(T).Name}");
                        }
                        ReturnResults<T> rr = new ReturnResults<T>();
                        rr = await ctr.BaseSaveOrUpdate(entity as T);
                        if (rr.Succeeded)
                        {
                            ToolBarEnabledControl(MenuItemEnums.保存);
                            //提示服务器开启推送工作流
                            OriginalData beatData = ClientDataBuilder.BaseInfoChangeBuilder(typeof(T).Name);
                            MainForm.Instance.ecs.AddSendData(beatData);
                            //审计日志
                            AuditLogHelper.Instance.CreateAuditLog("保存", CurMenuInfo.CaptionCN);
                            list.Add(rr.ReturnObject);
                        }
                        //tb_Unit Entity = await ctr.AddReEntityAsync(entity);
                        //如果新增 保存后。还是新增加状态，因为增加另一条。所以保存不为灰色。所以会重复增加
                        break;
                    case ActionStatus.删除:

                        break;
                    default:
                        break;
                }
                entity.HasChanged = false;
            }
            return list;
        }



        /*
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
                    case Keys.Enter:
                        Query();
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
     */
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
            LimitQueryConditionsBuilder();
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


        public GridViewRelated GridRelated { get; set; }

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
            if (e.ColumnIndex == -1 || e.RowIndex == -1)
            {
                return;
            }
            if (dataGridView1.CurrentRow != null && dataGridView1.CurrentCell != null)
            {
                if (dataGridView1.CurrentRow.DataBoundItem is RUINORERP.Model.BaseEntity entity)
                {
                    GridRelated.GuideToForm(dataGridView1.Columns[e.ColumnIndex].Name, entity);
                }
            }
        }

        private void dataGridView1_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {

        }

        private void BaseList_Load(object sender, EventArgs e)
        {
            if (System.ComponentModel.LicenseManager.UsageMode == System.ComponentModel.LicenseUsageMode.Designtime || this.DesignMode)
            {
                return;
            }
            //InitProcess();
            if (Runway == BaseListRunWay.选中模式)
            {
                tsbtnSelected.Visible = true;
            }
            else
            {
                tsbtnSelected.Visible = false;
            }
            #region
            //if (this.Parent is KryptonPage kp)
            //{
            //    List<tb_MenuInfo> menuInfos = MainForm.Instance.MenuList.Where(m => m.IsVisble && m.EntityName == typeof(tb_CustomerVendor).Name && m.ClassPath == this.ToString()).ToList();
            //    if (menuInfos == null)
            //    {
            //        MessageBox.Show(this.ToString() + "A菜单不能为空，请联系管理员。");
            //        return;
            //    }
            //    else
            //    {
            //        //这个共用了客户和供应商 所以特殊处理，会有两行
            //        CurMenuInfo = menuInfos.Where(m => m.CaptionCN.Contains(kp.Text)).FirstOrDefault();
            //    }

            //}

            #endregion

            if (!this.DesignMode)
            {
                MenuPersonalization personalization = new MenuPersonalization();
                UserGlobalConfig.Instance.MenuPersonalizationlist.TryGetValue(CurMenuInfo.ClassPath, out personalization);
                if (personalization != null)
                {
                    decimal QueryShowColQty = personalization.QueryConditionShowColsQty;
                    LoadQueryConditionToUI(QueryShowColQty);
                }
                else
                {
                    LoadQueryConditionToUI(4);
                }
            }
            //默认收起查询框
            Refreshs();

        }


        private void kryptonHeaderGroupTop_CollapsedChanged(object sender, EventArgs e)
        {

        }


    }
}
