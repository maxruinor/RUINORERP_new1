
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
    /// 库存快照表 Processor
    /// </summary>
    public partial class tb_InventorySnapshotProcessor : BaseProcessor
    {
        /// <summary>
        /// 获取库存快照的查询条件
        /// </summary>
        /// <returns>查询过滤器</returns>
        public override QueryFilter GetQueryFilter()
        {
            QueryFilter queryFilter = new QueryFilter();

            // 添加过滤条件
            var lambda = Expressionable.Create<tb_InventorySnapshot>()
                .ToExpression();
            queryFilter.FilterLimitExpressions.Add(lambda);

            // 设置查询字段
            // 产品详情 - 支持组合查询（产品编码、产品名称等）
            queryFilter.SetQueryField<tb_InventorySnapshot>(c => c.ProdDetailID, true);

            // 库位 - 支持下拉选择
            queryFilter.SetQueryField<tb_InventorySnapshot>(c => c.Location_ID, true);

            // 货架 - 支持下拉选择
            queryFilter.SetQueryField<tb_InventorySnapshot>(c => c.Rack_ID, true);

            // 实际库存 - 支持范围查询
            queryFilter.SetQueryField<tb_InventorySnapshot>(c => c.Quantity);

            // 期初数量 - 支持范围查询
            queryFilter.SetQueryField<tb_InventorySnapshot>(c => c.InitInventory);

            // 在途库存 - 支持范围查询
            queryFilter.SetQueryField<tb_InventorySnapshot>(c => c.On_the_way_Qty);

            // 拟销售量 - 支持范围查询
            queryFilter.SetQueryField<tb_InventorySnapshot>(c => c.Sale_Qty);

            // 在制数量 - 支持范围查询
            queryFilter.SetQueryField<tb_InventorySnapshot>(c => c.MakingQty);

            // 未发料量 - 支持范围查询
            queryFilter.SetQueryField<tb_InventorySnapshot>(c => c.NotOutQty);

            // 快照时间 - 支持日期范围查询
            queryFilter.SetQueryField<tb_InventorySnapshot>(c => c.SnapshotTime);

            // 最新入库时间 - 支持日期范围查询
            queryFilter.SetQueryField<tb_InventorySnapshot>(c => c.LatestStorageTime);

            // 最新出库时间 - 支持日期范围查询
            queryFilter.SetQueryField<tb_InventorySnapshot>(c => c.LatestOutboundTime);

            // 最后盘点时间 - 支持日期范围查询
            queryFilter.SetQueryField<tb_InventorySnapshot>(c => c.LastInventoryDate);

            // 备注说明 - 支持模糊查询
            queryFilter.SetQueryField<tb_InventorySnapshot>(c => c.Notes);

            return queryFilter;
        }
    }
}
