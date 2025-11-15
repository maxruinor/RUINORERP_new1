
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：12/18/2024 18:07:05
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

namespace RUINORERP.Business
{
    /// <summary>
    /// 销售出库统计分析
    /// </summary>
    public partial class View_SaleOutItemsController<T>:BaseController<T> where T : class
    {
        //public async override Task<List<T>> GetPrintDataSource(long MainID)
        //{
        //    //var queryable = _appContext.Db.Queryable<tb_SaleOrderDetail>();
        //    //var list = _appContext.Db.Queryable(queryable).LeftJoin<View_ProdDetail>((o, d) => o.ProdDetailID == d.ProdDetailID).Select(o => o).ToList();
        //    List<View_SaleOutItems> list = await _appContext.Db.CopyNew().Queryable<View_SaleOutItems>().Where(m => m.id == MainID)
        //                     .Includes(a => a.tb_customervendor)
        //                    .Includes(a => a.tb_employee)
        //                      .AsNavQueryable()//加这个前面,超过三级在前面加这一行，并且第四级无VS智能提示，但是可以用
        //                      .Includes(a => a.tb_PurEntryReDetails, b => b.tb_proddetail, c => c.tb_prod, d => d.tb_unit)
        //                       .AsNavQueryable()//加这个前面,超过三级在前面加这一行，并且第四级无VS智能提示，但是可以用
        //                         .ToListAsync();
        //    return list as List<T>;
        //}


    }
}



