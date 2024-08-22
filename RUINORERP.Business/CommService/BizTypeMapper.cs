using RUINORERP.Global;
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
    public class BizTypeMapper
    {
        private Dictionary<BizType, Type> mapping;

        public BizTypeMapper()
        {
            mapping = new Dictionary<BizType, Type>();

            mapping.Add(BizType.产品档案, typeof(View_ProdDetail));
            // 手动添加枚举值与表名的对应关系
            mapping.Add(BizType.BOM物料清单, typeof(tb_BOM_S));
            mapping.Add(BizType.销售订单, typeof(tb_SaleOrder));
            mapping.Add(BizType.销售出库单, typeof(tb_SaleOut));
            mapping.Add(BizType.销售退回单, typeof(tb_SaleOutRe));
            mapping.Add(BizType.采购订单, typeof(tb_PurOrder));
            mapping.Add(BizType.采购入库单, typeof(tb_PurEntry));
            mapping.Add(BizType.采购入库退回单, typeof(tb_PurEntryRe));
            //mapping.Add(BizType.返厂入库, typeof(tb_Return));
            //mapping.Add(BizType.返厂出库, typeof(tb_StockCheck));
            mapping.Add(BizType.盘点单, typeof(tb_Stocktake));
            mapping.Add(BizType.其他入库单, typeof(tb_StockIn));
            mapping.Add(BizType.其他出库单, typeof(tb_StockOut));
            mapping.Add(BizType.费用报销单, typeof(tb_FM_ExpenseClaim));
            mapping.Add(BizType.其他费用支出, typeof(tb_FM_OtherExpense));
            mapping.Add(BizType.其他费用收入, typeof(tb_FM_OtherExpense));
            mapping.Add(BizType.请购单, typeof(tb_BuyingRequisition));
            mapping.Add(BizType.制令单, typeof(tb_ManufacturingOrder));

            mapping.Add(BizType.生产计划单, typeof(tb_ProductionPlan));
            mapping.Add(BizType.生产领料单, typeof(tb_MaterialRequisition));
            mapping.Add(BizType.生产退料单, typeof(tb_MaterialReturn));
            mapping.Add(BizType.生产需求分析, typeof(tb_ProductionDemand));
            mapping.Add(BizType.缴库单, typeof(tb_FinishedGoodsInv));
            mapping.Add(BizType.借出单, typeof(tb_ProdBorrowing));
            mapping.Add(BizType.归还单, typeof(tb_ProdReturning));
            mapping.Add(BizType.产品组合单, typeof(tb_ProdMerge));
            mapping.Add(BizType.产品分割单, typeof(tb_ProdSplit));
            mapping.Add(BizType.套装组合, typeof(tb_ProdBundle));

            //mapping.Add(BizType.退料单, typeof(tb_Return));

            // 省略其他枚举值与表名的对应关系
        }

        public Type GetTableType(BizType bizType)
        {
            if (mapping.ContainsKey(bizType))
            {
                return mapping[bizType];
            }
            throw new ArgumentException("无效的业务类型", nameof(bizType));
        }

        public BizType GetBizType(string tableName)
        {
            BizType bizType = BizType.默认数据;
            try
            {
                foreach (KeyValuePair<BizType, Type> entry in mapping)
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
            foreach (KeyValuePair<BizType, Type> entry in mapping)
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
