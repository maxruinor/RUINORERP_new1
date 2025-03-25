using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.UI.ChartAnalyzer
{
    public class PagedDataSource : IChartDataSource
    {
        public async Task<ChartDataSet> GetDataAsync(ChartRequest request)
        {
            const int pageSize = 5000;
            var resultSet = new ChartDataSet();

            for (int page = 1; ; page++)
            {
                var pageData = await GetPageData(request, page, pageSize);
                if (pageData == null) break;

                MergeDataSet(resultSet, pageData);
            }

            return resultSet;
        }

        private async Task<ChartDataSet> GetPageData(ChartRequest request, int page, int size)
        {
            // 实现分页逻辑
        }
    }
}
