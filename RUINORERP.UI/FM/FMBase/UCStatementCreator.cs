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
using RUINORERP.Business.CommService;
using RUINORERP.Global.Model;
using RUINORERP.UI.ToolForm;
using Microsoft.Extensions.Logging;
using NPOI.SS.Formula.Functions;

namespace RUINORERP.UI.FM
{
    //对账单创建中心
    [MenuAttrAssemblyInfo("对账中心", ModuleMenuDefine.模块定义.财务管理, ModuleMenuDefine.财务管理.对账管理, BizType.对账单)]
    [SharedIdRequired]
    public partial class UCStatementCreator : BaseBillQueryMC<tb_FM_ReceivablePayable, tb_FM_ReceivablePayableDetail>
    {
        public UCStatementCreator()
        {
            InitializeComponent();
            base.RelatedBillEditCol = (c => c.ARAPNo);
        }
        //public ReceivePaymentType PaymentType { get; set; }


        /// <summary>
        /// 如果需要查询条件查询，就要在子类中重写这个方法
        /// 这里用一套表两套查询条件
        /// </summary>
        public override void QueryConditionBuilder()
        {
            BaseProcessor baseProcessor = Startup.GetFromFacByName<BaseProcessor>(typeof(tb_FM_ReceivablePayableProcessorByStatement).Name);
            QueryConditionFilter = baseProcessor.GetQueryFilter();
        }



        public override void BuildLimitQueryConditions()
        {

            //这里外层来实现对客户供应商的限制
            string customerVendorId = "".ToFieldName<tb_CustomerVendor>(c => c.CustomerVendor_ID);

            //应收付款中的往来单位额外添加一些条件
            //还是将来利用行级权限来实现？
            var lambdaCv = Expressionable.Create<tb_CustomerVendor>()
              //.AndIF(PaymentType == ReceivePaymentType.收款, t => t.IsCustomer == true)
              //.AndIF(PaymentType == ReceivePaymentType.付款, t => t.IsVendor == true)
              .ToExpression();
            QueryField queryField = QueryConditionFilter.QueryFields.Where(c => c.FieldName == customerVendorId).FirstOrDefault();
            queryField.SubFilter.FilterLimitExpressions.Add(lambdaCv);

            var lambda = Expressionable.Create<tb_FM_ReceivablePayable>()
                              .And(t => t.isdeleted == false)
                              .And(t => t.AllowAddToStatement == true)
                              .And(t => t.LocalBalanceAmount != 0)
                              .And(t => t.ARAPStatus == (int)ARAPStatus.待审核 || t.ARAPStatus == (int)ARAPStatus.待支付 || t.ARAPStatus == (int)ARAPStatus.部分支付)
                         //对账时不管收付款都要查出来。所以才在这里重新建一个
                         //.And(t => t.ReceivePaymentType == (int)PaymentType)
                         // .AndIF(AuthorizeController.GetOwnershipControl(MainForm.Instance.AppContext),t => t.Employee_ID == MainForm.Instance.AppContext.CurUserInfo.UserInfo.Employee_ID)
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
            List<ContextMenuController> list = new List<ContextMenuController>();
            list.Add(new ContextMenuController("【生成对账单】", true, false, "NewSumDataGridView_生成对账单"));
            return list;
        }
        public override void BuildContextMenuController()
        {
            List<EventHandler> ContextClickList = new List<EventHandler>();
            ContextClickList.Add(NewSumDataGridView_生成对账单);
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


        #endregion

        private async void NewSumDataGridView_生成对账单(object sender, EventArgs e)
        {

            List<tb_FM_ReceivablePayable> selectlist = GetSelectResult();
            List<tb_FM_ReceivablePayable> RealList = new List<tb_FM_ReceivablePayable>();
            StringBuilder msg = new StringBuilder();
            int counter = 1;
            foreach (var item in selectlist)
            {
                ReceivePaymentType PaymentType = (ReceivePaymentType)item.ReceivePaymentType;
                //只有审核状态才可以转换为收款单
                bool canConvert = item.ARAPStatus == (int)ARAPStatus.待支付 && item.ApprovalStatus == (int)ApprovalStatus.已审核 && item.ApprovalResults.HasValue && item.ApprovalResults.Value;
                if (canConvert || item.ARAPStatus == (int)ARAPStatus.部分支付)
                {
                    RealList.Add(item);
                }
                else
                {
                    msg.Append(counter.ToString() + ") ");
                    msg.Append($"当前应{PaymentType.ToString()}单 {item.ARAPNo}状态为【 {((ARAPStatus)item.ARAPStatus.Value).ToString()}】 无法生成{PaymentType.ToString()}对账单。").Append("\r\n");
                    counter++;
                }
            }

            if (RealList.GroupBy(g => g.Currency_ID).Select(g => g.Key).Count() > 1)
            {
                msg.Append($"多选时，币别相同才能合并到一个对账单");
                MessageBox.Show(msg.ToString(), "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }


            //多选时。要相同客户才能合并到一个收款单
            if (RealList.GroupBy(g => g.CustomerVendor_ID).Select(g => g.Key).Count() > 1)
            {
                msg.Append($"多选时，要相同客户才能合并到一个对账单");
                #region 显示多个客户抬头

                // 显示前10个单据编号，其余用省略号表示
                string CustomerVendors = string.Join(", ",
                    RealList.Select(item => item.tb_customervendor).Distinct().Select(c => c.CVName));
                CustomerVendors = CustomerVendors.TrimEnd(',');
                int Count = RealList.Select(item => item.tb_customervendor).Distinct().Count();
                //if (RealList.Select(item => item.CustomerVendor_ID).Count() > 5)
                //{
                //    CustomerVendors += $" 等 {CustomerVendors.Count} 张单据\r\n";
                //}

                #endregion
                if (MessageBox.Show(msg.ToString(), "系统提示", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk, MessageBoxDefaultButton.Button2) == DialogResult.No)
                {
                    //您已选择 N 个客户，生成对账单时将合并这些客户的往来数据，是否继续？
                    MessageBox.Show($"您已选择 {Count} 个客户:{CustomerVendors}，生成对账单时将合并这些客户的往来数据，请确认是否继续？", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    return;
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
                msg.Append($"请至少选择一行数据转为对账单");
                MessageBox.Show(msg.ToString(), "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var paymentController = MainForm.Instance.AppContext.GetRequiredService<tb_FM_StatementController<tb_FM_Statement>>();
            tb_FM_Statement statement = await paymentController.BuildStatement(RealList);

            MenuPowerHelper menuPowerHelper;
            menuPowerHelper = Startup.GetFromFac<MenuPowerHelper>();

            BizType bizType = BizType.对账单;
            tb_MenuInfo RelatedMenuInfo = null;
            RelatedMenuInfo = MainForm.Instance.MenuList.Where(m => m.IsVisble
                 && m.BizType == (int)bizType
                 && m.BIBaseForm == "BaseBillEditGeneric`2")
                     .FirstOrDefault();
            if (RelatedMenuInfo != null)
            {
                await menuPowerHelper.ExecuteEvents(RelatedMenuInfo, statement);
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
            base.ChildSummaryCols.Add(c => c.LocalPayableAmount);
            base.ChildSummaryCols.Add(c => c.TaxLocalAmount);
        }


        public override void BuildInvisibleCols()
        {
            base.MasterInvisibleCols.Add(c => c.ARAPId);
            base.MasterInvisibleCols.Add(c => c.ReceivePaymentType);
            base.MasterInvisibleCols.Add(c => c.SourceBillId);
        }


        protected override void Delete(List<tb_FM_ReceivablePayable> Datas)
        {
            MessageBox.Show("应收应付记录不能删除？", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        private void UCReceivablePayableQuery_Load(object sender, EventArgs e)
        {

            #region 双击单号后按业务类型查询显示对应业务窗体
            base._UCBillMasterQuery.GridRelated.ComplexType = true;
            //由这个列来决定单号显示哪个的业务窗体
            base._UCBillMasterQuery.GridRelated.SetComplexTargetField<tb_FM_ReceivablePayable>(c => c.SourceBizType, c => c.SourceBillNo);
            BizTypeMapper mapper = new BizTypeMapper();
            //将枚举中的值循环
            foreach (var biztype in Enum.GetValues(typeof(BizType)))
            {
                var tableName = mapper.GetTableType((BizType)biztype);
                if (tableName == null)
                {
                    continue;
                }
                ////这个参数中指定要双击的列单号。是来自另一组  一对一的指向关系
                //因为后面代码去查找时，直接用的 从一个对象中找这个列的值。但是枚举显示的是名称。所以这里直接传入枚举的值。
                KeyNamePair keyNamePair = new KeyNamePair(((int)((BizType)biztype)).ToString(), tableName.Name);
                base._UCBillMasterQuery.GridRelated.SetRelatedInfo<tb_FM_ReceivablePayable>(c => c.SourceBillNo, keyNamePair);
            }
            #endregion

        }
    }
}
