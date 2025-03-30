using Org.BouncyCastle.Asn1.Cms;
using RUINORERP.UI.ChartFramework.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.UI.ChartFramework.Data
{
    // Data/VirtualizationDataProxy.cs
    public class VirtualizationDataProxy : IChartDataSource
    {
        private readonly IDataLoader _loader;
        private readonly int _chunkSize = 5000;

        public async Task<ChartDataSet> GetDataAsync(ChartRequest request)
        {
            var totalCount = await _loader.GetTotalCountAsync(request);
            var virtualData = new VirtualChartDataSet(totalCount);

            // 预加载首屏数据
            var firstChunk = await _loader.LoadChunkAsync(request, 0, _chunkSize);
            virtualData.UpdateChunk(0, firstChunk);

            return virtualData;
        }
    }

    public class VirtualChartDataSet : ChartDataSet
    {
        private readonly Dictionary<int, DataChunk> _loadedChunks = new();

        public VirtualChartDataSet(int totalCount)
        {
            MetaData.MaxDataPoints = totalCount;
        }

        public void UpdateChunk(int chunkIndex, DataChunk chunk)
        {
            _loadedChunks[chunkIndex] = chunk;
            // 触发局部更新事件...
        }

        // 重写索引器实现按需加载
        public override double GetValue(int seriesIndex, int pointIndex)
        {
            var chunkIndex = pointIndex / _chunkSize;
            if (!_loadedChunks.TryGetValue(chunkIndex, out var chunk))
            {
                // 触发异步加载
                _ = LoadChunkAsync(chunkIndex);
                return double.NaN; // 临时返回空值
            }
            return chunk[seriesIndex, pointIndex % _chunkSize];
        }
    }
}
