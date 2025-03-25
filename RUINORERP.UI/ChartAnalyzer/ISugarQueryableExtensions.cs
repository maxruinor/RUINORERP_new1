using Org.BouncyCastle.Crypto.Agreement.JPake;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.UI.ChartAnalyzer.Extensions
{
    public static class ISugarQueryableExtensions
    {
         public static void GroupByDynamic<T>(this ISugarQueryable<IGrouping<DynamicGroupKey, T>> source, IEnumerable<string> groupProperties)
        {

        }
            
            public static void AddRange<T>(this ISugarQueryable<T> source, IEnumerable<T> items)
        {
            foreach (var item in items)
            {
                 
            }
        }
    }
}
