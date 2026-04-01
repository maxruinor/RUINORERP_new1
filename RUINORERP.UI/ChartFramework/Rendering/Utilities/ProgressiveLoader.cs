using RUINORERP.Model.ChartFramework.Contracts;
using RUINORERP.Model.ChartFramework.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.UI.ChartFramework.Rendering.Utilities
{
    /// <summary>
    /// 鍒嗗眰鍔犺浇鍣?(鐢ㄤ簬澶ф暟鎹噺鍦烘櫙)
    /// </summary>
    public class ProgressiveLoader
    {
        private const int DEFAULT_BATCH_SIZE = 100;

        /// <summary>
        /// 鍒嗘壒鍔犺浇鏁版嵁
        /// </summary>
        public async Task<List<dynamic>> LoadDataProgressivelyAsync(
            IDataProvider provider,
            DataRequest request,
            int batchSize = DEFAULT_BATCH_SIZE,
            IProgress<double> progress = null)
        {
            var allData = new List<dynamic>();
            int pageIndex = 1;
            bool hasMore = true;

            while (hasMore)
            {
                // 鑾峰彇涓€鎵规暟鎹?
                var batch = await LoadBatchAsync(provider, request, pageIndex, batchSize);

                if (batch == null || !batch.Any())
                {
                    hasMore = false;
                    break;
                }

                allData.AddRange(batch);
                pageIndex++;

                // 鎶ュ憡杩涘害
                progress?.Report((double)pageIndex / 10); // 浼扮畻杩涘害

                // 濡傛灉鏁版嵁閲忓皬浜庢壒娆″ぇ灏忥紝璇存槑宸茬粡鏄渶鍚庝竴鎵?
                if (batch.Count < batchSize)
                {
                    hasMore = false;
                }
            }

            return allData;
        }

        /// <summary>
        /// 鍔犺浇鍗曟壒鏁版嵁
        /// </summary>
        private async Task<List<dynamic>> LoadBatchAsync(
            IDataProvider provider,
            DataRequest request,
            int pageIndex,
            int pageSize)
        {
            // TODO: 瀹炵幇鍒嗛〉鍔犺浇閫昏緫
            // 鐩墠 IDataProvier 涓嶆敮鎸佸垎椤碉紝闇€瑕佹墿灞曟帴鍙ｆ垨浣跨敤鍏朵粬鏂瑰紡

            // 涓存椂鏂规锛氫竴娆℃€у姞杞芥墍鏈夋暟鎹悗鎴彇
            var allData = await provider.GetDataAsync(request);
            
            // 杩欓噷闇€瑕佹牴鎹疄闄呮暟鎹粨鏋勫鐞?
            // 鐢变簬 ChartData 涓嶆槸鍒楄〃缁撴瀯锛岄渶瑕佺壒娈婂鐞?
            return new List<dynamic>();
        }

        /// <summary>
        /// 寮傛鍔犺浇骞跺閲忔洿鏂板浘琛?
        /// </summary>
        public async Task LoadAndRenderProgressivelyAsync(
            IDataProvider provider,
            DataRequest request,
            Action<ChartData> onChunkReceived,
            int chunkSize = 50)
        {
            var accumulatedData = new ChartData
            {
                Title = request.Title ?? "缁熻鍒嗘瀽",
                Series = new List<DataSeries>()
            };

            // 鍏堣幏鍙栧厓鏁版嵁
            var dimensions = provider.GetDimensions();
            var metrics = provider.GetMetrics();

            // 鍒濆鍖栫郴鍒?
            foreach (var metric in metrics)
            {
                accumulatedData.Series.Add(new DataSeries
                {
                    Name = metric.DisplayName,
                    Values = new List<double>()
                });
            }

            // 鍒嗘壒鍔犺浇
            int processedCount = 0;
            var rawData = await LoadDataProgressivelyAsync(provider, request, chunkSize);

            // 姣忓鐞嗕竴鎵规暟鎹氨閫氱煡 UI 鏇存柊
            foreach (var item in rawData)
            {
                ProcessDataItem(item, accumulatedData, request);
                processedCount++;

                if (processedCount % chunkSize == 0)
                {
                    onChunkReceived?.Invoke(accumulatedData);
                    await Task.Delay(50); // 閬垮厤 UI 闃诲
                }
            }

            // 鏈€鍚庝竴娆″畬鏁存洿鏂?
            onChunkReceived?.Invoke(accumulatedData);
        }

        /// <summary>
        /// 澶勭悊鍗曚釜鏁版嵁椤?
        /// </summary>
        private void ProcessDataItem(dynamic item, ChartData accumulatedData, DataRequest request)
        {
            // 娣诲姞鍒板垎绫绘爣绛?
            // 杩欓噷闇€瑕佹牴鎹疄闄呮暟鎹粨鏋勮皟鏁?

            // 娣诲姞鍒板悇涓郴鍒?
            foreach (var series in accumulatedData.Series)
            {
                // 浠庡姩鎬佸璞℃彁鍙栧€?
                // 鍏蜂綋瀹炵幇鍙栧喅浜庢暟鎹簮杩斿洖鐨勭粨鏋?
            }
        }
    }

    /// <summary>
    /// 铏氭嫙婊氬姩鍥捐〃瀹夸富 (澶勭悊瓒呭ぇ鏁版嵁闆?
    /// </summary>
    public class VirtualizedChartHost
    {
        private readonly List<ChartSegment> _segments = new();
        private int _visibleStartIndex;
        private int _visibleEndIndex;

        /// <summary>
        /// 璁＄畻鍙鑼冨洿
        /// </summary>
        public Range CalculateVisibleRange(int totalItems, int viewportHeight, int itemHeight)
        {
            var visibleCount = (int)Math.Ceiling((double)viewportHeight / itemHeight);
            return new Range(_visibleStartIndex, Math.Min(_visibleStartIndex + visibleCount, totalItems));
        }

        /// <summary>
        /// 鍔犺浇鍙鑼冨洿鐨勬
        /// </summary>
        public void LoadSegmentsForRange(Range range)
        {
            foreach (var segment in _segments)
            {
                segment.Visible = range.IntersectsWith(segment.Range);
            }
        }

        /// <summary>
        /// 璁剧疆鍙鑼冨洿
        /// </summary>
        public void SetVisibleRange(int startIndex, int endIndex)
        {
            _visibleStartIndex = startIndex;
            _visibleEndIndex = endIndex;
        }
    }

    /// <summary>
    /// 鍥捐〃娈?
    /// </summary>
    public class ChartSegment
    {
        public Range Range { get; set; }
        public bool Visible { get; set; } = true;
        public ChartData Data { get; set; }
    }

    /// <summary>
    /// 鑼冨洿绫?
    /// </summary>
    public class Range
    {
        public int Start { get; }
        public int End { get; }

        public Range(int start, int end)
        {
            Start = start;
            End = end;
        }

        public bool IntersectsWith(Range other)
        {
            return Start <= other.End && End >= other.Start;
        }
    }
}

