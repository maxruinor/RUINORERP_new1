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


using AutoUpdateTools;
using RUINORERP.UI.BaseForm;
using RUINORERP.Common.Extensions;
using RUINORERP.Global.EnumExt;
using RUINORERP.UI.UControls;
using LiveChartsCore.Geo;
using Netron.GraphLib;
using RUINORERP.Business.CommService;
using RUINORERP.Global.Model;
using RUINORERP.UI.ToolForm;
using Microsoft.Extensions.Logging;
using NPOI.SS.Formula.Functions;
using Org.BouncyCastle.Asn1.X509.Qualified;
using FastReport.DevComponents.DotNetBar.Controls;

namespace RUINORERP.UI.FM
{
    //对应单查询
    [MenuAttrAssemblyInfo("对账单查询", ModuleMenuDefine.模块定义.财务管理, ModuleMenuDefine.财务管理.对账管理, BizType.对账单)]
    [SharedIdRequired]
    public partial class UCFMStatementQuery : BaseBillQueryMC<tb_FM_Statement, tb_FM_StatementDetail>
    {
        public UCFMStatementQuery()
        {
            InitializeComponent();
            base.RelatedBillEditCol = (c => c.StatementNo);
        }
        //public ReceivePaymentType PaymentType { get; set; }
        public override void BuildLimitQueryConditions()
        {

            //这里外层来实现对客户供应商的限制
            string customerVendorId = "".ToFieldName<tb_CustomerVendor>(c => c.CustomerVendor_ID);

            //应收付款中的往来单位额外添加一些条件
            var lambdaCv = Expressionable.Create<tb_CustomerVendor>()
              //.AndIF(PaymentType == ReceivePaymentType.收款, t => t.IsCustomer == true)
              //.AndIF(PaymentType == ReceivePaymentType.付款, t => t.IsVendor == true)
              .ToExpression();
            QueryField queryField = QueryConditionFilter.QueryFields.Where(c => c.FieldName == customerVendorId).FirstOrDefault();
            queryField.SubFilter.FilterLimitExpressions.Add(lambdaCv);


            var lambda = Expressionable.Create<tb_FM_Statement>()
                              .And(t => t.isdeleted == false)
                         //.And(t => t.ReceivePaymentType == (int)PaymentType)
                         //.AndIF(AuthorizeController.GetOwnershipControl(MainForm.Instance.AppContext),t => t.Employee_ID == MainForm.Instance.AppContext.CurUserInfo.UserInfo.Employee_ID)
                         .ToExpression();//注意 这一句 不能少
            QueryConditionFilter.FilterLimitExpressions.Add(lambda);

            base.LimitQueryConditions = lambda;
        }
        public override void AddExcludeMenuList()
        {
            base.AddExcludeMenuList("批量处理");
            base.AddExcludeMenuList(MenuItemEnums.反结案);
            base.AddExcludeMenuList(MenuItemEnums.复制性新增);
            base.AddExcludeMenuList(MenuItemEnums.数据特殊修正);
            base.AddExcludeMenuList(MenuItemEnums.结案);
            base.AddExcludeMenuList(MenuItemEnums.删除);
            base.AddExcludeMenuList(MenuItemEnums.提交);
            base.AddExcludeMenuList(MenuItemEnums.新增);
            base.AddExcludeMenuList(MenuItemEnums.导入);
            base.AddExcludeMenuList(MenuItemEnums.修改);
            base.AddExcludeMenuList(MenuItemEnums.保存);
        }



        #region 转为收付款单
        public override List<ContextMenuController> AddContextMenu()
        {
            //List<EventHandler> ContextClickList = new List<EventHandler>();
            //ContextClickList.Add(NewSumDataGridView_转为收付款单);
            List<ContextMenuController> list = new List<ContextMenuController>();
            list.Add(new ContextMenuController("【转为收付款单】", true, false, "NewSumDataGridView_转为收付款单"));
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

            List<tb_FM_Statement> selectlist = GetSelectResult();
            List<tb_FM_Statement> RealList = new List<tb_FM_Statement>();
            StringBuilder msg = new StringBuilder();
            int counter = 1;
            foreach (var item in selectlist)
            {
                ReceivePaymentType PaymentType = (ReceivePaymentType)item.ReceivePaymentType;
                //只有审核状态才可以转换为收款单
                bool canConvert = item.StatementStatus == (int)StatementStatus.已确认 && item.ApprovalStatus == (int)ApprovalStatus.已审核 && item.ApprovalResults.HasValue && item.ApprovalResults.Value;
                if (canConvert || item.StatementStatus == (int)StatementStatus.部分结算)
                {
                    RealList.Add(item);
                }
                else
                {
                    msg.Append(counter.ToString() + ") ");
                    msg.Append($"应{PaymentType.ToString()}对账单 {item.StatementNo}状态为【 {((StatementStatus)item.StatementStatus.Value).ToString()}】 无法生成{PaymentType.ToString()}单。").Append("\r\n");
                    counter++;
                }
            }

            #region 提前判断是付款还是收款

            // 计算收款和付款的总额
            // 收款类型：使用绝对值确保为正数
            decimal totalReceivable = RealList
                .Where(x => x.ReceivePaymentType == (int)ReceivePaymentType.收款)  // 收款类型
                .Sum(x => Math.Abs(x.ClosingBalanceLocalAmount));

            // 付款类型：使用绝对值确保为正数
            decimal totalPayable = RealList
                .Where(x => x.ReceivePaymentType == (int)ReceivePaymentType.付款)  // 付款类型
                .Sum(x => Math.Abs(x.ClosingBalanceLocalAmount));

            // 计算净额：付款 - 收款
            // 净额>0表示需要付款，净额<0表示需要收款
            decimal netAmount = totalPayable - totalReceivable;
            ReceivePaymentType LastPaymentType = ReceivePaymentType.付款;
            if (netAmount > 0)
            {
                LastPaymentType = ReceivePaymentType.付款;
            }
            else if (netAmount < 0)
            {
                LastPaymentType = ReceivePaymentType.收款;
            }
            // 净额为0时保持默认的付款类型，后续处理会自动处理
            #endregion


            //多选时。要相同客户才能合并到一个收款单
            if (RealList.GroupBy(g => g.CustomerVendor_ID).Select(g => g.Key).Count() > 1)
            {

                msg.Append($"多选时，要相同客户才能合并到一个{LastPaymentType.ToString()}单");
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
                msg.Append($"请至少选择一行数据转为收{LastPaymentType.ToString()}单");
                MessageBox.Show(msg.ToString(), "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var paymentController = MainForm.Instance.AppContext.GetRequiredService<tb_FM_PaymentRecordController<tb_FM_PaymentRecord>>();
            tb_FM_PaymentRecord ReturnObject = paymentController.BuildPaymentRecord(RealList).Result;


            tb_FM_PaymentRecord paymentRecord = ReturnObject;
            MenuPowerHelper menuPowerHelper;
            menuPowerHelper = Startup.GetFromFac<MenuPowerHelper>();

            string Flag = string.Empty;
            //对账单的
            if (ReturnObject.ReceivePaymentType == (int)ReceivePaymentType.收款)
            {
                Flag = typeof(RUINORERP.UI.FM.UCFMReceivedRecord).FullName;
            }
            else
            {
                Flag = typeof(RUINORERP.UI.FM.UCFMPaymentRecord).FullName;
            }

            tb_MenuInfo RelatedMenuInfo = MainForm.Instance.MenuList.Where(m => m.IsVisble
        && m.EntityName == nameof(tb_FM_PaymentRecord)
        && m.BIBaseForm == "BaseBillEditGeneric`2" && m.ClassPath == Flag)
            .FirstOrDefault();
            if (RelatedMenuInfo != null)
            {
                await menuPowerHelper.ExecuteEvents(RelatedMenuInfo, paymentRecord);
            }
        }
        #endregion






        public override void BuildSummaryCols()
        {
            base.MasterSummaryCols.Add(c => c.ClosingBalanceForeignAmount);
            base.MasterSummaryCols.Add(c => c.ClosingBalanceLocalAmount);
            base.MasterSummaryCols.Add(c => c.OpeningBalanceForeignAmount);
            base.MasterSummaryCols.Add(c => c.OpeningBalanceLocalAmount);
            base.MasterSummaryCols.Add(c => c.TotalPaidForeignAmount);
            base.MasterSummaryCols.Add(c => c.TotalPaidLocalAmount);
            base.MasterSummaryCols.Add(c => c.TotalPayableForeignAmount);
            base.MasterSummaryCols.Add(c => c.TotalPayableLocalAmount);
            base.MasterSummaryCols.Add(c => c.TotalReceivableForeignAmount);
            base.MasterSummaryCols.Add(c => c.TotalReceivableLocalAmount);
            base.MasterSummaryCols.Add(c => c.TotalReceivedForeignAmount);
            base.MasterSummaryCols.Add(c => c.TotalReceivedLocalAmount);

            base.ChildSummaryCols.Add(c => c.IncludedForeignAmount);
            base.ChildSummaryCols.Add(c => c.IncludedLocalAmount);
            base.ChildSummaryCols.Add(c => c.RemainingForeignAmount);
            base.ChildSummaryCols.Add(c => c.RemainingLocalAmount);
            base.ChildSummaryCols.Add(c => c.WrittenOffForeignAmount);
            base.ChildSummaryCols.Add(c => c.WrittenOffLocalAmount);
        }


        public override void BuildInvisibleCols()
        {
            base.MasterInvisibleCols.Add(c => c.StatementId);
            base.MasterInvisibleCols.Add(c => c.ARAPNos);

            //if (PaymentType == ReceivePaymentType.收款)
            //{
            //    //应收款，不需要对方的收款信息。付款才要显示
            //    base.MasterInvisibleCols.Add(c => c.PayeeInfoID);
            //    base.MasterInvisibleCols.Add(c => c.PayeeAccountNo);
            //}

        }


        protected override void Delete(List<tb_FM_Statement> Datas)
        {
            MessageBox.Show("对账单记录不能删除？", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }
        public GridViewDisplayTextResolver DisplayTextResolver;
        private void UCReceivablePayableQuery_Load(object sender, EventArgs e)
        {
            base._UCBillChildQuery.GridRelated.ComplexType = true;
            DisplayTextResolver = new GridViewDisplayTextResolver(typeof(tb_FM_Statement));
            //显示时目前只缓存了基础数据。单据也可以考虑id显示编号。后面来实现。如果缓存优化好了
            DisplayTextResolver.Initialize(_UCBillChildQuery.newSumDataGridViewChild);

            #region 双击单号后按业务类型查询显示对应业务窗体
            _UCBillChildQuery.GridRelated.SetRelatedInfo<tb_FM_StatementDetail, tb_FM_ReceivablePayable>(c => c.ARAPId, r => r.ARAPNo);
            #endregion
        }
    }
}
