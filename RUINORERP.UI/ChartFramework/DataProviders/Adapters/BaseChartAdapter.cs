using RUINORERP.Model.ChartFramework.Contracts;
using RUINORERP.Model.ChartFramework.Models;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.UI.ChartFramework.DataProviders.Adapters
{
    /// <summary>
    /// 鍥捐〃鏁版嵁閫傞厤鍣ㄥ熀绫?(缁熶竴妯℃澘)
    /// </summary>
    public abstract class BaseChartAdapter : IDataProvider
    {
        protected readonly ISqlSugarClient _db;

        protected BaseChartAdapter(ISqlSugarClient db)
        {
            _db = db;
        }

        /// <summary>
        /// 鑾峰彇涓昏〃鍚?(鐢卞瓙绫诲疄鐜?
        /// </summary>
        protected abstract string PrimaryTableName { get; }

        /// <summary>
        /// 鑾峰彇缁村害閰嶇疆 (鐢卞瓙绫诲疄鐜?
        /// </summary>
        public abstract IEnumerable<DimensionConfig> GetDimensions();

        /// <summary>
        /// 鑾峰彇鎸囨爣閰嶇疆 (鐢卞瓙绫诲疄鐜?
        /// </summary>
        public abstract IEnumerable<MetricConfig> GetMetrics();

        /// <summary>
        /// 鑾峰彇鍥捐〃鏁版嵁 (妯℃澘鏂规硶)
        /// </summary>
        public async Task<ChartData> GetDataAsync(DataRequest request)
        {
            ValidateRequest(request);

            // 1. 鏋勫缓鍩虹鏌ヨ
            var query = BuildBaseQuery(request);

            // 2. 搴旂敤鏃堕棿杩囨护
            ApplyTimeFilter(ref query, request);

            // 3. 搴旂敤鑷畾涔夎繃婊?
            ApplyCustomFilters(ref query, request);

            // 4. 搴旂敤鍒嗙粍
            ApplyGroupBy(ref query, request);

            // 5. 鎵ц鏌ヨ
            var rawData = await ExecuteQueryAsync(query);

            // 6. 杞崲涓?ChartData
            return TransformToChartData(rawData, request);
        }

        /// <summary>
        /// 楠岃瘉璇锋眰鍙傛暟
        /// </summary>
        protected virtual void ValidateRequest(DataRequest request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            if (string.IsNullOrWhiteSpace(request.TimeField))
                throw new ArgumentException("蹇呴』鎸囧畾鏃堕棿瀛楁");
        }

        /// <summary>
        /// 鏋勫缓鍩虹鏌ヨ (鍙敱瀛愮被閲嶅啓)
        /// </summary>
        protected virtual ISugarQueryable<dynamic> BuildBaseQuery(DataRequest request)
        {
            return _db.Queryable<dynamic>(PrimaryTableName);
        }

        /// <summary>
        /// 搴旂敤鏃堕棿杩囨护 (鍙敱瀛愮被鎵╁睍)
        /// </summary>
        protected virtual void ApplyTimeFilter(ref ISugarQueryable<dynamic> query, DataRequest request)
        {
            if (request.StartTime.HasValue)
            {
                query = query.Where($"{request.TimeField} >= @StartTime", 
                    new { StartTime = request.StartTime.Value });
            }

            if (request.EndTime.HasValue)
            {
                query = query.Where($"{request.TimeField} <= @EndTime", 
                    new { EndTime = request.EndTime.Value });
            }
        }

        /// <summary>
        /// 搴旂敤鑷畾涔夎繃婊?(鍙敱瀛愮被鎵╁睍)
        /// </summary>
        protected virtual void ApplyCustomFilters(ref ISugarQueryable<dynamic> query, DataRequest request)
        {
            // 瀛愮被鍙互閲嶅啓姝ゆ柟娉曟坊鍔犵壒瀹氳繃婊ら€昏緫
        }

        /// <summary>
        /// 搴旂敤鍒嗙粍 (鍙敱瀛愮被鎵╁睍)
        /// </summary>
        protected virtual void ApplyGroupBy(ref ISugarQueryable<dynamic> query, DataRequest request)
        {
            if (request.Dimensions != null && request.Dimensions.Any())
            {
                var groupExpr = string.Join(",", request.Dimensions.Select(d => d.FieldName));
                query = query.GroupBy(groupExpr);
            }
        }

        /// <summary>
        /// 鎵ц鏌ヨ (鍙敱瀛愮被鎵╁睍)
        /// </summary>
        protected virtual async Task<List<dynamic>> ExecuteQueryAsync(ISugarQueryable<dynamic> query)
        {
            return await query.ToListAsync();
        }

        /// <summary>
        /// 杞崲涓?ChartData(鐢卞瓙绫诲疄鐜?
        /// </summary>
        protected abstract ChartData TransformToChartData(List<dynamic> rawData, DataRequest request);

        /// <summary>
        /// 鏍煎紡鍖栫淮搴︽爣绛?
        /// </summary>
        protected virtual string FormatDimensionLabel(dynamic item, DataRequest request)
        {
            if (request.Dimensions == null || !request.Dimensions.Any())
                return "鏈煡";

            var labelParts = request.Dimensions.Select(d =>
            {
                try
                {
                    var prop = item.GetType().GetProperty(d.FieldName);
                    var value = prop?.GetValue(item);

                    // 鐗规畩澶勭悊鏃ユ湡绫诲瀷
                    if (value is DateTime dt)
                    {
                        return request.RangeType switch
                        {
                            TimeRangeType.Daily => dt.ToString("yyyy-MM-dd"),
                            TimeRangeType.Monthly => dt.ToString("yyyy-MM"),
                            TimeRangeType.Quarterly => $"Q{(dt.Month - 1) / 3 + 1}-{dt.Year}",
                            TimeRangeType.Yearly => dt.ToString("yyyy"),
                            _ => dt.ToString("yyyy-MM-dd")
                        };
                    }

                    return value?.ToString() ?? d.FieldName;
                }
                catch
                {
                    return d.FieldName;
                }
            });

            return string.Join("-", labelParts);
        }
    }
}

