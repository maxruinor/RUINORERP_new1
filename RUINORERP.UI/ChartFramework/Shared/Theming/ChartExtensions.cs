﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SkiaSharp;
using System.Globalization;
using RUINORERP.UI.ChartFramework.Core.Models;
using LiveChartsCore.SkiaSharpView.Drawing;
using LiveChartsCore;
namespace RUINORERP.UI.ChartFramework.Shared.Theming
{
    public static class ChartExtensions
    {
        /// <summary>
        /// 将十六进制颜色字符串转换为SKColor
        /// </summary>
        public static SKColor ToSKColor(this string hexColor)
        {
            hexColor = hexColor.Replace("#", string.Empty);

            byte r = byte.Parse(hexColor.Substring(0, 2), NumberStyles.HexNumber);
            byte g = byte.Parse(hexColor.Substring(2, 2), NumberStyles.HexNumber);
            byte b = byte.Parse(hexColor.Substring(4, 2), NumberStyles.HexNumber);

            return hexColor.Length == 8
                ? new SKColor(r, g, b, byte.Parse(hexColor.Substring(6, 2), NumberStyles.HexNumber))
                : new SKColor(r, g, b);
        }


        // 扩展方法：从LiveCharts系列获取我们的DataSeries
        //public static class ChartExtensions
        //{
        //    public static DataSeries GetDataSeries(this ISeries series)
        //    {
        //        return series is IStrokedAndFilled<SkiaSharpDrawingContext> styledSeries
        //            ? styledSeries.Tag as DataSeries
        //            : null;
        //    }

        //}
    }
}

