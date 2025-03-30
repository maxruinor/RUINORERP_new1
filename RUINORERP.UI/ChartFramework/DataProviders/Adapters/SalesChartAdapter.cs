using RUINORERP.UI.ChartAnalyzer;
using RUINORERP.UI.ChartFramework.Core;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.UI.ChartFramework.DataProviders.Adapters
{
    // Adapters/Sales/SalesChartAdapter.cs
 
        public class SalesChartAdapter : ChartDataSourceBase
        {
             public SalesChartAdapter(ISqlSugarClient db) : base(db) { }
        

        public override async Task<ChartDataSet> GetDataAsync(ChartRequest request)
            {
                var (sql, parameters) = new SalesQueryBuilder(request).Build();
                var rawData = await _db.Ado.SqlQueryAsync<dynamic>(sql, parameters);

                return new SalesDataTransformer(request).Transform(rawData);
            }

            public override IEnumerable<DimensionConfig> GetAvailableDimensions()
            {
                return base.GetAvailableDimensions().Concat(new[]
                {
                new DimensionConfig("Customer.Level", "客户等级", DimensionType.String),
                new DimensionConfig("Product.Category", "产品类别", DimensionType.String)
            });
            }
        }
    }
 
