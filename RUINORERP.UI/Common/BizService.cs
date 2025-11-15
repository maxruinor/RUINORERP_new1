using RUINORERP.Business.CommService;

using RUINORERP.Business.Cache;
using RUINORERP.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.UI.Common
{
    public static class BizService
    {
        /// <summary>
        /// 获取对应的汇率，无结果返回null
        /// </summary>
        /// <param name="fromCurrencyID"></param>
        /// <param name="toCurrencyID"></param>
        /// <returns></returns>
        public static async Task<decimal> GetExchangeRate(long fromCurrencyID, long toCurrencyID)
        {
            tb_CurrencyExchangeRate rate = await MainForm.Instance.AppContext.Db.CopyNew().Queryable<tb_CurrencyExchangeRate>()
                        .Where(m => m.BaseCurrencyID == fromCurrencyID && m.TargetCurrencyID == toCurrencyID)
                        .FirstAsync();
            if (rate != null)
            {
                return rate.ExecuteExchRate.Value;
            }
            else
            {
                return 1;
            }
        }


        /// <summary>
        /// 将外币换为人民币
        /// </summary>
        /// <param name="fromCurrencyID">外币</param>
        /// <param name="toCurrencyID">人民币</param>
        /// <returns></returns>
        public static decimal GetExchangeRateFromCache(long fromCurrencyID, long toCurrencyID)
        {
            List<tb_CurrencyExchangeRate> rates = new List<tb_CurrencyExchangeRate>();

            // 使用静态缓存管理器方法获取实体列表
            var rslist = CacheManager.GetEntityList<tb_CurrencyExchangeRate>();
            if (rslist == null || rslist.Count == 0)
            {
                rslist = MainForm.Instance.AppContext.Db.CopyNew().Queryable<tb_CurrencyExchangeRate>().ToList();
            }
            else
            {
                rates = rslist;
            }
            tb_CurrencyExchangeRate rate = rates.Where(m => m.BaseCurrencyID == fromCurrencyID && m.TargetCurrencyID == toCurrencyID).FirstOrDefault();
            if (rate == null)
            {
                return 1;
            }
            else
            {
                return rate.ExecuteExchRate.Value;
            }
        }




    }
}
