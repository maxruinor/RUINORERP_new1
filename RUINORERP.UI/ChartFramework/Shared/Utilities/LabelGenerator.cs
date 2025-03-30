using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.UI.ChartFramework.Data.Transformers
{
    // Data/Transformers/LabelGenerator.cs
    public static class LabelGenerator
    {
        public static string[] CreateCategoryLabels<T>(
            IEnumerable<T> sourceData,
            Func<T, object> valueSelector,
            LabelFormat? format = null)
        {
            format ??= new AutoLabelFormat();

            return sourceData
                .Select(item => FormatLabel(valueSelector(item), format))
                .ToArray();
        }

        private static string FormatLabel(object rawValue, LabelFormat format)
        {
            return rawValue switch
            {
                DateTime dt => format.FormatDateTime(dt),
                double num => format.FormatNumber(num),
                _ => rawValue.ToString()
            };
        }
    }

    // 标签格式化策略
    public abstract class LabelFormat
    {
        public abstract string FormatDateTime(DateTime value);
        public abstract string FormatNumber(double value);
    }

    // 自动格式化（根据区域设置）
    public class AutoLabelFormat : LabelFormat
    {
        public override string FormatDateTime(DateTime value)
            => value.ToString(CultureInfo.CurrentUICulture.DateTimeFormat.ShortDatePattern);

        public override string FormatNumber(double value)
            => value.ToString("N2", CultureInfo.CurrentUICulture);
    }
}
