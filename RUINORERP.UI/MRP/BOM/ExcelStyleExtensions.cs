using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.UI.MRP.BOM
{
    // 样式扩展方法
    public static class ExcelStyleExtensions
    {


        // 在ExcelStyleExtensions类中添加
        public static void SetTitleStyle(this ExcelStyle style)
        {
            style.Font.Bold = true;
            style.Font.Size = 18; // 适当缩小字号
            style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            style.VerticalAlignment = ExcelVerticalAlignment.Center;
            style.Fill.PatternType = ExcelFillStyle.Solid;
            style.Fill.BackgroundColor.SetColor(Color.LightGray);
        }

        public static void SetSummaryStyle(this ExcelStyle style)
        {
            style.Font.Bold = true;
            style.Font.Color.SetColor(Color.DarkBlue);
            style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
            style.Fill.PatternType = ExcelFillStyle.Solid;
            style.Fill.BackgroundColor.SetColor(Color.LightYellow);
        }


    }
}
