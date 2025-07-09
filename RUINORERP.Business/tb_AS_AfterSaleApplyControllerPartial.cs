
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：07/08/2025 16:15:00
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
using RUINORERP.Business.CommService;
using RUINORERP.Global;

namespace RUINORERP.Business
{
    /// <summary>
    /// 售后申请单 -登记，评估，清单，确认。目标是维修翻新
    /// </summary>
    public partial class tb_AS_AfterSaleApplyController<T> : BaseController<T> where T : class
    {

        /// <summary>
        /// 转换为售后交付单
        /// </summary>
        /// <param name="AfterSaleApply"></param>
        public tb_AS_AfterSaleDelivery ToAfterSaleDelivery(tb_AS_AfterSaleApply AfterSaleApply)
        {
            tb_AS_AfterSaleDelivery entity = new tb_AS_AfterSaleDelivery();
            //转单
            if (AfterSaleApply != null)
            {
                entity = mapper.Map<tb_AS_AfterSaleDelivery>(AfterSaleApply);
                entity.ApprovalOpinions = "快捷转单";
                entity.ApprovalResults = null;
                entity.DataStatus = (int)DataStatus.草稿;
                entity.ApprovalStatus = (int)ApprovalStatus.未审核;
                entity.Approver_at = null;
                entity.Approver_by = null;
                entity.PrintStatus = 0;
                entity.ActionStatus = ActionStatus.新增;
                entity.ApprovalOpinions = "";
                entity.Modified_at = null;
                entity.Modified_by = null;
                List<string> tipsMsg = new List<string>();
                List<tb_AS_AfterSaleDeliveryDetail> details = mapper.Map<List<tb_AS_AfterSaleDeliveryDetail>>(AfterSaleApply.tb_AS_AfterSaleApplyDetails);
                List<tb_AS_AfterSaleDeliveryDetail> NewDetails = new List<tb_AS_AfterSaleDeliveryDetail>();

                for (global::System.Int32 i = 0; i < details.Count; i++)
                {
                    var aa = details.Select(c => c.ProdDetailID).ToList().GroupBy(x => x).Where(x => x.Count() > 1).Select(x => x.Key).ToList();
                    if (aa.Count > 0 && details[i].ASApplyDetailID > 0)
                    {
                        #region 产品ID可能大于1行，共用料号情况
                        tb_AS_AfterSaleApplyDetail item = AfterSaleApply.tb_AS_AfterSaleApplyDetails.FirstOrDefault(c => c.ProdDetailID == details[i].ProdDetailID
                        && c.Location_ID == details[i].Location_ID
                        && c.ASApplyDetailID == details[i].ASApplyDetailID);
                        details[i].Quantity = item.ConfirmedQuantity - item.DeliveredQty;// 已经交付数量去掉
                        if (details[i].Quantity > 0)
                        {
                            NewDetails.Add(details[i]);
                        }
                        //else
                        //{
                        //    tipsMsg.Add($"售后申请单{AfterSaleApply.ASApplyNo}，{item.tb_proddetail.tb_prod.CNName + item.tb_proddetail.tb_prod.Specifications}已交付数为{item.DeliveredQty}，可退库数为{details[i].Quantity}，当前行数据忽略！");
                        //}

                        #endregion
                    }
                    else
                    {
                        #region 每行产品ID唯一
                        tb_AS_AfterSaleApplyDetail item = AfterSaleApply.tb_AS_AfterSaleApplyDetails.FirstOrDefault(c => c.ProdDetailID == details[i].ProdDetailID
                          && c.Location_ID == details[i].Location_ID
                        && c.ASApplyDetailID == details[i].ASApplyDetailID);
                        details[i].Quantity = item.ConfirmedQuantity - item.DeliveredQty;// 已经交付数量去掉
                        if (details[i].Quantity > 0)
                        {
                            NewDetails.Add(details[i]);
                        }
                        //else
                        //{
                        //    tipsMsg.Add($"当前订单的SKU:{item.tb_proddetail.SKU}已出库数量为{details[i].Quantity}，当前行数据将不会加载到明细！");
                        //}
                        #endregion
                    }

                }

                if (NewDetails.Count == 0)
                {
                    tipsMsg.Add($"售后申请单:{entity.ASApplyNo}已全部交付，请检查是否正在重复交付！");
                }

                entity.tb_AS_AfterSaleDeliveryDetails = NewDetails;

                //如果这个订单已经有出库单 则第二次运费为0
                if (AfterSaleApply.tb_AS_AfterSaleDeliveries != null && AfterSaleApply.tb_AS_AfterSaleDeliveries.Count > 0)
                {
                    tipsMsg.Add($"当前【售后申请单】已经有交付记录！");
                }
                entity.DeliveryDate = System.DateTime.Now;
                BusinessHelper.Instance.InitEntity(entity);
                entity.ASDeliveryNo = BizCodeGenerator.Instance.GetBizBillNo(BizType.售后交付单);
                entity.tb_as_aftersaleapply = AfterSaleApply;
                entity.TotalDeliveryQty = NewDetails.Sum(c => c.Quantity);
              
                BusinessHelper.Instance.InitEntity(entity);
                //保存到数据库

            }
            return entity;
        }





    }
}



