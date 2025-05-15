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
using RUINORERP.UI.CommonUI;
using RUINORERP.UI.FormProperty;
using System.Web.UI;
using Control = System.Windows.Forms.Control;
using SqlSugar;
using SourceGrid.Cells.Models;
using RUINORERP.Business.CommService;
using SixLabors.ImageSharp.Memory;
using Netron.NetronLight;
using RUINOR.WinFormsUI.CustomPictureBox;
using RUINORERP.UI.UserCenter;
using RUINORERP.UI.UserPersonalized;
using RUINORERP.UI.UControls;
using Newtonsoft.Json;
using Fireasy.Common.Extensions;
using ContextMenuController = RUINORERP.UI.UControls.ContextMenuController;
using Netron.Automatology;



namespace RUINORERP.UI.BaseForm
{

    /// <summary>
    /// 基本资料的列表，是否需要加一个标记来表示 在菜单中编辑 ，还是在 其他窗体时 关联编辑。看后面的业务情况。
    /// </summary>
    [PreCheckMustOverrideBaseClass]
    public partial class BaseListGeneric<T> : BaseUControl, IContextMenuInfoAuth where T : class
    {

        //public virtual ToolStripItem[] AddExtendButton()
        //{
        //    //返回空的数组
        //    return new ToolStripItem[] { };
        //}
        public virtual List<ContextMenuController> AddContextMenu()
        {
            List<ContextMenuController> list = new List<ContextMenuController>();
            list.Add(new ContextMenuController("【批量处理】", true, false, "NewSumDataGridView_标记已打印"));
            return list;
        }

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

            dataGridView1.XmlFileName = tableName;

            dataGridView1.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            //这里设置了指定列不可见
            foreach (var item in InvisibleCols)
            {
                KeyValuePair<string, bool> kv = new KeyValuePair<string, bool>();
                dataGridView1.FieldNameList.TryRemove(item, out kv);
            }
            //这里设置指定列默认隐藏。可以手动配置显示
            foreach (var item in DefaultHideCols)
            {
                KeyValuePair<string, bool> kv = new KeyValuePair<string, bool>();
                dataGridView1.FieldNameList.TryRemove(item, out kv);
                KeyValuePair<string, bool> Newkv = new KeyValuePair<string, bool>(kv.Key, false);
                dataGridView1.FieldNameList.TryAdd(item, Newkv);
            }


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
        /// 手动设置的。优化级比较自动的FKValueColNameTBList高
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
            //图片特殊处理
            if (dataGridView1.Columns[e.ColumnIndex].Name == "Image" || e.Value.GetType().Name == "Byte[]")
            {
                if (e.Value != null)
                {
                    if (!(e.Value is byte[]))
                    {
                        return;
                    }
                    System.IO.MemoryStream buf = new System.IO.MemoryStream((byte[])e.Value);
                    System.Drawing.Image image = System.Drawing.Image.FromStream(buf, true);
                    if (image != null)
                    {
                        //缩略图 这里用缓存 ?
                        System.Drawing.Image thumbnailthumbnail = UITools.CreateThumbnail(image, 100, 100);
                        e.Value = thumbnailthumbnail;
                        return;
                    }
                }
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
                if (FKValueColNameTBList.Count > 0)
                {
                    foreach (var item in FKValueColNameTBList)
                    {
                        colName = UIHelper.ShowGridColumnsNameValue(item.Value, item.Key, e.Value);
                        if (!string.IsNullOrEmpty(colName))
                        {
                            break;
                        }
                    }
                }
                else
                {
                    colName = UIHelper.ShowGridColumnsNameValue<T>(colDbName, e.Value);
                }

            }
            if (!string.IsNullOrEmpty(colName))
            {
                e.Value = colName;
                return;
            }



            //处理创建人 修改人，因为这两个字段没有做外键。固定的所以可以统一处理

        }


        public GridViewDisplayTextResolverGeneric<T> DisplayTextResolver = new GridViewDisplayTextResolverGeneric<T>();

        public BaseListGeneric()
        {
            InitializeComponent();
            if (System.ComponentModel.LicenseManager.UsageMode != System.ComponentModel.LicenseUsageMode.Designtime)
            {
                if (!this.DesignMode)
                {
                    frm = new frmFormProperty();
                    GridRelated = new GridViewRelated();
                    ctr = Startup.GetFromFacByName<BaseController<T>>(typeof(T).Name + "Controller");
                    //权限菜单
                    if (CurMenuInfo == null || CurMenuInfo.ClassPath.IsNullOrEmpty())
                    {
                        CurMenuInfo = MainForm.Instance.MenuList.Where(m => m.IsVisble && m.EntityName == typeof(T).Name && m.ClassPath == this.ToString()).FirstOrDefault();
                        if ((CurMenuInfo == null || CurMenuInfo.ClassPath.IsNullOrEmpty()) && !MainForm.Instance.AppContext.IsSuperUser)
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
                            UIHelper.ControlButton(CurMenuInfo, subItem);

                        }
                        if (item is ToolStripDropDownButton subItemDr)
                        {
                            UIHelper.ControlButton(CurMenuInfo, subItemDr);
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

                    #region 添加按钮

                    Krypton.Toolkit.KryptonButton button设置查询条件 = new Krypton.Toolkit.KryptonButton();
                    button设置查询条件.Text = "设置查询条件";
                    button设置查询条件.ToolTipValues.Description = "对查询条件进行个性化设置。";
                    button设置查询条件.ToolTipValues.EnableToolTips = true;
                    button设置查询条件.ToolTipValues.Heading = "提示";
                    button设置查询条件.Click += button设置查询条件_Click;
                    button设置查询条件.Width = 120;
                    frm.flowLayoutPanelButtonsArea.Controls.Add(button设置查询条件);

                    Krypton.Toolkit.KryptonButton button表格显示设置 = new Krypton.Toolkit.KryptonButton();
                    button表格显示设置.Text = "表格显示设置";
                    button表格显示设置.ToolTipValues.Description = "对表格显示设置进行个性化设置。";
                    button表格显示设置.ToolTipValues.EnableToolTips = true;
                    button表格显示设置.ToolTipValues.Heading = "提示";
                    button表格显示设置.Click += button表格显示设置_Click;
                    button表格显示设置.Width = 120;
                    frm.flowLayoutPanelButtonsArea.Controls.Add(button表格显示设置);
                    #endregion

                    /*
                    // 初始化解析器
                    var resolver = new DataGridViewDisplayNameResolver
         
                    // 绑定到DataGridView
                    resolver.Attach(dataGridView1);
                    dataGridView1.CellFormatting -= DataGridView1_CellFormatting;
                    */
                    dataGridView1.CellFormatting -= DataGridView1_CellFormatting;
                    DisplayTextResolver.Initialize(dataGridView1);

                    //ForeignKeyMapping moduleMapping = new ForeignKeyMapping
                    //{
                    //    TableName = "Modules",
                    //    KeyFieldName = "Id",
                    //    ValueFieldName = "Name",
                    //    IsSpecialField = false,
                    //    IsSelfReferencing = false
                    //};
                    //resolver.AddForeignKeyMapping("ModuleId", moduleMapping);


                    //resolver.AddColumnDisplayType("Image", "Image");

                    // resolver.AddForeignKeyColumnMapping("Module", "ModuleId");
                }
            }

        }

        private async void button表格显示设置_Click(object sender, EventArgs e)
        {
            await UIBizSrvice.SetGridViewAsync(typeof(T), this.dataGridView1, CurMenuInfo, true);
        }

        private async void button设置查询条件_Click(object sender, EventArgs e)
        {
            bool rs = await UIBizSrvice.SetQueryConditionsAsync(CurMenuInfo, QueryConditionFilter, QueryDtoProxy);
            if (rs)
            {
                QueryDtoProxy = LoadQueryConditionToUI();
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
            BuildRelatedDisplay();
        }

        /// <summary>
        /// 构建关联显示的一些数据
        /// </summary>
        public virtual void BuildRelatedDisplay()
        {

        }

        public virtual void BuildSummaryCols()
        {

        }

        public virtual void BuildInvisibleCols()
        {
            //if (_UCBillChildQuery_Related.InvisibleCols == null)
            //{
            //    _UCBillChildQuery_Related.InvisibleCols = new List<string>();
            //}
            //_UCBillChildQuery_Related.InvisibleCols.AddRange(ExpressionHelper.ExpressionListToStringList(ChildRelatedInvisibleCols));
            //_UCBillChildQuery_Related.DefaultHideCols = new List<string>();

            //UIHelper.ControlColumnsInvisible(CurMenuInfo, _UCBillChildQuery_Related.InvisibleCols, _UCBillChildQuery_Related.DefaultHideCols);
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




        /// <summary>
        /// 在已经查询出来的数据中，排除主键，获取其他列重复的记录
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        protected BindingSortCollection<T> GetDuplicatesList()
        {
            BindingSortCollection<T> DuplicatesList = new BindingSortCollection<T>();
            List<T> list = new List<T>();
            list = ListDataSoure.Cast<T>().ToList();
            string pkName = UIHelper.GetPrimaryKeyColName(typeof(T));

            Func<T, Tuple<object[]>> keySelector2 = p =>
            {
                PropertyInfo[] properties = typeof(T).GetProperties()
                    .Where(prop => prop.GetCustomAttribute<SugarColumn>()?.IsIgnore == false && prop.Name != pkName)
                    .ToArray();
                var values = properties.Select(prop => prop.GetValue(p)).ToArray();
                return Tuple.Create(values);
            };

            // 使用自定义比较器进行分组
            var duplicatesList = list.GroupBy(
                keySelector2,
                new CustomTupleEqualityComparer<Tuple<object[]>>(new string[] { pkName }) // 使用适当的比较器
            ).Where(g => g.Count() > 1)
             .Select(g => g.Skip(1))//排除掉第一个元素，这个是第一个重复的元素，要保留
            .SelectMany(g => g)
            .ToList();
            DuplicatesList = duplicatesList.ToBindingSortCollection<T>();
            return DuplicatesList;
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
                    if (this.dataGridView1.UseSelectedColumn)
                    {
                        //多选模式时批量删除
                        await BatchDelete();
                    }
                    else
                    {
                        await Delete();
                    }

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
                    Property();
                    break;
                case MenuItemEnums.帮助:
                    SysHelp(CurMenuInfo.CaptionCN);
                    break;
                default:
                    break;
            }
        }

        public frmFormProperty frm = null;
        protected virtual void Property()
        {
            if (frm.ShowDialog() == DialogResult.OK)
            {
                //保存属性
                ToolBarEnabledControl(MenuItemEnums.属性);
                //AuditLogHelper.Instance.CreateAuditLog<T>("属性", EditEntity);
            }
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
                if (frmadd.usedActionStatus)
                {
                    frmadd.BindData(NewObj as BaseEntity, ActionStatus.新增);
                }
                else
                {
                    frmadd.BindData(NewObj as BaseEntity);
                }

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
                frmadd.BindData(obj as BaseEntity, ActionStatus.新增);
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
                frmadd.CurMenuInfo = this.CurMenuInfo;
                frmadd.Text = this.CurMenuInfo.CaptionCN + "新增";
                frmadd.bindingSourceEdit = bindingSourceList;
                object obj = frmadd.bindingSourceEdit.AddNew();
                //ctr.InitEntity(obj);
                BusinessHelper.Instance.InitEntity(obj);
                //如果obj转基类为空 ，原因是 载入时没有查询出一个默认的框架出来

                if (frmadd.usedActionStatus)
                {
                    frmadd.BindData(obj as BaseEntity, ActionStatus.新增);
                }
                else
                {
                    frmadd.BindData(obj as BaseEntity);
                }

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
                frmadd.BindData(obj as BaseEntity, ActionStatus.新增);
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
        protected async virtual Task<bool> Delete()
        {
            bool rs = false;
            if (MessageBox.Show("系统不建议删除基本资料\r\n确定删除吗？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
            {
                T loc = (T)this.bindingSourceList.Current;
                string PKColName = UIHelper.GetPrimaryKeyColName(typeof(T));
                object PKValue = this.bindingSourceList.Current.GetPropertyValue(PKColName);
                this.bindingSourceList.Remove(loc);
                rs = await ctr.BaseDeleteAsync(loc);
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

                    //根据要缓存的列表集合来判断是否需要上传到服务器。让服务器分发到其他客户端
                    KeyValuePair<string, string> pair = new KeyValuePair<string, string>();
                    //只处理需要缓存的表
                    if (BizCacheHelper.Manager.NewTableList.TryGetValue(typeof(T).Name, out pair))
                    {
                        //如果有更新变动就上传到服务器再分发到所有客户端
                        OriginalData odforCache = ActionForClient.删除缓存<T>(PKColName, PKValue.ToLong());
                        byte[] buffer = CryptoProtocol.EncryptClientPackToServer(odforCache);
                        MainForm.Instance.ecs.client.Send(buffer);

                    }

                }
            }
            return rs;
        }

        protected async virtual Task<int> BatchDelete()
        {

            List<T> SelectedList = new List<T>();
            //多选模式时
            if (dataGridView1.UseSelectedColumn)
            {
                foreach (var item in bindingSourceList)
                {
                    if (item is T sourceEntity)
                    {
                        var selected = (sourceEntity as BaseEntity).Selected;
                        if (selected.HasValue && selected.Value)
                        {
                            SelectedList.Add(sourceEntity);
                        }
                    }
                }
            }
            bool rs = false;
            int counter = 0;
            if (MessageBox.Show($"系统不建议删除基本资料\r\n确定删除选择的【{SelectedList.Count}】条记录吗？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
            {
                // rs = await MainForm.Instance.AppContext.Db.DeleteNav<T>(SelectedList).IncludesAllFirstLayer().ExecuteCommandAsync();
                // counter = await MainForm.Instance.AppContext.Db.Deleteable<T>(SelectedList).ExecuteCommandAsync();

                try
                {
                    bool tryDelete = await ctr.BaseDeleteAsync(SelectedList); //可以执行。但是有外键。无法删除
                    if (tryDelete)
                    {
                        AuditLogHelper.Instance.CreateAuditLog($"批量删除{counter}条记录", CurMenuInfo.CaptionCN);
                    }
                }
                catch (Exception ex)
                {
                    MainForm.Instance.logger.LogInformation(ex, "批量删除时出错,再次单个尝试导航删除.");
                    //再次尝试单个导航删除
                    for (int i = 0; i < SelectedList.Count; i++)
                    {
                        T loc = SelectedList[i];
                        string PKColName = UIHelper.GetPrimaryKeyColName(typeof(T));
                        object PKValue = SelectedList[i].GetPropertyValue(PKColName);
                        this.bindingSourceList.Remove(loc);
                        rs = await ctr.BaseDeleteByNavAsync(loc);
                        if (rs)
                        {
                            counter++;
                            if (MainForm.Instance.AppContext.SysConfig.IsDebug)
                            {
                                MainForm.Instance.logger.LogInformation($"删除:{typeof(T).Name}，主键值：{PKValue.ToString()} ");
                            }
                            AuditLogHelper.Instance.CreateAuditLog("删除", CurMenuInfo.CaptionCN);
                        }
                    }
                }




            }

            #region 更新缓存
            if (SelectedList.Count == counter)
            {
                foreach (var item in SelectedList)
                {
                    string PKColName = UIHelper.GetPrimaryKeyColName(typeof(T));
                    object PKValue = item.GetPropertyValue(PKColName);
                    if (MainForm.Instance.AppContext.SysConfig.IsDebug)
                    {
                        MainForm.Instance.logger.LogInformation($"删除:{typeof(T).Name}，主键值：{PKValue.ToString()} ");
                    }

                    //提示服务器开启推送工作流
                    OriginalData beatDataDel = ClientDataBuilder.BaseInfoChangeBuilder(typeof(T).Name);
                    MainForm.Instance.ecs.AddSendData(beatDataDel);

                    //根据要缓存的列表集合来判断是否需要上传到服务器。让服务器分发到其他客户端
                    KeyValuePair<string, string> pair = new KeyValuePair<string, string>();
                    //只处理需要缓存的表
                    if (BizCacheHelper.Manager.NewTableList.TryGetValue(typeof(T).Name, out pair))
                    {
                        //如果有更新变动就上传到服务器再分发到所有客户端
                        OriginalData odforCache = ActionForClient.删除缓存<T>(PKColName, PKValue.ToLong());
                        byte[] buffer = CryptoProtocol.EncryptClientPackToServer(odforCache);
                        MainForm.Instance.ecs.client.Send(buffer);
                    }
                }

            }

            #endregion


            return counter;
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
                frmaddg.CurMenuInfo = this.CurMenuInfo;
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


        protected virtual void Modify(BaseEdit frmadd)
        {
            if (bindingSourceList.Current != null)
            {
                RevertCommand command = new RevertCommand();
                frmadd.bindingSourceEdit = bindingSourceList;
                T CurrencyObj = (T)bindingSourceList.Current;
                BaseEntity bty = CurrencyObj as BaseEntity;
                bty.ActionStatus = ActionStatus.加载;
                //ctr.EditEntity(bty);
                BusinessHelper.Instance.EditEntity(bty);
                frmadd.BindData(bty, ActionStatus.修改);
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
                RevertCommand command = new RevertCommand();
                frmadd.bindingSourceEdit = bindingSourceList;
                T CurrencyObj = (T)bindingSourceList.Current;
                BaseEntity bty = CurrencyObj as BaseEntity;
                if (frmadd.usedActionStatus)
                {
                    frmadd.BindData(bty as BaseEntity, ActionStatus.修改);
                }
                else
                {
                    frmadd.BindData(bty as BaseEntity);
                }

                //缓存当前编辑的对象。如果撤销就回原来的值
                T oldobj = CloneHelper.DeepCloneObjectAdv<T>((T)bindingSourceList.Current);
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



        KryptonPage AdvPage = null;



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
        protected async virtual void ExtendedQuery(bool UseAutoNavQuery = false)
        {

            if (ValidationHelper.hasValidationErrors(this.Controls))
                return;
            if (QueryDtoProxy == null)
            {
                return;
            }

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

            List<T> list = await ctr.BaseQuerySimpleByAdvancedNavWithConditionsAsync(true, QueryConditionFilter, QueryDtoProxy, pageNum, pageSize, UseAutoNavQuery);

            List<string> masterlist = RuinorExpressionHelper.ExpressionListToStringList(SummaryCols);
            if (masterlist.Count > 0)
            {
                dataGridView1.IsShowSumRow = true;
                dataGridView1.SumColumns = masterlist.ToArray();
            }

            ListDataSoure.DataSource = list.ToBindingSortCollection();//这句是否能集成到上一层生成
            dataGridView1.DataSource = ListDataSoure;

            ToolBarEnabledControl(MenuItemEnums.查询);
        }



        /// <summary>
        /// 保存默认隐藏的列  
        /// HashSet比List性能更好
        /// 为了提高性能，特别是当 InvisibleCols 和 DefaultHideCols 列表较大时，可以使用 HashSet<string> 替代 List<string>。HashSet<string> 的查找性能更高（平均时间复杂度为 O(1)），而 List<string> 的查找性能为 O(n)。
        /// </summary>
        public HashSet<string> DefaultHideCols { get; set; } = new HashSet<string>();


        /// <summary>
        /// 默认不是模糊查询
        /// </summary>
        /// <param name="useLike"></param>
        public BaseEntity LoadQueryConditionToUI(decimal QueryConditionShowColQty = 4)
        {
            //为了验证设置的属性
            this.AutoValidate = AutoValidate.EnableAllowFocusChange;
            //UIQueryHelper<T> uIQueryHelper = new UIQueryHelper<T>();
            kryptonPanel条件生成容器.GetType().GetProperty("DoubleBuffered", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic).SetValue(kryptonPanel条件生成容器, true, null);
            kryptonPanel条件生成容器.Visible = false;
            kryptonPanel条件生成容器.Controls.Clear();
            kryptonPanel条件生成容器.SuspendLayout();
            if (MainForm.Instance.AppContext.CurrentUser_Role == null && MainForm.Instance.AppContext.IsSuperUser)
            {
                QueryDtoProxy = UIGenerateHelper.CreateQueryUI(typeof(T), true, kryptonPanel条件生成容器, QueryConditionFilter, QueryConditionShowColQty);
            }
            else
            {

                //暂时默认了uselike
                tb_UIMenuPersonalization menuSetting = MainForm.Instance.AppContext.CurrentUser_Role_Personalized.tb_UIMenuPersonalizations.FirstOrDefault(c => c.MenuID == CurMenuInfo.MenuID);
                if (menuSetting != null)
                {
                    QueryDtoProxy = UIGenerateHelper.CreateQueryUI(typeof(T), true, kryptonPanel条件生成容器, QueryConditionFilter, menuSetting);
                }
                else
                {
                    QueryDtoProxy = UIGenerateHelper.CreateQueryUI(typeof(T), true, kryptonPanel条件生成容器, QueryConditionFilter, QueryConditionShowColQty);
                }
            }



            kryptonPanel条件生成容器.ResumeLayout();
            kryptonPanel条件生成容器.Visible = true;
            if (InvisibleCols == null)
            {
                InvisibleCols = new HashSet<string>();
            }
            List<string> expInvisibleCols = RuinorExpressionHelper.ExpressionListToStringList(InvisibleColsExp);
            InvisibleCols.AddRange(expInvisibleCols.ToArray());

            //ControlSingleTableColumnsInvisible(InvisibleCols);

            DefaultHideCols = new HashSet<string>();
            UIHelper.ControlColumnsInvisible(CurMenuInfo, InvisibleCols, DefaultHideCols, false);

            //这里设置了指定列不可见
            foreach (var item in InvisibleCols)
            {
                KeyValuePair<string, bool> kv = new KeyValuePair<string, bool>();
                dataGridView1.FieldNameList.TryRemove(item, out kv);
            }
            //这里设置指定列默认隐藏。可以手动配置显示
            foreach (var item in DefaultHideCols)
            {
                KeyValuePair<string, bool> kv = new KeyValuePair<string, bool>();
                dataGridView1.FieldNameList.TryRemove(item, out kv);
                KeyValuePair<string, bool> Newkv = new KeyValuePair<string, bool>(kv.Key, false);
                dataGridView1.FieldNameList.TryAdd(item, Newkv);
            }


            List<T> list = new List<T>();
            bindingSourceList.DataSource = list.ToBindingSortCollection();//这句是否能集成到上一层生成
            dataGridView1.DataSource = bindingSourceList;

            return QueryDtoProxy;

        }


        /// <summary>
        /// 保存不可见的列
        /// </summary>
        public List<Expression<Func<T, object>>> InvisibleColsExp { get; set; } = new List<Expression<Func<T, object>>>();

        /// <summary>
        /// 保存不可见的列
        /// </summary>
        public HashSet<string> InvisibleCols { get; set; } = new HashSet<string>();


        /// <summary>
        /// 
        /// </summary>
        /// <param name="QueryParameters"></param>
        /// <param name="nodeParameter"></param>
        internal override void LoadQueryParametersToUI(BaseEntity QueryParameters, QueryParameter nodeParameter)
        {
            if (QueryParameters != null && nodeParameter != null)
            {
                if (nodeParameter.queryFilter != null)
                {
                    QueryConditionFilter = nodeParameter.queryFilter;
                }

                //nodeParameter参数中包含了这个实体的KEY主键是可以通过主键来查询到准确的一行数据
                // QueryConditionFilter.SetQueryField<tb_CRM_FollowUpPlans>(c => c.Customer_id, true);

                #region  因为时间不会去掉选择，这里特殊处理
                foreach (var item in nodeParameter.queryFilter.QueryFields)
                {
                    Type propertyType = null;
                    if (item.FieldPropertyInfo.PropertyType.IsGenericType && item.FieldPropertyInfo.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                    {
                        propertyType = item.FieldPropertyInfo.PropertyType.GenericTypeArguments[0];
                    }
                    else
                    {
                        propertyType = item.FieldPropertyInfo.PropertyType;
                    }

                    if (propertyType.Name == "DateTime")
                    {
                        //因为查询UI生成时。自动 转换成代理类如：tb_SaleOutProxy，并且时间是区间型式,将起为null即可
                        QueryDtoProxy.SetPropertyValue(item.FieldName + "_Start", null);

                        if (kryptonPanel条件生成容器.Controls.Find(item.FieldName, true)[0] is UCAdvDateTimerPickerGroup timerPickerGroup)
                        {
                            timerPickerGroup.dtp1.Checked = false;
                            timerPickerGroup.dtp2.Checked = false;
                        }
                        //KryptonDateTimePicker dtp = _UCBillQueryCondition.kryptonPanelQuery.Controls.Find(item.FieldName + "_Start", true) as KryptonDateTimePicker;
                        //if (dtp != null)
                        //{
                        //    dtp.check
                        //}
                    }


                }

                #endregion

                QueryDtoProxy = QueryParameters;
                ExtendedQuery(true);
            }
            else
            {
                Refreshs();
            }
        }





        /// <summary>
        /// 与高级查询执行结果公共使用，如果null时，则执行普通查询？
        /// </summary>
        /// <param name="UseNavQuery">是否使用自动导航</param>
        //[MustOverride]
        public async override void Query(bool UseAutoNavQuery = false)
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
                List<T> list = new List<T>();
                if (UseAutoNavQuery)
                {
                    list = await MainForm.Instance.AppContext.Db.Queryable<T>().WhereIF(expression != null, expression)
                   .IncludesAllFirstLayer()//自动更新导航 只能两层。这里项目中有时会失效，具体看文档
                   .ToListAsync();
                }
                else
                {
                    list = await MainForm.Instance.AppContext.Db.Queryable<T>().WhereIF(expression != null, expression)
                  .ToListAsync();
                }

                List<string> masterlist = RuinorExpressionHelper.ExpressionListToStringList(SummaryCols);
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
                ExtendedQuery(UseAutoNavQuery);
            }
            ToolBarEnabledControl(MenuItemEnums.查询);


        }

        //[Obsolete("该方法已经废弃，请使用UIHelper.ControlColumnsInvisible")]
        ///// <summary>
        ///// 控制字段是否显示，添加到里面的是不显示的
        ///// </summary>
        ///// <param name="InvisibleCols"></param>
        //public void ControlSingleTableColumnsInvisible(HashSet<string> InvisibleCols, HashSet<string> DefaultHideCols = null)
        //{
        //    if (!MainForm.Instance.AppContext.IsSuperUser)
        //    {
        //        if (CurMenuInfo.tb_P4Fields != null)
        //        {
        //            List<tb_P4Field> P4Fields =
        //           CurMenuInfo.tb_P4Fields
        //           .Where(p => p.RoleID == MainForm.Instance.AppContext.CurrentUser_Role.RoleID
        //           && p.tb_fieldinfo.IsChild && !p.IsVisble).ToList();
        //            foreach (var item in P4Fields)
        //            {
        //                if (item != null)
        //                {
        //                    if (item.tb_fieldinfo != null)
        //                    {
        //                        //if ((!item.tb_fieldinfo.IsEnabled || !item.IsVisble) && item.tb_fieldinfo.IsChild)
        //                        bool Add = !item.IsVisble && !item.tb_fieldinfo.IsChild && !InvisibleCols.Contains(item.tb_fieldinfo.FieldName);
        //                        if ((!item.tb_fieldinfo.IsEnabled && !item.tb_fieldinfo.IsChild) || Add)
        //                        {
        //                            if (!InvisibleCols.Contains(item.tb_fieldinfo.FieldName))
        //                            {
        //                                InvisibleCols.Add(item.tb_fieldinfo.FieldName);
        //                            }
        //                        }

        //                        if (DefaultHideCols != null)
        //                        {
        //                            if (item.tb_fieldinfo.DefaultHide && !item.tb_fieldinfo.IsChild && !InvisibleCols.Contains(item.tb_fieldinfo.FieldName))
        //                            {
        //                                DefaultHideCols.Add(item.tb_fieldinfo.FieldName);
        //                            }
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //    }
        //}

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
                        //if (MainForm.Instance.AppContext.SysConfig.IsDebug)
                        //{
                        //    MainForm.Instance.logger.LogInformation($"保存:{typeof(T).Name}");
                        //}

                        if (entity.PrimaryKeyID > 0)
                        {
                            BusinessHelper.Instance.EditEntity(entity);
                        }
                        else
                        {
                            BusinessHelper.Instance.InitEntity(entity);
                        }


                        ReturnResults<T> rr = new ReturnResults<T>();
                        rr = await ctr.BaseSaveOrUpdate(entity as T);
                        if (rr.Succeeded)
                        {
                            //保存图片
                            #region 

                            if (ReflectionHelper.ExistPropertyName<T>(nameof(entity.RowImage)) && entity.RowImage != null)
                            {
                                if (entity.RowImage.image != null)
                                {
                                    if (!entity.RowImage.oldhash.Equals(entity.RowImage.newhash, StringComparison.OrdinalIgnoreCase)
                                     && entity.GetPropertyValue("PaymentCodeImagePath").ToString() == entity.RowImage.ImageFullName)
                                    {
                                        HttpWebService httpWebService = Startup.GetFromFac<HttpWebService>();
                                        //如果服务器有旧文件 。可以先删除
                                        if (!string.IsNullOrEmpty(entity.RowImage.oldhash))
                                        {
                                            string oldfileName = entity.RowImage.Dir + entity.RowImage.realName + "-" + entity.RowImage.oldhash;
                                            string deleteRsult = await httpWebService.DeleteImageAsync(oldfileName, "delete123");
                                            MainForm.Instance.PrintInfoLog("DeleteImage:" + deleteRsult);
                                        }
                                        string newfileName = entity.RowImage.GetUploadfileName();
                                        ////上传新文件时要加后缀名
                                        string uploadRsult = await httpWebService.UploadImageAsync(newfileName + ".jpg", entity.RowImage.ImageBytes, "upload");
                                        if (uploadRsult.Contains("UploadSuccessful"))
                                        {
                                            //重要
                                            entity.RowImage.ImageFullName = entity.RowImage.UpdateImageName(entity.RowImage.newhash);
                                            entity.SetPropertyValue("PaymentCodeImagePath", entity.RowImage.ImageFullName);
                                            await ctr.BaseSaveOrUpdate(entity as T);
                                            //成功后。旧文件名部分要和上传成功后新文件名部分一致。后面修改只修改新文件名部分。再对比
                                            MainForm.Instance.PrintInfoLog("UploadSuccessful for base List:" + newfileName);
                                        }
                                        else
                                        {
                                            MainForm.Instance.LoginWebServer();
                                        }
                                    }
                                }
                            }
                            #endregion
                            //保存路径

                            ToolBarEnabledControl(MenuItemEnums.保存);
                            //提示服务器开启推送工作流
                            OriginalData beatData = ClientDataBuilder.BaseInfoChangeBuilder(typeof(T).Name);
                            MainForm.Instance.ecs.AddSendData(beatData);
                            //审计日志
                            AuditLogHelper.Instance.CreateAuditLog("保存", CurMenuInfo.CaptionCN);
                            list.Add(rr.ReturnObject);

                            //根据要缓存的列表集合来判断是否需要上传到服务器。让服务器分发到其他客户端
                            KeyValuePair<string, string> pair = new KeyValuePair<string, string>();
                            //只处理需要缓存的表
                            if (BizCacheHelper.Manager.NewTableList.TryGetValue(typeof(T).Name, out pair))
                            {
                                //如果有更新变动就上传到服务器再分发到所有客户端
                                OriginalData odforCache = ActionForClient.更新缓存<T>(rr.ReturnObject);
                                byte[] buffer = CryptoProtocol.EncryptClientPackToServer(odforCache);
                                MainForm.Instance.ecs.client.Send(buffer);
                            }

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


        protected override void Exit(object thisform)
        {
            UIBizSrvice.SaveGridSettingData(CurMenuInfo, dataGridView1, typeof(T));
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



        protected override void Refreshs()
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
                //如果双击选择列，则不是编辑
                if (e.ColumnIndex != -1)
                {
                    if (dataGridView1.Columns[e.ColumnIndex].Name != "Selected")
                    {
                        Modify();
                    }
                }
                else
                {
                    Modify();
                }
                
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

        private async void BaseList_Load(object sender, EventArgs e)
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
            #region 请求缓存
            //通过表名获取需要缓存的关系表再判断是否存在。没有就从服务器请求。这种是全新的请求。后面还要设计更新式请求。
            if (ColDisplayTypes.Count > 0)
            {
                foreach (Type item in ColDisplayTypes)
                {
                    UIBizSrvice.RequestCache(item);
                }
            }
            UIBizSrvice.RequestCache<T>();
            #endregion

            if (!this.DesignMode)
            {
                dataGridView1.NeedSaveColumnsXml = false;
                await UIBizSrvice.SetGridViewAsync(typeof(T), this.dataGridView1, CurMenuInfo);
                //dataGridView1.ColumnWidthChanged -= DataGridView_ColumnWidthChanged;
                //dataGridView1.ColumnWidthChanged += DataGridView_ColumnWidthChanged;

                QueryDtoProxy = LoadQueryConditionToUI(4);
                BaseDataGridView1 = dataGridView1;

            }

            BuildContextMenuController();
        }
        /// <summary>
        /// 创建右键菜单
        /// </summary>
        public virtual void BuildContextMenuController()
        {

        }
        /*
        private void DataGridView_ColumnWidthChanged(object sender, DataGridViewColumnEventArgs e)
        {
            //保存在内存中
            #region 列有变化就保存到内存，关闭时保存到数据库设置中
            tb_UserPersonalized userPersonalized = MainForm.Instance.AppContext.CurrentUser_Role_Personalized;
            if (userPersonalized == null)
            {
                return;
            }

            tb_UIMenuPersonalization menuPersonalization = userPersonalized.tb_UIMenuPersonalizations.FirstOrDefault(t => t.MenuID == CurMenuInfo.MenuID && t.UserPersonalizedID == userPersonalized.UserPersonalizedID);
            //这里是列的控制情况 
            if (menuPersonalization.tb_UIGridSettings == null)
            {
                menuPersonalization.tb_UIGridSettings = new List<tb_UIGridSetting>();
            }
            tb_UIGridSetting GridSetting = menuPersonalization.tb_UIGridSettings.FirstOrDefault(c => c.GridKeyName == typeof(T).Name && c.UIMenuPID == menuPersonalization.UIMenuPID);
            if (GridSetting == null)
            {
                GridSetting = new tb_UIGridSetting();
                GridSetting.GridKeyName = typeof(T).Name;
                GridSetting.GridType = dataGridView1.GetType().Name;
                GridSetting.UIMenuPID = menuPersonalization.UIMenuPID;
                menuPersonalization.tb_UIGridSettings.Add(GridSetting);
            }
            List<ColumnDisplayController> originalColumnDisplays = new List<ColumnDisplayController>();
            //如果数据有则加载，无则加载默认的
            if (!string.IsNullOrEmpty(GridSetting.ColsSetting))
            {
                object objList = JsonConvert.DeserializeObject(GridSetting.ColsSetting);
                if (objList != null && objList.GetType().Name == "JArray")//(Newtonsoft.Json.Linq.JArray))
                {
                    var jsonlist = objList as Newtonsoft.Json.Linq.JArray;
                    originalColumnDisplays = jsonlist.ToObject<List<ColumnDisplayController>>();
                }
            }
            else
            {
                //找到最原始的数据来自于硬编码
                originalColumnDisplays = UIHelper.GetColumnDisplayList(typeof(T));

                // 获取Graphics对象
                Graphics graphics = dataGridView1.CreateGraphics();
                originalColumnDisplays.ForEach(c =>
                {
                    c.GridKeyName = typeof(T).Name;
                    // 计算文本宽度
                    float textWidth = UITools.CalculateTextWidth(c.ColDisplayText, dataGridView1.Font, graphics);
                    c.ColWidth = (int)textWidth + 10; // 加上一些额外的空间
                });
            }

            if (dataGridView1.ColumnDisplays == null)
            {
                dataGridView1.ColumnDisplays = new List<ColumnDisplayController>();
                foreach (DataGridViewColumn dc in dataGridView1.Columns)
                {
                    ColumnDisplayController cdc = new ColumnDisplayController();
                    cdc.GridKeyName = typeof(T).Name;
                    cdc.ColDisplayText = dc.HeaderText;
                    cdc.ColDisplayIndex = dc.DisplayIndex;
                    cdc.ColWidth = dc.Width;
                    cdc.ColEncryptedName = dc.Name;
                    cdc.ColName = dc.Name;
                    cdc.IsFixed = dc.Frozen;
                    cdc.Visible = dc.Visible;
                    cdc.DataPropertyName = dc.DataPropertyName;
                    originalColumnDisplays.Add(cdc);
                }
            }

            // 检查并添加条件
            foreach (var oldCol in originalColumnDisplays)
            {
                // 检查existingConditions中是否已经存在相同的条件
                if (!dataGridView1.ColumnDisplays.Any(ec => ec.ColName == oldCol.ColName && ec.GridKeyName == typeof(T).Name))
                {
                    // 如果不存在 
                    dataGridView1.ColumnDisplays.Add(oldCol);
                }
                else
                {
                    //更新一下标题
                    var colset = dataGridView1.ColumnDisplays.FirstOrDefault(ec => ec.ColName == oldCol.ColName && ec.GridKeyName == typeof(T).Name);
                    colset = oldCol;
                }
            }

            ColumnDisplayController columnDisplay = dataGridView1.ColumnDisplays.FirstOrDefault(c => c.ColName == e.Column.Name);
            if (columnDisplay != null)
            {
                columnDisplay.ColWidth = e.Column.Width;
            }

            #endregion
        }
        */


        private void kryptonHeaderGroupTop_CollapsedChanged(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == -1)
            {
                return;
            }
            if (dataGridView1.CurrentCell == null)
            {
                return;
            }
            if (dataGridView1.CurrentCell.Value == null)
            {
                return;
            }

            //图片特殊处理
            if (dataGridView1.Columns[e.ColumnIndex].Name == "Images" || dataGridView1.CurrentCell.Value.GetType().Name == "Byte[]")
            {
                if (dataGridView1.CurrentCell.Value != null)
                {
                    System.IO.MemoryStream buf = new System.IO.MemoryStream((byte[])dataGridView1.CurrentCell.Value);
                    System.Drawing.Image image = System.Drawing.Image.FromStream(buf, true);
                    if (image != null)
                    {
                        frmPictureViewer frmShow = new frmPictureViewer();
                        frmShow.PictureBoxViewer.Image = image;
                        frmShow.ShowDialog();
                    }
                }
            }

        }

        public virtual void dataGridView1_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {

        }
    }
}

