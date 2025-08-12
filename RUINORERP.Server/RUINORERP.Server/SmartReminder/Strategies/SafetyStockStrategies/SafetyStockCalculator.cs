using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.Server.SmartReminder.Strategies.SafetyStockStrategies
{
    public class SafetyStockCalculator
    {
        // 成品安全库存 = Z值 * √(平均提前期) * 需求标准差
        public decimal CalculateForFinishedGoods(List<decimal> historicalSales, int leadTimeDays, double serviceLevel)
        {
            var demandStdDev = CalculateStandardDeviation(historicalSales);
            var zScore = GetZScore(serviceLevel); // 服务水平系数
            return (decimal)(zScore * Math.Sqrt(leadTimeDays) * (double)demandStdDev);
        }

        // 原材料安全库存 = 日均消耗量 × 最大补货周期
        public decimal CalculateForRawMaterial(List<decimal> materialUsages, int maxReplenishmentDays)
        {
            var avgDailyUsage = materialUsages.Average();
            return avgDailyUsage * maxReplenishmentDays;
        }

        private double GetZScore(double serviceLevel)
        {
            // 服务水平与Z值映射表 (95%->1.65, 99%->2.33)
            return serviceLevel switch
            {
                >= 0.99 => 2.33,
                >= 0.95 => 1.65,
                _ => 1.28 // 90%
            };
        }

        private decimal CalculateStandardDeviation(List<decimal> values)
        {
            var avg = (double)values.Average();
            var sum = values.Sum(v => Math.Pow((double)v - avg, 2));
            return (decimal)Math.Sqrt(sum / (values.Count - 1));
        }
    }
}
