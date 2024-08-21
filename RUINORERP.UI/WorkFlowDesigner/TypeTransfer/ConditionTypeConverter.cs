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
    public class ConditionTypeConverter<T> : System.ComponentModel.TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if (value is string)
            {
                // 将字符串转换为 WFCondition 对象
                string strValue = (string)value;

                // 解析字符串并创建 WFCondition 对象
                WFCondition condition = new WFCondition();
                condition.Name = strValue;

                return condition;
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
                // 将 WFCondition 对象转换为字符串
                WFCondition condition = (WFCondition)value;

                return condition.Name;
            }

            return base.ConvertTo(context, culture, value, destinationType);
        }
    }
}
