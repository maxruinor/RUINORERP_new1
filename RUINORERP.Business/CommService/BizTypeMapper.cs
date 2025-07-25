﻿using RUINORERP.Global;
using RUINORERP.Model;
using RUINORERP.Model.CommonModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.Business.CommService
{
    /// <summary>
    /// 业务类型与表名的映射关系（实体名）
    /// </summary>
    [Obsolete("用EnhancedBizTypeMapper代替")]
    public class BizTypeMapper
    {
        private Dictionary<BizType, Type> mapping;

        public Dictionary<BizType, Type> Mapping { get => mapping; set => mapping = value; }

        public BizTypeMapper()
        {
            Mapping = new Dictionary<BizType, Type>();

            //mapping.Add(BizType.产品档案, typeof(View_ProdDetail));

            Mapping.Add(BizType.产品档案, typeof(View_ProdDetail));
            // 手动添加枚举值与表名的对应关系


            Mapping.Add(BizType.BOM物料清单, typeof(tb_BOM_S));
            Mapping.Add(BizType.销售订单, typeof(tb_SaleOrder));
            Mapping.Add(BizType.销售出库单, typeof(tb_SaleOut));
            Mapping.Add(BizType.销售退回单, typeof(tb_SaleOutRe));
            Mapping.Add(BizType.采购订单, typeof(tb_PurOrder));
            Mapping.Add(BizType.采购入库单, typeof(tb_PurEntry));
            Mapping.Add(BizType.采购退货单, typeof(tb_PurEntryRe));
            mapping.Add(BizType.采购退货入库, typeof(tb_PurReturnEntry));
            //mapping.Add(BizType.返厂出库, typeof(tb_StockCheck));
            Mapping.Add(BizType.盘点单, typeof(tb_Stocktake));
            Mapping.Add(BizType.其他入库单, typeof(tb_StockIn));
            Mapping.Add(BizType.其他出库单, typeof(tb_StockOut));
            Mapping.Add(BizType.费用报销单, typeof(tb_FM_ExpenseClaim));
            Mapping.Add(BizType.其他费用支出, typeof(tb_FM_OtherExpense));
            Mapping.Add(BizType.其他费用收入, typeof(tb_FM_OtherExpense));
            Mapping.Add(BizType.请购单, typeof(tb_BuyingRequisition));
            Mapping.Add(BizType.制令单, typeof(tb_ManufacturingOrder));
            Mapping.Add(BizType.生产计划单, typeof(tb_ProductionPlan));
            Mapping.Add(BizType.生产领料单, typeof(tb_MaterialRequisition));
            Mapping.Add(BizType.生产退料单, typeof(tb_MaterialReturn));
            Mapping.Add(BizType.需求分析, typeof(tb_ProductionDemand));
            Mapping.Add(BizType.缴库单, typeof(tb_FinishedGoodsInv));
            Mapping.Add(BizType.借出单, typeof(tb_ProdBorrowing));
            Mapping.Add(BizType.归还单, typeof(tb_ProdReturning));
            Mapping.Add(BizType.产品组合单, typeof(tb_ProdMerge));
            Mapping.Add(BizType.产品分割单, typeof(tb_ProdSplit));
            Mapping.Add(BizType.套装组合, typeof(tb_ProdBundle));
            Mapping.Add(BizType.包装信息, typeof(tb_Packing));
            Mapping.Add(BizType.产品转换单, typeof(tb_ProdConversion));
            Mapping.Add(BizType.调拨单, typeof(tb_StockTransfer));
            //Mapping.Add(BizType.返厂出库, typeof(tb_Return));
            Mapping.Add(BizType.CRM跟进计划, typeof(tb_CRM_FollowUpPlans));
            Mapping.Add(BizType.CRM跟进记录, typeof(tb_CRM_FollowUpRecords));
            Mapping.Add(BizType.返工退库单, typeof(tb_MRP_ReworkReturn));
            Mapping.Add(BizType.返工入库单, typeof(tb_MRP_ReworkEntry));

            Mapping.Add(BizType.付款申请单, typeof(tb_FM_PaymentApplication));

            Mapping.Add(BizType.预付款单, typeof(tb_FM_PreReceivedPayment));
            Mapping.Add(BizType.预收款单, typeof(tb_FM_PreReceivedPayment));

            Mapping.Add(BizType.付款单, typeof(tb_FM_PaymentRecord));
            Mapping.Add(BizType.收款单, typeof(tb_FM_PaymentRecord));

            Mapping.Add(BizType.应付款单, typeof(tb_FM_ReceivablePayable));
            Mapping.Add(BizType.应收款单, typeof(tb_FM_ReceivablePayable));
            Mapping.Add(BizType.销售价格调整单, typeof(tb_FM_PriceAdjustment));
            Mapping.Add(BizType.采购价格调整单, typeof(tb_FM_PriceAdjustment));

            Mapping.Add(BizType.收款核销, typeof(tb_FM_PaymentSettlement));
            Mapping.Add(BizType.付款核销, typeof(tb_FM_PaymentSettlement));


            Mapping.Add(BizType.售后申请单, typeof(tb_AS_AfterSaleApply));
            Mapping.Add(BizType.售后交付单, typeof(tb_AS_AfterSaleDelivery));
            Mapping.Add(BizType.维修工单, typeof(tb_AS_RepairOrder));
            Mapping.Add(BizType.维修入库单, typeof(tb_AS_RepairInStock));

            //mapping.Add(BizType.退料单, typeof(tb_Return));
            // 省略其他枚举值与表名的对应关系
        }

        public Type GetTableType(BizType bizType)
        {
            if (Mapping.ContainsKey(bizType))
            {
                return Mapping[bizType];
            }
            else
            {
                if (true)
                {

                }
                return null;
            }

            //throw new ArgumentException("无效的业务类型", nameof(bizType));
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="entity">有些共用表的单据。需要根据具体的实体中的某个字段来区别类型</param>
        /// <returns></returns>
        public BizType GetBizType(string tableName)
        {
            BizType bizType = BizType.默认数据;
            try
            {
                foreach (KeyValuePair<BizType, Type> entry in Mapping)
                {
                    if (((Type)entry.Value).Name == tableName)
                    {
                        bizType = entry.Key;
                    }
                }
            }
            catch (Exception ex)
            {
                //throw new ArgumentException("无效的表名", nameof(tableName));
                // throw;
                return BizType.默认数据;
            }
            return bizType;
        }




        public BizType GetBizType(Type table)
        {
            foreach (KeyValuePair<BizType, Type> entry in Mapping)
            {
                if (((Type)entry.Value).Name == table.Name)
                {
                    return entry.Key;
                }
            }

            throw new ArgumentException("无效的表名", nameof(table));
        }


        //public string GetBillNoCol()
        //{
        //    BillConverterFactory bcf = Startup.GetFromFac<BillConverterFactory>();
        //    CommBillData cbd = new CommBillData();

        //        cbd = bcf.GetBillData(typeof(M), null);

        //}


    }
}
