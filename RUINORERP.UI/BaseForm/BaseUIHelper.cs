using RUINORERP.Global.CustomAttribute;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.UI.BaseForm
{
    public class BaseUIHelper
    {


        /// <summary>
        /// 获取主键
        /// </summary>
        /// <typeparam name="T">SqlSugar框架实体类</typeparam>
        /// <returns></returns>
        public static string GetEntityPrimaryKey<T>()
        {
            string primaryColName = string.Empty;
            foreach (PropertyInfo field in typeof(T).GetProperties())
            {

                //获取指定类型的自定义特性
                object[] attrs = field.GetCustomAttributes(false);
                foreach (var attr in attrs)
                {
                    if (attr is SubtotalResultAttribute)
                    {
                        SubtotalResultAttribute subtotalResultAttribute = attr as SubtotalResultAttribute;
                    }

                    if (attr is SubtotalAttribute)
                    {
                        SubtotalAttribute subtotalAttribute = attr as SubtotalAttribute;

                    }
                    if (attr is SummaryAttribute)
                    {
                        SummaryAttribute summaryAttribute = attr as SummaryAttribute;
                    }
                    if (attr is ToolTipAttribute)
                    {
                        ToolTipAttribute toolTipAttribute = attr as ToolTipAttribute;

                    }
                    if (attr is ReadOnlyAttribute)//图片只读
                    {
                        ReadOnlyAttribute readOnlyAttribute = attr as ReadOnlyAttribute;
                        //  col.ReadOnly = readOnlyAttribute.IsReadOnly;
                    }
                    if (attr is VisibleAttribute)//明细的产品ID隐藏
                    {
                        VisibleAttribute visibleAttribute = attr as VisibleAttribute;
                        // col.Visible = visibleAttribute.Visible;
                    }
                    if (attr is SugarColumn)
                    {
                        SugarColumn sugarColumn = attr as SugarColumn;
                        if (sugarColumn.IsPrimaryKey)
                        {
                            primaryColName = sugarColumn.ColumnName;
                            return primaryColName;
                            break;
                        }
                        // col.SugarCol = sugarColumn;
                    }

                }
            }
            return primaryColName;
        }

    }
}
