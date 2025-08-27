
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：10/09/2023 12:16:09
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
using SqlSugar;

namespace RUINORERP.Business
{
    /// <summary>
    /// 产品详情视图
    /// </summary>
    public partial class View_ProdDetailController<T>
    {

 



        /// <summary>
        /// 高级查询
        /// </summary>
        /// <returns></returns>
        public async Task<List<View_ProdDetail>> QueryByWhereAsync(Expression<Func<View_ProdDetail, bool>> exp)
        {
            //var querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<View_ProdDetail>();
            var querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<View_ProdDetail>().Where(exp);
            return await querySqlQueryable.ToListAsync();
        }


        /// <summary>
        /// where的条件查询
        /// </summary>
        /// <param name="exp"></param>
        /// <returns></returns>
        public List<View_ProdDetail> BaseQueryByWhereTop(Expression<Func<View_ProdDetail, bool>> exp, int top)
        {
            var querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<View_ProdDetail>()
                .Includes(c => c.tb_prod, d => d.tb_Packings, e => e.tb_BoxRuleses)
                .Includes(c => c.tb_prod, d => d.tb_Packings, e => e.tb_PackingDetails)
                .Includes(c => c.tb_bom_s, e => e.tb_BOM_SDetails)
                .Includes(c => c.tb_Packing_forSku, e => e.tb_BoxRuleses)
                .Includes(c => c.tb_Packing_forSku, e => e.tb_PackingDetails)
                .IncludesAllFirstLayer()//自动导航
                .OrderBy(c=>c.Type_ID)
                .Take(top).Where(exp);
            return querySqlQueryable.ToList();
        }


    }
}