
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：09/13/2024 18:44:00
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
    /// 包装规格表
    /// </summary>
    public partial class tb_PackingController<T> : BaseController<T> where T : class
    {
        public async override Task<List<T>> GetPrintDataSource(long ID)
        {
            List<tb_Packing> list = await _appContext.Db.CopyNew().Queryable<tb_Packing>().Where(m => m.Pack_ID == ID)
                            .Includes(a => a.tb_prod, d => d.tb_unit)
                            .Includes(a => a.tb_prod, d => d.tb_producttype)
                            .Includes(a => a.tb_proddetail, c => c.tb_prod, d => d.tb_unit)
                            .Includes(a => a.tb_proddetail, c => c.tb_prod, d => d.tb_producttype)
                             .AsNavQueryable()//加这个前面,超过三级在前面加这一行，并且第四级无VS智能提示，但是可以用
                             .Includes(a => a.tb_PackingDetails, b => b.tb_proddetail, c => c.tb_prod, d => d.tb_unit)
                              .AsNavQueryable()//加这个前面,超过三级在前面加这一行，并且第四级无VS智能提示，但是可以用
                                .Includes(a => a.tb_PackingDetails, b => b.tb_proddetail, c => c.tb_prod, d => d.tb_producttype)
                            .Includes(a => a.tb_BoxRuleses, b => b.tb_cartoonbox)
                            .ToListAsync();
            return list as List<T>;
        }






    }
}



