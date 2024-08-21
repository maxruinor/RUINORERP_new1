using RUINORERP.UI.WorkFlowDesigner.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.UI.WorkFlowDesigner.TypeTransfer
{
    public class ListConditionTypeConverter : System.ComponentModel.TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if (value is string)
            {
                // 将字符串转换为列表条件
                string strValue = (string)value;

                // 解析字符串并创建列表条件
                List<WFCondition> conditions = new List<WFCondition>();
                // 根据字符串的格式进行解析和创建条件

                return conditions;
            }

            return base.ConvertFrom(context, culture, value);
        }

        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            return destinationType == typeof(string) || base.CanConvertTo(context, destinationType);
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == typeof(string))
            {
                // 将列表条件转换为字符串
                List<WFCondition> conditions = (List<WFCondition>)value;

                // 将列表条件转换为字符串表示
                string strValue = string.Join(",", conditions.Select(c => c.ToString()));

                return strValue;
            }

            return base.ConvertTo(context, culture, value, destinationType);
        }
    }
}
