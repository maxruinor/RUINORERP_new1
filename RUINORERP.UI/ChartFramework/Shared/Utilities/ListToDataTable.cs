using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.UI.ChartFramework.Shared.Utilities
{
    // 辅助类：将任意列表转换为DataTable
    public static class ListToDataTable
    {
        public static DataTable ConvertToDataTable(IList list)
        {
            DataTable table = new DataTable();

            if (list == null || list.Count == 0)
                return table;

            // 获取属性
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(list[0]);

            // 创建列
            foreach (PropertyDescriptor prop in properties)
            {
                table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
            }

            // 填充数据
            foreach (var item in list)
            {
                DataRow row = table.NewRow();
                foreach (PropertyDescriptor prop in properties)
                {
                    row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                }
                table.Rows.Add(row);
            }

            return table;
        }
    }
}
