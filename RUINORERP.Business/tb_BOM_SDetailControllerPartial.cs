
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：05/14/2024 15:01:00
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
using System.Runtime.ConstrainedExecution;

namespace RUINORERP.Business
{
    /// <summary>
    /// 标准物料表BOM明细-要适当冗余
    /// </summary>
    public partial class tb_BOM_SDetailController<T> : BaseController<T> where T : class
    {



       

        /// <summary>
        /// 带参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
        public virtual async Task<List<tb_BOM_SDetail>> QueryByNavWithSubBom(Expression<Func<tb_BOM_SDetail, bool>> exp)
        {
            List<tb_BOM_SDetail> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_BOM_SDetail>().Where(exp)

                           // .Includes(t => t.tb_producttype)
                           .Includes(t => t.view_ProdInfo)
                            .Includes(t => t.tb_unit)
                            .Includes(t => t.tb_unit_conversion)
                            .Includes(t => t.tb_bom_s)
                            .Includes(t => t.tb_proddetail, s => s.tb_bom_s)
                            .Includes(t => t.tb_proddetail, s => s.tb_Inventories)
                            .ToListAsync();

            foreach (var item in list)
            {
                item.AcceptChanges();
            }

            _cacheManager.UpdateEntityList<tb_BOM_SDetail>(list);
            return list;
        }

    }
}



