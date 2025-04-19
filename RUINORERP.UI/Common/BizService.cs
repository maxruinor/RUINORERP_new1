using RUINORERP.Business.CommService;
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

        public static decimal GetExchangeRateFromCache(string fromCurrencyCode, string toCurrencyCode)
        {
           var rslist = BizCacheHelper.Manager.CacheEntityList.Get("tb_CurrencyExchangeRate");

            //tb_CurrencyExchangeRate rate = await _appContext.Db.CopyNew().Queryable<tb_CurrencyExchangeRate>()
            //            .Where(m => m.BaseCurrencyID == fromCurrencyID && m.TargetCurrencyID == toCurrencyID)
            //            .FirstAsync();
            //if (rate != null)
            //{
            //    return rate as T;
            //}
            //else
            //{
            //    return null;
            //}
            return 0;
        }




    }
}
