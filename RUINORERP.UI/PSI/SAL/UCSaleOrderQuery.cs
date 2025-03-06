using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using RUINORERP.UI.BaseForm;
using RUINORERP.UI.Common;
using RUINORERP.Model;
using RUINORERP.Global;
using RUINORERP.Business;
using RUINORERP.AutoMapper;
using AutoMapper;
using RUINORERP.Common.CollectionExtension;
using SqlSugar;
using RUINORERP.Common.Extensions;
using System.Collections;
using RUINORERP.Model.Base;
using RUINORERP.Business.Security;
using RUINOR.Core;
using RUINORERP.Common.Helper;
using RUINORERP.Business.Processor;
using Microsoft.Extensions.Logging;

namespace RUINORERP.UI.PSI.SAL
{

    [MenuAttrAssemblyInfo("销售订单查询", ModuleMenuDefine.模块定义.进销存管理, ModuleMenuDefine.进销存管理.销售管理, BizType.销售订单)]
    public partial class UCSaleOrderQuery : BaseBillQueryMC<tb_SaleOrder, tb_SaleOrderDetail>
    {
        public UCSaleOrderQuery()
        {
            InitializeComponent();
            base.RelatedBillEditCol = (c => c.SOrderNo);

          

            //显示转出库单
            tsbtnBatchConversion.Visible = true;
            //base._UCBillMasterQuery.ColDisplayType = typeof(tb_SaleOrder);
        }
        public override void BuildLimitQueryConditions()
        {
            //创建表达式
            var lambda = Expressionable.Create<tb_SaleOrder>()
                             //.AndIF(CurMenuInfo.CaptionCN.Contains("客户"), t => t.IsCustomer == true)
                             // .AndIF(CurMenuInfo.CaptionCN.Contains("供应商"), t => t.IsVendor == true)
                             .And(t => t.isdeleted == false)

                               // .And(t => t.Is_enabled == true)
                               .AndIF(AuthorizeController.GetSaleLimitedAuth(MainForm.Instance.AppContext), t => t.Employee_ID == MainForm.Instance.AppContext.CurUserInfo.UserInfo.Employee_ID)//限制了销售只看到自己的客户,采购不限制

                            .ToExpression();//注意 这一句 不能少
            base.LimitQueryConditions = lambda;
            QueryConditionFilter.FilterLimitExpressions.Add(lambda);
        }

        /// <summary>
        /// 如果需要查询条件查询，就要在子类中重写这个方法
        /// </summary>
        public override void BuildQueryCondition()
        {
            BaseProcessor baseProcessor = Startup.GetFromFacByName<BaseProcessor>(typeof(tb_SaleOrder).Name + "Processor");
            QueryConditionFilter = baseProcessor.GetQueryFilter();

            //非超级用户时，只能查看自己的订单,如果设置的销售业务限制范围的话
            var lambda = Expressionable.Create<tb_SaleOrder>()
                .AndIF(AuthorizeController.GetSaleLimitedAuth(MainForm.Instance.AppContext) && !MainForm.Instance.AppContext.IsSuperUser, t => t.Employee_ID == MainForm.Instance.AppContext.CurUserInfo.UserInfo.Employee_ID)//限制了销售只看到自己的客户,采购不限制
              .ToExpression();

            QueryConditionFilter.FilterLimitExpressions.Add(lambda);
        }

        public override void BuildSummaryCols()
        {
            base.MasterSummaryCols.Add(c => c.TotalQty);
            base.MasterSummaryCols.Add(c => c.TotalAmount);
            base.MasterSummaryCols.Add(c => c.TotalTaxAmount);
            base.MasterSummaryCols.Add(c => c.TotalUntaxedAmount);
            base.MasterSummaryCols.Add(c => c.ShipCost);
            base.MasterSummaryCols.Add(c => c.CollectedMoney);
            base.MasterSummaryCols.Add(c => c.PrePayMoney);
            base.MasterSummaryCols.Add(c => c.Deposit);

            base.ChildSummaryCols.Add(c => c.Quantity);
            base.ChildSummaryCols.Add(c => c.SubtotalUntaxedAmount);
            base.ChildSummaryCols.Add(c => c.CommissionAmount);
            base.ChildSummaryCols.Add(c => c.SubtotalTaxAmount);
            base.ChildSummaryCols.Add(c => c.SubtotalCostAmount);
            base.ChildSummaryCols.Add(c => c.SubtotalTransAmount);

        }

        public override void BuildInvisibleCols()
        {
            base.MasterInvisibleCols.Add(c => c.TotalCost);
            base.ChildInvisibleCols.Add(c => c.Cost);
            base.ChildInvisibleCols.Add(c => c.SubtotalCostAmount);
        }


        /// <summary>
        /// 批量转换为销售出库单
        /// </summary>
        public async override void BatchConversion()
        {
            tb_SaleOutController<tb_SaleOut> ctr = Startup.GetFromFac<tb_SaleOutController<tb_SaleOut>>();
            List<tb_SaleOrder> selectlist = GetSelectResult();
            int conter = 0;
            foreach (var item in selectlist)
            {
                //只有审核状态才可以转换为出库单
                if (item.DataStatus == (int)DataStatus.确认 && item.ApprovalStatus == (int)ApprovalStatus.已审核 && item.ApprovalResults.HasValue && item.ApprovalResults.Value)
                {
                    if (item.tb_SaleOuts != null && item.tb_SaleOuts.Count > 0)
                    {
                        if (MessageBox.Show($"当前订单{item.SOrderNo}：已经生成过出库单，\r\n确定再次生成吗？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                        {

                        }
                        else
                        {
                            continue;
                        }

                    }

                    tb_SaleOut saleOut = SaleOrderToSaleOut(item);
                    ReturnMainSubResults<tb_SaleOut> rsrs = await ctr.BaseSaveOrUpdateWithChild<tb_SaleOut>(saleOut);
                    if (rsrs.Succeeded)
                    {
                        conter++;
                    }
                    else
                    {
                        MainForm.Instance.uclog.AddLog("转换出错:" + rsrs.ErrorMsg);
                    }
                }
                else
                {
                    MainForm.Instance.uclog.AddLog(string.Format("当前订单:{0}的状态为{1},不能转换为销售出库单。", item.SOrderNo, ((DataStatus)item.DataStatus).ToString()));
                    continue;
                }

            }
            MainForm.Instance.uclog.AddLog("转换完成,成功订单数量:" + conter);
            MainForm.Instance.logger.LogInformation("转换完成,成功订单数量:" + conter);
        }

        /// <summary>
        /// 转换为销售出库单
        /// </summary>
        /// <param name="saleorder"></param>
        public tb_SaleOut SaleOrderToSaleOut(tb_SaleOrder saleorder)
        {
            tb_SaleOut entity = new tb_SaleOut();
            //转单
            if (saleorder != null)
            {
                IMapper mapper = RUINORERP.Business.AutoMapper.AutoMapperConfig.RegisterMappings().CreateMapper();
                entity = mapper.Map<tb_SaleOut>(saleorder);
                List<tb_SaleOutDetail> details = mapper.Map<List<tb_SaleOutDetail>>(saleorder.tb_SaleOrderDetails);
                //转单要TODO
                //转换时，默认认为订单出库数量就等于这次出库数量，是否多个订单累计？，如果是UI录单。则只是默认这个数量。也可以手工修改
                List<tb_SaleOutDetail> NewDetails = new List<tb_SaleOutDetail>();
                for (global::System.Int32 i = 0; i < details.Count; i++)
                {
                    tb_SaleOrderDetail item = saleorder.tb_SaleOrderDetails.FirstOrDefault(c => c.ProdDetailID == details[i].ProdDetailID);
                    details[i].Quantity = details[i].Quantity - item.TotalDeliveredQty;// 减掉已经出库的数量
                    details[i].SubtotalTransAmount = details[i].TransactionPrice * details[i].Quantity;
                    details[i].SubtotalCostAmount = details[i].Cost * details[i].Quantity;
                    if (details[i].Quantity > 0)
                    {
                        NewDetails.Add(details[i]);
                    }
                }

                entity.tb_SaleOutDetails = NewDetails;
                entity.ApprovalOpinions = "批量转单";
                entity.ApprovalResults = null;
                entity.DataStatus = (int)DataStatus.草稿;
                entity.ApprovalStatus = (int)ApprovalStatus.未审核;
                entity.ApprovalResults = null;
                entity.Approver_at = null;
                entity.Approver_by = null;
                entity.PrintStatus = 0;
                entity.ActionStatus = ActionStatus.新增;
                if (NewDetails.Count != details.Count)
                {
                    //已经出库过，第二次不包括 运费
                    entity.TotalQty = NewDetails.Sum(c => c.Quantity);
                    entity.TotalCost = NewDetails.Sum(c => c.Cost * c.Quantity);
                    entity.TotalAmount = NewDetails.Sum(c => c.TransactionPrice * c.Quantity);
                    entity.CollectedMoney = NewDetails.Sum(c => c.TransactionPrice * c.Quantity);
                    entity.TotalTaxAmount = NewDetails.Sum(c => c.SubtotalTaxAmount);
                    entity.TotalUntaxedAmount = NewDetails.Sum(c => c.SubtotalUntaxedAmount);
                }

                if (saleorder.DeliveryDate.HasValue)
                {
                    entity.OutDate = saleorder.DeliveryDate.Value;
                    entity.DeliveryDate = saleorder.DeliveryDate;
                }
                else
                {
                    entity.OutDate = System.DateTime.Now;
                    entity.DeliveryDate = System.DateTime.Now;
                }

                BusinessHelper.Instance.InitEntity(entity);

                if (entity.SOrder_ID.HasValue && entity.SOrder_ID > 0)
                {
                    entity.CustomerVendor_ID = saleorder.CustomerVendor_ID;
                    entity.SaleOrderNo = saleorder.SOrderNo;
                }
                entity.SaleOutNo = BizCodeGenerator.Instance.GetBizBillNo(BizType.销售出库单);
                BusinessHelper.Instance.InitEntity(entity);
                //保存到数据库

            }
            return entity;
        }


        //public override List<tb_SaleOrder> GetPrintDatas(List<tb_SaleOrder> EditEntitys)
        //{
        //    List<tb_SaleOrder> datas = new List<tb_SaleOrder>();
        //    foreach (var item in EditEntitys)
        //    {
        //        tb_SaleOrderController<tb_SaleOrder> ctr = Startup.GetFromFac<tb_SaleOrderController<tb_SaleOrder>>();
        //        var PrintData = ctr.GetPrintData(item.SOrder_ID);
        //        datas.Add(PrintData[0] as tb_SaleOrder);
        //    }
        //    return datas;
        //}


        //public override List<tb_SaleOrder> GetPrintDatas(tb_SaleOrder EditEntity)
        //{
        //    List<tb_SaleOrder> datas = new List<tb_SaleOrder>();
        //    tb_SaleOrderController<tb_SaleOrder> ctr = Startup.GetFromFac<tb_SaleOrderController<tb_SaleOrder>>();
        //    List<tb_SaleOrder> PrintData = ctr.GetPrintData(EditEntity.SOrder_ID);
        //    return PrintData;
        //}


        /*
 /// <summary>
 /// 销售订单审核，审核成功后，库存中的拟销售量增加，同时检查数量和金额，总数量和总金额不能小于明细小计的和
 /// </summary>
 /// <returns></returns>
 protected async override Task<ApprovalEntity> Review(tb_SaleOrder EditEntity)
 {
     if (EditEntity == null)
     {
         return null;
     }

     //同时检查数量和金额，总数量和总金额不能小于明细小计的和
     if (EditEntity.TotalQty < EditEntity.tb_SaleOrderDetails.Sum(c => c.Quantity))
     {
         MainForm.Instance.PrintInfoLog($"订单：{EditEntity.SOrderNo}:总数量不能小于明细小计之和！");
         return null;
     }

     if (EditEntity.TotalAmount < EditEntity.tb_SaleOrderDetails.Sum(c => c.TransactionPrice * c.Quantity))
     {
         MainForm.Instance.PrintInfoLog($"订单：{EditEntity.SOrderNo}:总金额不能小于明细小计之和！");
         return null;
     }
     ApprovalEntity ae =await base.Review(EditEntity);

     return ae;
 }


 /// <summary>
 /// 销售订单反审
 /// </summary>
 /// <param name="EditEntitys"></param>
 /// <returns></returns>
 public async override Task<bool> ReReview(List<tb_SaleOrder> EditEntitys)
 {
     if (EditEntitys == null)
     {
         return false;
     }
     foreach (tb_SaleOrder EditEntity in EditEntitys)
     {
         #region 反审
         //反审，要审核过，并且通过了，才能反审。
         if (EditEntity.ApprovalStatus.Value == (int)ApprovalStatus.已审核 && !EditEntity.ApprovalResults.HasValue)
         {
             MainForm.Instance.uclog.AddLog("已经审核,且【同意】的单据才能反审核。");
             continue;
         }


         if (EditEntity.tb_SaleOrderDetails == null || EditEntity.tb_SaleOrderDetails.Count == 0)
         {
             MainForm.Instance.uclog.AddLog("单据中没有明细数据，请确认录入了完整数量和金额。", UILogType.警告);
             continue;
         }

         RevertCommand command = new RevertCommand();
         //缓存当前编辑的对象。如果撤销就回原来的值
         tb_SaleOrder oldobj = CloneHelper.DeepCloneObject<tb_SaleOrder>(EditEntity);
         command.UndoOperation = delegate ()
         {
             //Undo操作会执行到的代码 意思是如果退审核，内存中审核的数据要变为空白（之前的样子）
             CloneHelper.SetValues<tb_SaleOrder>(EditEntity, oldobj);
         };

         tb_SaleOrderController<tb_SaleOrder> ctr = Startup.GetFromFac<tb_SaleOrderController<tb_SaleOrder>>();

         ReturnResults<bool> rr = await ctr.AntiApprovalAsync(EditEntity);
         if (rr.Succeeded)
         {
             //if (MainForm.Instance.WorkflowItemlist.ContainsKey(""))
             //{

             //}
             //这里审核完了的话，如果这个单存在于工作流的集合队列中，则向服务器说明审核完成。
             //这里推送到审核，启动工作流  队列应该有一个策略 比方优先级，桌面不动1 3 5分钟 
             //OriginalData od = ActionForClient.工作流审批(pkid, (int)BizType.盘点单, ae.ApprovalResults, ae.ApprovalComments);
             //MainForm.Instance.ecs.AddSendData(od);

             //审核成功
         }
         else
         {
             //审核失败 要恢复之前的值
             command.Undo();
             MainForm.Instance.PrintInfoLog($"{EditEntity.SOrderNo}反审失败{rr.ErrorMsg},请联系管理员！", Color.Red);
         }

         #endregion
     }
     return true;
 }
 */

        public async override Task<bool> CloseCase(List<tb_SaleOrder> EditEntitys)
        {
            if (EditEntitys == null)
            {
                return false;
            }
            //已经审核的并且通过的情况才能结案
            List<tb_SaleOrder> needCloseCases = EditEntitys.Where(c => c.DataStatus == (int)DataStatus.确认 && c.ApprovalStatus == (int)ApprovalStatus.已审核 && c.ApprovalResults.HasValue && c.ApprovalResults.Value).ToList();
            if (needCloseCases.Count == 0)
            {
                MainForm.Instance.PrintInfoLog($"要结案的数据为：{needCloseCases.Count}:请检查数据！");
                return false;
            }

            tb_SaleOrderController<tb_SaleOrder> ctr = Startup.GetFromFac<tb_SaleOrderController<tb_SaleOrder>>();
            ReturnResults<bool> rs = await ctr.BatchCloseCaseAsync(needCloseCases);
            if (rs.Succeeded)
            {
                MainForm.Instance.PrintInfoLog($"结案操作成功！", Color.Red);
                MainForm.Instance.logger.LogInformation($"结案操作成功！");
                //if (MainForm.Instance.WorkflowItemlist.ContainsKey(""))
                //{

                //}
                //这里审核完了的话，如果这个单存在于工作流的集合队列中，则向服务器说明审核完成。
                //这里推送到审核，启动工作流  队列应该有一个策略 比方优先级，桌面不动1 3 5分钟 
                //OriginalData od = ActionForClient.工作流审批(pkid, (int)BizType.盘点单, ae.ApprovalResults, ae.ApprovalComments);
                //MainForm.Instance.ecs.AddSendData(od);
                base.Query(QueryDto);
            }
            else
            {
                MainForm.Instance.PrintInfoLog($"结案操作失败,原因是{rs.ErrorMsg},如果无法解决，请联系管理员！", Color.Red);
            }

            return true;
        }





    }
}
