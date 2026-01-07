
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：01/07/2026
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
using SqlSugar;


namespace RUINORERP.Business.Processor
{
    /// <summary>
    /// 库存流水表 Processor
    /// </summary>
    public partial class tb_InventoryTransactionProcessor : BaseProcessor
    {
        /// <summary>
        /// 获取库存流水的查询条件
        /// </summary>
        /// <returns>查询过滤器</returns>
        public override QueryFilter GetQueryFilter()
        {
            QueryFilter queryFilter = new QueryFilter();

            // 添加过滤条件
            var lambda = Expressionable.Create<tb_InventoryTransaction>()
                .ToExpression();
            queryFilter.FilterLimitExpressions.Add(lambda);

            // 设置查询字段
            // 产品详情 - 支持组合查询（产品编码、产品名称等）
            queryFilter.SetQueryField<tb_InventoryTransaction>(c => c.ProdDetailID, true);

            // 库位 - 支持下拉选择
            queryFilter.SetQueryField<tb_InventoryTransaction>(c => c.Location_ID, true);

            // 业务类型 - 支持下拉选择（从业务类型字典加载）
            queryFilter.SetQueryField<tb_InventoryTransaction>(c => c.BizType, true);

            // 业务单据编号 - 支持模糊查询
            queryFilter.SetQueryField<tb_InventoryTransaction>(c => c.ReferenceNo);

            // 变动数量 - 支持范围查询
            //queryFilter.SetQueryField<tb_InventoryTransaction>(c => c.QuantityChange);

            //// 变后数量 - 支持范围查询
            //queryFilter.SetQueryField<tb_InventoryTransaction>(c => c.AfterQuantity);

            //// 批号 - 支持范围查询
            //queryFilter.SetQueryField<tb_InventoryTransaction>(c => c.BatchNumber);

            // 单位成本 - 支持范围查询
            queryFilter.SetQueryField<tb_InventoryTransaction>(c => c.UnitCost);

            // 操作时间 - 支持日期范围查询
            queryFilter.SetQueryField<tb_InventoryTransaction>(c => c.TransactionTime);

            // 操作人 - 支持下拉选择
            queryFilter.SetQueryField<tb_InventoryTransaction>(c => c.OperatorId, true);

            // 备注说明 - 支持模糊查询
            queryFilter.SetQueryField<tb_InventoryTransaction>(c => c.Notes);

            return queryFilter;
        }
    }
}
