using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.Server.SmartReminder.InvReminder.Analyzer
{
    public class SalesAnalyzer
    {
        //public async Task<SalesTrend> AnalyzeTrendAsync(int productId, TimeRange range)
        //{
        //    var query = _db.Queryable<SalesRecord>()
        //        .Where(s => s.ProductId == productId)
        //        .Where(s => s.SaleTime >= range.Start && s.SaleTime <= range.End)
        //        .GroupBy(s => s.SaleTime.Date)
        //        .Select(s => new {
        //            Date = s.SaleTime.Date,
        //            Total = s.Sum(x => x.Quantity)
        //        });

        //    var data = await query.ToListAsync();
        //    return CalculateTrend(data);
        //}
    }
}
