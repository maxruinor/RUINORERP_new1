using RUINORERP.UI.ChartFramework.Core;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.UI.ChartFramework.Extensions.Theming
{
    // 样式扩展
    public static class StyleExtensions
    {
        public static SKPathEffect ToPathEffect(this LineType style)
        {
            return style switch
            {
                LineType.Dashed => SKPathEffect.CreateDash(new[] { 10f, 5f }, 0),
                LineType.Dotted => SKPathEffect.CreateDash(new[] { 2f, 3f }, 0),
                _ => null
            };
        }
    }
}
