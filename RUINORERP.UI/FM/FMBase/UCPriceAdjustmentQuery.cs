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

using AutoUpdateTools;
using RUINORERP.UI.BaseForm;
using RUINORERP.Common.Extensions;
using RUINORERP.Global.EnumExt;
using RUINORERP.UI.UControls;
using LiveChartsCore.Geo;
using Netron.GraphLib;
using RUINORERP.Business.CommService;
using RUINORERP.Global.Model;

namespace RUINORERP.UI.FM
{
    //价格调整单
    public partial class UCPriceAdjustmentQuery : BaseBillQueryMC<tb_FM_PriceAdjustment, tb_FM_PriceAdjustmentDetail>, UI.AdvancedUIModule.IContextMenuInfoAuth
    {
        public UCPriceAdjustmentQuery()
        {
            InitializeComponent();
            base.RelatedBillEditCol = (c => c.AdjustNo);
        }
        public ReceivePaymentType PaymentType { get; set; }
        public override void BuildLimitQueryConditions()
        {

            //这里外层来实现对客户供应商的限制
            string customerVendorId = "".ToFieldName<tb_CustomerVendor>(c => c.CustomerVendor_ID);

            //应收付款中的往来单位额外添加一些条件
            var lambdaCv = Expressionable.Create<tb_CustomerVendor>()
                .AndIF(PaymentType == ReceivePaymentType.收款, t => t.IsCustomer == true)
                .AndIF(PaymentType == ReceivePaymentType.付款, t => t.IsVendor == true)
              .ToExpression();
            QueryField queryField = QueryConditionFilter.QueryFields.Where(c => c.FieldName == customerVendorId).FirstOrDefault();
            queryField.SubFilter.FilterLimitExpressions.Add(lambdaCv);


            var lambda = Expressionable.Create<tb_FM_PriceAdjustment>()
                              .And(t => t.isdeleted == false)
                             .And(t => t.ReceivePaymentType == (int)PaymentType)
                            .AndIF(AuthorizeController.GetOwnershipControl(MainForm.Instance.AppContext),
                             t => t.Employee_ID == MainForm.Instance.AppContext.CurUserInfo.UserInfo.Employee_ID)
                         .ToExpression();//注意 这一句 不能少
            QueryConditionFilter.FilterLimitExpressions.Add(lambda);

            base.LimitQueryConditions = lambda;
        }
        public override void AddExcludeMenuList()
        {
            base.AddExcludeMenuList("批量处理");
            base.AddExcludeMenuList(MenuItemEnums.反结案);
            base.AddExcludeMenuList(MenuItemEnums.结案);
        }




        #region 转为应收应付款单
        public override List<ContextMenuController> AddContextMenu()
        {
            List<ContextMenuController> list = new List<ContextMenuController>();
            list.Add(new ContextMenuController($"【转为应{PaymentType.ToString()}单】", true, false, "NewSumDataGridView_转为应收付款单"));
            return list;
        }
        public override void BuildContextMenuController()
        {
            List<EventHandler> ContextClickList = new List<EventHandler>();
            ContextClickList.Add(NewSumDataGridView_转为应收付款单);
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
        private async void NewSumDataGridView_转为应收付款单(object sender, EventArgs e)
        {

            List<tb_FM_PriceAdjustment> selectlist = GetSelectResult();
            List<tb_FM_PriceAdjustment> RealList = new List<tb_FM_PriceAdjustment>();
            StringBuilder msg = new StringBuilder();
            int counter = 1;
            foreach (var item in selectlist)
            {
                //只有审核状态才可以转换为收款单
                bool canConvert = item.DataStatus == (long)DataStatus.确认 && item.ApprovalStatus == (int)ApprovalStatus.已审核 && item.ApprovalResults.HasValue && item.ApprovalResults.Value;
                if (canConvert)
                {
                    RealList.Add(item);
                }
                else
                {
                    msg.Append(counter.ToString() + ") ");
                    msg.Append($"当价格调整单-{PaymentType.ToString()}， {item.AdjustNo}状态为【 {((DataStatus)item.DataStatus).ToString()}】 无法生成应{PaymentType.ToString()}单。").Append("\r\n");
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
                msg.Append("请至少选择一行数据转换");
                MessageBox.Show(msg.ToString(), "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var paymentController = MainForm.Instance.AppContext.GetRequiredService<tb_FM_ReceivablePayableController<tb_FM_ReceivablePayable>>();

            //多选时。要相同客户才能合并到一个收款单
            if (RealList.Count() > 1)
            {
                //多选时直接保存到数据库了。
                msg.Append("不支持多行同时生成，请选择一行数据转换");
                MessageBox.Show(msg.ToString(), "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            else
            {
                bool isExist = await paymentController.IsExistAsync(c => c.SourceBillId.Value == RealList[0].AdjustId);
                string payTypeText = string.Empty;
                if (RealList[0].ReceivePaymentType == (int)ReceivePaymentType.收款)
                {
                    payTypeText = "应收款单";
                }
                else
                {
                    payTypeText = "应付款单";
                }
                if (isExist)
                {
                    msg.Append($"当前价格调整单{RealList[0].AdjustNo},已生成过{payTypeText}，不能再次生成");
                    MessageBox.Show(msg.ToString(), "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                #region 暂时只支持一单一单生成转向指定的单据介面等待用户检查修改
                tb_FM_ReceivablePayable rr = await paymentController.BuildReceivablePayable(RealList[0]);
                tb_FM_ReceivablePayable receivablePayable = rr;
                MenuPowerHelper menuPowerHelper;
                menuPowerHelper = Startup.GetFromFac<MenuPowerHelper>();

                string Flag = string.Empty;
                if (PaymentType == ReceivePaymentType.收款)
                {
                    Flag = typeof(RUINORERP.UI.FM.UCPriceAdjustment).FullName;
                }
                else
                {
                    Flag = typeof(RUINORERP.UI.FM.UCPriceAdjustment).FullName;
                }

                tb_MenuInfo RelatedMenuInfo = MainForm.Instance.MenuList.Where(m => m.IsVisble
            && m.EntityName == nameof(tb_FM_ReceivablePayable)
            && m.BIBaseForm == "BaseBillEditGeneric`2" && m.ClassPath == Flag)
                .FirstOrDefault();
                if (RelatedMenuInfo != null)
                {
                    menuPowerHelper.ExecuteEvents(RelatedMenuInfo, receivablePayable);
                }
                #endregion
            }

        }
        #endregion




        public override void BuildSummaryCols()
        {
            base.MasterSummaryCols.Add(c => c.TaxTotalDiffLocalAmount);
            base.MasterSummaryCols.Add(c => c.TotalLocalDiffAmount);
            base.ChildSummaryCols.Add(c => c.TaxAmount_Diff);
            base.ChildSummaryCols.Add(c => c.TotalAmount_Diff_WithTax);
            base.ChildSummaryCols.Add(c => c.TotalAmount_Diff);
            base.ChildSummaryCols.Add(c => c.Quantity);

        }


        public override void BuildInvisibleCols()
        {
            base.MasterInvisibleCols.Add(c => c.AdjustId);
            base.MasterInvisibleCols.Add(c => c.ReceivePaymentType);
            base.MasterInvisibleCols.Add(c => c.SourceBillId);
            if (PaymentType == ReceivePaymentType.收款)
            {
                //应收款，不需要对方的收款信息。收款才要显示
                //base.MasterInvisibleCols.Add(c => c.PayeeInfoID);
                //base.MasterInvisibleCols.Add(c => c.PayeeAccountNo);
            }

        }


        private void UCReceivablePayableQuery_Load(object sender, EventArgs e)
        {

            #region 双击单号后按业务类型查询显示对应业务窗体
            base._UCBillMasterQuery.GridRelated.ComplexType = true;
            //由这个列来决定单号显示哪个的业务窗体
            base._UCBillMasterQuery.GridRelated.SetComplexTargetField<tb_FM_PriceAdjustment>(c => c.SourceBizType, c => c.SourceBillNo);
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
                base._UCBillMasterQuery.GridRelated.SetRelatedInfo<tb_FM_PriceAdjustment>(c => c.SourceBillNo, keyNamePair);
            }
            #endregion

        }
    }
}
