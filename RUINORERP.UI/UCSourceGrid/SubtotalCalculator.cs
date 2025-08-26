using System;
using System.Reflection;
using SourceGrid;

namespace RUINORERP.UI.UCSourceGrid
{
    public static class SubtotalCalculator
    {
        /// <summary>
        /// 执行小计计算
        /// </summary>
        public static void Subtotal(CalculateFormula formula, SourceGridDefine gridDefine, int rowIndex)
        {
            if (formula == null || formula.TagetCol == null || gridDefine == null)
                return;

            // 获取当前行数据对象
            var rowData = gridDefine.grid.Rows[rowIndex].RowData;
            if (rowData == null)
                return;

            // 执行计算
            object result = CalculationExecutor.Execute(formula, rowData);
            if (result == null)
                return;

            // 更新单元格值
            UpdateCellValue(gridDefine, rowIndex, formula.TagetCol, result);

            // 更新数据对象
            UpdateDataObjectProperty(rowData, formula.TagetCol.ColName, result);
        }

        /// <summary>
        /// 更新网格单元格值
        /// </summary>
        private static void UpdateCellValue(SourceGridDefine gridDefine, int rowIndex, SGDefineColumnItem targetCol, object value)
        {
            int columnIndex = gridDefine.grid.Columns.GetColumnInfo(targetCol.UniqueId).Index;
            if (columnIndex >= 0 && rowIndex >= 0 && rowIndex < gridDefine.grid.Rows.Count)
            {
                gridDefine.grid[rowIndex, columnIndex].Value = value;
            }
        }

        /// <summary>
        /// 更新数据对象属性
        /// </summary>
        private static void UpdateDataObjectProperty(object dataObject, string propertyName, object value)
        {
            if (dataObject == null || string.IsNullOrEmpty(propertyName))
                return;

            PropertyInfo property = dataObject.GetType().GetProperty(propertyName);
            if (property != null && value != null)
            {
                // 转换值类型以匹配属性类型
                object convertedValue = Convert.ChangeType(value, property.PropertyType);
                property.SetValue(dataObject, convertedValue);
            }
        }
    }
}
