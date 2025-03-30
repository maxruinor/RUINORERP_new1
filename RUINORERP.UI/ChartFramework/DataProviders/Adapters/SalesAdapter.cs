using RUINORERP.Model;
using RUINORERP.UI.ChartFramework.Adapters;
using RUINORERP.UI.ChartFramework.Models;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.UI.ChartFramework.DataProviders.Adapters
{
    // Data/Adapters/SalesAdapter.cs
    public class SalesAdapter : SqlDataProviderBase
    {
        public SalesAdapter(ISqlSugarClient db) : base(db) { }

        protected override string PrimaryTableName => "tb_Sales_Orders";

        protected override ISugarQueryable<dynamic> BuildBaseQuery(DataRequest request)
        {
            return _db.Queryable<tb_SaleOrder>()
                     .LeftJoin<tb_SaleOrderDetail>((o, d) => o.SOrder_ID == d.SOrder_ID)
                     .Select((o, d) => (dynamic)new {
                         o.SaleDate,
                         d.ProdDetailID,
                         d.Quantity
                        
                     });
        }
    }
}
