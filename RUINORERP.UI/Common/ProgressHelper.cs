using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.UI.Common
{
    public static class ProgressHelper
    {
        public static int CalculatePercentage(int current, int total)
        {
            double raw = (double)(current + 1) / total * 100;
            return Math.Min(100, (int)Math.Round(raw));
        }
    }
}
