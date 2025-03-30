using LiveChartsCore.SkiaSharpView.WinForms;
using RUINORERP.UI.ChartFramework.Core.Contracts;
using RUINORERP.UI.ChartFramework.Core.Models;
using RUINORERP.UI.ChartFramework.Models;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.UI.ChartFramework.Adapters
{
    public abstract class SqlDataProviderBase : IDataProvider
    {

        public abstract IEnumerable<MetricConfig> GetMetrics();
        // 必须实现的抽象成员
        public abstract IEnumerable<DimensionConfig> GetDimensions();

        protected readonly ISqlSugarClient _db;

        protected SqlDataProviderBase(ISqlSugarClient db)
        {
            _db = db;
        }


        protected abstract string PrimaryTableName { get; }
        public abstract Task<ChartData> GetDataAsync(DataRequest request);



        //protected abstract string GetMainTableName();
        protected virtual string GetSelectExpression(DataRequest request)
        {
            return string.Empty;
        }
        protected virtual void ApplyGroupBy(ref ISugarQueryable<dynamic> query, DataRequest request)
        {

        }
        protected abstract ChartData TransformToChartData(List<dynamic> rawData, DataRequest request);

        protected virtual ISugarQueryable<dynamic> BuildBaseQuery(DataRequest request)
        {
            return _db.Queryable<dynamic>(PrimaryTableName);
        }
        protected virtual void ApplyTimeFilter(ref ISugarQueryable<dynamic> query, DataRequest request)
        {
            if (request.StartTime.HasValue)
                query.Where($"{request.TimeField} >= @StartTime", new { request.StartTime });

            if (request.EndTime.HasValue)
                query.Where($"{request.TimeField} <= @EndTime", new { request.EndTime });
        }

    }
}
