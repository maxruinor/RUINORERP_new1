using RUINORERP.Common.Helper;
using SourceGrid;
using SourceGrid.Cells.Controllers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using RUINORERP.Common.Extensions;
using RUINORERP.Business.Security;
using RUINORERP.UI.Common;
using Netron.GraphLib;
using RUINORERP.Model;
using Microsoft.Extensions.Logging;
using Mysqlx;
using ZXing.Common;
using SourceGrid.Cells.Editors;
using NPOI.SS.UserModel;
using System.Reflection;
using SourceGrid.Cells.Views;
using SourceGrid.Cells.Models;
using System.Text.RegularExpressions;
using RUINORERP.Business.CommService;

namespace RUINORERP.UI.UCSourceGrid
{

    /// <summary>
    /// 单据操作行为控制器
    /// 第一行编辑成功后才下一行可录入
    /// 太具体的业务逻辑用事件抛出去外层处理
    /// 
    /// 先处理第一行。产品ID给出值后 行头给值后 再第二行可以编辑
    /// </summary>
    /// <typeparam name="P">产品表</typeparam>
    /// <typeparam name="T">单据明细表</typeparam>
    public class BillOperateController : ControllerBase// where T : class
    {

        private SourceGridDefine _currentGridDefine;
        /// <summary>
        /// 当前格子的定义
        /// </summary>
        public SourceGridDefine CurrGridDefine { get => _currentGridDefine; set => _currentGridDefine = value; }

        public BillOperateController(SourceGridDefine _CurrentGridDefine)
        {
            CurrGridDefine = _CurrentGridDefine;
        }

        /// <summary>
        /// 添加数据行时生成的事件  这里定义事件，还会在SourceGridHelper定义一下。用来事件传递。架构这样。暂时没有优化。
        /// </summary>
        public event AddDataRowDelegate OnAddDataRow;

        public delegate void AddDataRowDelegate(object rowObj);



        //sgd 要提供当前操作的行 列 值

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


        /// <summary>
        /// 
        /// </summary>
        /// <param name="rowObj">行所在实体的对象实例</param>
        /// <param name="CurrGridDefine"></param>
        /// <param name="Position"></param>
        public delegate void CalculateColumnValue(object rowObj, SourceGridDefine CurrGridDefine, Position Position);

        /// <summary>
        /// 计算数据事件
        /// </summary>
        public event CalculateColumnValue OnCalculateColumnValue;


        public override void OnClick(SourceGrid.CellContext sender, EventArgs e)
        {
            base.OnClick(sender, e);
            // CurrGridDefine.CurrentCellLocation = sender.Position;
        }
        public override void OnKeyPress(CellContext sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Escape && CurrGridDefine.grid.Focused)
            {

            }
            else
            {
                base.OnKeyPress(sender, e);
            }

            //如果有总计点到最后一行。直接返回
            if (CurrGridDefine.HasSummaryRow && sender.Position.Row == CurrGridDefine.grid.RowsCount - 1)
            {
                return;
            }
            if (e.KeyChar == (char)Keys.Enter)
            {
                //如果是编辑状态则回车编辑控件

                if (sender.Cell.Editor != null && sender.Cell.Editor.IsEditing)
                {
                    CellContext editCellContext = sender.Cell.Editor.EditCellContext;
                    //点击按钮
                    //bool success = sender.Cell.Editor.InternalEndEdit(false);
                    //if (success)
                    //{
                    //    Grid.Controller.OnEditEnded(editCellContext, EventArgs.Empty);
                    //}
                    //return success;
                }

            }
        }

        //这里是任何变动都会执行
        public override async void OnValueChanged(SourceGrid.CellContext sender, EventArgs e)
        {
            if (CurrGridDefine.grid.Columns[sender.Position.Column].Tag != null &&
                CurrGridDefine.grid.Columns[sender.Position.Column].Tag is SGDefineColumnItem CurrDefinedColumn)
            {
                #region 数据的变化


                if (sender.Value == null && CurrDefinedColumn.IsPrimaryBizKeyColumn)
                {
                    //清空关联值
                    CurrGridDefine.SetDependTargetValue(null, sender.Position, null, CurrDefinedColumn.ColName);
                    return;
                }

                if (!CurrDefinedColumn.GuideToTargetColumn || CurrDefinedColumn.IsRowHeaderCol)
                {
                    //如果不是单据明细值的变化不需要处理
                    return;
                }
                var columninfo = CurrGridDefine.grid.Columns.GetColumnInfo(CurrDefinedColumn.UniqueId);
                if (columninfo == null)
                {
                    return;
                }
                int targetColumnIndex = columninfo.Index;

                //要把当前合法的值给到 真正的对象
                var setcurrentObj = CurrGridDefine.grid.Rows[sender.Position.Row].RowData;
                if (setcurrentObj != null)
                {
                    //将UI值转换后赋值给对象 很重要
                    //如果sender.value=null ，如果是将业务主键都置为null。则对应数据行清空。其他全部置为null
                    if (sender.Value == null)
                    {
                        CurrGridDefine.grid[sender.Position.Row, targetColumnIndex].Value = null;
                        PropertyInfo propertyInfo = setcurrentObj.GetType().GetPropertyInfo(CurrDefinedColumn.ColName);
                        setcurrentObj.SetPropertyInfoToNull(propertyInfo);
                        return;
                    }
                    if (string.IsNullOrEmpty(sender.Value.ToString()) && CurrDefinedColumn.ColPropertyInfo.PropertyType.FullName == "System.Int64")
                    {
                        return;
                    }
                    var realTypeVal = sender.Value.ChangeTypeSafely(CurrDefinedColumn.ColPropertyInfo.PropertyType);
                    ReflectionHelper.SetPropertyValue(setcurrentObj, CurrDefinedColumn.ColName, realTypeVal);
                    #region 处理特殊情况  比方 时间值为：{0001-01-01 0:00:00}
                    switch (CurrDefinedColumn.CustomFormat)
                    {
                        case CustomFormatType.DateTime:
                            if (realTypeVal.ToString() == "0001-01-01 0:00:00" || realTypeVal.ToString() == "1900-01-01 0:00:00")
                            {
                                //如果可空，则NULL，否则指定为最小时间 1975？
                                var propertyInfo = setcurrentObj.GetPropertyValue(CurrDefinedColumn.ColName);
                                //判断属性是否可以为null
                                if (propertyInfo != null)
                                {

                                }
                            }
                            break;
                        case CustomFormatType.Bool:
                            ReflectionHelper.SetPropertyValue(setcurrentObj, CurrDefinedColumn.ColName, realTypeVal);
                            bool bl = realTypeVal.ToBool();
                            if (bl == true)
                            {
                                CurrGridDefine.grid[sender.Position.Row, targetColumnIndex].DisplayText = "是";
                            }
                            else
                            {
                                CurrGridDefine.grid[sender.Position.Row, targetColumnIndex].DisplayText = "否";
                            }

                            break;
                        case CustomFormatType.WebPathImage:
                            var model = sender.Cell.Model.FindModel(typeof(SourceGrid.Cells.Models.ValueImageWeb));
                            SourceGrid.Cells.Models.ValueImageWeb valueImageWeb = (SourceGrid.Cells.Models.ValueImageWeb)model;
                            if (sender.Value != null && !string.IsNullOrEmpty(sender.Value.ToString()) && string.IsNullOrEmpty(valueImageWeb.CellImageHashName))
                            {
                                valueImageWeb.CellImageHashName = sender.Value.ToString();
                                HttpWebService httpWebService = Startup.GetFromFac<HttpWebService>();
                                try
                                {
                                    valueImageWeb.CellImageBytes = await httpWebService.DownloadImgFileAsync(valueImageWeb.GetNewRealfileName());
                                }
                                catch (Exception ex)
                                {
                                    MainForm.Instance.uclog.AddLog(ex.Message, Global.UILogType.错误);
                                }
                            }
                            if (valueImageWeb.CellImageBytes != null && valueImageWeb.CellImageBytes.Length > 0)
                            {
                                CurrGridDefine.grid[sender.Position.Row, targetColumnIndex].Value = sender.Value;
                                CurrGridDefine.grid[sender.Position.Row, targetColumnIndex].Tag = valueImageWeb.CellImageBytes;
                                //刷新单元格图片显示外观
                                CurrGridDefine.grid[sender.Position.Row, targetColumnIndex].View = new SourceGrid.Cells.Views.RemoteImageView(ImageProcessor.ByteArrayToImage(valueImageWeb.CellImageBytes));
                                //if (CurrGridDefine.grid[sender.Position.Row, targetColumnIndex].View is SingleImageWeb imageWebview)
                                //{
                                //    imageWebview.OnLoadImage -= ImageWebview_OnLoadImage;
                                //    imageWebview.OnLoadImage += ImageWebview_OnLoadImage;
                                //    CurrGridDefine.grid.GetCell(sender.Position).View.Refresh(sender);
                                //}
                                ////CurrGridDefine.grid[sender.Position.Row, targetColumnIndex].View.Refresh(sender);
                            }

                            break;
                        case CustomFormatType.Image:
                            CurrGridDefine.grid[sender.Position.Row, targetColumnIndex].Value = sender.Value;
                            break;
                        case CustomFormatType.DefaultFormat:
                        default:
                            ReflectionHelper.SetPropertyValue(setcurrentObj, CurrDefinedColumn.ColName, realTypeVal);
                            break;
                    }
                    #endregion

                }
                else
                {
                    CurrGridDefine.grid[sender.Position.Row, targetColumnIndex].Value = null;
                    return;
                }


                //如果他在关联列中。则用条件去搜索是否匹配。不然验证不通过

                //    //sender.Value
                //    //比方 得到条码。知道值 和列名去搜索产品，完了得到第一个对象。直接给其他关联列值。（还要给明细中ID值）
                //    //1、防止在event还没注册时就被调用
                //    if (OnValidateDataCell != null)
                //    {
                //        //触发事件
                //        OnValidateDataCell("");
                //    }
                //    //2、还可以使用null条件运算符
                //    //OnValidateDataRows?.Invoke(name); 
                //    // MessageBox.Show(sender.Grid, string.Format(val, sender.Position, sender.Value));

                #region 关联列值的设置
                var col = CurrDefinedColumn;
                if (col.RelatedCols != null && col.RelatedCols.Count > 0)
                {
                    foreach (var item in col.RelatedCols)
                    {
                        foreach (var rc in item.Value)
                        {
                            SGDefineColumnItem targetCol = CurrGridDefine.DefineColumns.FirstOrDefault(c => c.ColName == rc.ColTargetName);
                            string newValue = rc.NewValue;

                            //定义一个最终的参数数组,通过反射得到真正的参数值
                            List<object> lastPara = new List<object>();
                            for (int i = 0; i < rc.ValueParameters.Length; i++)
                            {
                                //如果指向其他列有值。则取外键实体的名称的值
                                if (!string.IsNullOrEmpty(rc.ValueParameters[i].PointToColName))
                                {
                                    object id = ReflectionHelper.GetPropertyValue(setcurrentObj, rc.ValueParameters[i].ParameterColName);
                                    //取值时没有使用指向性列名，用的是初始时 默认指向的名称编号等，这里是不是可以优化成可以指定。或不指定。不指定的话，使用默认的时。SetCol_RelatedValue 可以少输入一个参数
                                    var obj = BizCacheHelper.Instance.GetEntity(rc.ValueParameters[i].FkTableType.Name, id);
                                    if (obj != null)
                                    {
                                        object value = ReflectionHelper.GetPropertyValue(obj, rc.ValueParameters[i].PointToColName);
                                        lastPara.Add(value);
                                    }
                                }
                                else
                                {
                                    lastPara.Add(ReflectionHelper.GetPropertyValue(setcurrentObj, rc.ValueParameters[i].ParameterColName));
                                }
                            }
                            var lastnewValue = string.Format(newValue, lastPara.ToArray());
                            ReflectionHelper.SetPropertyValue(setcurrentObj, rc.ColTargetName, lastnewValue.ToString());

                            CurrGridDefine.grid[sender.Position.Row, CurrGridDefine.grid.Columns.GetColumnInfo(targetCol.UniqueId).Index].Value = lastnewValue.ToString();
                        }
                    }
                }
                #endregion

                //这里计算是一些公共的，针对具体单据的计算 需要用一个事件抛出来处理
                #region 小计加总计(正向)

                //这里的思路是 a+b=c  ,a b 字段变化才会进入计划，所以如果 a,b是小计，是由另一个计算来的。则不会变化。。
                //就使用上溯源，如果a是其他的组的目标，则找到相关组的操作数变化来触发
                //太复杂。容易出错，多次执行，用长公式代替？
                if (CurrGridDefine.SubtotalCalculate.Count > 0)
                {
                    //向上找
                    //if (CurrGridDefine.SubtotalCalculate.Where(c => c.TagetCol.ColName == CurrDefinedColumn.ColName).Any())
                    //{
                    //    SubtotalFormula sf = new SubtotalFormula();
                    //    sf = CurrGridDefine.SubtotalCalculate.FirstOrDefault(c => c.Parameter.Where(k => k == (CurrDefinedColumn.ColName)).Any());
                    //    Subtotal(sf, CurrGridDefine, sender.Position.Row);
                    //}
                    //当前找
                    if (CurrGridDefine.SubtotalCalculate.Where(c => c.Parameter.Where(k => k == (CurrDefinedColumn.ColName)).Any()).Any())
                    {
                        List<CalculateFormula> sflist = new List<CalculateFormula>();
                        sflist = CurrGridDefine.SubtotalCalculate.Where(c => c.Parameter.Where(k => k == (CurrDefinedColumn.ColName)).Any()).ToList();
                        ///一个结果列来自多个公式
                        foreach (CalculateFormula sf in sflist)
                        {
                            try
                            {
                                //正向计算
                                Subtotal(sf, CurrGridDefine, sender.Position.Row);
                            }
                            catch (Exception error)
                            {
                                MainForm.Instance.logger.LogError("出现应用程序未处理的异常,请更新到新版本，如果无法解决，请联系管理员！\r\n" + error.Message, error);
                            }
                        }

                    }

                }

                #endregion




                #region  总计  要放最后，因为其他计算会用到的结果值

                if (CurrDefinedColumn.Summary)
                {
                    decimal totalTemp = 0;
                    //去掉首尾行
                    for (int r = 1; r < CurrGridDefine.grid.RowsCount - 1; r++)
                    {
                        if (CurrGridDefine.grid[r, targetColumnIndex].Value != null && CurrGridDefine.grid.Rows[r].RowData != null)
                        {
                            decimal CurrentTemp = 0;
                            if (decimal.TryParse(CurrGridDefine.grid[r, targetColumnIndex].Value.ToString(), out CurrentTemp))
                            {
                                totalTemp = CurrentTemp + totalTemp;
                            }
                        }
                    }

                    CurrGridDefine.grid[CurrGridDefine.grid.RowsCount - 1, targetColumnIndex].Value = totalTemp;
                    //最后一行 C# 中各种格式化
                    //https://www.cnblogs.com/zhangxiaoxia/p/15429877.html
                    if (CurrDefinedColumn.CustomFormat == CustomFormatType.CurrencyFormat)
                    {
                        CurrGridDefine.grid[CurrGridDefine.grid.RowsCount - 1, targetColumnIndex].DisplayText = string.Format("{0:C}", totalTemp);//这样才显示了货币符号
                                                                                                                                                  //CurrGridDefine.grid[CurrGridDefine.grid.RowsCount - 1, targetColumnIndex].Value = string.Format("{0:C}", totalTemp); //这样才显示了货币符号
                    }

                }
                #endregion

                //存在参与计算的才重新计划总数后面是否优化为  变动了才算？
                //这个事件传到sourcegridhelper.OnCalculateColumnValue
                if (OnCalculateColumnValue != null && CurrDefinedColumn .Summary)
                {
                    var currentObj = CurrGridDefine.grid.Rows[sender.Position.Row].RowData;
                    OnCalculateColumnValue(currentObj, CurrGridDefine, sender.Position);
                }

                #region 主要处理显示的格式，因为经过了计算 必要时覆盖上面的赋值过程

                switch (CurrDefinedColumn.CustomFormat)
                {

                    case CustomFormatType.DefaultFormat:
                        break;
                    case CustomFormatType.DateTime:
                        break;
                    case CustomFormatType.PercentFormat:
                        object cellvalue = CurrGridDefine.grid[sender.Position.Row, targetColumnIndex].Value;
                        if (cellvalue != null)
                        {
                            //实际上面转换过一次了。
                            var realvalue = cellvalue.ChangeTypeSafely(CurrDefinedColumn.ColPropertyInfo.PropertyType);
                            if (CurrGridDefine.grid[sender.Position.Row, targetColumnIndex].Editor != null)
                            {
                                CurrGridDefine.grid[sender.Position.Row, targetColumnIndex].DisplayText = CurrGridDefine.grid[sender.Position.Row, targetColumnIndex].Editor.ValueToDisplayString(realvalue);
                            }
                        }

                        break;
                    case CustomFormatType.CurrencyFormat:

                        //var ColCurrencyTypeConverter = new DevAge.ComponentModel.Converter.CurrencyTypeConverter(typeof(decimal));???还使用吗？
                        //CurrGridDefine.grid[sender.Position.Row, targetColumnIndex].Value = realTypeVal;
                        //CurrGridDefine.grid[sender.Position.Row, targetColumnIndex].DisplayText = string.Format("{0:C}", realTypeVal);

                        break;
                    case CustomFormatType.DecimalPrecision:
                        break;
                    default:
                        break;
                }
                #endregion

                //base.OnValueChanged(sender, e);
                #endregion
            }


        }

        private void ImageWebview_OnLoadImage(System.Drawing.Image GridImage, byte[] ImageBytes)
        {
            GridImage = ImageProcessor.ByteArrayToImage(ImageBytes);
        }



        /// <summary>
        /// 通过公式计算小计等
        /// </summary>
        /// <param name="sf"></param>
        /// <param name="CurrGridDefine"></param>
        /// <param name="rowindex"></param>
        private void Subtotal(CalculateFormula sf, SourceGridDefine CurrGridDefine, int rowindex)
        {
            if (sf != null)
            {
                //这里如果能算出 逆运算式更好。
                //https://blog.sina.com.cn/s/blog_8442c8f70101cgx4.html
                var currentObj = CurrGridDefine.grid.Rows[rowindex].RowData;
                //如果存在计算条件，则先计算条件是不是满足
                #region  计算公式
                if (sf.CalcCondition != null && sf.CalcCondition.expCondition != null)
                {
                    bool isOK = sf.CalcCondition.GetConditionResult(currentObj);
                    if (!isOK)
                    {
                        return;
                    }
                    else
                    {

                    }
                }

                #endregion

                string resultFormula = sf.StringFormula;
                //for (int i = 0; i < sf.Parameter.Count; i++)
                //{
                //    newstr = newstr.Replace(sf.Parameter[i], "{" + i + "}");
                //}
                // 调用方法进行替换
                resultFormula = ReplaceParameters(sf.StringFormula, sf.Parameter);

                string lastStr = resultFormula;
                for (int i = 0; i < sf.Parameter.Count; i++)
                {
                    string subItem = ReflectionHelper.GetPropertyValue(currentObj, sf.Parameter[i]).ToString();
                    decimal subDec = 0;
                    {
                        if (string.IsNullOrEmpty(subItem.ToString()))
                        {
                            subItem = "0";
                        }
                        if (decimal.TryParse(subItem.ToString(), out subDec))
                        {
                            subItem = subDec.ToString();
                        }
                        else
                        {
                            MainForm.Instance.uclog.AddLog(sf.Parameter[i] + "参数转换列时出错", Global.UILogType.错误);
                        }
                    }
                    string p = "{" + i + "}";
                    resultFormula = resultFormula.Replace(p, subItem);
                }
                DataTable dt = new DataTable();
                object obj = dt.Compute(resultFormula, "");
                //C# 判断数据是否为NaN的方法
                if (obj.ToString() == "NaN")
                {
                    obj = 0;
                }

                //最大小数位
                int maxDecimalPlaces = 4;
                decimal amount = 0.00m;

                #region 处理显示格式
                int sfRealIndex = CurrGridDefine.grid.Columns.GetColumnInfo(sf.TagetCol.UniqueId).Index;
                switch (sf.TagetCol.CustomFormat)
                {
                    case CustomFormatType.PercentFormat:

                        //实际上面转换过一次了。
                        var realvalue = obj.ChangeTypeSafely(sf.TagetCol.ColPropertyInfo.PropertyType);

                        amount = RoundToNDecimalPlaces(realvalue, maxDecimalPlaces);
                        ReflectionHelper.SetPropertyValue(currentObj, sf.TagetCol.ColName, amount);
                        CurrGridDefine.grid[rowindex, sfRealIndex].Value = amount;
                        CurrGridDefine.grid[rowindex, sfRealIndex].DisplayText = CurrGridDefine.grid[rowindex, sfRealIndex].Editor.ValueToDisplayString(amount);
                        break;
                    case CustomFormatType.CurrencyFormat:
                        //var ColCurrencyTypeConverter = new DevAge.ComponentModel.Converter.CurrencyTypeConverter(typeof(decimal));
                       // int maxDecimalPlaces = AuthorizeController.GetMoneyDataPrecision(MainForm.Instance.AppContext);
                        //TODO 这里暂时用4位 By watson 
                       // maxDecimalPlaces = 4;
                     
                        amount = RoundToNDecimalPlaces(obj, maxDecimalPlaces);
                        CurrGridDefine.grid[rowindex, sfRealIndex].Value = amount;
                        CurrGridDefine.grid[rowindex, sfRealIndex].DisplayText = string.Format("{0:C}", amount.ToString());
                        break;
                    case CustomFormatType.DecimalPrecision:
                        ReflectionHelper.SetPropertyValue(currentObj, sf.TagetCol.ColName, decimal.Parse(obj.ToString()));
                        CurrGridDefine.grid[rowindex, sfRealIndex].Value = decimal.Parse(obj.ToString());
                        break;

                    case CustomFormatType.DefaultFormat:
                    default:
                        ReflectionHelper.SetPropertyValue(currentObj, sf.TagetCol.ColName, int.Parse(obj.ToString()));
                        CurrGridDefine.grid[rowindex, sfRealIndex].Value = int.Parse(obj.ToString());
                        break;
                }
                #endregion



            }
        }
        public static string ReplaceParameters(string formula, List<string> parameters)
        {
            for (int i = 0; i < parameters.Count; i++)
            {
                // 构建正则表达式，确保匹配整个单词
                string pattern = @"\b" + Regex.Escape(parameters[i]) + @"\b";
                formula = Regex.Replace(formula, pattern, "{" + i + "}");
            }
            return formula;
        }

        #region 将小数保留到指定位数
        public static decimal RoundToNDecimalPlaces(object value, int numberOfDecimalPlaces)
        {
            // 检查输入是否可以转换为 Decimal
            if (value is decimal)
            {
                decimal input = (decimal)value;
                return RoundToNDecimalPlaces(input, numberOfDecimalPlaces);
            }
            else if (value is int || value is double)
            {
                return value.ToDecimal();
            }
            else
            {
                throw new ArgumentException("输入的值必须是 Decimal 类型。");
            }
        }

        public static decimal RoundToNDecimalPlaces(decimal value, int numberOfDecimalPlaces)
        {
            // 创建 DecimalFormat 实例，指定小数位数
            var format = new NPOI.SS.Util.DecimalFormat("#." + new string('0', numberOfDecimalPlaces));

            // 使用格式进行格式化
            return (decimal)format.Format(value).ToDecimal();
        }
        #endregion


        public override void OnFocusEntering(CellContext sender, CancelEventArgs e)
        {
            base.OnFocusEntering(sender, e);

            if (CurrGridDefine.grid.Columns[sender.Position.Column].Tag != null &&
    CurrGridDefine.grid.Columns[sender.Position.Column].Tag is SGDefineColumnItem CurrDefinedColumn)
            {
                #region 进入焦点
                if (sender.Value == null)
                {
                    // return;
                }
                //进入时就把这个值保存到tag中，用于后面编辑验证逻辑，对比改过的值是否变化过
                sender.Tag = sender.Value;
                SourceGridHelper sh = new SourceGridHelper();
                //CurrGridDefine.CurrentCellLocation = sender.Position;
                //如果有总计点到最后一行。直接返回
                if (CurrGridDefine.HasSummaryRow && sender.Position.Row == CurrGridDefine.grid.RowsCount - 1)
                {
                    return;
                }
                bool canEdit = false;
                if (sender.Position.Row == 1)
                {
                    canEdit = true;
                }
                if (sender.Position.Row > 1)
                {
                    //上一行有值。就下当前行可以编辑
                    if (CurrGridDefine.grid[sender.Position.Row - 1, 0].Value != null)
                    {
                        if (!string.IsNullOrEmpty(CurrGridDefine.grid[sender.Position.Row - 1, 0].Value.ToString()))
                        {
                            canEdit = true;
                        }
                    }

                }

                if (canEdit && CurrGridDefine.grid.Rows[sender.Position.Row].RowData == null)
                {
                    CurrGridDefine.grid.Rows[sender.Position.Row].RowData = CurrGridDefine.BindingSourceLines.AddNew();
                    if (OnAddDataRow != null)
                    {
                        OnAddDataRow(CurrGridDefine.grid.Rows[sender.Position.Row].RowData);
                    }

                    /*
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
                    */

                    //设置编辑时的行号 不要减-1  因为第一行是列头
                    CurrGridDefine.grid[sender.Position.Row, 0].Value = sender.Position.Row;
                    sh.SetRowEditable(CurrGridDefine.grid, new int[] { sender.Position.Row }, CurrGridDefine);
                    if (sender.Cell.Editor != null)
                    {
                        //   sender.Cell.View = SourceGridDefine.editView;
                    }

                }

                //如果当前行能编辑并且是倒数第二行了
                if (canEdit && sender.Position.Row == CurrGridDefine.grid.Rows.Count - 2
                    && CurrGridDefine.grid.Rows[sender.Position.Row - 1].RowData != null)//去掉标题行和总计行
                {
                    sh.InsertRow(CurrGridDefine.grid, CurrGridDefine, true);
                }
                if (canEdit && sender.Cell.Editor != null)
                {
                    //为了解决：在编辑状态下，修改相关的列的计算数据混乱的问题，这里进入编辑状态时，标记一个列为编辑的起始列
                    CurrDefinedColumn.IsEdit = false;
                }

                #endregion
            }
        }

        public override void OnFocusLeaving(CellContext sender, CancelEventArgs e)
        {
            base.OnFocusLeaving(sender, e);
            if (CurrGridDefine.grid.Columns[sender.Position.Column].Tag != null &&
   CurrGridDefine.grid.Columns[sender.Position.Column].Tag is SGDefineColumnItem CurrDefinedColumn)
            {
                #region 离开焦点
                //如果有总计点到最后一行。直接返回
                if (CurrGridDefine.HasSummaryRow && sender.Position.Row == CurrGridDefine.grid.RowsCount - 1)
                {
                    return;
                }
                //恢复背景色
                if (CurrDefinedColumn.CustomFormat == CustomFormatType.CurrencyFormat)
                {
                    sender.Cell.View = CurrGridDefine.ViewNormalMoney;
                }
                else if (CurrDefinedColumn.CustomFormat == CustomFormatType.Image)
                {
                    // sender.Cell.View = CurrGridDefine.ImagesViewModel;
                    //如果有图片值才设置，不然还是和其他一样
                    //if (sender.Value != null)
                    //{
                    //    sender.Cell.View = new SourceGrid.Cells.Views.SingleImage();
                    //}
                    //else
                    //{
                    //    sender.Cell.View = CurrGridDefine.ViewNormal;
                    //}
                }
                else if (CurrDefinedColumn.CustomFormat == CustomFormatType.WebPathImage)
                {
                    // sender.Cell.View = CurrGridDefine.ImagesViewModel;
                    //如果有图片值才设置，不然还是和其他一样
                    //if (sender.Value != null)
                    //{
                    //    sender.Cell.View = CurrGridDefine.ImagesWebViewModel;
                    //}
                    //else
                    //{
                    //    sender.Cell.View = CurrGridDefine.ViewNormal;
                    //}
                }
                else
                {
                    sender.Cell.View = CurrGridDefine.ViewNormal;
                }


                //1、防止在event还没注册时就被调用
                if (OnValidateDataRows != null)
                {
                    //触发事件
                    //OnValidateDataRows("");
                }
                //2、还可以使用null条件运算符
                //OnValidateDataRows?.Invoke(name); 
                //
                if (sender.Cell.Editor != null)
                {
                    if (sender.Cell.Editor.IsEditing)
                    {
                        sender.EndEdit(false);
                    }
                }

                //为了解决：在编辑状态下，修改相关的列的计算数据混乱的问题，这里进入编辑状态时，标记一个列为编辑的起始列
                CurrDefinedColumn.IsEdit = true;
                #endregion
            }
        }


        public override void OnEditStarting(CellContext sender, CancelEventArgs e)
        {
            if (CurrGridDefine.grid.Columns[sender.Position.Column].Tag != null &&
    CurrGridDefine.grid.Columns[sender.Position.Column].Tag is SGDefineColumnItem CurrDefinedColumn)
            {
                //这个远程图片列，值为图片名称，是自动生成的。
                if (CurrDefinedColumn.CustomFormat == CustomFormatType.WebPathImage)
                {
                    if (sender.Value == null || string.IsNullOrEmpty(sender.Value.ToString()))
                    {

                        var model = sender.Cell.Model.FindModel(typeof(SourceGrid.Cells.Models.ValueImageWeb));
                        sender.Cell.Model.RemoveModel(model);
                        sender.Cell.Model.AddModel(new ValueImageWeb());
                        var NewModel = sender.Cell.Model.FindModel(typeof(SourceGrid.Cells.Models.ValueImageWeb));
                        SourceGrid.Cells.Models.ValueImageWeb valueImageWeb = (SourceGrid.Cells.Models.ValueImageWeb)NewModel;
                        sender.Value = valueImageWeb.CellImageHashName;
                    }
                    //else
                    //{
                    //    sender.Cell.View = CurrGridDefine.ImagesWebViewModel;
                    //}


                    sender.Cell.Editor.ApplyEdit();
                }
            }
        }
        public override void OnEditStarted(CellContext sender, EventArgs e)
        {
        }
        public override void OnEditEnded(CellContext sender, EventArgs e)
        {
            if (CurrGridDefine.grid.Columns[sender.Position.Column].Tag != null &&
    CurrGridDefine.grid.Columns[sender.Position.Column].Tag is SGDefineColumnItem CurrDefinedColumn)
            {
                //如果当前行有完整数据时，人为清空关键字段，则清空关联值
                if (sender.Value == null || sender.Value.IsNullOrEmpty())
                {
                    //一行数据中包括两个部分：一个是真实明细表中的字段，另一个是产品或其他公共部分。没有保存到数据库。只是参考引用显示作用。
                    //真实明细是可以清空编辑的。公共部分不能清空编辑。清空认为重新输入，所以这里判断是否是公共部分，如果是，则进行清空关联值
                    if (!CurrDefinedColumn.IsCoreContent)
                    {
                        //清空关联值
                        CurrGridDefine.SetDependTargetValue(null, sender.Position, null, CurrDefinedColumn.ColName);
                        if (CurrDefinedColumn.CustomFormat == CustomFormatType.Image)
                        {
                            //因为是清空。所以图片也要恢复成默认视图
                            sender.Cell.View = CurrGridDefine.ViewNormal;
                        }
                        if (CurrDefinedColumn.CustomFormat == CustomFormatType.WebPathImage)
                        {
                            //因为是清空。所以图片也要恢复成默认视图
                            //sender.Cell.View = CurrGridDefine.ViewNormal;
                        }
                    }
                    return;
                }
                if (sender.Value == null)
                {
                    return;
                }
                //编辑后的样式如果是货币要右对齐显示
                if (CurrDefinedColumn.CustomFormat == CustomFormatType.CurrencyFormat)
                {
                    sender.Cell.View.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleRight;
                }
                else
                {
                    sender.Cell.View.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleLeft;
                }
                var setcurrentObj = CurrGridDefine.grid.Rows[sender.Position.Row].RowData;
                if (setcurrentObj == null)
                {
                    return;
                }
                //如果编辑后的值等于原来的值。则不进行后面的操作。特别是反向计算。不然会一直循环重复计算。导致数据错误
                //var oldValue = setcurrentObj.GetPropertyValue(CurrDefinedColumn.ColName);
                //if (oldValue != null && (oldValue.ToString().Equals(sender.Value.ToString())))
                //{
                //    return;
                //}
                if (CurrDefinedColumn.CustomFormat == CustomFormatType.WebPathImage)
                {
                    if (sender.Cell.Editor != null && sender.Cell.Editor is ImageWebPickEditor webPicker)
                    {
                        //如果图片存在，则显示图片
                        if (System.IO.File.Exists(webPicker.AbsolutelocPath))
                        {
                            //sender.Cell.View = CurrGridDefine.ImagesWebViewModel;
                        }
                        else if (false)
                        {

                        }
                        else
                        {
                            //sender.Cell.View = CurrGridDefine.ViewNormal;
                        }
                    }

                }

                //这里计算是一些公共的，针对具体单据的计算 需要用一个事件抛出来处理
                #region 小计加总计(反向)

                //这里的思路是 a+b=c  ,a b 字段变化才会进入计划，所以如果 a,b是小计，是由另一个计算来的。则不会变化。。
                //就使用上溯源，如果a是其他的组的目标，则找到相关组的操作数变化来触发
                //太复杂。容易出错，多次执行，用长公式代替？
                if (CurrGridDefine.SubtotalCalculateReverse.Count > 0)
                {
                    //向上找
                    //if (CurrGridDefine.SubtotalCalculate.Where(c => c.TagetCol.ColName == CurrDefinedColumn.ColName).Any())
                    //{
                    //    SubtotalFormula sf = new SubtotalFormula();
                    //    sf = CurrGridDefine.SubtotalCalculate.FirstOrDefault(c => c.Parameter.Where(k => k == (CurrDefinedColumn.ColName)).Any());
                    //    Subtotal(sf, CurrGridDefine, sender.Position.Row);
                    //}
                    //当前找 
                    //正向反向的区别是保存的公式在不同的集合中，并且正向是值改变就自动触发，反向是对应的列的值结束编辑时触发
                    if (CurrGridDefine.SubtotalCalculateReverse.Where(c => c.Parameter.Where(k => k == (CurrDefinedColumn.ColName)).Any()).Any())
                    {
                        List<CalculateFormula> sflist = new List<CalculateFormula>();
                        sflist = CurrGridDefine.SubtotalCalculateReverse.Where(c => c.Parameter.Where(k => k == (CurrDefinedColumn.ColName)).Any()).ToList();
                        ///一个结果列来自多个公式
                        foreach (CalculateFormula sf in sflist)
                        {
                            try
                            {
                                Subtotal(sf, CurrGridDefine, sender.Position.Row);
                            }
                            catch (Exception error)
                            {
                                MainForm.Instance.logger.LogError("出现应用程序未处理的异常,请更新到新版本，如果无法解决，请联系管理员！\r\n" + error.Message, error);
                            }
                        }

                    }

                    //存在参与计算的才重新计划总数后面是否优化为  变动了才算？
                    //这个事件传到sourcegridhelper.OnCalculateColumnValue
                    if (OnCalculateColumnValue != null && CurrDefinedColumn.Summary)
                    {
                        var currentObj = CurrGridDefine.grid.Rows[sender.Position.Row].RowData;
                        OnCalculateColumnValue(currentObj, CurrGridDefine, sender.Position);
                    }

                }

                #endregion
            }
        }

    }



}
