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
using RUINORERP.Model.Base;
using RUINORERP.Business.Security;
using RUINORERP.Business.Processor;
using RUINORERP.Global.EnumExt;
using RUINORERP.UI.UControls;
using LiveChartsCore.Geo;
using Netron.GraphLib;
using RUINORERP.Business.CommService;
using RUINORERP.Global.Model;

namespace RUINORERP.UI.PSI.SAL
{

    [MenuAttrAssemblyInfo("销售退回单查询", ModuleMenuDefine.模块定义.进销存管理, ModuleMenuDefine.进销存管理.销售管理, BizType.销售退回单)]
    public partial class UCSaleOutReQuery : BaseBillQueryMC<tb_SaleOutRe, tb_SaleOutReDetail>, UI.AdvancedUIModule.IContextMenuInfoAuth
    {
        public UCSaleOutReQuery()
        {
            InitializeComponent();
            base.RelatedBillEditCol = (c => c.ReturnNo);
        }
        #region 转为收付款单
        public override List<ContextMenuController> AddContextMenu()
        {
            //List<EventHandler> ContextClickList = new List<EventHandler>();
            //ContextClickList.Add(NewSumDataGridView_转为收付款单);
            List<ContextMenuController> list = new List<ContextMenuController>();
            //退款单
            list.Add(new ContextMenuController("【转为红冲应收款单】", true, false, "NewSumDataGridView_转为红冲应收款单"));
            return list;
        }
        public override void BuildContextMenuController()
        {
            List<EventHandler> ContextClickList = new List<EventHandler>();
            ContextClickList.Add(NewSumDataGridView_转为红冲应收款单);
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
        private async void NewSumDataGridView_转为红冲应收款单(object sender, EventArgs e)
        {
            List<tb_SaleOutRe> selectlist = GetSelectResult();
            if (selectlist.Count > 1)
            {
                MessageBox.Show("生成红冲应收款单每次只能选择一个销售退回单。");
                return;
            }
            List<tb_SaleOutRe> RealList = new List<tb_SaleOutRe>();
            StringBuilder msg = new StringBuilder();
            int counter = 1;
            foreach (var item in selectlist)
            {
                //只有审核状态才可以转换为应收 红冲
                bool canConvert = item.DataStatus == (long)DataStatus.确认 && item.ApprovalStatus == (int)ApprovalStatus.已审核 && item.ApprovalResults.HasValue && item.ApprovalResults.Value;
                if (canConvert)
                {
                    RealList.Add(item);
                }
                else
                {
                    msg.Append(counter.ToString() + ") ");
                    msg.Append($"当前销售退回单 {item.ReturnNo}状态为【 {((DataStatus)item.DataStatus).ToString()}】 无法生【红冲】应收款单。").Append("\r\n");
                    counter++;
                }
            }
            //多选时。要相同客户才能合并到一个收款单
            if (RealList.GroupBy(g => g.CustomerVendor_ID).Select(g => g.Key).Count() > 1)
            {
                msg.Append("多选时，要相同客户才能合并到一个【红冲】应收款单");
                MessageBox.Show(msg.ToString(), "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
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
                msg.Append("请至少选择一行数据转为【红冲】应收款单");
                MessageBox.Show(msg.ToString(), "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var ReceivablePayableController = MainForm.Instance.AppContext.GetRequiredService<tb_FM_ReceivablePayableController<tb_FM_ReceivablePayable>>();
            tb_FM_ReceivablePayable ReceivablePayable = await ReceivablePayableController.BuildReceivablePayable(RealList[0]);
            MenuPowerHelper menuPowerHelper;
            menuPowerHelper = Startup.GetFromFac<MenuPowerHelper>();
            string Flag = string.Empty;
            //红冲收款
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
        #endregion
        public override void SetGridViewDisplayConfig()
        {
            _UCBillMasterQuery.GridRelated.SetRelatedInfo<tb_SaleOutRe, tb_SaleOut>(c => c.SaleOut_NO, r => r.SaleOutNo);
            base.SetGridViewDisplayConfig();
        }

        public override void BuildInvisibleCols()
        {
            //引用的订单号ID不需要显示。因为有一个单号冗余显示了。
            base.MasterInvisibleCols.Add(c => c.SaleOut_MainID);
            base.ChildInvisibleCols.Add(c => c.SaleOutDetail_ID);
        }

        protected async override void Delete(List<tb_SaleOutRe> Datas)
        {
            if (Datas == null || Datas.Count == 0)
            {
                //提示一下删除成功
                MainForm.Instance.uclog.AddLog("提示", "没有要删除的数据");
                return;
            }

            if (MessageBox.Show("系统不建议删除单据资料\r\n确定删除吗？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
            {
                int counter = 0;
                foreach (var item in Datas)
                {
                    //https://www.runoob.com/w3cnote/csharp-enum.html
                    var dataStatus = (DataStatus)(item.GetPropertyValue(typeof(DataStatus).Name).ToInt());
                    if (dataStatus == DataStatus.新建 || dataStatus == DataStatus.草稿)
                    {
                        if (item.Created_by.HasValue && item.Created_by.Value != MainForm.Instance.AppContext.CurUserInfo.Id)
                        {
                            MessageBox.Show("只能删除自己创建的销售退回单。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            continue;
                        }

                        BaseController<tb_SaleOutRe> ctr = Startup.GetFromFacByName<BaseController<tb_SaleOutRe>>(typeof(tb_SaleOutRe).Name + "Controller");
                        bool rs = await ctr.BaseDeleteAsync(item);
                        if (rs)
                        {
                            counter++;
                        }
                    }
                    else
                    {
                        MainForm.Instance.uclog.AddLog("提示", $"单据状态为{dataStatus}无法删除");
                    }
                }
                MainForm.Instance.uclog.AddLog("提示", $"成功删除数据：{counter}条.");
            }
        }

        public override void BuildLimitQueryConditions()
        {
            //创建表达式
            var lambda = Expressionable.Create<tb_SaleOutRe>()
                             // .AndIF(CurMenuInfo.CaptionCN.Contains("供应商"), t => t.IsVendor == true)
                             .And(t => t.isdeleted == false)
                               .AndIF(AuthorizeController.GetSaleLimitedAuth(MainForm.Instance.AppContext), t => t.Employee_ID == MainForm.Instance.AppContext.CurUserInfo.UserInfo.Employee_ID)//限制了销售只看到自己的客户,采购不限制
                            .ToExpression();//注意 这一句 不能少
            base.LimitQueryConditions = lambda;
        }

        /// <summary>
        /// 如果需要查询条件查询，就要在子类中重写这个方法
        /// </summary>
        public override void BuildQueryCondition()
        {
            base.BuildQueryCondition();
            var lambda = Expressionable.Create<tb_SaleOutRe>()
            .AndIF(AuthorizeController.GetSaleLimitedAuth(MainForm.Instance.AppContext) && !MainForm.Instance.AppContext.IsSuperUser, t => t.Employee_ID == MainForm.Instance.AppContext.CurUserInfo.UserInfo.Employee_ID)//限制了销售只看到自己的客户,采购不限制
            .ToExpression();
            QueryConditionFilter.FilterLimitExpressions.Add(lambda);
        }

        public override void BuildSummaryCols()
        {
            base.MasterSummaryCols.Add(c => c.TotalQty);
            base.MasterSummaryCols.Add(c => c.TotalAmount);
            base.MasterSummaryCols.Add(c => c.ForeignTotalAmount);
            base.MasterSummaryCols.Add(c => c.FreightIncome);
            base.MasterSummaryCols.Add(c => c.TotalCommissionAmount);

            base.ChildSummaryCols.Add(c => c.Quantity);
            base.ChildSummaryCols.Add(c => c.SubtotalUntaxedAmount);
            base.ChildSummaryCols.Add(c => c.CustomizedCost);
            base.ChildSummaryCols.Add(c => c.Cost);
            base.ChildSummaryCols.Add(c => c.SubtotalTaxAmount);
            base.ChildSummaryCols.Add(c => c.SubtotalTransAmount);
            base.ChildSummaryCols.Add(c => c.SubtotalCostAmount);
            base.ChildSummaryCols.Add(c => c.CommissionAmount);
        }

    }

}
