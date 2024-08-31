using DevAge.ComponentModel.Converter;
using DevAge.Drawing;
using DevAge.Windows.Forms;
using FastReport.DevComponents.DotNetBar;
using FastReport.Utils;
using NPOI.SS.Formula.Functions;

//using Google.Protobuf.Reflection;
//using NetTaste;
using RUINORERP.Business;
using RUINORERP.Common.Extensions;
using RUINORERP.Common.Helper;
using RUINORERP.Global.CustomAttribute;
using RUINORERP.Model;
using RUINORERP.UI.BaseForm;
using RUINORERP.UI.Common;
using RUINORERP.UI.UCSourceGrid;
using SourceGrid;
using SourceGrid.Cells;
using SourceGrid.Cells.Editors;
using SourceGrid.Selection;
using SqlSugar;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Linq.Dynamic;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Winista.Text.HtmlParser.Data;
using WorkflowCore.Primitives;
//using ZXing.Common;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TrayNotify;

namespace RUINORERP.UI.UCSourceGrid
{

    /// <summary>
    /// 绑定明细的一个帮助类
    /// </summary>
    /// <typeparam name="P">产品表</typeparam>
    /// <typeparam name="T">明细表</typeparam>
    public class SourceGridHelper
    {
        public delegate void ValidateDataRowsDelegate(object rowObj);
        /// <summary>
        /// 验证行数据
        /// </summary>
        public event ValidateDataRowsDelegate OnValidateDataRows;


        public delegate void ValidateDataCellDelegate(object rowObj);
        /// <summary>
        /// 验证数据
        /// </summary>
        public event ValidateDataCellDelegate OnValidateDataCell;


        public delegate void CalculateColumnValue(object rowObj, SourceGridDefine griddefine, Position Position);

        /// <summary>
        /// 计算列值
        /// </summary>
        public event CalculateColumnValue OnCalculateColumnValue;



        public delegate void LoadMultiRowData(object rows, Position position);

        /// <summary>
        /// 选数据时多行选择
        /// </summary>
        public event LoadMultiRowData OnLoadMultiRowData;


        //处理一个特殊情况：如BOM明细时，如果明细中的产品有配方，即他是由其它产品组成的,他的成本和制造费用自于配方表中的数据。要把这些数据带到BOM明细行中。如果没有配方则是来自于产品详情的成本，这里用一个事件处理

        /// <summary>
        /// 关联列的赋值处理
        /// </summary>
        /// <typeparam name="R"></typeparam>
        /// <param name="sourceObj">View_ProDetail</param>
        /// <param name="rowObj"></param>
        /// <param name="griddefine"></param>
        /// <param name="Position"></param>
        public delegate void LoadRelevantFields<R>(object _View_ProdDetail, object rowObj, SourceGridDefine griddefine, Position Position);

        /// <summary>
        ///关联列的赋值处理，特殊情况具体调用时实现
        /// </summary>
        public event LoadRelevantFields<object> OnLoadRelevantFields;





        /// <summary>
        /// 提取单据明细列。并且全标记为目标列
        /// </summary>
        /// <typeparam name="BillDetail"></typeparam>
        /// <returns></returns>
        public List<SourceGridDefineColumnItem> GetGridColumns<BillDetail>()
        {
            List<SourceGridDefineColumnItem> listCols = new List<SourceGridDefineColumnItem>();
            List<SourceGridDefineColumnItem> cols1 = SourceGridDefine.GetSourceGridDefineColumnItems<BillDetail>();
            listCols.AddRange(cols1);
            //指定了关键字段ProdDetailID
            // List<SourceGridDefineColumnItem> cols2 = SourceGridDefine.GetSourceGridDefineColumnItems<BillDetail>(BizKeyTargetCol);
            //var mb = BizKeyTargetCol.GetMemberInfo();
            //string keyColName = mb.Name;

            foreach (var item in listCols)
            {
                if (typeof(BillDetail).Name == item.BelongingObjectType.Name)
                {
                    item.GuideToTargetColumn = true;
                    item.IsCoreContent = true;
                }
                //if (item.ColName == keyColName)
                //{
                //    item.IsPrimaryBizKeyColumn = true;
                //}
            }

            return listCols;
        }

        /// <summary>
        /// 设置要加载显示的列，   指定关键字段ProdDetailID
        /// </summary>
        /// <typeparam name="Prod">产品视图实体</typeparam>
        /// <typeparam name="BillDetail">单据明细</typeparam>
        /// <param name="TagColsExps">目标列集合在明细部分，可能也同时存在于产品视图公共部分中</param>
        /// <param name="BizKeyTargetCol">业务主键 一般指产品ID,必需设置，否则会使用没有描述的规则过滤掉</param>
        /// <param name="ShowSelected">明细中是否需要显示选择一列，如果要显示，默认只是添加的表格中。默认是visble=false。要手动显示</param>
        /// <returns></returns>
        public List<SourceGridDefineColumnItem> GetGridColumns<Prod, BillDetail>(Expression<Func<BillDetail, long?>> BizKeyTargetCol, bool ShowSelected)
        {
            List<SourceGridDefineColumnItem> listCols = new List<SourceGridDefineColumnItem>();
            List<SourceGridDefineColumnItem> cols1 = SourceGridDefine.GetSourceGridDefineColumnItems<Prod>();
            //指定了关键字段ProdDetailID
            List<SourceGridDefineColumnItem> cols2 = SourceGridDefine.GetSourceGridDefineColumnItems<BillDetail>(BizKeyTargetCol, ShowSelected);
            listCols.AddRange(cols1);
            listCols.AddRange(cols2);

            /* 优秀代码学习
            List<string> RepeatColNames = new List<string>();
            RepeatColNames = listCols.Select(c => c.ColName).ToList().GroupBy(x => x).Where(x => x.Count() > 1).Select(x => x.Key).ToList();

            //去掉公共部分。只留下单据明细部分
            foreach (var item in RepeatColNames)
            {
                SourceGridDefineColumnItem c = cols1.Where(c => c.ColName == item).FirstOrDefault();
                cols1.Remove(c);
            }
            listCols = new List<SourceGridDefineColumnItem>();
            listCols.AddRange(cols1);
            listCols.AddRange(cols2);
            */


            var mb = BizKeyTargetCol.GetMemberInfo();
            string keyColName = mb.Name;

            foreach (var item in listCols)
            {
                if (typeof(BillDetail).Name == item.BelongingObjectType.Name)
                {
                    item.GuideToTargetColumn = true;
                    item.IsCoreContent = true;
                }
                if (item.ColName == keyColName)
                {
                    item.IsPrimaryBizKeyColumn = true;
                }
            }



            /*

     

            SourceGridDefineColumnItem tagcol = this.DefineColumns.FirstOrDefault(d => d.ColName == key);
            //设置关联列。以及主要的目标列

            DependColumn TargCol = new DependColumn();
            TargCol.ColCaption = tagcol.ColCaption;
            TargCol.ColName = tagcol.ColName;
            TargCol.IsPrimaryKeyIdentityColumn = true;
            TargCol.Visible = false;

            DependencyQuery dq = new DependencyQuery();
            dq.RelatedCols = dq.SetDependencys<Share>();
            dq.RelatedCols.Add(TargCol);//添加目标列

            dq.SourceList = new List<object>();// ((IEnumerable<dynamic>)list) as List<object>;
            if (Productlist != null)
            {
                for (int i = 0; i < Productlist.Count; i++)
                {
                    dq.SourceList.Add(Productlist[i]);
                }
            }
            DependQuery = dq;
            */

            return listCols;
        }


        /// <summary>
        /// 设置要加载显示的列，   指定关键字段ProdDetailID
        /// </summary>
        /// <typeparam name="Prod">产品视图实体</typeparam>
        /// <typeparam name="BillDetail">单据明细</typeparam>
        /// <typeparam name="OtherInfo">其它展示，只读</typeparam>
        /// <param name="TagColsExps">目标列集合在明细部分，可能也同时存在于产品视图公共部分中</param>
        /// <param name="BizKeyTargetCol">业务主键 一般指产品ID,必需设置，否则会使用没有描述的规则过滤掉</param>
        /// <param name="ShowSelected">明细中是否需要显示选择一列，如果要显示，默认只是添加的表格中。默认是visble=false。要手动显示</param>
        /// <returns></returns>
        public List<SourceGridDefineColumnItem> GetGridColumns<Prod, BillDetail, OtherInfo>(Expression<Func<BillDetail, long?>> BizKeyTargetCol, bool ShowSelected)
        {
            List<SourceGridDefineColumnItem> listCols = new List<SourceGridDefineColumnItem>();
            List<SourceGridDefineColumnItem> cols1 = SourceGridDefine.GetSourceGridDefineColumnItems<Prod>();
            List<SourceGridDefineColumnItem> cols0 = SourceGridDefine.GetSourceGridDefineColumnItems<OtherInfo>();

            //OtherInfo 这个不能编辑，只读
            foreach (var item in cols0)
            {
                item.ReadOnly = true;
            }
            //指定了关键字段ProdDetailID
            List<SourceGridDefineColumnItem> cols2 = SourceGridDefine.GetSourceGridDefineColumnItems<BillDetail>(BizKeyTargetCol, ShowSelected);
            listCols.AddRange(cols1);
            listCols.AddRange(cols2);
            listCols.AddRange(cols0);
            /* 优秀代码学习
            List<string> RepeatColNames = new List<string>();
            RepeatColNames = listCols.Select(c => c.ColName).ToList().GroupBy(x => x).Where(x => x.Count() > 1).Select(x => x.Key).ToList();

            //去掉公共部分。只留下单据明细部分
            foreach (var item in RepeatColNames)
            {
                SourceGridDefineColumnItem c = cols1.Where(c => c.ColName == item).FirstOrDefault();
                cols1.Remove(c);
            }
            listCols = new List<SourceGridDefineColumnItem>();
            listCols.AddRange(cols1);
            listCols.AddRange(cols2);
            */


            var mb = BizKeyTargetCol.GetMemberInfo();
            string keyColName = mb.Name;

            foreach (var item in listCols)
            {
                if (typeof(BillDetail).Name == item.BelongingObjectType.Name)
                {
                    item.GuideToTargetColumn = true;
                    item.IsCoreContent = true;
                }
                if (item.ColName == keyColName)
                {
                    item.IsPrimaryBizKeyColumn = true;
                }
            }



            /*

     

            SourceGridDefineColumnItem tagcol = this.DefineColumns.FirstOrDefault(d => d.ColName == key);
            //设置关联列。以及主要的目标列

            DependColumn TargCol = new DependColumn();
            TargCol.ColCaption = tagcol.ColCaption;
            TargCol.ColName = tagcol.ColName;
            TargCol.IsPrimaryKeyIdentityColumn = true;
            TargCol.Visible = false;

            DependencyQuery dq = new DependencyQuery();
            dq.RelatedCols = dq.SetDependencys<Share>();
            dq.RelatedCols.Add(TargCol);//添加目标列

            dq.SourceList = new List<object>();// ((IEnumerable<dynamic>)list) as List<object>;
            if (Productlist != null)
            {
                for (int i = 0; i < Productlist.Count; i++)
                {
                    dq.SourceList.Add(Productlist[i]);
                }
            }
            DependQuery = dq;
            */

            return listCols;
        }


        /// <summary>
        /// 加载已经的数据明细显示到表格
        /// </summary>
        /// <typeparam name="C"></typeparam>
        /// <param name="grid1"></param>
        /// <param name="sgdefine"></param>
        /// <param name="_details"></param>
        /// <param name="BizKeyTargetColExp"></param>
        public void InsertItemDataToGrid<C>(SourceGrid.Grid grid1, SourceGridDefine sgdefine,
            List<C> _details, Expression<Func<C, long?>> BizKeyTargetColExp, Position position) where C : class
        {
            int i = position.Row;
            InsertItemData<C>(grid1, sgdefine, _details, BizKeyTargetColExp, i, false);
        }



        public void InsertItemData<C>(SourceGrid.Grid grid1, SourceGridDefine sgdefine,
            List<C> _details, Expression<Func<C, long?>> BizKeyTargetColExp, int rowPosition, bool isLoadData) where C : class
        {
            int i = rowPosition;

            int row头尾 = 1;
            if (grid1.HasSummary)
            {
                row头尾++;
            }
            //因为是可以从中间插入，原来可以有值，所以加上i 
            int needAddrows = _details.Count + i - (grid1.Rows.Count - row头尾);
            if (needAddrows > 0)
            {
                //默认只给了10行，如果不够刚要添加
                //默认10行数据中，还要加上列头一行，如果有统计 最后也算一行。所以算12行
                for (int r = 0; r < needAddrows; r++)
                {
                    this.InsertRow(grid1, sgdefine, true);
                    // AddRow(grid1, sgdefine, true);
                }
            }

            var mb = BizKeyTargetColExp.GetMemberInfo();
            string key = mb.Name;

            //为了显示公共部分，将带出的数据明细，从数据源里取产品信息来填充
            //List<object> products = new List<object>();

            foreach (C item in _details)
            {
                sgdefine.BindingSourceLines.Add(item);
                //载入数据就是相对完整的  固定按行号添加新行
                if (grid1.Rows[i].RowData == null)
                {
                    grid1.Rows[i].RowData = sgdefine.BindingSourceLines.List[i - 1];//数据源是0开始，表格0是列头所有是1开始
                    //grid1.Rows[i].RowData = sgdefine.BindingSourceLines.AddNew();
                }
                //优化后开始
                #region 优化
                foreach (SourceGridDefineColumnItem dc in sgdefine.ToArray())
                {
                    SourceGrid.Position pt = new SourceGrid.Position(i, dc.ColIndex);
                    SourceGrid.CellContext currContext = new SourceGrid.CellContext(dc.ParentGridDefine.grid, pt);

                    //设置为可以编辑
                    SetRowEditable(dc.ParentGridDefine.grid, new int[] { i }, dc.ParentGridDefine);

                    //如果行头，就添加右键删除菜单
                    if (dc.IsRowHeaderCol)
                    {
                        currContext.Value = i;
                        PopupMenuForRowHeader pop = currContext.Cell.FindController<PopupMenuForRowHeader>();
                        if (pop == null)
                        {
                            PopupMenuForRowHeader menuController = new PopupMenuForRowHeader(i, dc.ParentGridDefine.grid, sgdefine);
                            if (currContext.Cell.Controller != null)
                            {
                                currContext.Cell.Controller.AddController(menuController);
                            }

                        }
                    }
                    else
                    {
                        if (dc.BelongingObjectType == null)
                        {
                            continue;
                        }
                        //真正明细部分
                        if (dc.BelongingObjectType.Name == item.GetType().Name)
                        {
                            object cellvalue = ReflectionHelper.GetPropertyValue(item, dc.ColName);
                            if (!cellvalue.IsNullOrEmpty())
                            {
                                currContext.DisplayText = ShowFKColumnText(dc, cellvalue, sgdefine);
                                currContext.Value = cellvalue;
                                currContext.Tag = item;
                                if (dc.ColName == "Selected")
                                {
                                    grid1[pt] = new SourceGrid.Cells.CheckBox(null, cellvalue.ToBool());
                                    continue;
                                }
                                //产品ID
                                if (dc.IsPrimaryBizKeyColumn)
                                {
                                    if (grid1.Rows[i].RowData != null)
                                    {
                                        var currentObj = grid1.Rows[i].RowData;
                                        ReflectionHelper.SetPropertyValue(currentObj, dc.ColName, cellvalue);
                                    }
                                }
                            }

                        }
                        else
                        {
                            //公共部分
                            var prodetailID = ReflectionHelper.GetPropertyValue(item, key);
                            var v_prod = sgdefine.SourceList.Find(x => ReflectionHelper.GetPropertyValue(x, key).ToString() == prodetailID.ToString());
                            if (v_prod != null)
                            {
                                object cellvalue = ReflectionHelper.GetPropertyValue(v_prod, dc.ColName);
                                if (!cellvalue.IsNullOrEmpty())
                                {
                                    if (!isLoadData)
                                    {
                                        //如果是加载数据，不用设置值，插入时才设置
                                        //如果这个列指定和目标，也要设置一下 ，但是这里是加载。都已经保存在数据库了。不需要？
                                        SetCellValue(dc, pt, v_prod, true);
                                    }

                                    //如果是与值不一样的名称显示，这种情况。1）行rowdata中已经是真正的数据。
                                    //如果有编辑器的也区分了。所以这里可以把值改为显示名称
                                    currContext.DisplayText = ShowFKColumnText(dc, cellvalue, sgdefine);
                                    if (!string.IsNullOrEmpty(currContext.DisplayText))
                                    {
                                        currContext.Value = currContext.DisplayText;
                                    }
                                    else
                                    {
                                        currContext.Value = cellvalue;
                                    }
                                    currContext.Tag = v_prod;

                                }

                            }



                        }

                    }
                }
                i++;

                #endregion

            }

        }


        /// <summary>
        /// 加载已经的数据明细显示到表格
        /// </summary>
        /// <typeparam name="C"></typeparam>
        /// <param name="grid1"></param>
        /// <param name="sgdefine"></param>
        /// <param name="_details"></param>
        /// <param name="BizKeyTargetColExp"></param>
        public void LoadItemDataToGrid<C>(SourceGrid.Grid grid1, SourceGridDefine sgdefine,
            List<C> _details, Expression<Func<C, long?>> BizKeyTargetColExp) where C : class
        {
            sgdefine.BindingSourceLines.Clear();
            //清空明细表格
            #region
            //先清空 不包含 列头和总计
            SourceGrid.RangeRegion rr = new SourceGrid.RangeRegion(new SourceGrid.Position(grid1.Rows.Count - 1, grid1.Columns.Count));
            for (int ii = 0; ii < grid1.Rows.Count; ii++)
            {
                grid1.Rows[ii].RowData = null;
            }
            grid1.ClearValues(rr);

            //先清空
            for (int r = 1; r < grid1.Rows.Count; r++)
            {
                foreach (SourceGridDefineColumnItem dc in sgdefine.ToArray())
                {
                    SourceGrid.Position pt = new SourceGrid.Position(r, dc.ColIndex);
                    SourceGrid.CellContext currContext = new SourceGrid.CellContext(dc.ParentGridDefine.grid, pt);
                    currContext.Value = null;
                    if (dc.ColName == "Selected")
                    {
                        grid1[pt] = new SourceGrid.Cells.Cell("");
                        grid1[pt].Value = null;
                        grid1[pt].Editor = null;
                        grid1[pt].View = dc.ParentGridDefine.ViewNormal;
                    }
                    if (pt.Row == grid1.Rows.Count - 1)
                    {
                        //貨幣格式時，如果編輯器為空，則設置為只讀時，不能顯示￥格式
                        if (dc.CustomFormat != CustomFormatType.CurrencyFormat)
                        {
                            grid1[pt].Editor = null;//只读
                        }

                        if (pt.Row == grid1.Rows.Count - 1 && pt.Column == 0)
                        {
                            grid1[pt].Value = "总计";
                        }
                    }
                }
            }

            #endregion

            //var detailItems = ((IEnumerable<dynamic>)_details).ToList();
            int i = 1;
            InsertItemData<C>(grid1, sgdefine, _details, BizKeyTargetColExp, i, true);
        }



        /// <summary>
        /// 插入合并列的数据，为了借节点一样显示数据
        /// </summary>
        /// <typeparam name="C"></typeparam>
        /// <param name="grid1"></param>
        /// <param name="sgdefine"></param>
        /// <param name="_details"></param>
        /// <param name="BizKeyTargetColExp"></param>
        /// <param name="ShowContents"></param>
        /// <param name="ColumnsSpanCount"></param>
        public int InsertItemDataToGridForColumnSpan<C>(SourceGrid.Grid grid1, SourceGridDefine sgdefine,
            List<C> _details, Expression<Func<C, long?>> BizKeyTargetColExp, string ShowContents, int ColumnsSpanCount) where C : class
        {

            SourceGrid.Cells.Views.Cell titleModel = new SourceGrid.Cells.Views.Cell();
            titleModel.BackColor = Color.SteelBlue;
            titleModel.ForeColor = Color.White;
            titleModel.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleLeft;

            //标题不算，所以加1，另起一行+1
            int row = sgdefine.BindingSourceLines.Count + 1 + 1;

            //0用于项行数据显示了
            grid1[row, 1] = new SourceGrid.Cells.Cell(ShowContents);
            grid1[row, 1].View = titleModel;
            grid1[row, 1].ColumnSpan = ColumnsSpanCount - 1;


            int i = 1;
            int row头尾 = 1;
            if (grid1.HasSummary)
            {
                row头尾++;
            }
            int needAddrows = row + 1 - (grid1.Rows.Count - row头尾);
            if (needAddrows > 0)
            {
                //默认只给了10行，如果不够刚要添加
                //默认10行数据中，还要加上列头一行，如果有统计 最后也算一行。所以算12行
                for (int r = 0; r < needAddrows; r++)
                {
                    this.InsertRow(grid1, sgdefine, true);
                }
            }

            return row;
        }



        public string ShowFKColumnText(SourceGridDefineColumnItem dc, object cellvalue, SourceGridDefine sgdefine)
        {
            string _DisplayText = string.Empty;
            #region 显示外键名称

            if (dc.FKRelationCol != null && !string.IsNullOrEmpty(cellvalue.ToString()))
            {
                if (sgdefine.Fk_KeyValuesList.ContainsKey(dc.FKRelationCol.FK_IDColName))
                {
                    string baseTableName = sgdefine.Fk_KeyValuesList[dc.FKRelationCol.FK_IDColName];
                    object obj = CacheHelper.Instance.GetValue(baseTableName, cellvalue);
                    if (obj != null && obj.ToString() != "System.Object")
                    {
                        _DisplayText = obj.ToString();
                    }
                }
            }
            #endregion

            return _DisplayText;
        }

        public void InitGrid(SourceGrid.Grid grid, SourceGridDefine griddefine, bool autofill, string xmlfileName)
        {

            griddefine.grid = grid;
            //_sourceGridDefine = griddefine;
            if (griddefine.Count == 0)
            {
                return;
            }

            InitGrid(grid, griddefine, xmlfileName);

            grid.Controller.AddController(new KeyDeleteController());

            if (autofill)
            {
                //默认自动填充10行 如果加载数据时 不够，刚要添加行
                for (int r = 1; r < 10; r++)
                {
                    AddRow(grid, griddefine, true);
                    //SourceGridHelper.SetGridReadOnly(grid1, sgd);
                }
            }
            if (griddefine.HasSummaryRow)
            {
                #region 总计
                //设置倒数第二行的高度，直接将最后一行挤到最底部；
                //grid2.Rows[grid2.Rows.Count - 2].Height = grid2.Height - grid2.Rows[0].Height * (grid2.RowsCount) + 10;
                //设置倒数第一行的内容
                //grid2.FixedRows = 20;
                //grid2.Rows[grid2.Rows.Count - 1]

                grid.Rows[grid.Rows.Count - 1].Height = 40;
                //grid[grid.Rows.Count - 1, 0] = new SourceGrid.Cells.Cell("总计", typeof(string));
                grid[grid.Rows.Count - 1, 0] = new SourceGrid.Cells.Cell("总计", typeof(string));
                grid[grid.Rows.Count - 1, 0].Editor = null;//只读

                for (int c = 0; c < griddefine.Count - 1; c++)
                {
                    string totalText = string.Empty;
                    if (griddefine[c].Summary)
                    {
                        if (griddefine[c].CustomFormat == CustomFormatType.CurrencyFormat)
                        {
                          //  totalText = string.Format("{0:C}", 0);
                            grid[grid.Rows.Count - 1, c] = new SourceGrid.Cells.Cell(totalText, typeof(decimal));
                            grid[grid.Rows.Count - 1, c].Value = 0;
                            grid[grid.Rows.Count - 1, c].DisplayText = string.Format("{0:C}", 0);
                        }
                        else if (1 == 1)
                        {
                           //这里显示格式化可以根据字段定义如果为int 或有小数
                            grid[grid.Rows.Count - 1, c] = new SourceGrid.Cells.Cell(0, typeof(int));
                            grid[grid.Rows.Count - 1, c].Value = 0;
                            grid[grid.Rows.Count - 1, c].DisplayText = string.Format("{0:##}", 0);
                        }
                        else
                        {
                            grid[grid.Rows.Count - 1, c] = new SourceGrid.Cells.Cell(totalText, typeof(string));
                        }

                    }

                    grid[grid.Rows.Count - 1, c].Editor = null;//总计行都无法编辑

                }

                //这里设置（最后一行）总计行的样式 后面一起优化
                CellBackColorAlternate SumViewNormal = new CellBackColorAlternate(Color.Khaki, Color.DarkKhaki);
                CellBackColorAlternate SumViewNormalMoney = new CellBackColorAlternate(Color.Khaki, Color.DarkKhaki);

                //Border
                DevAge.Drawing.BorderLine border = new DevAge.Drawing.BorderLine(Color.DarkKhaki, 1);
                DevAge.Drawing.RectangleBorder cellBorder = new DevAge.Drawing.RectangleBorder(border, border);
                SumViewNormal.Border = cellBorder;
                SumViewNormalMoney.Border = cellBorder;
                for (int c = 0; c < grid.ColumnsCount; c++)
                {

                    if (griddefine[c].CustomFormat == CustomFormatType.CurrencyFormat)
                    {
                        grid[grid.Rows.Count - 1, c].View = SumViewNormalMoney;
                        //金额靠右
                        grid[grid.Rows.Count - 1, c].View.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleRight;
                    }
                    else
                    {
                        grid[grid.Rows.Count - 1, c].View = SumViewNormal;
                    }

                }

                // 总计行背景色 应该被覆盖了。
                // grid[grid.Rows.Count - 1, 0].View.BackColor = Color.FromArgb(217, 217, 255);
                grid[grid.Rows.Count - 1, 0].Tag = "SummaryRow";
                //FreezeArea.Bottom;



                #endregion
            }
            //设置焦点
            // grid.Selection.FocusRow(1);


            //if (autofill)
            //{
            //    //重新设定 最后一个的大小
            //    int x = grid.Width - grid.BorderWidth - griddefine.length();
            //    if (x > 0)
            //        griddefine[griddefine.Count - 1].width += x;
            //    if (grid.Columns[griddefine.Count - 1].Width < griddefine[griddefine.Count - 1].width)
            //        grid.Columns[griddefine.Count - 1].Width = griddefine[griddefine.Count - 1].width;
            //}


            //GridHelper.AddSummaryRow(grid, griddefine);
            //grid.Redim(20,grid.Cols);
            // 按照 grid的 大小 添加行数
            //int r = GridHelper.gridmrows(grid);
            //grid.BorderWidth=1;
            //grid.BorderStyle=BorderStyle.Fixed3D;
            //r = grid.Height / grid.DefaultHeight - 2;
            //if (r <= 2)
            //{
            //    r = 15;
            //}
            //for (int i = 1; i < r; i++)
            //{
            //    AddRowForEdit(grid, griddefine, false);
            //}
            // grid.Height = (r + 2) * grid.DefaultHeight + grid.BorderWidth * 2;
            //  griddefine.needheight = (r + 1) * grid.DefaultHeight + grid.BorderWidth * 2 + grid.SummaryHeight + 4;

            griddefine.OnCalculateTotalValue += Griddefine_OnCalculateTotalValue;
        }

        public void Griddefine_OnCalculateTotalValue(SourceGridDefine griddefine)
        {
            if (OnCalculateColumnValue != null)
            {
                OnCalculateColumnValue(null, griddefine, new Position { });
            }
        }

        /// <summary>
        /// 定义了表格，
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="griddefine">索引</param>
        private void InitGrid(SourceGrid.Grid grid, SourceGridDefine griddefine, string customColumnsXMLFileName)
        {
            //启动时默认无选中
            grid.Selection.FocusStyle = SourceGrid.FocusStyle.None;
            grid.SelectionMode = SourceGrid.GridSelectionMode.Row;
            //创建头
            grid.Redim(1, griddefine.Count);

            //设置不能滚动的列数和行数，这里是"项"和第一行，
            grid.FixedColumns = 1;
            //grid.FixedRows = grid.Rows.Count - 1;
            grid.FixedRows = 1;
            //grid.AutoStretchColumnsToFitWidth = true;
            //grid.AutoStretchRowsToFitHeight = true;
            //grid.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            //grid.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            //grid[0, 0] = new SourceGrid.Cells.Header(null);

            //ColumnHeader view
            SourceGrid.Cells.Views.ColumnHeader viewColumnHeader = new SourceGrid.Cells.Views.ColumnHeader();
            DevAge.Drawing.VisualElements.ColumnHeader backHeader = new DevAge.Drawing.VisualElements.ColumnHeader();
            //backHeader.BackColor = Color.Maroon;
            backHeader.Style = ControlDrawStyle.Disabled;
            backHeader.BackColor = griddefine.RowHeadBackColor;
            backHeader.Border = DevAge.Drawing.RectangleBorder.NoBorder;

            viewColumnHeader.Background = backHeader;
            viewColumnHeader.ForeColor = Color.Black;
            viewColumnHeader.Font = new Font("宋体", 10, FontStyle.Bold);
            viewColumnHeader.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleCenter;


            #region 创建列
            //排除列头，因为有行头 2024
            PopupMenuForDeleteSelect popupMenuForDelete = new PopupMenuForDeleteSelect(grid, griddefine);

            PopupMenuForSelect menuForSelect = new PopupMenuForSelect(grid, griddefine);

            PopupMenuWithCustomColumns menuController = new PopupMenuWithCustomColumns(customColumnsXMLFileName);
            for (int i = 0; i < griddefine.Count; i++)
            {
                griddefine[i].ParentGridDefine = griddefine;
                if (griddefine[i].Summary)
                {
                    grid.HasSummary = true;
                }
                //HasHeader
                SourceGrid.Cells.ColumnHeader columnHeader = new SourceGrid.Cells.ColumnHeader();
                columnHeader.View = viewColumnHeader;
                columnHeader.Value = griddefine[i].ColCaption;
                //暂时默认所有列不自动排序  如果排序则全乱了。总计 和提前插入的空白行都以及数据修改全乱
                columnHeader.AutomaticSortEnabled = false;
                //griddefine[i].ColIndex = i;

                //if (griddefine[i].ColName == griddefine.DependQuery.TargetCol.TargetColumnName)
                //{
                //    griddefine.DependQuery.TargetCol.TargetColIndex = griddefine[i].ColIndex;
                //}
                ///控制列的可见性,这里设置为永远不可见，指定字段时
                if (griddefine[i].NeverVisible)
                {
                    grid.Columns[i].Visible = false;
                }
                else
                {
                    //默认是否隐藏
                    griddefine[i].Visible = !griddefine[i].DefaultHide;

                    //加载配置中的自定义显示列的控制
                    if (menuController.ConfigColItems.ContainsKey(griddefine[i].ColCaption))
                    {
                        griddefine[i].Visible = menuController.ConfigColItems[griddefine[i].ColCaption];
                    }
                    grid.Columns[i].Visible = griddefine[i].Visible;
                }

                if (griddefine[i].IsRowHeaderCol)
                {
                    grid.Columns.SetWidth(0, 35);
                    grid.Columns[i].AutoSizeMode = SourceGrid.AutoSizeMode.None;
                    grid.Columns[i].Width = 25;
                }
                else
                {
                    SetColumnEditor(griddefine[i]);
                }


                grid[0, i] = columnHeader;


                if (!griddefine[i].IsRowHeaderCol || griddefine[i].NeverVisible)
                {
                    //menuController.AddItems(griddefine[i].ColCaption, griddefine[i].ColName, griddefine[i].Visible);
                    //添加要控制的列

                    //menuController.OnColumnsVisible += delegate (int colIndex, string colName, bool visible)
                    //{
                    //    grid.Columns[colIndex].Visible = visible;
                    //};
                    menuController.AddItems(new KeyValuePair<string, SourceGridDefineColumnItem>(griddefine[i].ColCaption, griddefine[i]));
                    menuController.OnColumnsVisible += delegate (KeyValuePair<string, SourceGridDefineColumnItem> kv)
                    {
                        grid.Columns[kv.Value.ColIndex].Visible = kv.Value.Visible;
                    };
                }
                if (columnHeader.Value.ToString() == "项")
                {
                    grid[0, i].Controller.AddController(popupMenuForDelete);
                }
                else if (columnHeader.Value.ToString() == "选择")
                {
                    grid[0, i].Controller.AddController(menuForSelect);
                }
                else
                {
                    grid[0, i].Controller.AddController(menuController);
                }

                //===列宽控制作
                //if (i != grid.ColumnsCount - 1)//不是最后一列
                //{
                //    grid.Columns[i].Width = 90; colsWidth[i];
                //    otherColsWidth += colsWidth[i];
                //}
                //else //设置最后一列铺满整个grid
                //    grid.Columns[i].Width = grid1.Width - otherColsWidth - 2 * i;
                //==



                //grid.Columns[i].Width = griddefine[i].width;
                /*
                SourceGrid.Cells.ColumnHeader header = new SourceGrid.Cells.ColumnHeader("Header ");
                header.AutomaticSortEnabled = false;
                DevAge.Drawing.RectangleBorder border = new DevAge.Drawing.RectangleBorder();
                border.SetColor(Color.FromArgb(255, 128, 128, 192)); //边框颜色
                border.SetWidth(1);//边框
                header.View.Border = border;
                header.View.BackColor = (Color.FromArgb(255, 128, 128, 192)); //颜色
                header.View.ForeColor = (Color.FromArgb(255, 128, 128, 192)); //颜色
                //header.ColumnSelectorEnabled = true;
                //header.ColumnFocusEnabled = true;
                header.View.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleCenter;
                grid[0, i] = header;
                */
            }

            #endregion
            SetColumnsWidth(grid, griddefine);


            SourceGridDefineColumnItem selected = griddefine.DefineColumns.Find(c => c.ColName == "Selected");
            if (selected != null)
            {
                DevAge.Drawing.RectangleBorder border = new DevAge.Drawing.RectangleBorder(new DevAge.Drawing.BorderLine(Color.DarkGreen), new DevAge.Drawing.BorderLine(Color.DarkGreen));
                SourceGrid.Cells.Views.CheckBox checkView = new SourceGrid.Cells.Views.CheckBox();
                checkView.Background = new DevAge.Drawing.VisualElements.BackgroundLinearGradient(Color.ForestGreen, Color.White, 45);
                checkView.Border = border;
                selected.width = 40;
                grid.Columns[selected.ColIndex].Width = 40;
                // grid1[r, 2].View = checkView;
            }
            //if (grid.Tag is SourceGridDefine)
            //{
            //    SourceGridDefine dd = (SourceGridDefine)grid.Tag;
            //}


            grid.Tag = griddefine;

            griddefine.grid = grid;

            grid.AutoSizeCells();

        }

        public void AddRow(SourceGrid.Grid grid, SourceGridDefine define, bool RowReadonly)
        {
            int row = grid.Rows.Count - 1;
            int AddRowIndex = row + 1;
            if (AddRowIndex <= 0)
            {
                //AddRowIndex = 1;
                //这行也不行。实际原因是表格加载会慢，所以会先加载，然后插入行，不然会报错
            }
            //if (define.HasSummaryRow)
            //{
            //    AddRowIndex = AddRowIndex - 2;
            //}
            AddRows(AddRowIndex, grid, define, RowReadonly);
        }





        public void InsertRow(SourceGrid.Grid grid, SourceGridDefine define, bool RowReadonly)
        {

            int row = grid.Rows.Count - 1;
            int AddRowIndex = row + 1;
            if (define.HasSummaryRow)
            {
                AddRowIndex = AddRowIndex - 1;
            }
            if (AddRowIndex == -1)
            {
                // AddRowIndex = 0;
                //这行也不行。实际原因是表格加载会慢，所以会先加载，然后插入行，不然会报错
            }

            AddRows(AddRowIndex, grid, define, RowReadonly);
        }

        private void AddRows(int addRowIndex, SourceGrid.Grid grid, SourceGridDefine define, bool RowReadonly)
        {

            grid.Rows.Insert(addRowIndex);
            //grid.Rows.SetHeight(行数, 高度);
            if (define.HasRowHeader)
            {
                grid.Columns.SetWidth(0, 35);
            }


            //Grid_Minerals.Columns[0].Width = 20; 某一列宽度
            //创建本列上所有的单元格的值等属性
            for (int i = 0; i < define.Count; i++)
            {

                if (define.HasRowHeader && i == 0)
                {
                    //右键删除行的菜单
                    //PopupMenuForRowHeader menuController = new PopupMenuForRowHeader(addRowIndex, grid, define);
                    //行号
                    //SourceGrid.Cells.RowHeader rh = new SourceGrid.Cells.RowHeader((row + 1).ToString());

                    SourceGrid.Cells.RowHeader rh = new SourceGrid.Cells.RowHeader(null);
                    CellBackColorAlternate viewRowHeader = new CellBackColorAlternate(define.RowHeadBackColor, define.RowHeadBackColor);
                    rh.View = viewRowHeader;
                    rh.View.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleCenter;

                    SourceGrid.Cells.Views.Cell rowHelderCellView = new SourceGrid.Cells.Views.Cell();
                    //绿的背景渐变。只是不太好看。
                    rowHelderCellView.Background = new DevAge.Drawing.VisualElements.BackgroundLinearGradient(Color.ForestGreen, Color.White, 45);
                    //rh.View = rowHelderCellView;
                    //rh.AddController(menuController);
                    grid[addRowIndex, i] = rh;

                    continue;
                }
                else
                {
                    //提示控制器
                    SourceGrid.Cells.Controllers.ToolTipText toolTipController = new SourceGrid.Cells.Controllers.ToolTipText();
                    //toolTipController.ToolTipTitle = "asdfad";
                    //toolTipController.ToolTipIcon = ToolTipIcon.Info;
                    //toolTipController.IsBalloon = true;



                    //目前只加到要手输入的，非关联字段上
                    CustomKeyEvent tabkeyController = new CustomKeyEvent();

                    //Setup the controllers
                    CellClickEvent clickController = new CellClickEvent();

                    BillOperateController billController = new BillOperateController(define);
                    billController.OnValidateDataCell += (rowObj) =>
                    {

                    };

                    //目前认为是目标列才计算，并且 类型要为数字型
                    if (define[i].Summary || define[i].CustomFormat == CustomFormatType.PercentFormat)
                    {
                        billController.OnCalculateColumnValue += (rowdata, CurrGridDefine, rowIndex) =>
                        {
                            if (rowdata != null && OnCalculateColumnValue != null)
                            {
                                OnCalculateColumnValue(rowdata, grid.Tag as SourceGridDefine, rowIndex);
                            }
                        };
                    }

                    //第二列起不是列头
                    SourceGrid.Cells.Cell c = new SourceGrid.Cells.Cell(null);
                    if (!RowReadonly)
                    {
                        //值
                        if (!define[i].ReadOnly)
                        {
                            define[i].EditorForColumn.EnableEdit = RowReadonly;
                            c = new SourceGrid.Cells.Cell(null, define[i].EditorForColumn);
                        }
                    }

                    System.Reflection.PropertyInfo pi = null;
                    Type newcolType;
                    if (define[i].ColPropertyInfo == null)
                    {
                        newcolType = typeof(string);
                    }
                    else
                    {
                        pi = define[i].ColPropertyInfo;
                        if (pi.PropertyType.IsGenericType && pi.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                        {
                            newcolType = pi.PropertyType.GetGenericArguments()[0];
                        }
                        else
                        {
                            newcolType = pi.PropertyType;
                        }
                    }
                    switch (newcolType.FullName)
                    {

                        case "System.Char":
                        case "System.String":
                            break;
                        case "System.Guid":
                            break;
                        case "System.Decimal":
                            c = new SourceGrid.Cells.Cell(null);
                            //c = new SourceGrid.Cells.Cell(0, typeof(decimal));
                            //c = new SourceGrid.Cells.Cell(88.2M);
                            break;
                        case "System.Int16":
                        case "System.Int32":
                        case "System.Int64":
                            break;
                        case "System.Byte[]":
                            // c = new SourceGrid.Cells.Image(null);新加行时 不用要图片，这个图片列，好像是背景。会相同
                            break;
                        case "System.Boolean":
                            c = new SourceGrid.Cells.CheckBox(null, true);
                            if (RowReadonly)
                            {
                                c.Editor = null; //如果为空 是灰色无法编辑
                            }
                            if (define[i].ColName == "Selected")
                            {
                                c = new SourceGrid.Cells.Cell("");
                                c.Value = null;
                                c.Editor = null;
                                c.View = define.ViewNormal;
                            }
                            break;
                    }

                    if (!string.IsNullOrEmpty(define[i].ColCaption))
                    {
                        //c.ToolTipText = define[i].ToolTipText;
                        c.ToolTipText = define[i].ColCaption;
                        c.AddController(toolTipController);
                    }
                    if (!string.IsNullOrEmpty(define[i].FormatText))
                    {

                    }

                    if (define[i].DefaultValue != null)
                    {

                    }

                    //添加点击控制器
                    c.AddController(clickController);
                    //验证？
                    c.AddController(billController);

                    if (define[i].GuideToTargetColumn)
                    {
                        c.AddController(tabkeyController);
                    }



                    /*
                    #region 设置指定列为可编辑列
                    DependColumn dc = define.DependQuery.RelatedCols.Find(delegate (DependColumn dc)
                    { return dc.ColCaption == define[i].ColCaption; });
                    if (dc != null && dc.IsQueryCol)
                    {
                        //这里要重构
                        DependColumnController 更新关联列控制器 = new DependColumnController(define, define.DependQuery.RelatedCols);

                        更新关联列控制器.ConvertFunction = delegate (object valValue)
                        {
                            if (valValue.GetType() == define.DependencyType)
                            {
                                return valValue;
                                //return RUINORERP.Common.Helper.ReflectionHelper.GetPropertyValue(valValue, dc.ColName);
                            }
                            else
                            {
                                return valValue;
                            }
                        };
                        c.AddController(更新关联列控制器);
                    }
                    #endregion
                    */
                    if (newcolType.FullName == "System.Byte[]")
                    {
                        //  c.View = define.ImagesViewModel;
                        c.View = define.ViewNormal;
                    }
                    else if (define[i].CustomFormat == CustomFormatType.CurrencyFormat)
                    {
                        c.View = define.ViewNormalMoney;
                    }
                    else
                    {
                        c.View = define.ViewNormal;
                    }

                    grid[addRowIndex, i] = c;
                    //如果可空，默认显示空白
                    if (pi.PropertyType.Name == "Nullable`1")
                    {
                        c.DisplayText = string.Empty;
                    }
                    grid.Rows[addRowIndex].Height = 20;


                }

            }

        }

        public void DeleteRow(SourceGridDefine sgdefine, params int[] RowIndexs)
        {
            Grid grid = sgdefine.grid;
            //思路先把数据移掉一行，再重复设置一下右键删除行的菜单，并且重新设置行号,并且添加一行
            //注意这里可能有顺序，先删除数据行的数据，不然下面grid.remove可能是变化的索引
            foreach (var RowIndex in RowIndexs)
            {
                sgdefine.BindingSourceLines.Remove(grid.Rows[RowIndex].RowData);
            }
            // 对数组进行降序排序，以确保删除行时索引不会错乱
            //Array.Sort(RowIndexs, (a, b) => b.CompareTo(a));

            //这里按索引删除的话，索引会变化导致出错。所有删除一次。剩下的数据都上移一行。
            //这里按索引删除的话，删除一行时，后面的索引会变化导致出错。所以删除一次。剩下的数据都上移一行。
            //我的想法是，将数组放到一个堆栈中，从上移出来一个值，按这个值，即行号删除一行时。 堆栈中的其它值都加1 。这样循环堆栈。直到完成。
            // 创建一个堆栈来存储要删除的行索引
            Stack<int> stack = new Stack<int>(RowIndexs);
            while (stack.Count > 0)
            {
                int rowIndex = stack.Pop();
                grid.Rows.Remove(rowIndex);
                // 更新堆栈中剩余的行索引
                if (stack.Count > 0)
                {
                    int nextIndex = stack.Peek();
                    nextIndex--;
                }
            }
            SourceGridHelper sh = new SourceGridHelper();
            //插入一行新的空行
            for (int i = 0; i < RowIndexs.Length; i++)
            {
                sh.InsertRow(grid, sgdefine, true);
            }


            sgdefine.UseCalculateTotalValue(grid.Tag as SourceGridDefine);
            grid.Selection.ResetSelection(true);
            //删除一行后。后面的行的索引都会变化。要重新处理一次，并且行号重新显示
            foreach (GridRow row in grid.Rows)
            {
                //重新设置列行头跳过
                if (row.Index == 0)
                {
                    continue;
                }
                if (grid.HasSummary)
                {
                    //排除总计行
                    if (grid[row.Index, 0].Tag != null && grid[row.Index, 0].Tag.ToString() == "SummaryRow")
                    {
                        continue;
                    }
                }

                //行头给右键菜单 不为空的才是已经设置过了。有正常数据的行
                PopupMenuForRowHeader pop = grid[row.Index, 0].FindController<PopupMenuForRowHeader>();
                if (pop == null)
                {
                    //没有设置过右键菜单的，非正常数据行
                    grid[row.Index, 0].Value = row.Index;
                    grid[row.Index, 0].View = sgdefine.RowHeaderWithoutData;
                    //直接改背景有时不起作用，所以用控制视图改
                    //grid[row.Index, 0].View.BackColor = sgdefine.RowHeadBackColor;

                }
                else
                {
                    grid[row.Index, 0].Controller.RemoveController(pop);
                    PopupMenuForRowHeader menuController = new PopupMenuForRowHeader(row.Index, grid, sgdefine);
                    grid[row.Index, 0].Controller.AddController(menuController);
                    grid[row.Index, 0].Value = row.Index;
                    ////直接改背景有时不起作用，所以用控制视图改
                    grid[row.Index, 0].View = sgdefine.RowHeaderWithData;
                }

                //重新设置行号 跳过列头
                if (row.Index != 0)
                {
                    grid[row.Index, 0].Value = row.Index;
                }

            }

            //要更新总计行数据。实际这暂时是复制单据控制器里的代码。应该可以重构出一个公共的方法
            #region  总计 总计列
            if (grid.HasSummary)
            {
                foreach (SourceGridDefineColumnItem col in sgdefine)
                {
                    if (col.Summary)
                    {
                        #region 总计其中一列
                        decimal totalTemp = 0;
                        //去掉首尾行
                        for (int r = 1; r < grid.RowsCount - 1; r++)
                        {
                            if (grid[r, col.ColIndex].Value != null &&
                                grid.Rows[r].RowData != null
                                )
                            {
                                decimal CurrentTemp = 0;
                                if (decimal.TryParse(grid[r, col.ColIndex].Value.ToString(), out CurrentTemp))
                                {
                                    totalTemp = CurrentTemp + totalTemp;

                                }
                            }
                        }
                        //最后一行
                        grid[grid.RowsCount - 1, col.ColIndex].Value = totalTemp;
                        #endregion
                    }
                }

            }
            #endregion
        }

        public static void SetGridReadOnly(SourceGrid.Grid grid, SourceGridDefine define)
        {

            List<int> rowsIndex = new List<int>();
            foreach (var item in grid.Rows)
            {
                if (define.HasRowHeader)
                {

                }
                rowsIndex.Add(item.Index);
            }
            SetRowReadOnly(grid, rowsIndex.ToArray(), define);

            if (define.HasSummaryRow)
            {

            }

            grid.AutoSizeCells();
        }




        public static void SetRowReadOnly(SourceGrid.Grid grid, int[] rowIndexs, SourceGridDefine define)
        {
            int cc = 0;
            if (define.HasRowHeader)
            {
                cc = 1;
            }
            foreach (int r in rowIndexs)
            {
                for (int c = cc; c < grid.Columns.Count; c++)
                {
                    grid[r, c].Editor = null;
                    grid[r, c].Column.Visible = define[c].Visible;
                }
            }
        }


        /// <summary>
        /// 设置行中哪些列可以编辑
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="rowIndexs"></param>
        /// <param name="define"></param>
        public void SetRowEditable(SourceGrid.Grid grid, int[] rowIndexs, SourceGridDefine define)
        {
            int cc = 0;
            if (define.HasRowHeader)
            {
                cc = 1;
            }
            foreach (int r in rowIndexs)
            {
                for (int c = cc; c < grid.Columns.Count; c++)
                {
                    if (define[c].ColName == "Selected")
                    {
                        continue;
                    }
                    grid[r, c].Column.Visible = define[c].Visible;
                    if (!define[c].ReadOnly)
                    {
                        if (grid[r, c].Editor == null)
                        {
                            grid[r, c].Editor = define[c].EditorForColumn;
                            grid[r, c].Editor.EnableEdit = true;
                        }
                    }
                    else
                    {
                        grid[r, c].Editor = null;
                        //grid[r, c].Editor.EnableEdit = false;
                    }

                }
            }
        }

        /*
        public static void AddRow(SourceGrid.Grid grid, SourceGridDefine define)
        {
            int row = grid.RowsCount;
            grid.Rows.Insert(row);

            for (int i = 0; i < define.Count; i++)
            {
                if (i == 0 && define.HasRowHeader)
                {
                    grid[row, 0] = new SourceGrid.Cells.RowHeader(null);
                    continue;
                }
                grid[row, i] = new SourceGrid.Cells.Cell("", define[i].EditorForColumn);
            }
            grid.Selection.FocusRow(row);
        }
        */


        /// <summary>
        /// 早期代码。只保留参考
        /// </summary>
        /// <param name="dci"></param>
        /// <returns></returns>
        [Obsolete]
        private Cell GetGridCell(SourceGridDefineColumnItem dci)
        {
            //不同情况会有多种类型，先逻辑处理得到最终的类型
            Type newcolType;

            SourceGrid.Cells.Cell c = new SourceGrid.Cells.Cell(null, dci.EditorForColumn);
            System.Reflection.PropertyInfo pi = dci.ColPropertyInfo;
            //==

            // We need to check whether the property is NULLABLE
            if (pi.PropertyType.IsGenericType && pi.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                // If it is NULLABLE, then get the underlying type. eg if "Nullable<int>" then this will return just "int"
                newcolType = pi.PropertyType.GetGenericArguments()[0];
            }
            else
            {
                newcolType = pi.PropertyType;
            }

            #region 参考
            /*
            if (!pi.PropertyType.IsGenericType)
            {
                //非泛型

            }
            else
            {
                //泛型Nullable<>
                Type genericTypeDefinition = pi.PropertyType.GetGenericTypeDefinition();
                if (genericTypeDefinition == typeof(Nullable<>))
                {

                }
                else
                {

                }
            }
            */
            #endregion


            switch (newcolType.FullName)
            {

                case "System.Char":
                case "System.String":
                    c = new SourceGrid.Cells.Cell("", dci.EditorForColumn);


                    break;

                case "System.Guid":

                    break;
                case "System.Decimal":
                case "System.Int16":
                case "System.Int32":
                case "System.Int64":
                    //实体中，判断如果是外键，特别是属性指定绑在中的。
                    //加上一个特性 给出一些参数方便后面自动加载??????
                    //idatamodel = new SourceGrid2.DataModels.EditorTextBoxButton(typeof(string));
                    c = new SourceGrid.Cells.Cell(5, dci.EditorForColumn);
                    break;
                case "System.Byte[]":

                    break;
                case "System.Boolean":
                    //c = new SourceGrid.Cells.CheckBox(null, true);
                    c = new SourceGrid.Cells.Cell(true, typeof(Boolean));
                    break;
                case "System.Double":
                    c = new SourceGrid.Cells.Cell(1.5, typeof(double));
                    break;
                case "System.DateTime":
                    //idatamodel = new SourceGrid2.DataModels.EditorDateTime(typeof(string));
                    //                    c = new SourceGrid.Cells.Cell(DateTime.Now, typeof(DateTime));
                    c = new SourceGrid.Cells.Cell(null, typeof(DateTime));
                    //c.DataModel = idatamodel;
                    break;
                default:
                    break;
            }

            //==


            return c;
        }


        /// <summary>
        /// 初始化时设置每列对应的编辑器 并且设置了委托的给值验证的方法
        /// </summary>
        /// <param name="dci"></param>
        /// <returns></returns>
        private SourceGrid.Cells.Editors.EditorBase SetColumnEditor(SourceGridDefineColumnItem dci)
        {
            if (dci.ColName == null)
            {
                return null;
            }
            //SourceGrid.Cells.Views.Cell captionModel = new SourceGrid.Cells.Views.Cell();
            //captionModel.BackColor = grid.BackColor;

            EditorControlBase _editor = new SourceGrid.Cells.Editors.TextBox(typeof(string));

            System.Reflection.PropertyInfo pi = null;
            Type newcolType;
            if (dci.ColPropertyInfo == null)
            {
                newcolType = typeof(string);
            }
            else
            {
                pi = dci.ColPropertyInfo;
                // We need to check whether the property is NULLABLE
                if (pi.PropertyType.IsGenericType && pi.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                {
                    // If it is NULLABLE, then get the underlying type. eg if "Nullable<int>" then this will return just "int"
                    newcolType = pi.PropertyType.GetGenericArguments()[0];
                }
                else
                {
                    newcolType = pi.PropertyType;
                }

                #region 设置金额等类型的属性提取



                #endregion
            }
            #region 参考
            /*
            if (!pi.PropertyType.IsGenericType)
            {
                //非泛型

            }
            else
            {
                //泛型Nullable<>
                Type genericTypeDefinition = pi.PropertyType.GetGenericTypeDefinition();
                if (genericTypeDefinition == typeof(Nullable<>))
                {

                }
                else
                {

                }
            }
            */
            #endregion

            //这里的类型要对应好。不然可能会引起异常
            //https://www.cnblogs.com/tongdengquan/p/6090544.html SQL数据类型 金额
            #region 优化后的代码  
            //视图的列数据不全，也不需要编辑
            if (dci.SugarCol != null && dci.SugarCol.ColumnDataType != null)
            {

                switch (dci.SugarCol.ColumnDataType)
                {
                    case "datetime":
                        _editor = new SourceGrid.Cells.Editors.TextBoxUITypeEditor(typeof(DateTime));
                        break;
                    case "money":
                        _editor = new SourceGrid.Cells.Editors.TextBoxCurrency(typeof(System.Decimal));
                        //SourceGrid.Cells.Editors.TextBox l_CurrencyEditor = new SourceGrid.Cells.Editors.TextBox(typeof(decimal));
                        //货币格式 默认使用区域设置，如果单独设置了才生效。
                        //设置主要有 小数位数，是否补零。即可
                        CurrencyTypeConverter typeConverter = new DevAge.ComponentModel.Converter.CurrencyTypeConverter(typeof(decimal));
                        typeConverter.AutoAddZero = MainForm.Instance.AppContext.SysConfig.CurrencyDataPrecisionAutoAddZero;
                        _editor.TypeConverter = typeConverter;
                        System.Globalization.CultureInfo cultureInfo = new System.Globalization.CultureInfo("zh-CN", false);
                        NumberFormatInfo myNumberFormatInfo = (NumberFormatInfo)System.Globalization.CultureInfo.CurrentCulture.NumberFormat.Clone();
                        myNumberFormatInfo.CurrencyDecimalDigits = MainForm.Instance.AppContext.SysConfig.MoneyDataPrecision;
                        cultureInfo.NumberFormat = myNumberFormatInfo;
                        // 设置金额的精度
                        _editor.CultureInfo = cultureInfo;
                        break;
                    case "char":
                        _editor = new SourceGrid.Cells.Editors.TextBox(typeof(string));
                        break;
                    case "bigint":
                        if (dci.FKRelationCol != null)
                        {
                            _editor = SetComboxEditor(dci);
                        }
                        break;
                    case "decimal":
                        _editor = new SourceGrid.Cells.Editors.TextBoxNumeric(newcolType);
                        break;
                    case "int":
                        _editor = new SourceGrid.Cells.Editors.TextBoxNumeric(newcolType);
                        break;
                    case "text":
                        _editor = new EditorRichTextInput(typeof(string));
                        break;
                    case "varchar":
                        _editor = new SourceGrid.Cells.Editors.TextBox(typeof(string));
                        //如果字符长度大于多少。则用
                        if (dci.MaxLength > 0 && dci.MaxLength > 200)
                        {
                            _editor = new EditorRichTextInput(typeof(string));
                        }
                        break;
                    case "image":
                        _editor = new SourceGrid.Cells.Editors.ImagePicker();
                        // new SourceGrid.Cells.Image(null);
                        break;
                    case "bit":
                        _editor = new SourceGrid.Cells.Editors.CheckBox(typeof(bool));
                        if (pi.PropertyType.Name == "Nullable`1")
                        {
                            _editor.SetEditValue(null);
                        }


                        //grid1.ClearValues(rr);


                        break;

                    default:
                        _editor = new SourceGrid.Cells.Editors.TextBox(typeof(string));
                        break;
                }
            }
            else
            {
                #region 优化前的代码

                switch (newcolType.FullName)
                {

                    case "System.Char":
                    case "System.String":
                        _editor = new SourceGrid.Cells.Editors.TextBox(typeof(string));
                        //如果字符长度大于多少。则用
                        if (dci.MaxLength > 0 && dci.MaxLength > 200)
                        {
                            _editor = new EditorRichTextInput(typeof(string));

                        }
                        break;

                    case "System.Guid":

                        break;
                    case "System.Decimal":
                        //decimal mon钱 = 234213m;
                        if (dci.SugarCol != null)
                        {
                            if (dci.SugarCol.SqlParameterDbType.ToString() == "money")
                            {
                                _editor = new SourceGrid.Cells.Editors.TextBoxCurrency(typeof(System.Decimal));
                            }
                        }
                        else
                        {
                            _editor = new SourceGrid.Cells.Editors.TextBoxNumeric(typeof(System.Decimal));
                        }

                        break;
                    case "System.Int16":
                    case "System.Int32":
                    case "System.Int64":
                        _editor = new SourceGrid.Cells.Editors.TextBoxNumeric(newcolType);

                        break;
                    case "System.Byte[]":
                        _editor = new SourceGrid.Cells.Editors.ImagePicker();
                        break;
                    case "System.Boolean":
                        _editor = new SourceGrid.Cells.Editors.CheckBox(typeof(bool));
                        //_editor =new SourceGrid.Cells.Editors.
                        //c = new CheckBox(true);
                        if (dci.ColName == "Selected")
                        {
                            DevAge.Drawing.RectangleBorder border = new DevAge.Drawing.RectangleBorder(new DevAge.Drawing.BorderLine(Color.DarkGreen), new DevAge.Drawing.BorderLine(Color.DarkGreen));
                            SourceGrid.Cells.Views.CheckBox checkView = new SourceGrid.Cells.Views.CheckBox();
                            checkView.Background = new DevAge.Drawing.VisualElements.BackgroundLinearGradient(Color.ForestGreen, Color.White, 45);
                            checkView.Border = border;
                            // grid1[r, 2].View = checkView;

                        }

                        break;
                    case "System.Double":
                        _editor = new SourceGrid.Cells.Editors.TextBoxCurrency(typeof(double));
                        break;
                    case "System.DateTime":
                        _editor = new SourceGrid.Cells.Editors.TextBoxUITypeEditor(typeof(DateTime));
                        //idatamodel = new SourceGrid2.DataModels.EditorDateTime(typeof(string));
                        // c = new SourceGrid2.Cells.Real.Cell(DateTime.Today, typeof(DateTime));
                        break;

                    default:
                        _editor = new SourceGrid.Cells.Editors.TextBox(typeof(string));
                        break;
                }

                #endregion
            }

            #endregion
            //格式处理
            switch (dci.CustomFormat)
            {
                case CustomFormatType.DefaultFormat:
                    break;
                case CustomFormatType.PercentFormat:
                    SourceGrid.Cells.Editors.TextBox l_PercentEditor = new SourceGrid.Cells.Editors.TextBox(typeof(decimal));
                    l_PercentEditor.TypeConverter = new DevAge.ComponentModel.Converter.PercentTypeConverter(typeof(decimal));
                    _editor = l_PercentEditor;
                    break;
                case CustomFormatType.CurrencyFormat:
                    //money类型时上面已经处理过了。不要覆盖区域属性
                    if (_editor == null || _editor.CultureInfo == null)
                    {
                        SourceGrid.Cells.Editors.TextBox l_CurrencyEditor = new SourceGrid.Cells.Editors.TextBox(typeof(decimal));
                        l_CurrencyEditor.TypeConverter = new DevAge.ComponentModel.Converter.CurrencyTypeConverter(typeof(decimal));
                        _editor = l_CurrencyEditor;
                    }

                    break;
                case CustomFormatType.DecimalPrecision:
                    break;
                default:
                    break;
            }


            //这里不设置，验证会生效
            _editor.AllowNull = true;
            _editor.EnableEdit = true;
            if (dci.ColName == "Selected")
            {
                _editor.EnableEdit = false;
            }



            //相关联的列才会验证
            foreach (SourceGridDefineColumnItem item in dci.ParentGridDefine)
            {
                if (pi != null)
                {
                    if (pi.Name == item.ColName && !item.ReadOnly && !item.GuideToTargetColumn)
                    {
                        #region 设置下拉类型的值

                        //如果是主键值类型，有外键关系
                        if (newcolType.FullName == "System.Int64" && dci.FKRelationCol != null)
                        {
                            //用上面通用设置方法 SetComboxEditor ，不需要单独设置
                            //这里跳过
                            continue;
                        }

                        #endregion


                        //传入的P是View_prodetail 查询的来源
                        _editor = new UI.UCSourceGrid.EditorQuery(item.ColName, pi.PropertyType);
                        _editor.AllowNull = true;

                        //多选时的处理逻辑
                        ((UI.UCSourceGrid.EditorQuery)_editor).OnSelectMultiRowData += delegate (object rows)
                        {
                            if (rows != null)
                            {
                                var lastlist = ((IEnumerable<dynamic>)rows).ToList();
                                if (lastlist != null && lastlist.Count > 1)
                                {
                                    //多选
                                    if (OnLoadMultiRowData != null)
                                    {
                                        OnLoadMultiRowData(lastlist, _editor.EditPosition);
                                    }
                                }
                            }
                        };





                        #region 验证值
                        _editor.Control.Validating += delegate (object sender, CancelEventArgs cancelEvent)
                        {
                            if (_editor.EditPosition.Column == -1 && _editor.EditPosition.Row == -1)
                            {
                                return;
                            }

                            #region 验证值 不成功就弹出 或清空 
                            SourceGrid.CellContext currContext = new SourceGrid.CellContext(dci.ParentGridDefine.grid, _editor.EditPosition);
                            //注意，这里分两种情况，一种是自动手输的。一种是查询出来的正确值
                            DevAgeTextBoxButton sendControl = sender as DevAge.Windows.Forms.DevAgeTextBoxButton;
                            //返回的值是 两个，一个是具体的值，另一个是选中的对象集合


                            object val = sendControl.Value;
                            if (val == null && _editor.AllowNull)
                            {
                                cancelEvent.Cancel = false;
                                return;
                            }
                            else
                            {
                                //没有修改过，有保存焦点进入时的值并且不是选择过来的
                                if (currContext.CellRange.Start != new Position(-1, -1) && currContext.Tag != null && sendControl.Tag == null)
                                {
                                    if (currContext.Tag.ToString() == val.ToString())
                                    {
                                        cancelEvent.Cancel = false;
                                        return;
                                    }
                                }

                                //先找到主键，通过主键去找
                                SourceGridDefineColumnItem BizKeyCol = dci.ParentGridDefine.CastToList<SourceGridDefineColumnItem>().Where(c => c.IsPrimaryBizKeyColumn).FirstOrDefault();
                                //object dcKeyValue = string.Empty;
                                //if (BizKeyCol == null)
                                //{
                                //    throw new Exception("请设置明细表格中的查询对象业务主键列");
                                //}

                                string valValue = val.ToString();
                                #region 如果只是点来点去，值没有变化。暂时没有办法得到变化了没有。思路变为：如果当前行数据的产品id存在并且 tag为空时，就不需要重复验证
                                //object tempRowData = dci.ParentGridDefine.grid.Rows[_editor.EditPosition.Row].RowData;
                                //if (tempRowData != null)
                                //{
                                //    var cellvalue = RUINORERP.Common.Helper.ReflectionHelper.GetPropertyValue(tempRowData, BizKeyCol.ColName);
                                //    if (cellvalue.ToString().Trim().Length > 0 && sendControl.Tag == null)
                                //    {
                                //        //认为值没有变化
                                //        return;
                                //    }
                                //}

                                #endregion

                                //TODO这里是否应用缓存？
                                object proObj = null;
                                #region 选择出来的明细
                                if (sendControl.Tag != null)
                                {
                                    var rowlist = ((IEnumerable<dynamic>)sendControl.Tag).ToList();
                                    if (rowlist.Count == 0)
                                    {
                                        return;
                                    }

                                    object PObj = rowlist[0];
                                    var Pvalue = RUINORERP.Common.Helper.ReflectionHelper.GetPropertyValue(PObj, item.ColName);
                                    if (Pvalue.ToString() == valValue)//暂时以字符串来比较
                                    {
                                        /*
                                        #region 判断是否是重复的行值,
                                        var dcKeyValue = ReflectionHelper.GetPropertyValue(PObj, BizKeyCol.ColName);

                                        //循环找已经设置好的行的值对象，按主键去找
                                        foreach (var dgdr in dci.ParentGridDefine.grid.Rows)
                                        {
                                            //排除新行和自己行
                                            if (dgdr.RowData == null || dgdr.Index == _editor.EditPosition.Row)
                                            {
                                                continue;
                                            }
                                            var rowObj = dgdr.RowData;
                                            //也找到主键列的值来比较
                                            var rowvalueforKeyCol = ReflectionHelper.GetPropertyValue(rowObj, BizKeyCol.ColName);
                                            //如果通过主键找并且不与自己相同的行比，
                                            if (rowvalueforKeyCol.ToString() == dcKeyValue.ToString() && dgdr.Index != _editor.EditPosition.Row)
                                            {
                                                //如果他的行里的对象，存在这个值了，则不能重复添加
                                                cancelEvent.Cancel = true;
                                                MainForm.Instance.uclog.AddLog("数据行不能重复添加，请按【ESC】，取消当前行信息录入！", Global.UILogType.警告);
                                                return;
                                            }
                                        }
                                        #endregion
                                        */
                                        proObj = PObj;
                                        //TODO
                                        //实时改变当前行指定列的下拉值的范围？
                                        //找到 KEY VALUE  KEY:是条件，value是目标。比方验证时由产品ID决定BOM显示下拉情况
                                        foreach (var col in dci.ParentGridDefine.LimitedConditionsForSelectionRange)
                                        {
                                            if (col.Value.SugarCol == null)
                                            {
                                                continue;
                                            }
                                            if (col.Value.SugarCol.ColumnDataType == null)
                                            {
                                                continue;
                                            }

                                            if (col.Value.SugarCol.ColumnDataType == "bigint"
                                            && col.Value.EditorForColumn is SourceGrid.Cells.Editors.ComboBox)
                                            {
                                                SourceGrid.Cells.Editors.ComboBox cmbList = col.Value.EditorForColumn as SourceGrid.Cells.Editors.ComboBox;
                                                //如果有列的特殊设置。则将数据源重新设置
                                                //手动构造
                                                var limitedValue = ReflectionHelper.GetPropertyValue(PObj, col.Key.ColName);

                                                #region  实时修改下拉列表的值

                                                string tableName = col.Value.FKRelationCol.FKTableName;
                                                string typeName = "RUINORERP.Model." + tableName;

                                                if (CacheHelper.Manager.NewTableList.ContainsKey(tableName))
                                                {
                                                    string ColID = CacheHelper.Manager.NewTableList[tableName].Key;
                                                    string ColName = CacheHelper.Manager.NewTableList[tableName].Value;
                                                    BindingSource bs = new BindingSource();
                                                    var objlist = CacheHelper.Manager.CacheEntityList.Get(tableName);
                                                    if (objlist != null)
                                                    {
                                                        var Oldlist = ((IEnumerable<dynamic>)objlist).ToList();
                                                        //限制范围 https://www.cnblogs.com/zhanglb163/p/12839040.html

                                                        //性能最好
                                                        // var tlist = Oldlist.Where(m => m.ProdDetailID == limitedValue.ToLong()).ToList();

                                                        //性能其次 但是Model是明细类是动态的。
                                                        //var lambda = DynamicExpression.ParseLambda<Model, bool>("name.StartsWith(@0)", "1");
                                                        //var fun = expfun.Compile();
                                                        //list.Where(s => fun(s)).ToList();

                                                        //性能最差
                                                        //var tlist = Oldlist.AsQueryable().Where(col.Value.ColName + "==@0", 1742079575489384449).ToList();


                                                        var tlist = Oldlist;

                                                        //下面是动态查询,但是硬编码才性能最好。并且代码可行。所以这种由
                                                        //一个列的内容决定另一个列的内容。但是条件中，注意对应字段是否存在另一个集合中，
                                                        //有时，外键字段可能不存在另一个集合中。因为名称改了。如 单位，与单位换算中的单位ID就不一样。
                                                        //条件是业务主键时
                                                        //动态决定下拉值。有两种情况。一种是验证时。这时通过产品明细ID来限制。或其它字段。
                                                        //另一种是实时修改时,用selectIndex?
                                                        switch (col.Key.ColName)
                                                        {
                                                            case "ProdDetailID":
                                                                tlist = Oldlist.Where(m => m.ProdDetailID == limitedValue.ToLong()).ToList();
                                                                break;
                                                                //case "Unit_ID"://特别写列。反正这里是硬编码 Unit_ID 在换算表中是指向 Source_unit_id
                                                                //    tlist = Oldlist.Where(m => m.Source_unit_id == limitedValue.ToLong()).ToList();
                                                                break;
                                                            default:
                                                                //throw new Exception("请实现限制时：" + col.Key.ColName + "的动态查询");
                                                                break;
                                                        }


                                                        List<string> ids = new List<string>();
                                                        ConcurrentDictionary<string, string> names = new ConcurrentDictionary<string, string>();
                                                        foreach (var titem in tlist)
                                                        {
                                                            string id = ReflectionHelper.GetPropertyValue(titem, ColID).ToString();
                                                            ids.Add(id.ToString());
                                                            names.TryAdd(id, ReflectionHelper.GetPropertyValue(titem, ColName).ToString());
                                                        }


                                                        //这个要写在绑定验证的前面。这样会指定下拉列表的值
                                                        cmbList.StandardValues = ids.ToArray();

                                                        DevAge.ComponentModel.Validator.ComboxValueMapping comboMapping = new DevAge.ComponentModel.Validator.ComboxValueMapping(true);
                                                        comboMapping.ValueList = names;
                                                        comboMapping.DisplayStringList = ids;

                                                        //先解绑验证。不然会多次执行。因为开始就有绑定
                                                        comboMapping.UnBindValidator(cmbList);
                                                        comboMapping.BindValidator(cmbList);


                                                        #region  添加自动完成功能 智能提示功能

                                                        AutoCompleteStringCollection autoscList = new AutoCompleteStringCollection();
                                                        foreach (var nameitem in names)
                                                        {
                                                            autoscList.Add(nameitem.Value);
                                                        }
                                                        DevAge.Windows.Forms.DevAgeComboBox cmb = (DevAge.Windows.Forms.DevAgeComboBox)cmbList.Control;
                                                        //cmb.BeginUpdate();
                                                        //cmb.DisplayMember = DisplayMember;
                                                        //cmb.ValueMember = ValueMember;
                                                        //cmb.DataSource = names;
                                                        cmb.DropDownStyle = ComboBoxStyle.DropDown;
                                                        cmb.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
                                                        cmb.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
                                                        cmb.AutoCompleteCustomSource = autoscList;
                                                        //cmb.EndUpdate();
                                                        #endregion



                                                    }
                                                }

                                                #endregion
                                            }
                                        }

                                    }
                                }
                                #endregion

                                #region 手动输入的
                                if (sendControl.Tag == null)
                                {
                                    foreach (var Source in dci.ParentGridDefine.SourceList)
                                    {
                                        var Pvalue = RUINORERP.Common.Helper.ReflectionHelper.GetPropertyValue(Source, item.ColName);
                                        if (Pvalue.ToString() == valValue)//暂时以字符串来比较
                                        {
                                            /*
                                            #region 判断是否是重复的行值,
                                            var dcKeyValue = ReflectionHelper.GetPropertyValue(Source, BizKeyCol.ColName);
                                            //循环找已经设置好的行的值对象，按主键去找
                                            foreach (var dgdr in dci.ParentGridDefine.grid.Rows)
                                            {
                                                if (dgdr.RowData == null)
                                                {
                                                    continue;
                                                }
                                                var rowObj = dgdr.RowData;
                                                //也找到主键列的值来比较
                                                var rowvalueforKeyCol = ReflectionHelper.GetPropertyValue(rowObj, BizKeyCol.ColName);
                                                //如果通过主键找并且不与自己相同的行比，
                                                if (rowvalueforKeyCol.ToString() == dcKeyValue.ToString() && dgdr.Index != _editor.EditPosition.Row)
                                                {
                                                    //如果他的行里的对象，存在这个值了，则不能重复添加
                                                    cancelEvent.Cancel = true;
                                                    MainForm.Instance.uclog.AddLog("数据行不能重复添加，请按【ESC】，取消当前行信息录入！", Global.UILogType.警告);
                                                    return;
                                                }
                                            }
                                            #endregion
                                            */
                                            proObj = Source;
                                            break;
                                        }
                                    }
                                }
                                #endregion


                                if (proObj != null)
                                {
                                    sendControl.Tag = null;
                                    //dci.ParentGridDefine.SetDependTargetValue(val, _editor.EditPosition, proObj, dci.ColName);
                                    SetCellValue(dci, _editor.EditPosition, proObj, false);

                                    #region 关联列的赋值处理 ,特殊情况具体调用时实现 
                                    //多选
                                    if (OnLoadRelevantFields != null && _editor.EditPosition.Column != -1 && _editor.EditPosition.Row != -1)
                                    {
                                        OnLoadRelevantFields(proObj, dci.ParentGridDefine.grid[_editor.EditPosition].Row.RowData, dci.ParentGridDefine, _editor.EditPosition);
                                    }

                                    #endregion
                                    currContext.EndEdit(false);
                                }
                                else
                                {
                                    MainForm.Instance.uclog.AddLog("提示", "产品信息不存在。请重新打开程序重试。");
                                    sendControl.TextBox.SelectAll();
                                    using (QueryFormGeneric dg = new QueryFormGeneric())
                                    {
                                        dg.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
                                        dg.prodQuery.QueryField = item.ColName;
                                        //设置一下默认仓库，思路是，首先保存了主表的数据对象，然后拿到主表的仓库字段
                                        //这里是不是可以设置为事件来驱动,的并且可以指定字段
                                        Expression<Func<View_ProdDetail, object>> warehouse = x => x.Location_ID;
                                        if (dci.ParentGridDefine.DefineColumns.FirstOrDefault(c => c.ColName == warehouse.GetMemberInfo().Name) != null)
                                        {
                                            if (dci.ParentGridDefine.GridData != null)
                                            {
                                                dg.prodQuery.LocationID = dci.ParentGridDefine.GridData.GetPropertyValue(warehouse.GetMemberInfo().Name).ToLong();
                                            }

                                        }

                                        if (dg.ShowDialog() == DialogResult.OK)
                                        {
                                            sendControl.Tag = dg.prodQuery.QueryObjects;
                                            sendControl.Value = dg.prodQuery.QueryValue;
                                        }
                                        else
                                        {
                                            cancelEvent.Cancel = false;
                                            sendControl.TextBox.Text = string.Empty;
                                            //清空关联的UI上的值，即就是用产品详情带出来的值
                                            dci.ParentGridDefine.SetDependTargetValue(null, _editor.EditPosition, null, dci.ColName);
                                            return;
                                        }
                                    }
                                    cancelEvent.Cancel = true;
                                }
                            }
                            #endregion
                        };

                        _editor.Control.KeyDown += delegate (object sender, KeyEventArgs e)
                        {
                            #region 编辑时的键盘事件
                            SourceGrid.CellContext currContext = new SourceGrid.CellContext(dci.ParentGridDefine.grid, _editor.EditPosition);
                            currContext.EndEdit(false);
                            //sendControl.Button.Visible = false;
                            /*
                              Position pt = new Position(sender.Position.Row, sender.Position.Column);
                              if (e.KeyCode == Keys.Up)
                              {
                                  pt = new Position(sender.Position.Row - 1, sender.Position.Column);
                                  sender.Grid.Selection.Focus(pt, false);
                                  e.Handled = true;
                              }
                              if (e.KeyCode == Keys.Down)
                              {
                                  pt = new Position(sender.Position.Row + 1, sender.Position.Column);
                                  sender.Grid.Selection.Focus(pt, false);
                                  e.Handled = true;
                              }
                              if (e.KeyCode == Keys.Left)
                              {
                                  pt = new Position(sender.Position.Row, sender.Position.Column - 1);
                                  sender.Grid.Selection.Focus(pt, false);
                                  e.Handled = true;
                              }
                              if (e.KeyCode == Keys.Right)
                              {
                                  pt = new Position(sender.Position.Row, sender.Position.Column + 1);
                                  sender.Grid.Selection.Focus(pt, false);
                                  e.Handled = true;
                              }*/
                            #endregion
                        };

                        #endregion
                        break;
                    }

                }
            }

            //==
            _editor.EditableMode = dci.EditableMode;

            if (dci.ReadOnly)
            {
                _editor = null;
            }
            dci.ParentGridDefine.ColEditors.Add(new KeyValuePair<string, EditorBase>(dci.ColName, _editor));
            dci.EditorForColumn = _editor;

            return _editor;
        }




        /// <summary>
        /// 设置下拉的数据源
        /// </summary>
        /// <param name="dci"></param>
        /// <returns></returns>
        private SourceGrid.Cells.Editors.ComboBox SetComboxEditor(SourceGridDefineColumnItem dci)
        {
            string tableName = dci.FKRelationCol.FKTableName;
            string typeName = "RUINORERP.Model." + tableName;
            var _editor = new SourceGrid.Cells.Editors.ComboBox(typeof(string));

            if (CacheHelper.Manager.NewTableList.ContainsKey(tableName))
            {
                string ColID = CacheHelper.Manager.NewTableList[tableName].Key;
                string ColName = CacheHelper.Manager.NewTableList[tableName].Value;
                BindingSource bs = new BindingSource();
                var objlist = CacheHelper.Manager.CacheEntityList.Get(tableName);
                if (objlist != null)
                {
                    var tlist = ((IEnumerable<dynamic>)objlist).ToList();
                    List<string> ids = new List<string>();
                    ConcurrentDictionary<string, string> OutNames = new ConcurrentDictionary<string, string>();
                    foreach (var item in tlist)
                    {

                        //假如是库位选择  有一个没有启用。但是又要显示原来选择过的数据用于显示。编辑时不能选择没有启用的库位。如何处理实际是如何呢？
                        //这里要不要利用process中设置的条件来判断呢？


                        string id = ReflectionHelper.GetPropertyValue(item, ColID).ToString();
                        ids.Add(id.ToString());//设置一个主键集合 
                        OutNames.TryAdd(id, ReflectionHelper.GetPropertyValue(item, ColName).ToString());//设置一个显示名称的集合
                    }
                    if (tlist == null || tlist.Count == 0)
                    {
                        Business.CommService.CommonController bdc = Startup.GetFromFac<Business.CommService.CommonController>();
                        var list = bdc.GetBindSourceList(tableName);
                    }
                    else
                    {
                        SourceGrid.Cells.Editors.ComboBox ec = new SourceGrid.Cells.Editors.ComboBox(typeof(List<long>), ids.ToArray(), true);
                        ec.Control.FormattingEnabled = true;

                        DevAge.ComponentModel.Validator.ComboxValueMapping comboMapping = new DevAge.ComponentModel.Validator.ComboxValueMapping(true);
                        comboMapping.ValueList = OutNames;
                        comboMapping.DisplayStringList = ids;
                        comboMapping.BindValidator(ec);

                        #region  添加自动完成功能 智能提示功能

                        AutoCompleteStringCollection autoscList = new AutoCompleteStringCollection();
                        foreach (var item in OutNames)
                        {
                            autoscList.Add(item.Value);
                        }
                        DevAge.Windows.Forms.DevAgeComboBox cmb = (DevAge.Windows.Forms.DevAgeComboBox)ec.Control;
                        //cmb.BeginUpdate();
                        //cmb.DisplayMember = DisplayMember;
                        //cmb.ValueMember = ValueMember;
                        //cmb.DataSource = OutNames;
                        cmb.DropDownStyle = ComboBoxStyle.DropDown;
                        cmb.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
                        cmb.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
                        cmb.AutoCompleteCustomSource = autoscList;
                        //cmb.EndUpdate();
                        #endregion


                        #region  多级联动？
                        //没有触发。是因为绑定数据源？
                        ec.Control.DrawItem += delegate (object sender, DrawItemEventArgs e)
                        {

                        };

                        ec.Control.DropDown += delegate (object sender, EventArgs e)
                        {
                            int a = 1;
                        };

                        ec.Control.SelectedIndexChanged += delegate (object sender, EventArgs e)
                        {

                            Position position = _editor.EditPosition;
                            //这里是可以知道条件的下拉及其值，控制目标的下拉显示
                            SourceGridDefineColumnItem targetCol = null;
                            dci.ParentGridDefine.LimitedConditionsForSelectionRange.TryGetValue(dci, out targetCol);
                            if (targetCol != null)
                            {
                                if (targetCol.SugarCol != null && targetCol.SugarCol.ColumnDataType != null)
                                {
                                    if (targetCol.SugarCol.ColumnDataType == "bigint"
                                    && targetCol.EditorForColumn is SourceGrid.Cells.Editors.ComboBox)
                                    {
                                        SourceGrid.Cells.Editors.ComboBox cmbList = targetCol.EditorForColumn as SourceGrid.Cells.Editors.ComboBox;
                                        //如果有列的特殊设置。则将数据源重新设置
                                        //手动构造
                                        var limitedValue = ec.Control.SelectedItem;
                                        #region  实时修改下拉列表的值
                                        string MytableName = targetCol.FKRelationCol.FKTableName;
                                        string MytypeName = "RUINORERP.Model." + MytableName;

                                        if (CacheHelper.Manager.NewTableList.ContainsKey(MytableName))
                                        {
                                            string MColID = CacheHelper.Manager.NewTableList[MytableName].Key;
                                            string MColName = CacheHelper.Manager.NewTableList[MytableName].Value;
                                            BindingSource Mbs = new BindingSource();
                                            var Mobjlist = CacheHelper.Manager.CacheEntityList.Get(MytableName);
                                            if (Mobjlist != null)
                                            {
                                                var Oldlist = ((IEnumerable<dynamic>)Mobjlist).ToList();
                                                //限制范围 https://www.cnblogs.com/zhanglb163/p/12839040.html

                                                var otlist = Oldlist;

                                                //下面是动态查询,但是硬编码才性能最好。并且代码可行。所以这种由
                                                //一个列的内容决定另一个列的内容。但是条件中，注意对应字段是否存在另一个集合中，
                                                //有时，外键字段可能不存在另一个集合中。因为名称改了。如 单位，与单位换算中的单位ID就不一样。
                                                //条件是业务主键时
                                                switch (targetCol.ColName)
                                                {
                                                    case "ProdDetailID":
                                                        otlist = Oldlist.Where(m => m.ProdDetailID == limitedValue.ToLong()).ToList();
                                                        break;
                                                    case "UnitConversion_ID"://特别写列。反正这里是硬编码 Unit_ID 在换算表中是指向 Source_unit_id
                                                        otlist = Oldlist.Where(m => m.Source_unit_id == limitedValue.ToLong()).ToList();
                                                        break;
                                                    default:
                                                        throw new Exception("请实现限制时：" + targetCol.ColName + "的动态查询");
                                                        break;
                                                }


                                                List<string> Myids = new List<string>();
                                                ConcurrentDictionary<string, string> Mynames = new ConcurrentDictionary<string, string>();
                                                foreach (var item in otlist)
                                                {
                                                    string id = ReflectionHelper.GetPropertyValue(item, ColID).ToString();
                                                    Myids.Add(id.ToString());
                                                    Mynames.TryAdd(id, ReflectionHelper.GetPropertyValue(item, ColName).ToString());
                                                }


                                                //这个要写在绑定验证的前面。这样会指定下拉列表的值
                                                cmbList.StandardValues = Myids.ToArray();

                                                DevAge.ComponentModel.Validator.ComboxValueMapping MycomboMapping = new DevAge.ComponentModel.Validator.ComboxValueMapping(true);
                                                MycomboMapping.ValueList = Mynames;
                                                MycomboMapping.DisplayStringList = Myids;

                                                //先解绑验证。不然会多次执行。因为开始就有绑定
                                                MycomboMapping.UnBindValidator(cmbList);
                                                MycomboMapping.BindValidator(cmbList);


                                                #region  添加自动完成功能 智能提示功能

                                                AutoCompleteStringCollection MyautoscList = new AutoCompleteStringCollection();
                                                foreach (var item in Mynames)
                                                {
                                                    MyautoscList.Add(item.Value);
                                                }
                                                DevAge.Windows.Forms.DevAgeComboBox Mycmb = (DevAge.Windows.Forms.DevAgeComboBox)cmbList.Control;
                                                //cmb.BeginUpdate();
                                                //cmb.DisplayMember = DisplayMember;
                                                //cmb.ValueMember = ValueMember;
                                                //cmb.DataSource = Mynames;
                                                Mycmb.DropDownStyle = ComboBoxStyle.DropDown;
                                                Mycmb.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
                                                Mycmb.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
                                                Mycmb.AutoCompleteCustomSource = autoscList;
                                                //cmb.EndUpdate();
                                                #endregion



                                            }
                                        }

                                        #endregion
                                    }
                                }
                            }




                        };

                        #endregion
                        _editor = ec;





                        /*
                        int[] arrInt = new int[] { 0, 1, 2, 3, 4 };
                        string[] arrStr = new string[] { "0 - Zero", "1 - One", "2 - Two", "3 - Three", "4- Four" };
                        SourceGrid.Cells.Editors.ComboBox editorComboCustomDisplay = new SourceGrid.Cells.Editors.ComboBox(typeof(int), arrInt, true);
                        editorComboCustomDisplay.Control.FormattingEnabled = true;
                        DevAge.ComponentModel.Validator.ValueMapping comboMapping = new DevAge.ComponentModel.Validator.ValueMapping();
                        comboMapping.DisplayStringList = arrStr;
                        comboMapping.ValueList = arrInt;
                        comboMapping.SpecialList = arrStr;
                        comboMapping.SpecialType = typeof(string);
                        comboMapping.BindValidator(editorComboCustomDisplay);
                        editorComboCustomDisplay.SetEditValue(0);
                        _editor = editorComboCustomDisplay;*/

                        // InsertSelectItem<T>(key, value, tlist);
                        //bs.DataSource = tlist;
                    }
                }
            }
            return _editor;
        }



        /// <summary>
        /// 设置从公共部分指向单据明细的列集合 比方公共库位指定明细库位 值传过去
        /// </summary>
        /// <typeparam name="ProdcutShare"></typeparam>
        /// <typeparam name="BillDetail"></typeparam>
        /// <param name="define"></param>
        /// <param name="fromExp"></param>
        /// <param name="toExp"></param>
        public void SetPointToColumnPairs<ProdcutShare, BillDetail>(SourceGridDefine define, Expression<Func<ProdcutShare, object>> fromExp, Expression<Func<BillDetail, object>> toExp)
        {
            string fromColName = fromExp.GetMemberInfo().Name;
            string toColName = toExp.GetMemberInfo().Name;
            SourceGridDefineColumnItem fromdc = define.DefineColumns.Where(c => c.ColName == fromColName && c.BelongingObjectType.Name == typeof(ProdcutShare).Name).FirstOrDefault();
            if (fromdc == null)
            {
                return;
            }
            if (!define.PointToColumnPairList.ContainsKey(fromdc))
            {
                fromdc.Visible = false;
                fromdc.NeverVisible = true;
                SourceGridDefineColumnItem todc = define.DefineColumns.Where(c => c.ColName == toColName && c.BelongingObjectType.Name == typeof(BillDetail).Name).FirstOrDefault();
                if (todc == null)
                {
                    MainForm.Instance.uclog.AddLog("提醒", $"当前字段{toColName}没有提取到,请确认在单据明细{typeof(BillDetail).Name}中描述是否为空");
                    return;
                }
                define.PointToColumnPairList.TryAdd(fromdc, todc);
            }
        }


        /// <summary>
        /// 设置从查询结果对象指向单据明细的列集合
        /// 即：从产品查询QueryFormGeneric查出来的结果。放到 指定明细中某个列中
        /// </summary>
        /// <typeparam name="ProdcutShare"></typeparam>
        /// <typeparam name="BillDetail"></typeparam>
        /// <param name="define"></param>
        /// <param name="fromExp"></param>
        /// <param name="toExp"></param>
        public void SetQueryItemToColumnPairs<T, BillDetail>(SourceGridDefine define, Expression<Func<T, object>> fromExp, Expression<Func<BillDetail, object>> toExp)
        {
            string fromColName = fromExp.GetMemberInfo().Name;
            string toColName = toExp.GetMemberInfo().Name;
            if (!define.QueryItemToColumnPairList.ContainsKey(fromColName))
            {
                SourceGridDefineColumnItem todc = define.DefineColumns.Where(c => c.ColName == toColName && c.BelongingObjectType.Name == typeof(BillDetail).Name).FirstOrDefault();
                if (todc == null)
                {
                    MainForm.Instance.uclog.AddLog("提醒", $"当前字段{toColName}没有提取到,请确认在单据明细{typeof(BillDetail).Name}中描述是否为空");
                    return;
                }
                define.QueryItemToColumnPairList.TryAdd(fromColName, todc);
            }
        }



        /// <summary>
        /// 设置实时数据决定下拉选择的内容。根据传入的表达式和当前行的值
        /// </summary>
        /// <typeparam name="ProdcutShare"></typeparam>
        /// <typeparam name="BillDetail"></typeparam>
        /// <param name="define"></param>
        /// <param name="fromExp"></param>
        /// <param name="toExp"></param>
        public void SetCol_LimitedConditionsForSelectionRange<BillDetail>(SourceGridDefine define, Expression<Func<BillDetail, object>> ConditionFromExp, Expression<Func<BillDetail, object>> ToExp)
        {
            string ToColName = ToExp.GetMemberInfo().Name;
            string ConditionFromColName = ConditionFromExp.GetMemberInfo().Name;
            SourceGridDefineColumnItem fromdc = define.DefineColumns.Where(c => c.ColName == ConditionFromColName && c.BelongingObjectType.Name == typeof(BillDetail).Name).FirstOrDefault();
            if (fromdc == null)
            {
                return;
            }
            if (!define.LimitedConditionsForSelectionRange.ContainsKey(fromdc))
            {
                //fromdc.Visible = false;
                //fromdc.NeverVisible = true;
                SourceGridDefineColumnItem todc = define.DefineColumns.Where(c => c.ColName == ToColName && c.BelongingObjectType.Name == typeof(BillDetail).Name).FirstOrDefault();
                if (todc == null)
                {
                    return;
                }
                define.LimitedConditionsForSelectionRange.TryAdd(fromdc, todc);
            }
        }

        /// <summary>
        /// 同步更新单元格的值
        /// </summary>
        /// <typeparam name="C"></typeparam>
        /// <param name="sgdefine"></param>
        /// <param name="col"></param>
        /// <param name="details"></param>
        public void SynchronizeUpdateCellValue<C>(SourceGridDefine sgdefine, Expression<Func<C, object>> col, List<C> details)
        {
            List<SourceGridDefineColumnItem> cols = sgdefine.DefineColumns.Where(c => c.ColName == col.GetMemberInfo().Name).ToList();
            List<object> list = new List<object>();
            foreach (var item in details)
            {
                list.Add(item as object);
            }
            SynchronizeUpdateCellValue(sgdefine, cols, list);
        }

        //更新一列多行,有一个唯一的业务主键？
        public void SynchronizeUpdateCellValue(SourceGridDefine sgdefine, List<SourceGridDefineColumnItem> cols, List<object> details)
        {
            //SourceGrid.Position startpos,
            //       SourceGridDefine sgdefine = dc.ParentGridDefine;
            //sgdefine.grid[p.Row, qtc.Value.ColIndex].Value = newTagetValue;
            foreach (object detail in details)
            {
                foreach (SourceGridDefineColumnItem dc in cols)
                {
                    int rowindex = 0;
                    for (global::System.Int32 i = 0; i < sgdefine.grid.Rows.Count; i++)
                    {
                        if (sgdefine.grid.Rows[i].RowData == detail)
                        {
                            rowindex = sgdefine.grid.Rows[i].Index;
                            break;
                        }
                    }
                    sgdefine.grid[rowindex, dc.ColIndex].Value = detail.GetPropertyValue(dc.ColName);
                }
            }
        }

        /*
                //更新多行多列
                public void SynchronizeUpdateCellValue(SourceGridDefineColumnItem dc, SourceGrid.Position p, object rowObj, bool isbatch, bool isOnlyPointColumn = false)
                {

                }

                //更新一行多列
                public void SynchronizeUpdateCellValue(SourceGridDefineColumnItem dc, SourceGrid.Position p, object rowObj, bool isbatch, bool isOnlyPointColumn = false)
                {

                }

                //更新一行一列
                public void SynchronizeUpdateCellValue(SourceGridDefineColumnItem dc, SourceGrid.Position p, object rowObj, bool isbatch, bool isOnlyPointColumn = false)
                {

                }
           */

        /// <summary>
        /// 设置单元格的值
        /// </summary>
        /// <param name="dc">指定列</param>
        /// <param name="p"></param>
        /// <param name="rowObj"></param>
        /// <param name="isbatch"></param>
        /// <param name="isOnlyPointColumn">是否只设置指定列的值</param>
        public void SetCellValue(SourceGridDefineColumnItem dc, SourceGrid.Position p, object rowObj, bool isbatch, bool isOnlyPointColumn = false)
        {
            if (p.Column == -1 || p.Row == -1)
            {
                return;
            }
            SourceGridDefine sgdefine = dc.ParentGridDefine;
            foreach (SourceGridDefineColumnItem item in sgdefine)
            {
                if (isOnlyPointColumn)
                {
                    if (item.ColName != dc.ColName)
                    {
                        continue;
                    }
                }
                if (item.ColCaption == "载账数量")
                {

                }
                if (item.IsRowHeaderCol)
                {
                    continue;
                }
                //这里是设置明细中的主要业务字段
                if ((item.GuideToTargetColumn && !sgdefine.PointToColumnPairList.ContainsKey(item) && !item.IsPrimaryBizKeyColumn))
                {
                    //这里给默认值  明细中的。如数量为0？

                    #region 目标列要修改对应的绑定数据对象

                    if (sgdefine.grid.Rows[p.Row].RowData != null)
                    {
                        //设置目标的绑定数据值，就是产品ID
                        SourceGrid.CellContext processDefaultContext = new SourceGrid.CellContext(sgdefine.grid, new Position(p.Row, item.ColIndex));

                        var currentObj = sgdefine.grid.Rows[p.Row].RowData;
                        var cellDefaultValue = ReflectionHelper.GetPropertyValue(currentObj, item.ColName);

                        if (cellDefaultValue != null && !item.IsFKRelationColumn && cellDefaultValue.IsNotEmptyOrNull())
                        {
                            sgdefine.grid[p.Row, item.ColIndex].Value = cellDefaultValue;
                            switch (item.CustomFormat)
                            {
                                case CustomFormatType.DefaultFormat:
                                    break;
                                case CustomFormatType.PercentFormat:
                                    decimal pf = decimal.Parse(cellDefaultValue.ToString());
                                    sgdefine.grid[p.Row, item.ColIndex].Value = pf;
                                    break;
                                case CustomFormatType.CurrencyFormat:
                                    decimal cf = decimal.Parse(cellDefaultValue.ToString());
                                    sgdefine.grid[p.Row, item.ColIndex].Value = cf;
                                    break;
                                case CustomFormatType.DecimalPrecision:
                                    break;
                                case CustomFormatType.Bool:
                                    bool bl = cellDefaultValue.ToBool();
                                    if (bl == true)
                                    {
                                        sgdefine.grid[p.Row, item.ColIndex].DisplayText = "是";
                                    }
                                    else
                                    {
                                        sgdefine.grid[p.Row, item.ColIndex].DisplayText = "否";
                                    }
                                    break;
                                default:
                                    break;
                            }

                        }
                        //针对特殊的选择列，用一个控制器来给行对象值。
                        if (item.ColName == "Selected")
                        {
                            sgdefine.grid[p.Row, item.ColIndex].Editor = item.EditorForColumn;
                            sgdefine.grid[p.Row, item.ColIndex].Editor.EnableEdit = true;
                            sgdefine.grid[p.Row, item.ColIndex] = new SourceGrid.Cells.CheckBox(null, false);
                            if (sgdefine.grid[p.Row, item.ColIndex].Editor != null)
                            {
                                if (sgdefine.grid[p.Row, item.ColIndex].FindController(typeof(SelectedForCheckBoxController)) == null)
                                {
                                    SelectedForCheckBoxController SeletedController = new SelectedForCheckBoxController("Seleted", sgdefine);
                                    SeletedController.OnValidateDataCell += (MycurrentObj) =>
                                    {

                                    };
                                    sgdefine.grid[p.Row, item.ColIndex].AddController(SeletedController);
                                }

                            }

                        }
                        ///默认值处理
                        if (item.DefaultValue != null && item.GuideToTargetColumn)
                        {
                            var setcurrentObj = sgdefine.grid.Rows[p.Row].RowData;
                            //如果值是空的，就给默认值，时间要特殊处理
                            if (setcurrentObj != null && setcurrentObj.GetType().GetProperty(item.ColName) == null)
                            {
                                ReflectionHelper.SetPropertyValue(setcurrentObj, item.ColName, item.DefaultValue);
                            }
                            if (setcurrentObj != null && setcurrentObj.GetType().GetProperty(item.ColName) != null)
                            {
                                //时间为默认值的情况，格式不带时间
                                if (item.ColPropertyInfo.PropertyType == typeof(DateTime))
                                {
                                    if (setcurrentObj.GetPropertyValue(item.ColName).ToDateTime().Year == 1)
                                    {
                                        ReflectionHelper.SetPropertyValue(setcurrentObj, item.ColName, item.DefaultValue);
                                    }
                                    sgdefine.grid[p.Row, item.ColIndex].DisplayText = string.Format("{0:yyyy-MM-dd}", item.DefaultValue);
                                }

                            }

                            if (item.CustomFormat == CustomFormatType.CurrencyFormat)
                            {
                                sgdefine.grid[p.Row, item.ColIndex].DisplayText = string.Format("{0:C}", item.DefaultValue);
                            }
                            sgdefine.grid[p.Row, item.ColIndex].Value = item.DefaultValue;
                        }



                        //如果这个列属性的名称在查询结果中指定的集合中匹配。就设置一下目标
                        //查询结果字段名，如盘点单中的 查出来的数量，（实际库存，认为是载帐数量），指定以明细中的载帐数量
                        if (sgdefine.QueryItemToColumnPairList.Values.Contains(item))
                        {
                            if (sgdefine.grid.Rows[p.Row].RowData != null)
                            {

                                //值是来自于指定的列名
                                var qtc = sgdefine.QueryItemToColumnPairList.Where(v => v.Value == item).FirstOrDefault();

                                var newTagetValue = ReflectionHelper.GetPropertyValue(rowObj, qtc.Key);
                                //公共部分。只是显示用来看
                                sgdefine.grid[p.Row, item.ColIndex].Value = newTagetValue;

                                //目标存于明细中。是保存进数据库的
                                ReflectionHelper.SetPropertyValue(currentObj, qtc.Value.ColName, newTagetValue);
                                sgdefine.grid[p.Row, qtc.Value.ColIndex].Value = newTagetValue;

                            }
                        }

                    }



                    #endregion

                    continue;
                }
                #region 设置表格UI中的其他关联列的值
                SourceGrid.Position pt = new Position(p.Row, item.ColIndex);
                SourceGrid.CellContext processContext = new SourceGrid.CellContext(sgdefine.grid, pt);
                string displaytxt = string.Empty;
                object newValue = string.Empty;
                newValue = ReflectionHelper.GetPropertyValue(rowObj, item.ColName);

                if (item.IsFKRelationColumn && !string.IsNullOrEmpty(newValue.ToString()))
                {
                    displaytxt = ShowFKColumnText(item, newValue, sgdefine);
                }

                if (item.ColPropertyInfo.PropertyType.FullName == "System.Byte[]")
                {
                    System.Drawing.Image temp = RUINORERP.Common.Helper.ImageHelper.ConvertByteToImg(newValue as Byte[]);
                    processContext.Value = temp;
                    processContext.Cell.View = new SourceGrid.Cells.Views.SingleImage(temp);
                    continue;
                }
                if ((!object.Equals(processContext.Value, newValue)) || isbatch)
                {
                    processContext.DisplayText = displaytxt.ToString();
                    if (!string.IsNullOrEmpty(displaytxt) && !item.GuideToTargetColumn)
                    {
                        processContext.Value = displaytxt;
                    }
                    else
                    {
                        processContext.Value = newValue;
                    }

                    processContext.Tag = newValue;


                    //如果这个列属性指定匹配集合中。就设置一下目标
                    if (sgdefine.PointToColumnPairList.ContainsKey(item))
                    {
                        if (sgdefine.grid.Rows[pt.Row].RowData != null)
                        {
                            //公共部分。只是显示用来看
                            sgdefine.grid[pt.Row, item.ColIndex].Value = newValue;

                            //设置目标的绑定数据值，就是产品ID
                            var currentObj = sgdefine.grid.Rows[pt.Row].RowData;

                            //目标存于明细中。是保存进数据库的
                            ReflectionHelper.SetPropertyValue(currentObj, sgdefine.PointToColumnPairList[item].ColName, newValue);
                            sgdefine.grid[pt.Row, sgdefine.PointToColumnPairList[item].ColIndex].Value = newValue;

                        }
                    }




                }

                #endregion

                #region 目标列要修改对应的绑定数据对象
                if (item.IsPrimaryBizKeyColumn)
                {
                    if (sgdefine.grid.Rows[pt.Row].RowData != null)
                    {
                        //设置目标的绑定数据值，就是产品ID
                        var currentObj = sgdefine.grid.Rows[pt.Row].RowData;
                        ReflectionHelper.SetPropertyValue(currentObj, item.ColName, newValue);
                        //给值就给行号
                        sgdefine.grid[pt.Row, 0].Value = pt.Row.ToString();

                        //行头给右键菜单
                        PopupMenuForRowHeader pop = sgdefine.grid[pt.Row, 0].FindController<PopupMenuForRowHeader>();
                        if (pop == null)
                        {
                            PopupMenuForRowHeader menuController = new PopupMenuForRowHeader(pt.Row, sgdefine.grid, sgdefine);
                            sgdefine.grid[pt.Row, 0].Controller.AddController(menuController);
                        }

                        //行头加颜色标记
                        sgdefine.grid[pt.Row, 0].View = sgdefine.RowHeaderWithData;

                    }
                }

                #endregion

            }
        }


        /// <summary>
        /// 增加总计行
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="define"></param>
        public static void AddSummaryRow(Grid grid, SourceGridDefine define)
        {
            //grid.Summary = true;

            //添加  总和列
            for (int i = 0; i < define.Count; i++)
            {
                Cell c = new Cell();
                //c.View.BackColor = define.SummaryColor;
                //grid.SummaryCells[i] = c;
                //if (i == 0)
                //{
                //    grid.SummaryCells[i].Value = "合  计:";
                //    c.TextAlignment = ContentAlignment.MiddleCenter;
                //    c.Font = new Font("宋体", 13);
                //}
                //grid.SummaryCells[i].BindToGrid(grid);
                //if (define[i].currency) grid.SummaryCells[i].BackGround = true;
            }
        }

        public void SetAppearance()
        {

        }



        /// <summary>
        /// 设置隔行显示
        /// </summary>
        /// <param name="grid1"></param>
        /// <param name="define"></param>
        public void LastSetGrid(SourceGrid.Grid grid1, SourceGridDefine define)
        {
            //外观设置有覆盖性。
            //另外设置一个LastSet方法

            //隔行显示 
            SourceGrid.Cells.Views.Cell transparentView = new SourceGrid.Cells.Views.Cell();
            transparentView.BackColor = Color.White;
            //字体
            //transparentView.Font = new Font(FontFamily.GenericSansSerif, 9, FontStyle.Bold);
            transparentView.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleCenter;
            transparentView.ImageAlignment = DevAge.Drawing.ContentAlignment.MiddleCenter;

            SourceGrid.Cells.Views.Cell semiTransparentView = new SourceGrid.Cells.Views.Cell();
            semiTransparentView.BackColor = Color.LightCyan;
            semiTransparentView.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleCenter;
            semiTransparentView.ImageAlignment = DevAge.Drawing.ContentAlignment.MiddleCenter;
            /*

        for (int r = 1; r < grid1.RowsCount; r++)
        {
            for (int j = 1; j < grid1.ColumnsCount; j++)
            {
                if ((r - 1) % 2 == 0)
                    grid1[r, j].View = transparentView;
                else
                    grid1[r, j].View = semiTransparentView;
            }
            grid1.Rows[r].AutoSizeMode = SourceGrid.AutoSizeMode.None; //禁止调整
        }

        */

            for (int r = 0; r < grid1.RowsCount; r++)
                for (int c = 0; c < grid1.ColumnsCount; c++)
                {
                    if (!(r == 0 || c == 0))
                    {
                        if (r != 0)
                        {
                            #region 设置指定列为可编辑列
                            /*
                            DependColumn dc = define.DependQuery.RelatedCols.Find(delegate (DependColumn dc) { return dc.ColCaption == define[c].ColCaption; });
                            if (dc != null && dc.IsQueryCol)
                            {
                                //这里要重构
                                DependColumnController 更新关联列控制器 = new DependColumnController(define, define.DependQuery.RelatedCols);
                                更新关联列控制器.ConvertFunction = delegate (object valValue)
                                {
                                    if (valValue.GetType() == define.DependencyType)
                                    {
                                        return valValue;
                                        //return RUINORERP.Common.Helper.ReflectionHelper.GetPropertyValue(valValue, dc.ColName);
                                    }
                                    else
                                    {
                                        return valValue;
                                    }
                                };

                                SourceGrid.Cells.Views.Cell NeedEditView = new SourceGrid.Cells.Views.Cell();
                                //NeedEditView.BackColor = define.NeedEditBackColor;

                                DevAge.Drawing.RectangleBorder border = define.Selection.Border;
                                border.SetColor(define._SelectionBorderColor); //边框颜色

                                border.SetWidth(1);//边框

                                NeedEditView.Border = border;

                                //grid1[r, c].View = NeedEditView;

                                grid1[r, c].AddController(更新关联列控制器);

                            }
                            */

                            #endregion
                        }

                        #region 交替显示颜色
                        if ((r - 1) % 2 == 0)
                            grid1[r, c].View = transparentView;
                        else
                            grid1[r, c].View = semiTransparentView;
                        #endregion
                    }




                }


            grid1.AutoSizeCells();
        }

        /// <summary>
        /// 设置列的宽
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="griddefine"></param>
        public static void SetColumnsWidth(SourceGrid.Grid grid, SourceGridDefine griddefine)
        {
            for (int i = 0; i < grid.Columns.Count; i++)
            {
                if (i == 0)
                {
                    grid.Columns[0].Width = 30;
                }
                else
                {
                    grid.Columns[i].Width = 90;
                }
                grid.Columns[i].AutoSizeMode = SourceGrid.AutoSizeMode.None; //禁止调整宽
                grid.Columns[i].AutoSizeMode = griddefine[i].ColAutoSizeMode;
                //if (i != grid.ColumnsCount - 1)//不是最后一列
                //{
                //    grid.Columns[i].Width = 90; 
                //    colsWidth[i];
                //    otherColsWidth += colsWidth[i];
                //}
                //else //设置最后一列铺满整个grid
                //    grid.Columns[i].Width = grid1.Width - otherColsWidth - 2 * i;

            }
        }





    }
}
