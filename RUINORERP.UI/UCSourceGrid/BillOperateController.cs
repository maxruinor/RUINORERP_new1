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
        public override void OnValueChanged(SourceGrid.CellContext sender, EventArgs e)
        {
            base.OnValueChanged(sender, e);
            // string val = "Value of cell {0} is '{1}'";
            //MainForm.Instance.uclog.AddLog("CellChanged", string.Format(val, sender.Position, sender.Value));
            if (sender.Value == null || sender.Value.IsNullOrEmpty())
            {
                //清空关联值
                //CurrGridDefine.SetDependTargetValue(null, sender.Position, null, CurrGridDefine[sender.Position.Column].ColName);
                return;
            }
            else
            {
                //如果是关联列就跳过,如果是指向  导向列，即可指向明细的列则更新值
                //if (CurrGridDefine.DependQuery.RelatedCols.Any(c => c.ColIndex == sender.Position.Column && !c.GuideToTargetColumn))
                //{
                //    return;
                //}
            }
            if (!CurrGridDefine[sender.Position.Column].GuideToTargetColumn || CurrGridDefine[sender.Position.Column].IsRowHeaderCol)
            {
                //如果不是单据明细值的变化不需要处理
                return;
            }

            //将UI值转换后赋值给对象 很重要
            var realTypeVal = sender.Value.ChangeType_ByConvert(CurrGridDefine[sender.Position.Column].ColPropertyInfo.PropertyType);
            //var realTypeVal = Convert.ChangeType(sender.Value, CurrGridDefine[sender.Position.Column].ColPropertyInfo.PropertyType);

            //要把当前合法的值给到 真正的对象
            var setcurrentObj = CurrGridDefine.grid.Rows[sender.Position.Row].RowData;
            if (setcurrentObj != null)
            {

                ReflectionHelper.SetPropertyValue(setcurrentObj, CurrGridDefine[sender.Position.Column].ColName, realTypeVal);
            }
            else
            {
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
            var col = CurrGridDefine[sender.Position.Column];
            if (col.RelatedCols != null && col.RelatedCols.Count > 0)
            {
                foreach (var item in col.RelatedCols)
                {
                    foreach (var rc in item.Value)
                    {
                        SourceGridDefineColumnItem targetCol = CurrGridDefine.DefineColumns.FirstOrDefault(c => c.ColName == rc.ColTargetName);
                        string newValue = rc.NewValue;

                        //定义一个最终的参数数组,通过反射得到真正的参数值
                        List<object> lastPara = new List<object>();
                        for (int i = 0; i < rc.ValueParameters.Length; i++)
                        {
                            //如果指向其它列有值。则取外键实体的名称的值
                            if (!string.IsNullOrEmpty(rc.ValueParameters[i].PointToColName))
                            {
                                object id = ReflectionHelper.GetPropertyValue(setcurrentObj, rc.ValueParameters[i].ParameterColName);
                                //取值时没有使用指向性列名，用的是初始时 默认指向的名称编号等，这里是不是可以优化成可以指定。或不指定。不指定的话，使用默认的时。SetCol_RelatedValue 可以少输入一个参数
                                var obj = CacheHelper.Instance.GetEntity(rc.ValueParameters[i].FkTableType.Name, id);
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
                        CurrGridDefine.grid[sender.Position.Row, targetCol.ColIndex].Value = lastnewValue.ToString();

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
                //if (CurrGridDefine.SubtotalCalculate.Where(c => c.TagetCol.ColName == CurrGridDefine[sender.Position.Column].ColName).Any())
                //{
                //    SubtotalFormula sf = new SubtotalFormula();
                //    sf = CurrGridDefine.SubtotalCalculate.FirstOrDefault(c => c.Parameter.Where(k => k == (CurrGridDefine[sender.Position.Column].ColName)).Any());
                //    Subtotal(sf, CurrGridDefine, sender.Position.Row);
                //}
                //当前找
                if (CurrGridDefine.SubtotalCalculate.Where(c => c.Parameter.Where(k => k == (CurrGridDefine[sender.Position.Column].ColName)).Any()).Any())
                {
                    List<SubtotalFormula> sflist = new List<SubtotalFormula>();
                    sflist = CurrGridDefine.SubtotalCalculate.Where(c => c.Parameter.Where(k => k == (CurrGridDefine[sender.Position.Column].ColName)).Any()).ToList();
                    ///一个结果列来自多个公式
                    foreach (SubtotalFormula sf in sflist)
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

            }

            #endregion

            #region 处理显示格式

            switch (CurrGridDefine[sender.Position.Column].CustomFormat)
            {
                case CustomFormatType.DefaultFormat:
                    break;
                case CustomFormatType.PercentFormat:

                    object cellvalue = CurrGridDefine.grid[sender.Position.Row, sender.Position.Column].Value;
                    if (cellvalue != null)
                    {
                        //实际上面转换过一次了。
                        var realvalue = cellvalue.ChangeType_ByConvert(CurrGridDefine[sender.Position.Column].ColPropertyInfo.PropertyType);
                        if (CurrGridDefine.grid[sender.Position.Row, sender.Position.Column].Editor != null)
                        {
                            CurrGridDefine.grid[sender.Position.Row, sender.Position.Column].DisplayText = CurrGridDefine.grid[sender.Position.Row, sender.Position.Column].Editor.ValueToDisplayString(realvalue);
                        }

                    }

                    break;
                case CustomFormatType.CurrencyFormat:

                    //var ColCurrencyTypeConverter = new DevAge.ComponentModel.Converter.CurrencyTypeConverter(typeof(decimal));???还使用吗？
                    CurrGridDefine.grid[sender.Position.Row, sender.Position.Column].Value = realTypeVal;
                    CurrGridDefine.grid[sender.Position.Row, sender.Position.Column].DisplayText = string.Format("{0:C}", realTypeVal);

                    break;
                case CustomFormatType.DecimalPrecision:
                    break;


                case CustomFormatType.Bool:
                    bool bl = realTypeVal.ToBool();
                    if (bl == true)
                    {
                        CurrGridDefine.grid[sender.Position.Row, sender.Position.Column].DisplayText = "是";
                    }
                    else
                    {
                        CurrGridDefine.grid[sender.Position.Row, sender.Position.Column].DisplayText = "否";
                    }

                    break;
                default:
                    break;
            }
            #endregion


            #region  总计  要放最后，因为其他计算会用到的结果值

            if (CurrGridDefine[sender.Position.Column].Summary)
            {
                decimal totalTemp = 0;
                //去掉首尾行
                for (int r = 1; r < CurrGridDefine.grid.RowsCount - 1; r++)
                {
                    if (CurrGridDefine.grid[r, sender.Position.Column].Value != null && CurrGridDefine.grid.Rows[r].RowData != null)
                    {
                        decimal CurrentTemp = 0;
                        if (decimal.TryParse(CurrGridDefine.grid[r, sender.Position.Column].Value.ToString(), out CurrentTemp))
                        {
                            totalTemp = CurrentTemp + totalTemp;
                        }
                    }
                }

                CurrGridDefine.grid[CurrGridDefine.grid.RowsCount - 1, sender.Position.Column].Value = totalTemp;
                //最后一行 C# 中各种格式化
                //https://www.cnblogs.com/zhangxiaoxia/p/15429877.html
                if (CurrGridDefine[sender.Position.Column].CustomFormat == CustomFormatType.CurrencyFormat)
                {
                    CurrGridDefine.grid[CurrGridDefine.grid.RowsCount - 1, sender.Position.Column].DisplayText = string.Format("{0:C}", totalTemp);
                    CurrGridDefine.grid[CurrGridDefine.grid.RowsCount - 1, sender.Position.Column].Value = string.Format("{0:C}", totalTemp); //这样才显示了货币符号
                }

            }
            #endregion

            //存在参与计算的才重新计划总数后面是否优化为  变动了才算？
            //这个事件传到sourcegridhelper.OnCalculateColumnValue
            if (OnCalculateColumnValue != null && CurrGridDefine[sender.Position.Column].Summary)
            {
                var currentObj = CurrGridDefine.grid.Rows[sender.Position.Row].RowData;
                OnCalculateColumnValue(currentObj, CurrGridDefine, sender.Position);
            }


        }



        /// <summary>
        /// 通过公式计算小计等
        /// </summary>
        /// <param name="sf"></param>
        /// <param name="CurrGridDefine"></param>
        /// <param name="rowindex"></param>
        private void Subtotal(SubtotalFormula sf, SourceGridDefine CurrGridDefine, int rowindex)
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

                string newstr = sf.StringFormula;
                for (int i = 0; i < sf.Parameter.Count; i++)
                {
                    newstr = newstr.Replace(sf.Parameter[i], "{" + i + "}");
                }
                string lastStr = string.Empty;
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
                    newstr = newstr.Replace(p, subItem);
                }
                DataTable dt = new DataTable();
                object obj = dt.Compute(newstr, "");
                //C# 判断数据是否为NaN的方法
                if (obj.ToString() == "NaN")
                {
                    obj = 0;
                }

                #region 处理显示格式

                switch (CurrGridDefine[sf.TagetCol.ColIndex].CustomFormat)
                {
                
                    case CustomFormatType.PercentFormat:

                        //实际上面转换过一次了。
                        var realvalue = obj.ChangeType_ByConvert(CurrGridDefine[sf.TagetCol.ColIndex].ColPropertyInfo.PropertyType);
                        ReflectionHelper.SetPropertyValue(currentObj, sf.TagetCol.ColName, realvalue);
                        CurrGridDefine.grid[rowindex, CurrGridDefine[sf.TagetCol.ColIndex].ColIndex].Value = realvalue;
                        CurrGridDefine.grid[rowindex, CurrGridDefine[sf.TagetCol.ColIndex].ColIndex].DisplayText = CurrGridDefine.grid[rowindex, CurrGridDefine[sf.TagetCol.ColIndex].ColIndex].Editor.ValueToDisplayString(realvalue);
                        break;
                    case CustomFormatType.CurrencyFormat:
                        //var ColCurrencyTypeConverter = new DevAge.ComponentModel.Converter.CurrencyTypeConverter(typeof(decimal));
                        int maxDecimalPlaces = AuthorizeController.GetMoneyDataPrecision(MainForm.Instance.AppContext);
                        decimal amount = 0.00m;
                        amount = RoundToNDecimalPlaces(obj, maxDecimalPlaces);
                        CurrGridDefine.grid[rowindex, sf.TagetCol.ColIndex].Value = amount;
                        CurrGridDefine.grid[rowindex, sf.TagetCol.ColIndex].DisplayText = string.Format("{0:C}", amount.ToString());
                        break;
                    case CustomFormatType.DecimalPrecision:
                        ReflectionHelper.SetPropertyValue(currentObj, sf.TagetCol.ColName, decimal.Parse(obj.ToString()));
                        CurrGridDefine.grid[rowindex, CurrGridDefine[sf.TagetCol.ColIndex].ColIndex].Value = decimal.Parse(obj.ToString());
                        break;

                    case CustomFormatType.DefaultFormat:
                    default:
                        ReflectionHelper.SetPropertyValue(currentObj, sf.TagetCol.ColName, int.Parse(obj.ToString()));
                        CurrGridDefine.grid[rowindex, CurrGridDefine[sf.TagetCol.ColIndex].ColIndex].Value = int.Parse(obj.ToString());
                        break;
                }
                #endregion

                return;

                System.Reflection.PropertyInfo pi = null;
                pi = sf.TagetCol.ColPropertyInfo;
                Type newcolType; ;
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

                switch (newcolType.FullName)
                {
                    case "System.Decimal":
                        //decimal subDec = 0.00m;
                        if (!decimal.TryParse(obj.ToString(), out decimal subDec))
                        {
                            MainForm.Instance.uclog.AddLog(obj.ToString() + "参数为数值时出错", Global.UILogType.错误);
                        }
                        else
                        {

                            ReflectionHelper.SetPropertyValue(currentObj, sf.TagetCol.ColName, subDec);
                            CurrGridDefine.grid[rowindex, CurrGridDefine[sf.TagetCol.ColIndex].ColIndex].Value = subDec.ToString();
                        }
                        break;
                    case "System.Int16":
                    case "System.Int32":
                    case "System.Int64":
                        ReflectionHelper.SetPropertyValue(currentObj, sf.TagetCol.ColName, int.Parse(obj.ToString()));
                        CurrGridDefine.grid[rowindex, CurrGridDefine[sf.TagetCol.ColIndex].ColIndex].Value = int.Parse(obj.ToString());
                        break;
                }
            }
        }

        /// <summary>
        /// 通过公式计算小计等
        /// 反向计算
        /// </summary>
        /// <param name="sf"></param>
        /// <param name="CurrGridDefine"></param>
        /// <param name="rowindex"></param>
        private void SubtotalReverse(SubtotalFormula sf, SourceGridDefine CurrGridDefine, int rowindex)
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

                string newstr = sf.StringFormula;
                for (int i = 0; i < sf.Parameter.Count; i++)
                {
                    newstr = newstr.Replace(sf.Parameter[i], "{" + i + "}");
                }
                string lastStr = string.Empty;
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
                    newstr = newstr.Replace(p, subItem);
                }
                DataTable dt = new DataTable();
                object obj = dt.Compute(newstr, "");
                //C# 判断数据是否为NaN的方法
                if (obj.ToString() == "NaN")
                {
                    obj = 0;
                }

                #region 处理显示格式

                switch (CurrGridDefine[sf.TagetCol.ColIndex].CustomFormat)
                {
                    
                    case CustomFormatType.PercentFormat:

                        //实际上面转换过一次了。
                        var realvalue = obj.ChangeType_ByConvert(CurrGridDefine[sf.TagetCol.ColIndex].ColPropertyInfo.PropertyType);
                        ReflectionHelper.SetPropertyValue(currentObj, sf.TagetCol.ColName, realvalue);
                        CurrGridDefine.grid[rowindex, CurrGridDefine[sf.TagetCol.ColIndex].ColIndex].Value = realvalue;
                        CurrGridDefine.grid[rowindex, CurrGridDefine[sf.TagetCol.ColIndex].ColIndex].DisplayText = CurrGridDefine.grid[rowindex, CurrGridDefine[sf.TagetCol.ColIndex].ColIndex].Editor.ValueToDisplayString(realvalue);
                        break;
                    case CustomFormatType.CurrencyFormat:
                        //var ColCurrencyTypeConverter = new DevAge.ComponentModel.Converter.CurrencyTypeConverter(typeof(decimal));
                        int maxDecimalPlaces = AuthorizeController.GetMoneyDataPrecision(MainForm.Instance.AppContext);
                        decimal amount = 0.00m;
                        amount = RoundToNDecimalPlaces(obj, maxDecimalPlaces);
                        CurrGridDefine.grid[rowindex, sf.TagetCol.ColIndex].Value = amount;
                        CurrGridDefine.grid[rowindex, sf.TagetCol.ColIndex].DisplayText = string.Format("{0:C}", amount.ToString());
                        break;
                    case CustomFormatType.DecimalPrecision:
                        ReflectionHelper.SetPropertyValue(currentObj, sf.TagetCol.ColName, decimal.Parse(obj.ToString()));
                        CurrGridDefine.grid[rowindex, CurrGridDefine[sf.TagetCol.ColIndex].ColIndex].Value = decimal.Parse(obj.ToString());
                        break;
                    case CustomFormatType.DefaultFormat:
                    default:
                        ReflectionHelper.SetPropertyValue(currentObj, sf.TagetCol.ColName, int.Parse(obj.ToString()));
                        CurrGridDefine.grid[rowindex, CurrGridDefine[sf.TagetCol.ColIndex].ColIndex].Value = int.Parse(obj.ToString());
                        break;
                }
                #endregion

                return;

                System.Reflection.PropertyInfo pi = null;
                pi = sf.TagetCol.ColPropertyInfo;
                Type newcolType; ;
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

                switch (newcolType.FullName)
                {
                    case "System.Decimal":
                        //decimal subDec = 0.00m;
                        if (!decimal.TryParse(obj.ToString(), out decimal subDec))
                        {
                            MainForm.Instance.uclog.AddLog(obj.ToString() + "参数为数值时出错", Global.UILogType.错误);
                        }
                        else
                        {

                            ReflectionHelper.SetPropertyValue(currentObj, sf.TagetCol.ColName, subDec);
                            CurrGridDefine.grid[rowindex, CurrGridDefine[sf.TagetCol.ColIndex].ColIndex].Value = subDec.ToString();
                        }
                        break;
                    case "System.Int16":
                    case "System.Int32":
                    case "System.Int64":
                        ReflectionHelper.SetPropertyValue(currentObj, sf.TagetCol.ColName, int.Parse(obj.ToString()));
                        CurrGridDefine.grid[rowindex, CurrGridDefine[sf.TagetCol.ColIndex].ColIndex].Value = int.Parse(obj.ToString());
                        break;
                }
            }
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
                //设置编辑时的行号 不要减-1  因为第一行是列头
                CurrGridDefine.grid[sender.Position.Row, 0].Value = sender.Position.Row;
                sh.SetRowEditable(CurrGridDefine.grid, new int[] { sender.Position.Row }, CurrGridDefine);
                if (sender.Cell.Editor != null)
                {
                    sender.Cell.View = SourceGridDefine.editView;
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
                CurrGridDefine[sender.Position.Column].IsEdit = false;
            }

        }

        public override void OnFocusLeaving(CellContext sender, CancelEventArgs e)
        {
            base.OnFocusLeaving(sender, e);
            //如果有总计点到最后一行。直接返回
            if (CurrGridDefine.HasSummaryRow && sender.Position.Row == CurrGridDefine.grid.RowsCount - 1)
            {
                return;
            }
            //恢复背景色
            if (CurrGridDefine[sender.Position.Column].CustomFormat == CustomFormatType.CurrencyFormat)
            {
                sender.Cell.View = CurrGridDefine.ViewNormalMoney;
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
            CurrGridDefine[sender.Position.Column].IsEdit = true;
        }


        public override void OnEditStarting(CellContext sender, CancelEventArgs e)
        {
        }
        public override void OnEditStarted(CellContext sender, EventArgs e)
        {
        }
        public override void OnEditEnded(CellContext sender, EventArgs e)
        {
            if (sender.Value == null)
            {
                return;
            }
            //编辑后的样式如果是货币要右对齐显示
            if (CurrGridDefine[sender.Position.Column].CustomFormat == CustomFormatType.CurrencyFormat)
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
            //这里计算是一些公共的，针对具体单据的计算 需要用一个事件抛出来处理
            #region 小计加总计(反向)

            //这里的思路是 a+b=c  ,a b 字段变化才会进入计划，所以如果 a,b是小计，是由另一个计算来的。则不会变化。。
            //就使用上溯源，如果a是其他的组的目标，则找到相关组的操作数变化来触发
            //太复杂。容易出错，多次执行，用长公式代替？
            if (CurrGridDefine.SubtotalCalculateReverse.Count > 0)
            {
                //向上找
                //if (CurrGridDefine.SubtotalCalculate.Where(c => c.TagetCol.ColName == CurrGridDefine[sender.Position.Column].ColName).Any())
                //{
                //    SubtotalFormula sf = new SubtotalFormula();
                //    sf = CurrGridDefine.SubtotalCalculate.FirstOrDefault(c => c.Parameter.Where(k => k == (CurrGridDefine[sender.Position.Column].ColName)).Any());
                //    Subtotal(sf, CurrGridDefine, sender.Position.Row);
                //}
                //当前找
                if (CurrGridDefine.SubtotalCalculateReverse.Where(c => c.Parameter.Where(k => k == (CurrGridDefine[sender.Position.Column].ColName)).Any()).Any())
                {
                    List<SubtotalFormula> sflist = new List<SubtotalFormula>();
                    sflist = CurrGridDefine.SubtotalCalculateReverse.Where(c => c.Parameter.Where(k => k == (CurrGridDefine[sender.Position.Column].ColName)).Any()).ToList();
                    ///一个结果列来自多个公式
                    foreach (SubtotalFormula sf in sflist)
                    {
                        try
                        {
                            SubtotalReverse(sf, CurrGridDefine, sender.Position.Row);
                        }
                        catch (Exception error)
                        {
                            MainForm.Instance.logger.LogError("出现应用程序未处理的异常,请更新到新版本，如果无法解决，请联系管理员！\r\n" + error.Message, error);
                        }
                    }

                }

                //存在参与计算的才重新计划总数后面是否优化为  变动了才算？
                //这个事件传到sourcegridhelper.OnCalculateColumnValue
                if (OnCalculateColumnValue != null && CurrGridDefine[sender.Position.Column].Summary)
                {
                    var currentObj = CurrGridDefine.grid.Rows[sender.Position.Row].RowData;
                    OnCalculateColumnValue(currentObj, CurrGridDefine, sender.Position);
                }

            }

            #endregion
        }

    }



}
