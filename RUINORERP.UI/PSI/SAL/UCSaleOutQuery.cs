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
using RUINORERP.Business.Security;
using RUINORERP.Model.Base;
using RUINORERP.Business.Processor;
using RUINORERP.UI.UControls;

namespace RUINORERP.UI.PSI.SAL
{

    [MenuAttrAssemblyInfo("销售出库单查询", ModuleMenuDefine.模块定义.进销存管理, ModuleMenuDefine.进销存管理.销售管理, BizType.销售出库单)]
    public partial class UCSaleOutQuery : BaseBillQueryMC<tb_SaleOut, tb_SaleOutDetail>, UI.AdvancedUIModule.IContextMenuInfoAuth
    {
        public UCSaleOutQuery()
        {
            InitializeComponent();
            base.RelatedBillEditCol = (c => c.SaleOutNo);


        }
        public override List<ContextMenuController> AddContextMenu()
        {
            List<EventHandler> ContextClickList = new List<EventHandler>();
            ContextClickList.Add(NewSumDataGridView_转为退货单);
            ContextClickList.Add(NewSumDataGridView_转为应收款单);
            List<ContextMenuController> list = new List<ContextMenuController>();
            list.Add(new ContextMenuController("【转为退货单】", true, false, "NewSumDataGridView_转为退货单"));
            list.Add(new ContextMenuController("【转为应收款单】", true, false, "NewSumDataGridView_转为应收款单"));
            return list;
        }

        public override void BuildContextMenuController()
        {
            List<EventHandler> ContextClickList = new List<EventHandler>();
            ContextClickList.Add(NewSumDataGridView_转为退货单);
            ContextClickList.Add(NewSumDataGridView_转为应收款单);
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

        private void NewSumDataGridView_转为退货单(object sender, EventArgs e)
        {
            List<tb_SaleOut> selectlist = GetSelectResult();
            foreach (var item in selectlist)
            {
                //只有审核状态才可以转换为出库单
                if (item.DataStatus == (int)DataStatus.确认 && item.ApprovalStatus == (int)ApprovalStatus.已审核 && item.ApprovalResults.HasValue && item.ApprovalResults.Value)
                {
                    if (item.tb_SaleOutRes != null && item.tb_SaleOutRes.Count > 0)
                    {
                        if (MessageBox.Show($"当前【销售出库单】{item.SaleOutNo}：已经生成过【销售退回单】，\r\n确定再次生成吗？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                        {

                        }
                        else
                        {
                            continue;
                        }
                    }
                    tb_SaleOutController<tb_SaleOut> ctr = Startup.GetFromFac<tb_SaleOutController<tb_SaleOut>>();
                    tb_SaleOutRe saleOutre = ctr.SaleOutToSaleOutRe(item);

                    MenuPowerHelper menuPowerHelper;
                    menuPowerHelper = Startup.GetFromFac<MenuPowerHelper>();
                    tb_MenuInfo RelatedMenuInfo = MainForm.Instance.MenuList.Where(m => m.IsVisble && m.EntityName == nameof(tb_SaleOutRe) && m.BIBaseForm == "BaseBillEditGeneric`2").FirstOrDefault();
                    if (RelatedMenuInfo != null)
                    {
                        menuPowerHelper.ExecuteEvents(RelatedMenuInfo, saleOutre);
                    }
                    return;
                }
                else
                {
                    // 弹出提示窗口：没有审核的销售订单，无源转为出库单
                    MessageBox.Show($"当前【销售出库单】{item.SaleOutNo}：未审核，无法生成【销售退回单】", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private async void NewSumDataGridView_转为应收款单(object sender, EventArgs e)
        {
            List<tb_SaleOut> selectlist = GetSelectResult();
            if (selectlist.Count > 1)
            {
                MessageBox.Show("每次只能选择一个【销售出库单】转成【应收款单】");
                return;
            }
            List<tb_SaleOut> RealList = new List<tb_SaleOut>();
            StringBuilder msg = new StringBuilder();
            int counter = 1;
            foreach (var item in selectlist)
            {
                //只有审核状态才可以转换为应收
                bool canConvert = item.DataStatus == (long)DataStatus.确认 && item.ApprovalStatus == (int)ApprovalStatus.已审核 && item.ApprovalResults.HasValue && item.ApprovalResults.Value;
                if (canConvert)
                {
                    RealList.Add(item);
                }
                else
                {
                    msg.Append(counter.ToString() + ") ");
                    msg.Append($"当前销售出库单 {item.SaleOutNo}状态为【 {((DataStatus)item.DataStatus).ToString()}】 无法生【应收款单】。").Append("\r\n");
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
                msg.Append("请至少选择一行数据转为【应收款单】");
                MessageBox.Show(msg.ToString(), "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (RealList[0].TotalAmount == 0 && RealList[0].ForeignTotalAmount == 0)
            {
                msg.Append("【应收款单】的总金额必需大于零。");
                MessageBox.Show(msg.ToString(), "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            var ReceivablePayableController = MainForm.Instance.AppContext.GetRequiredService<tb_FM_ReceivablePayableController<tb_FM_ReceivablePayable>>();
            tb_FM_ReceivablePayable ReceivablePayable = await ReceivablePayableController.BuildReceivablePayable(RealList[0]);
            MenuPowerHelper menuPowerHelper;
            menuPowerHelper = Startup.GetFromFac<MenuPowerHelper>();
            string Flag = string.Empty;
            //红字收款
            Flag = typeof(RUINORERP.UI.FM.UCReceivable).FullName;

            tb_MenuInfo RelatedMenuInfo = MainForm.Instance.MenuList.Where(m => m.IsVisble
                        && m.EntityName == nameof(tb_FM_ReceivablePayable)
                        && m.BIBaseForm == "BaseBillEditGeneric`2" && m.ClassPath == Flag)
            .FirstOrDefault();
            if (RelatedMenuInfo != null)
            {
                menuPowerHelper.ExecuteEvents(RelatedMenuInfo, ReceivablePayable);
            }

        }

        public override void SetGridViewDisplayConfig()
        {
            //base.SetRelatedBillCols<tb_SaleOrder>(c => c.SOrderNo, r => r.SaleOrderNo);
            _UCBillMasterQuery.GridRelated.SetRelatedInfo<tb_SaleOut, tb_SaleOrder>(c => c.SaleOrderNo, r => r.SOrderNo);
            base.SetGridViewDisplayConfig();
        }


        public override void BuildInvisibleCols()
        {
            //在销售出库单中，引用的订单号ID不需要显示。因为有一个单号冗余显示了。
            base.MasterInvisibleCols.Add(c => c.SOrder_ID);
        }


        /// <summary>
        /// 如果需要查询条件查询，就要在子类中重写这个方法
        /// </summary>
        public override void QueryConditionBuilder()
        {
            BaseProcessor baseProcessor = Startup.GetFromFacByName<BaseProcessor>(typeof(tb_SaleOut).Name + "Processor");
            QueryConditionFilter = baseProcessor.GetQueryFilter();

        }

        public override void BuildSummaryCols()
        {
            base.MasterSummaryCols.Add(c => c.TotalQty);
            base.MasterSummaryCols.Add(c => c.TotalAmount);
            base.MasterSummaryCols.Add(c => c.TotalTaxAmount);
            base.MasterSummaryCols.Add(c => c.ForeignTotalAmount);
            base.MasterSummaryCols.Add(c => c.TotalCost);
            base.MasterSummaryCols.Add(c => c.FreightIncome);
            base.MasterSummaryCols.Add(c => c.FreightCost);
            base.MasterSummaryCols.Add(c => c.ForeignTotalAmount);
            base.MasterSummaryCols.Add(c => c.TotalCommissionAmount);

            base.ChildSummaryCols.Add(c => c.Quantity);
            base.ChildSummaryCols.Add(c => c.CommissionAmount);
            base.ChildSummaryCols.Add(c => c.SubtotalTaxAmount);
            base.ChildSummaryCols.Add(c => c.SubtotalCostAmount);
            base.ChildSummaryCols.Add(c => c.SubtotalTransAmount);
            base.ChildSummaryCols.Add(c => c.TotalReturnedQty);
            base.BuildSummaryCols();
        }

        public override void BuildLimitQueryConditions()
        {
            //创建表达式
            var lambda = Expressionable.Create<tb_SaleOut>()
                             //.AndIF(CurMenuInfo.CaptionCN.Contains("客户"), t => t.IsCustomer == true)
                             // .AndIF(CurMenuInfo.CaptionCN.Contains("供应商"), t => t.IsVendor == true)
                             .And(t => t.isdeleted == false)
                               .AndIF(AuthorizeController.GetSaleLimitedAuth(MainForm.Instance.AppContext), t => t.Employee_ID == MainForm.Instance.AppContext.CurUserInfo.UserInfo.Employee_ID)//限制了销售只看到自己的客户,采购不限制
                                                                                                                                                                                                // .And(t => t.Is_enabled == true)

                            .ToExpression();//注意 这一句 不能少
            base.LimitQueryConditions = lambda;
        }


        /// <summary>
        /// 初始化列表数据
        /// </summary>
        //internal void InitListData()
        //{
        //    base.newSumDataGridView1.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
        //    base.newSumDataGridView1.XmlFileName = this.Name + nameof(tb_SaleOut);
        //    base.newSumDataGridView1.FieldNameList = UIHelper.GetFieldNameColList(typeof(tb_SaleOut));
        //    bindingSourceListMain.DataSource = new List<tb_SaleOut>();
        //    base.newSumDataGridView1.DataSource = null;
        //    //绑定导航
        //    base.newSumDataGridView1.DataSource = bindingSourceListMain.DataSource;
        //}

        //public override List<tb_SaleOut> GetPrintDatas(tb_SaleOut EditEntity)
        //{
        //    List<tb_SaleOut> datas = new List<tb_SaleOut>();
        //    tb_SaleOutController<tb_SaleOut> ctr = Startup.GetFromFac<tb_SaleOutController<tb_SaleOut>>();
        //    List<tb_SaleOut> PrintData = ctr.GetPrintData(EditEntity.SaleOut_MainID);
        //    return PrintData;
        //}

        /*
        /// <summary>
        /// 采购入库审核成功后。如果有对应的采购订单引入，则将其结案，并把数量回写？
        /// </summary>
        /// <returns></returns>
        public async override Task<ApprovalEntity> Review(List<tb_SaleOut> EditEntitys)
        {
            if (EditEntitys == null)
            {
                return null;
            }
            //如果已经审核并且通过，则不能重复审核
            List<tb_SaleOut> needApprovals = EditEntitys.Where(
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

            tb_SaleOutController<tb_SaleOut> ctr = Startup.GetFromFac<tb_SaleOutController<tb_SaleOut>>();
            ReturnResults<bool> rs = await ctr.BatchApprovalAsync(needApprovals, ae);
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
                MainForm.Instance.PrintInfoLog($"{ae.bizName}:{ae.BillNo}审核失败,原因是{rs.ErrorMsg},如果无法解决，请联系管理员！", Color.Red);
            }

            return ae;
        }
             */

        public async override Task<bool> CloseCase(List<tb_SaleOut> EditEntitys)
        {
            if (EditEntitys == null)
            {
                return false;
            }
            //已经审核的并且通过的情况才能结案
            List<tb_SaleOut> needCloseCases = EditEntitys.Where(c => c.DataStatus == (int)DataStatus.确认 && c.ApprovalStatus == (int)ApprovalStatus.已审核 && c.ApprovalResults.HasValue && c.ApprovalResults.Value).ToList();
            if (needCloseCases.Count == 0)
            {
                MainForm.Instance.PrintInfoLog($"要结案的数据为：{needCloseCases.Count}:请检查数据！");
                return false;
            }

            tb_SaleOutController<tb_SaleOut> ctr = Startup.GetFromFac<tb_SaleOutController<tb_SaleOut>>();
            ReturnResults<bool> rs = await ctr.BatchCloseCaseAsync(needCloseCases);
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
                MainForm.Instance.PrintInfoLog($"结案操作失败,原因是{rs.ErrorMsg},如果无法解决，请联系管理员！", Color.Red);
            }

            return true;
        }

        /*
        /// <summary>
        /// 列表中不再实现反审，批量，出库反审情况极少。并且是仔细处理
        /// </summary>
        public async override Task<bool> ReReview(List<tb_SaleOut> EditEntitys)
        {
            if (EditEntitys == null)
            {
                return false;
            }
            tb_SaleOutController<tb_SaleOut> ctr = Startup.GetFromFac<tb_SaleOutController<tb_SaleOut>>();
            for (int i = 0; i < EditEntitys.Count; i++)
            {
                ReturnResults<bool> rs = await ctr.AntiApprovalAsync(EditEntitys[i]);
                if (rs.Succeeded)
                {

                    //if (MainForm.Instance.WorkflowItemlist.ContainsKey(""))
                    //{

                    //}
                    //这里审核完了的话，如果这个单存在于工作流的集合队列中，则向服务器说明审核完成。
                    //这里推送到审核，启动工作流  队列应该有一个策略 比方优先级，桌面不动1 3 5分钟 
                    //OriginalData od = ActionForClient.工作流审批(pkid, (int)BizType.盘点单, ae.ApprovalResults, ae.ApprovalComments);
                    //MainForm.Instance.ecs.AddSendData(od);

                    //审核成功
                    tsbtnAntiApproval.Enabled = false;

                }
                else
                {
                    //审核失败 要恢复之前的值
                    MainForm.Instance.PrintInfoLog($"销售出库单{EditEntitys[i].SaleOutNo}反审失败,{rs.ErrorMsg},请联系管理员！", Color.Red);
                }
            }

            return true;
        }
*/

    }



}
