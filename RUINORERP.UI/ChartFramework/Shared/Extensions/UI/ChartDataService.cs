using RUINORERP.UI.ChartFramework.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.UI.ChartFramework.Shared.Extensions.UI
{
    public class ChartDataService
    {// Services/ChartDataService.cs
        public void UpdateCategoryLabels(ChartData dataSet, IEnumerable<object> newLabels)
        {
            dataSet.MetaData.CategoryLabels = newLabels.Select(x => x.ToString()).ToArray();

            // 触发UI更新
            dataSet.MetaData.NotifyPropertyChanged(nameof(ChartMetaData.CategoryLabels));
        }
    }
}
