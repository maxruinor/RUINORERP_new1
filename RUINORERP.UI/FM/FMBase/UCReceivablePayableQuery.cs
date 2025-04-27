using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using RUINORERP.Business.LogicaService;
using RUINORERP.Model;
using RUINORERP.UI.Common;
using RUINORERP.Common;
using RUINORERP.Common.CollectionExtension;
using RUINOR.Core;
using RUINORERP.Common.Helper;
using RUINORERP.Business;

using RUINORERP.Business.AutoMapper;
using AutoMapper;
using RUINORERP.Model.Base;
using SqlSugar;
using Krypton.Navigator;
using RUINORERP.Business.Security;
using RUINORERP.Business.Processor;
using RUINORERP.UI.AdvancedUIModule;
using RUINORERP.Global.EnumExt.CRM;
using RUINORERP.Global;
using RUINORERP.Model.TransModel;
using RUINORERP.UI.SuperSocketClient;
using TransInstruction;
using AutoUpdateTools;
using RUINORERP.UI.BaseForm;
using RUINORERP.Common.Extensions;
using RUINORERP.Global.EnumExt;
using RUINORERP.UI.UControls;
using LiveChartsCore.Geo;
using Netron.GraphLib;

namespace RUINORERP.UI.FM
{
    //应收应付查询
    public partial class UCReceivablePayableQuery : BaseBillQueryMC<tb_FM_ReceivablePayable, tb_FM_ReceivablePayableDetail>, UI.AdvancedUIModule.IContextMenuInfoAuth
    {
        public UCReceivablePayableQuery()
        {
            InitializeComponent();
            base.RelatedBillEditCol = (c => c.ARAPNo);
            //标记没有明细子表
            HasChildData = false;
        }
        public ReceivePaymentType PaymentType { get; set; }
        public override void BuildLimitQueryConditions()
        {
            var lambda = Expressionable.Create<tb_FM_ReceivablePayable>()
                              .And(t => t.isdeleted == false)
                             .And(t => t.ReceivePaymentType == (int)PaymentType)
                            .AndIF(AuthorizeController.GetOwnershipControl(MainForm.Instance.AppContext),
                             t => t.Employee_ID == MainForm.Instance.AppContext.CurUserInfo.UserInfo.Employee_ID)
                         .ToExpression();//注意 这一句 不能少

            QueryConditionFilter.FilterLimitExpressions.Add(lambda);

            base.LimitQueryConditions = lambda;
        }

        public List<ContextMenuController> AddContextMenu()
        {
            List<EventHandler> ContextClickList = new List<EventHandler>();
            ContextClickList.Add(NewSumDataGridView_转为收付款单);
            List<ContextMenuController> list = new List<ContextMenuController>();
            if (PaymentType==ReceivePaymentType.收款)
            {
                list.Add(new ContextMenuController("【转为收款单】", true, false, "NewSumDataGridView_转为收付款单"));
            }
            else
            {
                list.Add(new ContextMenuController("【转为付款单】", true, false, "NewSumDataGridView_转为收付款单"));
            }
           
            return list;
        }
        public override void BuildContextMenuController()
        {
            List<EventHandler> ContextClickList = new List<EventHandler>();
            ContextClickList.Add(NewSumDataGridView_转为收付款单);
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
        private async void NewSumDataGridView_转为收付款单(object sender, EventArgs e)
        {
            List<tb_FM_ReceivablePayable> selectlist = GetSelectResult();
            foreach (var item in selectlist)
            {
                //只有审核状态才可以转换为出库单
                bool canConvert = item.FMPaymentStatus == (int)FMPaymentStatus.已审核 && item.ApprovalStatus == (int)ApprovalStatus.已审核 && item.ApprovalResults.HasValue && item.ApprovalResults.Value;
                if (canConvert||item.FMPaymentStatus == (int)FMPaymentStatus.部分核销)
                {

                    tb_FM_PaymentRecordController<tb_FM_PaymentRecord> paymentController = MainForm.Instance.AppContext.GetRequiredService<tb_FM_PaymentRecordController<tb_FM_PaymentRecord>>();
                    tb_FM_PaymentRecord paymentRecord = await paymentController.CreatePaymentRecord(item,false);
                    MenuPowerHelper menuPowerHelper;
                    menuPowerHelper = Startup.GetFromFac<MenuPowerHelper>();
                    tb_MenuInfo RelatedMenuInfo = MainForm.Instance.MenuList.Where(m => m.IsVisble && m.EntityName == nameof(tb_SaleOutRe) && m.BIBaseForm == "BaseBillEditGeneric`2").FirstOrDefault();
                    if (RelatedMenuInfo != null)
                    {
                        menuPowerHelper.ExecuteEvents(RelatedMenuInfo, paymentRecord);
                    }
                    return;
                }
                else
                {
                    if (PaymentType == ReceivePaymentType.收款)
                    {
                        MessageBox.Show($"当前应收款单 {item.ARAPNo} 无法生成收款单，请查询单据状态是否正确。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show($"当前应付款单 {item.ARAPNo} 无法生成收款单，请查询单据状态是否正确。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                        
                }
            }
        }

        public override void BuildSummaryCols()
        {
            base.MasterSummaryCols.Add(c => c.TaxTotalAmount);

            base.MasterSummaryCols.Add(c => c.TotalLocalPayableAmount);
            base.MasterSummaryCols.Add(c => c.TotalForeignPayableAmount);

            base.MasterSummaryCols.Add(c => c.ForeignBalanceAmount);
            base.MasterSummaryCols.Add(c => c.LocalBalanceAmount);

            base.MasterSummaryCols.Add(c => c.ForeignPaidAmount);
            base.MasterSummaryCols.Add(c => c.LocalPaidAmount);
        }


        public override void BuildInvisibleCols()
        {
            base.MasterInvisibleCols.Add(c => c.ARAPId);
            base.MasterInvisibleCols.Add(c => c.ReceivePaymentType);
            //base.ChildInvisibleCols.Add(c => c.SubtotalCostAmount);
        }


        protected override void Delete(List<tb_FM_ReceivablePayable> Datas)
        {
            MessageBox.Show("应收应付记录不能删除？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
            return;
        }

    }
}
