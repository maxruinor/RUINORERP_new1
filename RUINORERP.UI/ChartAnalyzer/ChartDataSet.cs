using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.UI.ChartAnalyzer
{
    /// <summary>
    /// 图表数据集
    /// </summary>
    public class ChartDataSet
    {
        public string[] Labels { get; set; }
        public Dictionary<string, double[]> SeriesData { get; set; }

        //public List<string> Labels { get; set; } = new List<string>();
        public List<ChartSeries> Series { get; set; } = new List<ChartSeries>();
    }

    public class ChartSeries
    {
        public string Name { get; set; }
        public List<double> Values { get; set; } = new List<double>();
    }
}
