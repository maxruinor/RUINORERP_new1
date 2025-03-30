using RUINORERP.UI.ChartFramework.Adapters;
using RUINORERP.UI.ChartFramework.Core;
using RUINORERP.UI.ChartFramework.Data.Interfaces;
using RUINORERP.UI.ChartFramework.Models.ChartFramework.Core.Models;
using RUINORERP.UI.ChartFramework.Models;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.UI.ChartDataSource
{
    // Data/Adapters/CustomerDataSource.cs
    public class NewCustomerDataSource : BaseProvider
    {
        public NewCustomerDataSource(ISqlSugarClient db) : base(db) { }

        public override IEnumerable<DimensionConfig> GetDimensions() => new[]
        {
        new DimensionConfig("Employee_ID", "业务员", DimensionType.String),
        new DimensionConfig("Region_ID", "区域", DimensionType.String),
        new DimensionConfig("Created_at", "创建时间", DimensionType.DateTime)
    };

        protected override string GetMainTableName() => "tb_CRM_Customer";

        protected override string GetSelectExpression(ChartRequest request) =>
            $"COUNT(1) AS Count, {request.GetTimeGroupExpression()} AS TimeGroup";

        protected override void ApplyGroupBy(ref ISugarQueryable<dynamic> query, ChartRequest request)
        {
            query.GroupBy(request.GetGroupByTimeField(request.TimeField));
        }

        protected override ChartDataSet TransformToChartDataSet(List<dynamic> rawData, ChartRequest request)
        {
            return new ChartDataSet
            {
                MetaData = new ChartMetaData
                {
                    CategoryLabels = rawData.Select(x => (string)FormatTimeLabel(x.TimeGroup, request)).ToArray()
                },
                Series = new List<ChartSeries>
            {
                new ChartSeries
                {
                    Name = "客户数量",
                    Values = rawData.Select(x => (double)x.Count).ToList()
                }
            }
            };
        }

        private string[] FormatTimeLabel(dynamic timeGroup, ChartRequest request)
        {
            return null;
        }

        public override IEnumerable<MetricConfig> GetMetrics()
        {
            throw new NotImplementedException();
        }
    }
}
