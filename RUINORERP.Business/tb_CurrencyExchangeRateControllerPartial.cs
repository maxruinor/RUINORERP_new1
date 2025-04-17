
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：04/16/2025 12:02:50
// **************************************
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using RUINORERP.IServices;
using RUINORERP.Repository.UnitOfWorks;
using RUINORERP.Model;
using FluentValidation.Results;
using RUINORERP.Services;
using RUINORERP.Extensions.Middlewares;
using RUINORERP.Model.Base;
using RUINORERP.Common.Extensions;
using RUINORERP.IServices.BASE;
using RUINORERP.Model.Context;
using System.Linq;
using RUINOR.Core;
using RUINORERP.Common.Helper;

namespace RUINORERP.Business
{
    /// <summary>
    /// 币别换算表
    /// </summary>
    public partial class tb_CurrencyExchangeRateController<T> : BaseController<T> where T : class
    {
        /// <summary>
        /// 获取对应的汇率，无结果返回null
        /// </summary>
        /// <param name="fromCurrencyID"></param>
        /// <param name="toCurrencyID"></param>
        /// <returns></returns>
        public async Task<T> GetExchangeRate(long fromCurrencyID, long toCurrencyID)
        {
            tb_CurrencyExchangeRate rate = await _appContext.Db.CopyNew().Queryable<tb_CurrencyExchangeRate>()
                        .Where(m => m.BaseCurrencyID == fromCurrencyID && m.TargetCurrencyID == toCurrencyID)
                        .FirstAsync();
            if (rate!=null)
            {
                return rate as T;
            }
            else
            {
                return null;
            }
        }




    }
}



