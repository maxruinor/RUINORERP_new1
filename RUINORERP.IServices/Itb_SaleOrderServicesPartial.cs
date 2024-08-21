
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：04/03/2023 16:06:01
// **************************************
using System;
using SqlSugar;
using System.Collections.Generic;
using RUINORERP.IServices.BASE;
using System.Threading.Tasks;
using RUINORERP.Model;
using RUINORERP.Model.QueryDto;


namespace RUINORERP.IServices
{
    /// <summary>
    /// 销售订单表
    /// </summary>
    public partial interface Itb_SaleOrderServices : IBaseServices<tb_SaleOrder>
    {
        Task<List<tb_SaleOrder>> QueryAsync(tb_SaleOrderQueryDto QueryCriteria, Pagination pagination);
    }
}