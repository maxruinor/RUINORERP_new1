using DevAge.Drawing;
using FastReport.DevComponents.Editors;
using RUINORERP.Common.Extensions;
using RUINORERP.Common.Helper;
using RUINORERP.Global.CustomAttribute;
using RUINORERP.Model;
using SourceGrid;
using SourceGrid.Cells.Editors;
using SourceGrid.Selection;
using SqlSugar;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RUINORERP.UI.UCSourceGrid
{


    //整个表格思路 数据这块是  两个层面 一个是显示用的。一个是用来DB数据库操作的。
    //先显示的，显示定义的数组， 数据是用关联列来表示，其中有一个ID列的标记，是明细ID 基础上所有单据明细都要有的



    /// <summary>
    /// 表格的设置
    /// </summary>
    public class SourceGridDefine : ArrayList
    {

        /// <summary>
        /// 保存主子表中的主表数据
        /// </summary>
        public object GridMasterData { get; set; }

        /// <summary>
        /// 主表的实体类型
        /// </summary>
        public Type GridMasterDataType { get; set; }



        public delegate void CalculateTotalValue(SourceGridDefine griddefine);

        /// <summary>
        /// 如果删除列，总计值要重新计算,关联到外部的事件
        /// </summary>
        public event CalculateTotalValue OnCalculateTotalValue;


        public void UseCalculateTotalValue(SourceGridDefine griddefine)
        {
            if (OnCalculateTotalValue != null)
            {
                OnCalculateTotalValue(griddefine);
            }
        }

        PopupMenu menuController = new PopupMenu();

        /// <summary>
        /// 获取表格中字段的列集合
        /// 这几个都有局限性 用SugarColumn过滤了一次
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="targetColExp">指定一定要添加的字段，一般是单据明细中的产品ID</param>
        /// <returns></returns>
        public static List<SGDefineColumnItem> GetSourceGridDefineColumnItems<T>()
        {
            return GetSourceGridDefineColumnItems<T>(null, false);
        }


        /// <summary>
        /// 获取表格中字段的列集合
        /// 这几个都有局限性 用SugarColumn过滤了一次
        ///过时了  不需要这样指定目标列了，后面重新设计思路是 一个集合
        ///并且指定出保存时不能为空的目标列？
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="targetColExp">指定一定要添加的字段，一般是单据明细中的产品ID</param>
        /// <returns></returns>
        public static List<SGDefineColumnItem> GetSourceGridDefineColumnItems<T>(Expression<Func<T, long?>> BizKeyTargetCol, bool ShowSelected)
        {
            string targetColName = string.Empty;
            if (BizKeyTargetCol != null)
            {
                var mb = BizKeyTargetCol.GetMemberInfo();
                targetColName = mb.Name;
            }
            List<SGDefineColumnItem> cols = new List<SGDefineColumnItem>();
            foreach (PropertyInfo field in typeof(T).GetProperties())
            {
                SGDefineColumnItem col = new SGDefineColumnItem();
                //获取指定类型的自定义特性
                object[] attrs = field.GetCustomAttributes(false);
                foreach (var attr in attrs)
                {

                    //用于是否为外键，是的话，编辑时生成下拉控件
                    if (attr is FKRelationAttribute)
                    {
                        FKRelationAttribute FKAttribute = attr as FKRelationAttribute;
                        col.FKRelationCol = FKAttribute;
                        col.IsFKRelationColumn = true;

                        //如果子表中引用的主表主键,则不显示
                        if (typeof(T).Name.Contains(FKAttribute.FKTableName) && typeof(T).Name.Replace("Detail", "") == FKAttribute.FKTableName)
                        {
                            col.NeverVisible = true;
                        }
                    }

                    if (attr is SubtotalResultAttribute)
                    {
                        SubtotalResultAttribute subtotalResultAttribute = attr as SubtotalResultAttribute;
                        col.SubtotalResult = true;
                    }

                    //if (attr is SubtotalAttribute)
                    //{
                    //    SubtotalAttribute subtotalAttribute = attr as SubtotalAttribute;
                    //    col.Subtotal = true;
                    //}
                    if (attr is SummaryAttribute)
                    {
                        SummaryAttribute summaryAttribute = attr as SummaryAttribute;
                        col.Summary = true;
                    }
                    if (attr is ToolTipAttribute)
                    {
                        ToolTipAttribute toolTipAttribute = attr as ToolTipAttribute;
                        col.ToolTipText = toolTipAttribute.Text;
                    }
                    if (attr is ReadOnlyAttribute)//图片只读
                    {
                        ReadOnlyAttribute readOnlyAttribute = attr as ReadOnlyAttribute;
                        col.ReadOnly = readOnlyAttribute.IsReadOnly;
                    }
                    if (attr is VisibleAttribute)//明细的产品ID隐藏
                    {
                        VisibleAttribute visibleAttribute = attr as VisibleAttribute;
                        col.Visible = visibleAttribute.Visible;
                    }

                    if (attr is SugarColumn)
                    {
                        SugarColumn sugarColumn = attr as SugarColumn;
                        if (string.IsNullOrEmpty(sugarColumn.ColumnDescription))
                        {
                            col.ColCaption = "";
                            //continue;
                        }
                        else
                        {
                            col.ColCaption = sugarColumn.ColumnDescription;
                        }
                        if (sugarColumn.Length > 0)
                        {
                            col.MaxLength = sugarColumn.Length;
                        }
                        //if (sugarColumn.IsIdentity)
                        //{
                        //    continue;
                        //}
                        //明细中的主键不用显示
                        if (sugarColumn.IsPrimaryKey)
                        {
                            col.NeverVisible = true;
                        }
                        col.SugarCol = sugarColumn;
                        if (col.SugarCol != null && col.SugarCol.ColumnDataType != null)
                        {
                            switch (col.SugarCol.ColumnDataType)
                            {
                                case "datetime":
                                    // _editor = new SourceGrid.Cells.Editors.TextBoxUITypeEditor(typeof(DateTime));
                                    col.CustomFormat = CustomFormatType.DateTime;
                                    break;
                                case "money":
                                    col.CustomFormat = CustomFormatType.CurrencyFormat;
                                    break;
                                case "bit":
                                    col.CustomFormat = CustomFormatType.Bool;
                                    break;
                                case "decimal":
                                    col.CustomFormat = CustomFormatType.DecimalPrecision;
                                    break;
                                case "image":
                                    col.CustomFormat = CustomFormatType.Image;
                                    break;
                            }
                        }

                        if (ShowSelected && field.Name == "Selected")
                        {
                            col.ColName = field.Name;
                            col.ColPropertyInfo = field;
                            col.BelongingObjectType = typeof(T);
                            cols.Add(col);
                            continue;
                        }

                        if (sugarColumn.ColumnName == targetColName || col.ColCaption.Trim().Length > 0)
                        {
                            col.ColName = field.Name;
                            col.ColPropertyInfo = field;
                            col.BelongingObjectType = typeof(T);
                            if (true)
                            {

                            }
                            cols.Add(col);
                        }

                    }

                }
            }
            return cols;
        }

        #region 全局背景色设置



        #region 全局性的单元格样式可以在这统一设置。像图片这个就不可以。得一个cell一个new

        /// <summary>
        /// 全局隔行显示背景色(货币列右对齐显示文字使用）
        /// </summary>
        private CellBackColorAlternate viewNormalMoney;

        /// <summary>
        /// 全局隔行显示背景色(货币列右对齐显示文字使用）
        /// </summary>
        public CellBackColorAlternate ViewNormalMoney
        {
            get => viewNormalMoney;
            set => viewNormalMoney = value;
        }



        private CellBackColorAlternate viewNormal;

        /// <summary>
        /// 全局隔行显示背景色
        /// </summary>
        public CellBackColorAlternate ViewNormal
        {
            get => viewNormal;
            set => viewNormal = value;
        }
        #endregion

        /// <summary>
        /// 编辑时的背景色天蓝 
        /// </summary>
        public static CellBackColor editView = new CellBackColor(Color.FromArgb(15, 179, 240));// 102, 153, 255 深蓝

        #endregion

        private void init()
        {

            #region 全局背景色设置
            //Border
            DevAge.Drawing.BorderLine border = new DevAge.Drawing.BorderLine(Color.DarkKhaki, 1);
            DevAge.Drawing.RectangleBorder cellBorder = new DevAge.Drawing.RectangleBorder(border, border);


            //正常时的背景色等样式 //这间隔颜色在三个地方设置了。不要轻易动
            CellBackColorAlternate viewNormal = new CellBackColorAlternate(Color.White, Color.LightCyan);
            viewNormal.Border = cellBorder;
            this.viewNormal = viewNormal;
            this.viewNormal.Name = "viewNormal";

            viewNormal.Border = cellBorder;
            this.viewNormalMoney = new CellBackColorAlternate(Color.White, Color.LightCyan);
            this.viewNormalMoney.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleRight;
            this.viewNormalMoney.Name = "viewNormalMoney";
            #endregion



        }




        private List<KeyValuePair<string, EditorBase>> colEditors = new List<KeyValuePair<string, EditorBase>>();
        /// <summary>
        /// 列名 编辑器集合，好像并没有用到。当时想法是 将这个缓存起来。后面用 
        /// 实际直接保存到了 dci.EditorForColumn
        /// </summary>
        public List<KeyValuePair<string, EditorBase>> ColEditors { get => colEditors; set => colEditors = value; }

        //private Position _currentCellLocation;

        /// <summary>
        /// 当前操作cell的坐标
        /// </summary>
        //public Position CurrentCellLocation { get => _currentCellLocation; set => _currentCellLocation = value; }


        private BindingSource _BindingSourceLines;


        /// <summary>
        /// 表格对应的列集合
        /// </summary>
        public List<SGDefineColumnItem> DefineColumns { get; set; }
        /// <summary>
        /// 公共部分给目标明细部分给值并且字段可能不相同，比方成本，价格
        /// </summary>
        public ConcurrentDictionary<SGDefineColumnItem, SGDefineColumnItem> PointToColumnPairList { get; set; } = new ConcurrentDictionary<SGDefineColumnItem, SGDefineColumnItem>();

        /// <summary>
        /// 以查询的结果中的列名为key，指定到明细中的列名为value的集合
        /// </summary>
        public ConcurrentDictionary<string, SGDefineColumnItem> QueryItemToColumnPairList { get; set; } = new ConcurrentDictionary<string, SGDefineColumnItem>();

        ///// <summary>
        ///// 单据明细中，小计列的计算组合比方成本，价格*qty
        ///// 参与计算的
        ///// </summary>
        //[Obsolete]//过时了，小计已经修改为更灵活的表达式计算
        //public ConcurrentDictionary<SourceGridDefineColumnItem, List<string>> SubtotalColumnGroup { get; set; } = new ConcurrentDictionary<SourceGridDefineColumnItem, List<string>>();



        //Real time setting selection range
        //bigint
        //实现一个功能，实时设置选择下拉值的范围，要提供下拉值的列名，下拉值的范围限制对象的列名，值则是当前操作行时的列对应的值
        //即比方：计划单明细时要指定BOM配方，如选择了C2这个品，则只显示C2的名下的配方。目前暂时只有一个配方。后面一个产品是可以有多个配方的
        /// <summary>
        /// 通过单位 找到 换算，通过 产品ID找到对应的BOM配方。key是已知道的条件列，value是目标列（要修改显示限制值的列）。
        /// </summary>
        public ConcurrentDictionary<SGDefineColumnItem, SGDefineColumnItem> LimitedConditionsForSelectionRange { get; set; } = new ConcurrentDictionary<SGDefineColumnItem, SGDefineColumnItem>();


        /// <summary>
        /// 用于小计计算的公式组合（作用于值变化的时候）
        /// 正向反向的区别是保存的公式在不同的集合中，并且正向是值改变就自动触发，反向是对应的列的值结束编辑时触发
        /// </summary>
        public List<CalculateFormula> SubtotalCalculate { get; set; } = new List<CalculateFormula>();


        /// <summary>
        /// 用于小计计算的公式组合(反向，反向应用于编辑结束时)
        /// 正向反向的区别是保存的公式在不同的集合中，并且正向是值改变就自动触发，反向是对应的列的值结束编辑时触发
        /// </summary>
        public List<CalculateFormula> SubtotalCalculateReverse { get; set; } = new List<CalculateFormula>();



        public SourceGridDefine(SourceGrid.Grid _grid, List<SGDefineColumnItem> DataColList, bool hasRowHeader)
        {
            //行头文字居中
            RowHeaderWithData.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleCenter;
            RowHeaderWithoutData.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleCenter;
            init();
            grid = _grid;
            SetSelection(grid);
            this.HasRowHeader = hasRowHeader;
            this.DefineColumns = DataColList;
            //判断是否有选择列

            SGDefineColumnItem SelectedCol = DataColList.Find(c => c.ColName.Contains("Selected"));
            if (SelectedCol != null)
            {
                SelectedCol.ColCaption = "选择";
                SelectedCol.ColIndex = 1;
                SelectedCol.Visible = false;
                SelectedCol.DefaultHide = true;
                SelectedCol.DefaultValue = false;
            }


            // 按照Selected列降序，其他列升序排序
            DataColList = DataColList.OrderByDescending(x => x.ColName == "Selected")
                               .ThenBy(x => x.ColIndex)
                               .ToList();

            //插入了一列为项的行头     //默认先插入行头
            if (hasRowHeader)
            {
                SGDefineColumnItem rowheaderCol = new SGDefineColumnItem("项", 40, null);
                rowheaderCol.ColCaption = "项";
                rowheaderCol.ColName = "项";
                rowheaderCol.ColIndex = 0;
                rowheaderCol.IsRowHeaderCol = true;
                DataColList.Insert(0, rowheaderCol);
                //一般这种情况不排序
            }

            SGDefineColumnItem[] cols = DataColList.ToArray();
            //这里设置的列是真实的数据列，不包括项行头那一行。所以标号索引从1开始，到多一列止，留0的位置 给行号列=》项
            for (int i = 0; i < cols.Length; i++)
            {
                cols[i].ColIndex = i;
                //按标题数字计算列宽
                if (cols[i].ColCaption.Length > 0)
                {
                    cols[i].Width = cols[i].ColCaption.Length * 20;
                    cols[i].ParentGridDefine = this;
                }
                //只有一列需要统计。则整个表格都有总计行。
                if (cols[i].Summary)
                {
                    this.HasSummaryRow = true;
                }

                this.Add(cols[i]);
            }

            //for (int i = 0; i < names.Length; i++)
            //{
            //    GridDefineColumnItem item = new GridDefineColumnItem(names[i], 0, false, null);
            //    item.name = names[i];
            //    if (currencys != null) item.currency = currencys[i];
            //    if (sobjects != null) item.selectobject = sobjects[i];
            //    if (width != null) item.width = width[i];

            //}
            //this.linecontrol = linecontrol;



        }
        /// <summary>
        /// 应该可以优化掉
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BindingSourceLines_ListChanged(object sender, ListChangedEventArgs e)
        {
            switch (e.ListChangedType)
            {
                case ListChangedType.Reset:
                    break;
                case ListChangedType.ItemAdded:
                    if (BindingSourceLines.Count == 0)
                    {
                        BindingSourceLines.ResetBindings(false);
                        return;
                    }
                    if (e.NewIndex >= BindingSourceLines.Count)
                    {
                        return;
                    }
                    BaseEntity detailAdd = BindingSourceLines.List[e.NewIndex] as BaseEntity;
                    detailAdd.ActionStatus = ActionStatus.修改;

                    //if (EditEntity != null)
                    //{
                    //    EditEntity.actionStatus = ActionStatus.修改;
                    //}

                    break;
                case ListChangedType.ItemDeleted:
                    if (e.NewIndex < BindingSourceLines.Count)
                    {
                        BaseEntity detail = BindingSourceLines.List[e.NewIndex] as BaseEntity;
                        detail.ActionStatus = ActionStatus.删除;
                    }

                    break;
                case ListChangedType.ItemMoved:
                    break;
                case ListChangedType.ItemChanged:
                    // entity = bindingSourceMain.List[e.NewIndex] as BaseEntity;
                    // if (entity.actionStatus == ActionStatus.无操作)
                    // {
                    //     entity.actionStatus = ActionStatus.修改;
                    // }
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

        private bool hasSummaryRow = true;

        private bool hasRowHeader;


        /// <summary>
        ///  key=unit_id, value=tableName
        /// 保存用于显示键值的集合，利用了缓存，到时缓存 要分布式处理实时更新 
        /// </summary>
        public ConcurrentDictionary<string, string> Fk_KeyValuesList { get; set; } = new ConcurrentDictionary<string, string>();


        /// <summary>
        /// 数据源
        /// </summary>
        public BindingSource BindingSourceLines { get => _BindingSourceLines; set => _BindingSourceLines = value; }


        private SourceGrid.Grid _grid;
        public Color HeadForecolor = Color.Black;

        /// <summary>
        /// 行头的颜色 淡紫色
        /// </summary>
        public Color RowHeadBackColor = Color.FromArgb(152, 152, 200);

        /// <summary>
        /// 有数据的行头的颜色 淡绿色
        /// </summary>
        public CellBackColorAlternate RowHeaderWithData = new CellBackColorAlternate(Color.LightGreen, Color.LightGreen);

        /// <summary>
        /// 无数据的行头的颜色 淡绿色
        /// </summary>
        public CellBackColorAlternate RowHeaderWithoutData = new CellBackColorAlternate(Color.FromArgb(152, 152, 200), Color.FromArgb(152, 152, 200));

        /// <summary>
        /// 总计行背景色 很淡的紫色
        /// </summary>
        public Color SummaryColor = Color.FromArgb(217, 217, 255);


        //添加行时的行头的背景色 奇偶间隔颜色  紫色到 淡紫色
        public CellBackColorAlternate OddAndEvenColor = new CellBackColorAlternate(Color.FromArgb(152, 152, 200), Color.FromArgb(153, 153, 220));

        /*
         https://www.zxcc.net/tools/static/colorpicker/index.html?   很好的颜色选择器 颜色工具
         */

        #region 选中背景及边框等
        /// <summary>
        /// 选中边框色
        /// </summary>
        public Color _SelectionBorderColor = Color.FromArgb(0, 255, 0);//明绿色
        public Color _SelectBackColor = Color.Blue;
        public Color _FocusBackColor = Color.FromArgb(200, 0, 128, 255);//玫红色艳丽

        /// <summary>
        /// 选择
        /// </summary>
        public SelectionBase Selection
        {
            get
            {
                return grid.Selection as SelectionBase;
            }
        }
        #endregion


        /// <summary>
        /// 表格主要依赖的类型
        /// 控制的主要的业务表名
        /// </summary>
        public string MainBizDependencyTypeName { get; set; }


        private List<object> sourceList = new List<object>();
        /// <summary>
        /// 缓存产品明细表数据
        /// </summary>
        public List<object> SourceList { get => sourceList; set => sourceList = value; }


        /// <summary>
        /// 设置关联的查询相关参数 
        /// </summary>
        /// <typeparam name="Prod">产品明细视图,数据来源</typeparam>
        /// <typeparam name="Share">共享部分</typeparam>
        /// <typeparam name="BillDetail">目标类型，就是单据明细</typeparam>
        /// <param name="list"></param>
        /// <param name="targetColExp">产品详情的ID表达式</param>
        public void sSetDependencyObject<Prod, Share, BillDetail>(List<Prod> list, Expression<Func<BillDetail, long?>> targetColExp)
        {
            //这显然与装箱/拆箱有关。 返回需要装箱的值类型的Lambda表达式将表示为UnaryExpressions，而返回引用类型的Lambda表达式将表示为MemberExpressions。
            //因为用了object为了兼容性
            //var bodyExpr = targetCol.Body as System.Linq.Expressions.MemberExpression;
            //if (bodyExpr == null)
            //{
            //    throw new ArgumentException("Expression must be a MemberExpression!", "expr");
            //}
            //var SugarColumnAttr = AttributeHelper.SetAttribute<SugarColumn>();
            //var colDesc = SugarColumnAttr.Get<T>(targetCol);

            var mb = targetColExp.GetMemberInfo();
            string key = mb.Name;

            SGDefineColumnItem tagcol = this.DefineColumns.FirstOrDefault(d => d.ColName == key);
            //设置关联列。以及主要的目标列

            //DependColumn TargCol = new DependColumn();
            //TargCol.ColCaption = tagcol.ColCaption;
            //TargCol.ColName = tagcol.ColName;
            //TargCol.IsPrimaryBizKeyColumn = true;
            //TargCol.Visible = false;

            //DependencyQuery dq = new DependencyQuery();
            //dq.RelatedCols = dq.SetDependencys<Share>();
            //dq.RelatedCols.Add(TargCol);//添加目标列

            this.SourceList = new List<object>();// ((IEnumerable<dynamic>)list) as List<object>;
            if (list != null)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    this.SourceList.Add(list[i]);
                }
            }
            // DependQuery = dq;

            BindingSourceLines.ListChanged += BindingSourceLines_ListChanged;
        }



        /// <summary>
        /// 为了显示名值键值对
        /// </summary>
        /// <param name="type"></param>
        public void SetFkColListForKeyValues(Type type)
        {
            string tableName = type.Name;
            foreach (var field in type.GetProperties())
            {
                //获取指定类型的自定义特性
                object[] attrs = field.GetCustomAttributes(false);
                foreach (var attr in attrs)
                {
                    if (attr is FKRelationAttribute)
                    {
                        FKRelationAttribute fkrattr = attr as FKRelationAttribute;
                        if (!this.Fk_KeyValuesList.ContainsKey(fkrattr.FK_IDColName))
                        {
                            this.Fk_KeyValuesList.TryAdd(fkrattr.FK_IDColName, fkrattr.FKTableName);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 设置关联的查询相关参数 
        /// </summary>
        /// <typeparam name="Prod">产品明细视图,数据来源</typeparam>
        /// <typeparam name="Share">共享部分</typeparam>
        /// <typeparam name="BillDetail">目标类型，就是单据明细</typeparam>
        /// <param name="list"></param>
        /// <param name="targetColExp">产品详情的ID表达式</param>
        public void SetDependencyObject<Share, BillDetail>(IList Productlist, params Expression<Func<BillDetail, object>>[] TagColsExps)
        {
            SetFkColListForKeyValues(typeof(Share));
            SetFkColListForKeyValues(typeof(BillDetail));

            //这显然与装箱/拆箱有关。 返回需要装箱的值类型的Lambda表达式将表示为UnaryExpressions，而返回引用类型的Lambda表达式将表示为MemberExpressions。
            //因为用了object为了兼容性
            //var bodyExpr = targetCol.Body as System.Linq.Expressions.MemberExpression;
            //if (bodyExpr == null)
            //{
            //    throw new ArgumentException("Expression must be a MemberExpression!", "expr");
            //}
            //var SugarColumnAttr = AttributeHelper.SetAttribute<SugarColumn>();
            //var colDesc = SugarColumnAttr.Get<T>(targetCol);

            //设置关联列。以及主要的目标列
            //DependencyQuery dq = new DependencyQuery();
            //  dq.RelatedCols = dq.SetDependencys<Share>();
            //foreach (var targetColExp in TagColsExps)
            //{
            //    var mb = targetColExp.GetMemberInfo();
            //    string key = mb.Name;
            //    SourceGridDefineColumnItem tagcol = this.DefineColumns.FirstOrDefault(d => d.ColName == key);
            //    if (tagcol != null)
            //    {
            //        //DependColumn TargCol = new DependColumn();
            //        //TargCol.ColCaption = tagcol.ColCaption;
            //        //TargCol.ColName = tagcol.ColName;
            //        tagcol.GuideToTargetColumn = true;
            //        // TargCol.Visible = false;
            //        // dq.RelatedCols.Add(TargCol);//添加目标列
            //    }
            //    else
            //    {
            //        MainForm.Instance.uclog.AddLog(key + "不存在于表格列中");
            //    }
            //}


            this.SourceList = new List<object>();// ((IEnumerable<dynamic>)list) as List<object>;
            if (Productlist != null)
            {
                for (int i = 0; i < Productlist.Count; i++)
                {
                    this.SourceList.Add(Productlist[i]);
                }
            }
            BindingSourceLines.ListChanged += BindingSourceLines_ListChanged;
        }

        /// <summary>
        /// 设置关联列值及目标列值
        /// </summary>
        public void SetDependTargetValue(object value, SourceGrid.Position currPosition, object productSharePart, string currentColName)
        {

            //清空所有 不要删除
            if (value == null)
            {
                //相当于当前的值清空。刚相关的也清空
                foreach (SGDefineColumnItem item in this)
                {
                    SourceGrid.Position pt = new Position(currPosition.Row, item.ColIndex);
                    SourceGrid.CellContext processContext = new SourceGrid.CellContext(this.grid, pt);
                    if (item.CustomFormat == CustomFormatType.Image)
                    {
                        processContext.Value = null;
                        processContext.Cell.View = ViewNormal;
                    }
                    if (item.CustomFormat == CustomFormatType.WebPathImage)
                    {
                        processContext.Tag = null;
                        //processContext.Cell.View = ViewNormal;
                    }
                    if (!item.IsRowHeaderCol)
                    {
                        processContext.Value = null;
                    }

                    if (this.grid.Rows[currPosition.Row].RowData != null && item.IsPrimaryBizKeyColumn)
                    {
                        //设置目标的绑定数据值，就是产品ID 这里是清空
                        var currentObj = this.grid.Rows[currPosition.Row].RowData;
                        ReflectionHelper.SetPropertyValue(currentObj, item.ColName, null);
                        //给值就给行号 删除时才重新全命名行号？
                        //this.grid[currPosition.Row, 0].Value = "";
                        this.grid[currPosition.Row, 0].View = RowHeaderWithoutData;
                    }
                }
                return;
            }




            foreach (SGDefineColumnItem item in this)
            {
                //跳过自己列
                if (currPosition.Column == item.ColIndex)
                {
                    continue;
                }

                #region 设置表格UI中的其他关联列的值
                SourceGrid.Position pt = new Position(currPosition.Row, item.ColIndex);
                SourceGrid.CellContext processContext = new SourceGrid.CellContext(this.grid, pt);

                var newotherVal = ReflectionHelper.GetPropertyValue(productSharePart, item.ColName);
                if (item.IsFKRelationColumn && !string.IsNullOrEmpty(newotherVal.ToString()))
                {
                    //这里写死了。是不太合理的。应该T 到时看是不是以一个表为标准，在所有单据明细中
                    newotherVal = Common.UIHelper.ShowGridColumnsNameValue<tb_Prod>(item.ColName, newotherVal);
                }

                if (newotherVal.GetType().FullName == "System.Byte[]")
                {
                    Image temp = ImageHelper.ConvertByteToImg(newotherVal as Byte[]);
                    processContext.Value = temp;
                    continue;
                }
                if (!object.Equals(processContext.Value, newotherVal))
                {
                    processContext.Value = newotherVal;
                }
                //ReflectionHelper.SetPropertyValue(productSharePart, item.ColName, null);
                #endregion

                #region 目标列要修改对应的绑定数据对象 实际是给真正的单据明细中的数据库中的列给值
                if (item.IsPrimaryBizKeyColumn)
                {
                    if (this.grid.Rows[currPosition.Row].RowData != null)
                    {
                        //设置目标的绑定数据值，就是产品ID
                        var currentObj = this.grid.Rows[currPosition.Row].RowData;
                        ReflectionHelper.SetPropertyValue(currentObj, item.ColName, newotherVal);
                        //给值就给行号
                        this.grid[currPosition.Row, 0].Value = currPosition.Row.ToString();
                        //行头给右键菜单
                        PopupMenuForRowHeader pop = this.grid[currPosition.Row, 0].FindController<PopupMenuForRowHeader>();
                        if (pop == null)
                        {
                            PopupMenuForRowHeader menuController = new PopupMenuForRowHeader(currPosition.Row, this.grid, this);
                            this.grid[currPosition.Row, 0].Controller.AddController(menuController);
                        }
                    }
                }
                #endregion




            }

        }


        new public SGDefineColumnItem this[int index]
        {
            get
            {
                return (SGDefineColumnItem)base[index];
            }
        }

        private Dictionary<string, SGDefineColumnItem> _columnItemsByName = new Dictionary<string, SGDefineColumnItem>();
        public void Add(SGDefineColumnItem item)
        {
            base.Add(item);
            if (!string.IsNullOrEmpty(item.ColName))
            {
                _columnItemsByName[item.ColName] = item;
            }
        }
        public SGDefineColumnItem this[string name]
        {
            get
            {
                if (_columnItemsByName.TryGetValue(name, out SGDefineColumnItem item))
                {
                    return item;
                }
                return null;
            }
        }


        public SGDefineColumnItem GetColumnDefineInfo<T>(Expression<Func<T, object>> TargetCol)
        {
            string targetColName = string.Empty;
            if (TargetCol != null)
            {
                var mb = TargetCol.GetMemberInfo();
                targetColName = mb.Name;
            }
            SGDefineColumnItem sgdci = DefineColumns.FirstOrDefault(w => w.ColName == targetColName);
            return sgdci;
        }



        /// <summary>
        /// 设置选中边框 编辑时 应该处理编辑控件
        /// </summary>
        private void SetSelection(SourceGrid.Grid _grid)
        {
            init();
            grid = _grid;
            Selection.BackColor = _SelectBackColor;

            //启用选多行
            grid.Selection.EnableMultiSelection = false;

            grid.TabStop = true;
            DevAge.Drawing.RectangleBorder border = Selection.Border;
            border.SetColor(_SelectionBorderColor); //边框颜色

            border.SetWidth(2);//边框
            Selection.Border.SetDashStyle(System.Drawing.Drawing2D.DashStyle.Dash);
            Selection.Border = border;

            //焦点背景色
            Selection.FocusBackColor = _FocusBackColor;
            //焦点背景色透明值 0为全透明 255 不透明
            Selection.FocusBackColor = Color.FromArgb(30, _FocusBackColor);
            //Selection.BindToGrid(grid);


        }


        /// <summary>
        /// 需要编辑的栏
        /// </summary>
        public Color NeedEditBackColor = Color.FromArgb(50, 0, 128, 255);



        public Grid grid { get => _grid; set => _grid = value; }

        public bool HasSummaryRow { get => hasSummaryRow; set => hasSummaryRow = value; }


        public bool HasRowHeader { get => hasRowHeader; set => hasRowHeader = value; }

    }


    public class CellBackColor : SourceGrid.Cells.Views.Cell
    {
        public CellBackColor(Color firstColor)
        {
            FirstBackground = new DevAge.Drawing.VisualElements.BackgroundSolid(firstColor);
        }

        private DevAge.Drawing.VisualElements.IVisualElement mFirstBackground;
        public DevAge.Drawing.VisualElements.IVisualElement FirstBackground
        {
            get { return mFirstBackground; }
            set { mFirstBackground = value; }
        }


        protected override void PrepareView(SourceGrid.CellContext context)
        {
            base.PrepareView(context);
            Background = FirstBackground;
        }
    }

    /// <summary>
    /// 奇偶行的背景色
    /// </summary>
    public class CellBackColorAlternate : SourceGrid.Cells.Views.Cell
    {
        private string _name;
        public string Name { get => _name; set => _name = value; }
        public CellBackColorAlternate(Color firstColor, Color secondColor)
        {
            FirstBackground = new DevAge.Drawing.VisualElements.BackgroundSolid(firstColor);
            SecondBackground = new DevAge.Drawing.VisualElements.BackgroundSolid(secondColor);
        }

        private DevAge.Drawing.VisualElements.IVisualElement mFirstBackground;
        public DevAge.Drawing.VisualElements.IVisualElement FirstBackground
        {
            get { return mFirstBackground; }
            set { mFirstBackground = value; }
        }

        private DevAge.Drawing.VisualElements.IVisualElement mSecondBackground;
        public DevAge.Drawing.VisualElements.IVisualElement SecondBackground
        {
            get { return mSecondBackground; }
            set { mSecondBackground = value; }
        }

        protected override void OnDrawContent(GraphicsCache graphics, RectangleF area)
        {
            base.OnDrawContent(graphics, area);
        }

        protected override void PrepareView(SourceGrid.CellContext context)
        {
            base.PrepareView(context);

            if (Math.IEEERemainder(context.Position.Row, 2) == 0)
                Background = FirstBackground;
            else
                Background = SecondBackground;
        }
    }

    internal class CheckBoxBackColorAlternate : SourceGrid.Cells.Views.CheckBox
    {
        public CheckBoxBackColorAlternate(Color firstColor, Color secondColor)
        {
            FirstBackground = new DevAge.Drawing.VisualElements.BackgroundSolid(firstColor);
            SecondBackground = new DevAge.Drawing.VisualElements.BackgroundSolid(secondColor);
        }

        private DevAge.Drawing.VisualElements.IVisualElement mFirstBackground;
        public DevAge.Drawing.VisualElements.IVisualElement FirstBackground
        {
            get { return mFirstBackground; }
            set { mFirstBackground = value; }
        }

        private DevAge.Drawing.VisualElements.IVisualElement mSecondBackground;
        public DevAge.Drawing.VisualElements.IVisualElement SecondBackground
        {
            get { return mSecondBackground; }
            set { mSecondBackground = value; }
        }

        protected override void PrepareView(SourceGrid.CellContext context)
        {
            base.PrepareView(context);

            if (Math.IEEERemainder(context.Position.Row, 2) == 0)
                Background = FirstBackground;
            else
                Background = SecondBackground;
        }

    }


    /// <summary>
    /// 自定义格式
    /// </summary>
    public enum CustomFormatType
    {
        /// <summary>
        /// 默认
        /// </summary>
        DefaultFormat,

        /// <summary>
        /// 百分比
        /// </summary>
        PercentFormat,

        /// <summary>
        /// 枚举值下拉选项
        /// </summary>
        EnumOptions,
        /// <summary>
        /// 货币
        /// </summary>
        CurrencyFormat,

        /// <summary>
        /// 精度
        /// </summary>
        DecimalPrecision,

        /// <summary>
        /// bool显示true/false改为是否
        /// </summary>
        Bool,

        /// <summary>
        /// 图片
        /// </summary>
        Image,

        /// <summary>
        /// 图片路径形式保存，会上传图片到服务器的
        /// </summary>
        WebPathImage,

        DateTime,
    }

}
