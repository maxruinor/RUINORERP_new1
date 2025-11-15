
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：03/14/2025 20:39:48
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

using RUINORERP.Model.Base;
using RUINORERP.Common.Extensions;
using RUINORERP.IServices.BASE;
using RUINORERP.Model.Context;
using System.Linq;
using RUINOR.Core;
using RUINORERP.Common.Helper;
using RUINORERP.Global;

namespace RUINORERP.Business
{
    /// <summary>
    /// 产品详细表
    /// </summary>
    public partial class tb_ProdDetailController<T> : BaseController<T> where T : class
    {


        /// <summary>
        /// 要清空产品的配置默认的对应关系
        /// </summary>
        /// <param name="SKU"></param>
        /// <returns></returns>
        public async Task<bool> ClearBOM_Prod_Mapping(string SKU)
        {
            var affectedRows = await _unitOfWorkManage.GetDbClient().
            Updateable<tb_ProdDetail>()
            .SetColumns(it => it.BOM_ID == null)
            .Where(it => it.SKU == SKU).ExecuteCommandHasChangeAsync();
            return affectedRows;
        }

        /// <summary>
        /// 设置产品的默认配置
        /// </summary>
        /// <param name="SKU"></param>
        /// <param name="BOM_ID"></param>
        /// <returns></returns>
        public async Task<bool> SetBOM_Prod_Mapping(string SKU, long BOM_ID)
        {
            var affectedRows = await _unitOfWorkManage.GetDbClient().
            Updateable<tb_ProdDetail>()
            .SetColumns(it => it.BOM_ID == BOM_ID)
            .Where(it => it.SKU == SKU).ExecuteCommandHasChangeAsync();
            return affectedRows;
        }


    }
}



