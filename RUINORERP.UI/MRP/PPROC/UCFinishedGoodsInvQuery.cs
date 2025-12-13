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
using RUINORERP.Common.Extensions;
using SqlSugar;
using RUINORERP.Common.Helper;
using RUINOR.Core;
using RUINORERP.Business.Security;
using RUINORERP.Business.Processor;
using RUINORERP.Model.Base;
using RUINORERP.UI.UControls;

namespace RUINORERP.UI.PSI.PUR
{

    [MenuAttrAssemblyInfo("缴库单查询", ModuleMenuDefine.模块定义.生产管理, ModuleMenuDefine.生产管理.制程生产, BizType.缴库单)]
    public partial class UCFinishedGoodsInvQuery : BaseBillQueryMC<tb_FinishedGoodsInv, tb_FinishedGoodsInvDetail>, UI.AdvancedUIModule.IContextMenuInfoAuth
    {
        public UCFinishedGoodsInvQuery()
        {
            InitializeComponent();
            base.RelatedBillEditCol = (c => c.DeliveryBillNo);

            base.ChildRelatedEntityType = typeof(tb_PurOrderDetail);
            //  base.OnQueryRelatedChild += UCPurEntryQuery_OnQueryRelatedChild;
        }

        public override void BuildInvisibleCols()
        {
            //引用的订单号ID不需要显示。因为有一个单号冗余显示了。
            base.MasterInvisibleCols.Add(c => c.MOID);
            base.MasterInvisibleCols.Add(c => c.FG_ID);
        }

        public override List<ContextMenuController> AddContextMenu()
        {
            List<ContextMenuController> list = new List<ContextMenuController>();
            list.Add(new ContextMenuController("【转为应付款单(制作费)】", true, false, "NewSumDataGridView_转为应付款单"));
            return list;
        }

        public override void BuildContextMenuController()
        {
            List<EventHandler> ContextClickList = new List<EventHandler>();
            ContextClickList.Add(NewSumDataGridView_转为应付款单);
            List<ContextMenuController> list = new List<ContextMenuController>();
            list = AddContextMenu();

            UIHelper.ControlContextMenuInvisible(CurMenuInfo, list);

            if (_UCBillMasterQuery != null)
            {
                //base.dataGridView1.Use是否使用内置右键功能 = false;
                ContextMenuStrip newContextMenuStrip = _UCBillMasterQuery.newSumDataGridViewMaster.GetContextMenu(_UCBillMasterQuery.newSumDataGridViewMaster.ContextMenuStrip
                    , ContextClickList, list, true
                    );
                _UCBillMasterQuery.newSumDataGridViewMaster.ContextMenuStrip = newContextMenuStrip;
            }
        }
        private async void NewSumDataGridView_转为应付款单(object sender, EventArgs e)
        {
            List<tb_FinishedGoodsInv> selectlist = GetSelectResult();
            if (selectlist.Count > 1)
            {
                MessageBox.Show("每次只能选择一个【缴库单】转成【应付款单(制作费)】");
                return;
            }
            List<tb_FinishedGoodsInv> RealList = new List<tb_FinishedGoodsInv>();
            StringBuilder msg = new StringBuilder();
            int counter = 1;
            foreach (var item in selectlist)
            {
                //只有审核状态才可以转换为应收
                bool canConvert = item.DataStatus == (long)DataStatus.确认 && item.ApprovalStatus == (int)ApprovalStatus.审核通过 && item.ApprovalResults.HasValue && item.ApprovalResults.Value;
                if (canConvert)
                {
                    RealList.Add(item);
                }
                else
                {
                    msg.Append(counter.ToString() + ") ");
                    msg.Append($"当前缴库单 {item.DeliveryBillNo}状态为【 {((DataStatus)item.DataStatus).ToString()}】 无法生成【应付款单(制作费)】。").Append("\r\n");
                    counter++;
                }
            }

            if (msg.ToString().Length > 0)
            {
                MessageBox.Show(msg.ToString(), "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                if (RealList.Count == 0)
                {
                    return;
                }
            }

            if (RealList.Count == 0)
            {
                msg.Append("请至少选择一行数据转为【应付款单(制作费)】");
                MessageBox.Show(msg.ToString(), "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var ReceivablePayableController = MainForm.Instance.AppContext.GetRequiredService<tb_FM_ReceivablePayableController<tb_FM_ReceivablePayable>>();
            tb_FM_ReceivablePayable ReceivablePayable = await ReceivablePayableController.BuildReceivablePayable(RealList[0], false);
            MenuPowerHelper menuPowerHelper;
            menuPowerHelper = Startup.GetFromFac<MenuPowerHelper>();
            string Flag = string.Empty;
            //付款
            Flag = typeof(RUINORERP.UI.FM.UCPayable).FullName;

            tb_MenuInfo RelatedMenuInfo = MainForm.Instance.MenuList.Where(m => m.IsVisble
                        && m.EntityName == nameof(tb_FM_ReceivablePayable)
                        && m.BIBaseForm == "BaseBillEditGeneric`2" && m.ClassPath == Flag)
            .FirstOrDefault();
            if (RelatedMenuInfo != null)
            {
                await menuPowerHelper.ExecuteEvents(RelatedMenuInfo, ReceivablePayable);
            }

        }


        public override void BuildLimitQueryConditions()
        {
            //创建表达式
            var lambda = Expressionable.Create<tb_FinishedGoodsInv>()
                             //.AndIF(CurMenuInfo.CaptionCN.Contains("客户"), t => t.IsCustomer == true)
                             // .AndIF(CurMenuInfo.CaptionCN.Contains("供应商"), t => t.IsVendor == true)
                             .And(t => t.isdeleted == false)

                               // .And(t => t.Is_enabled == true)
                               .AndIF(AuthorizeController.GetPurBizLimitedAuth(MainForm.Instance.AppContext), t => t.Employee_ID == MainForm.Instance.AppContext.CurUserInfo.UserInfo.Employee_ID)//限制了销售只看到自己的客户,采购不限制
                            .ToExpression();//注意 这一句 不能少
            base.LimitQueryConditions = lambda;
        }

        /// <summary>
        /// 如果需要查询条件查询，就要在子类中重写这个方法
        /// </summary>
        public override void QueryConditionBuilder()
        {
            BaseProcessor baseProcessor = Startup.GetFromFacByName<BaseProcessor>(typeof(tb_FinishedGoodsInv).Name + "Processor");
            QueryConditionFilter = baseProcessor.GetQueryFilter();


        }




        public override void BuildSummaryCols()
        {
            base.MasterSummaryCols.Add(c => c.TotalQty);
            base.MasterSummaryCols.Add(c => c.TotalProductionCost);
            base.MasterSummaryCols.Add(c => c.TotalNetWorkingHours);
            base.MasterSummaryCols.Add(c => c.TotalNetMachineHours);
            base.MasterSummaryCols.Add(c => c.TotalMaterialCost);
            base.MasterSummaryCols.Add(c => c.TotalManuFee);
            base.MasterSummaryCols.Add(c => c.TotalApportionedCost);
  
            base.ChildSummaryCols.Add(c => c.Qty);
            base.ChildSummaryCols.Add(c => c.ApportionedCost);
            base.ChildSummaryCols.Add(c => c.MaterialCost);
            base.ChildSummaryCols.Add(c => c.NetMachineHours);
            base.ChildSummaryCols.Add(c => c.NetWorkingHours);
            base.ChildSummaryCols.Add(c => c.ProductionAllCost);
            base.ChildSummaryCols.Add(c => c.ManuFee);
   
        }



        /*

        /// <summary>
        /// 采购入库审核成功后。如果有对应的采购订单引入，则将其结案，并把数量回写？
        /// </summary>
        /// <returns></returns>
        public async override Task<ApprovalEntity> Review(List<tb_FinishedGoodsInv> EditEntitys)
        {
            if (EditEntitys == null)
            {
                return null;
            }
            //如果已经审核并且通过，则不能重复审核
            List<tb_FinishedGoodsInv> needApprovals = EditEntitys.Where(
                c => ((c.ApprovalStatus.HasValue
                && c.ApprovalStatus.Value == (int)ApprovalStatus.已审核
                && c.ApprovalResults.HasValue && !c.ApprovalResults.Value))
                || (c.ApprovalStatus.HasValue && c.ApprovalStatus == (int)ApprovalStatus.未审核)
                ).ToList();

            if (needApprovals.Count == 0)
            {
                MainForm.Instance.PrintInfoLog($"要审核的数据为：{needApprovals.Count}:请检查数据！");
                return null;
            }


            ApprovalEntity ae = base.BatchApproval(needApprovals);
            if (ae.ApprovalStatus == (int)ApprovalStatus.未审核)
            {
                return null;
            }

            tb_FinishedGoodsInvController<tb_FinishedGoodsInv> ctr = Startup.GetFromFac<tb_FinishedGoodsInvController<tb_FinishedGoodsInv>>();
            ReturnResults<bool> rs = await ctr.BatchApproval(needApprovals, ae);
            if (rs.Succeeded)
            {
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
                MainForm.Instance.PrintInfoLog($"{ae.bizName}:{ae.BillNo}审核失败,请联系管理员！", Color.Red);
            }

            return ae;
        }
        */
        private void UCFinishedGoodsInvQuery_Load(object sender, EventArgs e)
        {
            _UCBillMasterQuery.GridRelated.SetRelatedInfo<tb_FinishedGoodsInv, tb_ManufacturingOrder>(a => a.MONo, b => b.MONO);
 
        }
    }



}
