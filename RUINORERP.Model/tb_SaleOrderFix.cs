
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：04/24/2025 10:38:01
// **************************************
using System;
using SqlSugar;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Collections.Concurrent;
using RUINORERP.Global.CustomAttribute;
using Newtonsoft.Json;
using System.Linq;

namespace RUINORERP.Model
{
    /// <summary>
    /// 销售订单
    /// </summary>
    public partial class tb_SaleOrder : BaseEntity, ICloneable
    {
        /// <summary>
        /// 重写ToDataContent方法，只记录销售订单的关键数据
        /// </summary>
        /// <returns></returns>
        public override string ToDataContent()
        {
            var keyData = new Dictionary<string, object>
            {
                { nameof(SOrder_ID), SOrder_ID },
                { nameof(SOrderNo), SOrderNo },
                { nameof(CustomerVendor_ID), CustomerVendor_ID },
                { nameof(TotalQty), TotalQty },
                { nameof(TotalAmount), TotalAmount },
                { nameof(TotalTaxAmount), TotalTaxAmount },
                { nameof(SaleDate), SaleDate },
                { nameof(PreDeliveryDate), PreDeliveryDate },
                { nameof(DataStatus), DataStatus },
                { nameof(ApprovalStatus), ApprovalStatus },
                { nameof(ApprovalResults), ApprovalResults }
            };

            // 如果需要记录明细信息，可以选择性地包含
            if (tb_SaleOrderDetails != null && tb_SaleOrderDetails.Any())
            {
                var detailsData = tb_SaleOrderDetails.Select(d => new
                {
                    d.SaleOrderDetail_ID,
                    d.ProdDetailID,
                    d.Quantity,
                    d.UnitPrice,
                    d.TransactionPrice,
                    d.SubtotalTransAmount,
                    d.TaxRate,
                    d.SubtotalTaxAmount
                }).ToList();

                keyData["Details"] = detailsData;
            }

            return JsonConvert.SerializeObject(keyData);
        }
    }
    public partial class tb_SaleOrderDetail : BaseEntity, ICloneable
    {
        /// <summary>
        /// 重写ToDataContent方法，只记录销售订单明细的关键数据
        /// </summary>
        /// <returns></returns>
        public override string ToDataContent()
        {
            var keyData = new Dictionary<string, object>
            {
                { nameof(SaleOrderDetail_ID), SaleOrderDetail_ID },
                { nameof(SOrder_ID), SOrder_ID },
                { nameof(ProdDetailID), ProdDetailID },
                { nameof(Location_ID), Location_ID },
                { nameof(Quantity), Quantity },
                { nameof(UnitPrice), UnitPrice },
                { nameof(Discount), Discount },
                { nameof(TransactionPrice), TransactionPrice },
                { nameof(SubtotalTransAmount), SubtotalTransAmount },
                { nameof(TaxRate), TaxRate },
                { nameof(SubtotalTaxAmount), SubtotalTaxAmount },
                { nameof(Gift), Gift }
            };

            return JsonConvert.SerializeObject(keyData);
        }
    }

}

