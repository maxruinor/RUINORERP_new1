using RUINORERP.Model.ChartFramework.Contracts;
using RUINORERP.Model.ChartFramework.Models;
using System;
using System.Threading.Tasks;

namespace RUINORERP.UI.ChartFramework.Shared.Utilities
{
    /// <summary>
    /// 虚拟化数据代理 (简化版)
    /// </summary>
    public class VirtualizationDataProxy
    {
        public async Task<ChartData> GetDataAsync(IDataProvider provider, DataRequest request)
        {
            // 简化实现，直接获取数据
            return await provider.GetDataAsync(request);
        }
    }
}
