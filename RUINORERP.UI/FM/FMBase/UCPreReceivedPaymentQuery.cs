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
using RUINORERP.Global.Model;

using RUINORERP.Business.CommService;
using FastReport.Table;

namespace RUINORERP.UI.FM
{
    /// <summary>
    /// 预收 预付查询
    /// </summary>
    public partial class UCPreReceivedPaymentQuery : BaseBillQueryMC<tb_FM_PreReceivedPayment, tb_FM_PreReceivedPayment>
    {
        public UCPreReceivedPaymentQuery()
        {
            InitializeComponent();
            //可以设置双击打开单据的字段 关联单据字段设置
            base.RelatedBillEditCol = (c => c.PreRPNO);

            //标记没有明细子表
            HasChildData = false;
        }

        public override void AddExcludeMenuList()
        {
            base.AddExcludeMenuList(MenuItemEnums.反结案);
            base.AddExcludeMenuList(MenuItemEnums.结案);
        }


        public ReceivePaymentType PaymentType { get; set; }
        public override void BuildLimitQueryConditions()
        {
            var lambda = Expressionable.Create<tb_FM_PreReceivedPayment>()
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
            //List<EventHandler> ContextClickList = new List<EventHandler>();
            //ContextClickList.Add(NewSumDataGridView_转为收付款单);
            List<ContextMenuController> list = new List<ContextMenuController>();
            list.Add(new ContextMenuController("【转为退款单】", true, false, "NewSumDataGridView_转为退款单"));
            return list;
        }


        public override void BuildContextMenuController()
        {
            List<EventHandler> ContextClickList = new List<EventHandler>();
            ContextClickList.Add(NewSumDataGridView_转为退款单);
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
        private async void NewSumDataGridView_转为退款单(object sender, EventArgs e)
        {
            List<tb_FM_PreReceivedPayment> selectlist = GetSelectResult();
            foreach (var item in selectlist)
            {
                bool canConvert = item.PrePaymentStatus == (long)PrePaymentStatus.待核销 && item.ApprovalStatus == (int)ApprovalStatus.已审核 && item.ApprovalResults.HasValue && item.ApprovalResults.Value;
                if (canConvert || item.PrePaymentStatus == (long)PrePaymentStatus.部分核销)
                {
                    //审核就是收款 或 付款了 ，则生成退款单  （负数的收款单）
                    //这个状态要退款单审核后回写
                    //entity.FMPaymentStatus = (int)FMPaymentStatus.已冲销;//退款  余额有多少退多少。
                    if (item.ForeignBalanceAmount > 0 || item.LocalBalanceAmount > 0)
                    {
                        tb_FM_PaymentRecordController<tb_FM_PaymentRecord> paymentController = MainForm.Instance.AppContext.GetRequiredService<tb_FM_PaymentRecordController<tb_FM_PaymentRecord>>();
                        bool isRefund = true;
                        tb_FM_PaymentRecord paymentRecord = await paymentController.CreatePaymentRecord(item, isRefund);
                        MenuPowerHelper menuPowerHelper;
                        menuPowerHelper = Startup.GetFromFac<MenuPowerHelper>();

                        string Flag = string.Empty;
                        if (CurMenuInfo == null)
                        {
                            MessageBox.Show("请联系管理员，配置入口菜单");
                        }
                        else
                        {
                            Flag = CurMenuInfo.UIPropertyIdentifier;
                        }

                        tb_MenuInfo RelatedMenuInfo = MainForm.Instance.MenuList.Where(m => m.IsVisble
                        && m.EntityName == nameof(tb_FM_PaymentRecord)
                        && m.BIBaseForm == "BaseBillEditGeneric`2"
                        && m.UIPropertyIdentifier == Flag
                        ).FirstOrDefault();
                        if (RelatedMenuInfo != null)
                        {
                            menuPowerHelper.ExecuteEvents(RelatedMenuInfo, paymentRecord);
                        }
                        return;
                    }
                    else
                    {
                        //没有金额可退。

                    }

                }
                else
                {
                    if (PaymentType == ReceivePaymentType.付款)
                    {
                        MessageBox.Show($"当前预付款单 {item.PreRPNO} 无法生成退款单，请查询单据状态是否正确。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show($"当前预收款单 {item.PreRPNO} 无法生成退款单，请查询单据状态是否正确。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }

                }
            }
        }

        public override void BuildSummaryCols()
        {
            base.MasterSummaryCols.Add(c => c.LocalPrepaidAmount);
            base.MasterSummaryCols.Add(c => c.LocalPaidAmount);
            base.MasterSummaryCols.Add(c => c.LocalBalanceAmount);
            base.MasterSummaryCols.Add(c => c.ForeignPrepaidAmount);
            base.MasterSummaryCols.Add(c => c.ForeignPaidAmount);
            base.MasterSummaryCols.Add(c => c.ForeignBalanceAmount);

        }


        public override void BuildInvisibleCols()
        {
            base.MasterInvisibleCols.Add(c => c.SourceBillId);
            base.MasterInvisibleCols.Add(c => c.ReceivePaymentType);
            if (PaymentType == ReceivePaymentType.收款)
            {
                //收款不显示，付款给别人。才显示别人的收款信息
                base.MasterInvisibleCols.Add(c => c.PayeeInfoID);
                base.MasterInvisibleCols.Add(c => c.PayeeAccountNo);
            }


            //base.ChildInvisibleCols.Add(c => c.SubtotalCostAmount);
        }



        protected async override void Delete(List<tb_FM_PreReceivedPayment> Datas)
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
                        BaseController<tb_FM_PreReceivedPayment> ctr = Startup.GetFromFacByName<BaseController<tb_FM_PreReceivedPayment>>(typeof(tb_FM_PreReceivedPayment).Name + "Controller");
                        bool rs = await ctr.BaseDeleteAsync(item);
                        if (rs)
                        {
                            counter++;
                        }
                    }
                    else
                    {
                        MainForm.Instance.uclog.AddLog("提示", "已【确认】【审核】的生效单据无法删除");
                    }
                }
                MainForm.Instance.uclog.AddLog("提示", $"成功删除数据：{counter}条.");
            }
        }

        private void UCPreReceivedPaymentQuery_Load(object sender, EventArgs e)
        {
            if (base._UCBillMasterQuery == null)
            {
                return;
            }
            base._UCBillMasterQuery.GridRelated.ComplexType = true;
            //由这个列来决定单号显示哪个的业务窗体
            base._UCBillMasterQuery.GridRelated.SetComplexTargetField<tb_FM_PreReceivedPayment>(c => c.SourceBizType, c => c.SourceBillNo);
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
                base._UCBillMasterQuery.GridRelated.SetRelatedInfo<tb_FM_PreReceivedPayment>(c => c.SourceBillNo, keyNamePair);
            }

        }
    }
}
