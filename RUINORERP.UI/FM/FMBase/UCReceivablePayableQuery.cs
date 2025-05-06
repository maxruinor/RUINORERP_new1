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
            HasChildData = true;
        }
        public ReceivePaymentType PaymentType { get; set; }
        public override void BuildLimitQueryConditions()
        {
            var lambda = Expressionable.Create<tb_FM_ReceivablePayable>()
                              .And(t => t.isdeleted == false)
                             .And(t => t.ReceivePaymentType == (int)PaymentType)
                            .AndIF(AuthorizeController.GetOwnershipControl(MainForm.Instance.AppContext),
                             t => t.Created_by == MainForm.Instance.AppContext.CurUserInfo.UserInfo.Employee_ID)
                         .ToExpression();//注意 这一句 不能少

            QueryConditionFilter.FilterLimitExpressions.Add(lambda);

            base.LimitQueryConditions = lambda;
        }
        public override void AddExcludeMenuList()
        {
            base.AddExcludeMenuList("批量处理");
            base.AddExcludeMenuList(MenuItemEnums.反结案);
            base.AddExcludeMenuList(MenuItemEnums.反审);
            base.AddExcludeMenuList(MenuItemEnums.复制性新增);
            base.AddExcludeMenuList(MenuItemEnums.数据特殊修正);
            base.AddExcludeMenuList(MenuItemEnums.结案);
            base.AddExcludeMenuList(MenuItemEnums.删除);
            base.AddExcludeMenuList(MenuItemEnums.审核);
            base.AddExcludeMenuList(MenuItemEnums.提交);
            base.AddExcludeMenuList(MenuItemEnums.新增);
            base.AddExcludeMenuList(MenuItemEnums.导入);
            base.AddExcludeMenuList(MenuItemEnums.修改);
            base.AddExcludeMenuList(MenuItemEnums.保存);
        }

        #region 添加生成对账单

        /// <summary>
        /// 添加回收
        /// </summary>
        /// <returns></returns>
        public ToolStripItem[] AddExtendButton(tb_MenuInfo menuInfo)
        {
            ToolStripButton toolStripButton生成对账单 = new System.Windows.Forms.ToolStripButton();
            toolStripButton生成对账单.Text = "生成对账单";
            toolStripButton生成对账单.Image = global::RUINORERP.UI.Properties.Resources.MakeSureCost;
            toolStripButton生成对账单.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolStripButton生成对账单.Name = "生成对账单MakesureCost";
            toolStripButton生成对账单.Visible = false;//默认隐藏
            UIHelper.ControlButton(CurMenuInfo, toolStripButton生成对账单);
            toolStripButton生成对账单.ToolTipText = "生成对账单。";
            toolStripButton生成对账单.Click += new System.EventHandler(this.toolStripButton生成对账单_Click);

            System.Windows.Forms.ToolStripItem[] extendButtons = new System.Windows.Forms.ToolStripItem[] { toolStripButton生成对账单 };
            this.BaseToolStrip.Items.AddRange(extendButtons);
            return extendButtons;

        }

        private async void toolStripButton生成对账单_Click(object sender, EventArgs e)
        {
            /*后面再设计一个对账功能菜单？生成的回写一下？ 搞一个对账的表？*/
            /*
            if (base.bindingSourceList.Current != null && dataGridView1.CurrentCell != null)
            {
                //  弹出提示说：您确定将这个公司回收投入到公海吗？
                if (bindingSourceList.Current is View_Inventory ViewInventory)
                {
                    var amountRule = new AmountValidationRule();
                    using (var inputForm = new frmInputObject(amountRule))
                    {
                        inputForm.DefaultTitle = "请输入成本价格";
                        tb_Inventory Inventory = await MainForm.Instance.AppContext.Db.Queryable<tb_Inventory>().Where(c => c.Inventory_ID == ViewInventory.Inventory_ID).SingleAsync();
                        if (inputForm.ShowDialog() == DialogResult.OK)
                        {
                            if (MessageBox.Show($"确定将产品:【{ViewInventory.SKU + "-" + ViewInventory.CNName}】库存成本设为：{inputForm.InputContent}吗？", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                            {
                                Inventory.CostFIFO = inputForm.InputContent.ToDecimal();
                                Inventory.CostMonthlyWA = inputForm.InputContent.ToDecimal();
                                Inventory.CostMovingWA = inputForm.InputContent.ToDecimal();
                                Inventory.Inv_AdvCost = inputForm.InputContent.ToDecimal();
                                Inventory.Inv_Cost = inputForm.InputContent.ToDecimal();

                                int result = await MainForm.Instance.AppContext.Db.Updateable<tb_Inventory>(Inventory).UpdateColumns(t => new { t.CostFIFO, t.CostMonthlyWA, t.CostMovingWA, t.Inv_AdvCost, t.Inv_Cost }).ExecuteCommandAsync();
                                if (result > 0)
                                {
                                    MainForm.Instance.ShowStatusText("生成对账单成功!");
                                    
                                }
                            }
                        }
                    }

                }
            }
            */


        }


        #endregion



        public List<ContextMenuController> AddContextMenu()
        {
            //List<EventHandler> ContextClickList = new List<EventHandler>();
            //ContextClickList.Add(NewSumDataGridView_转为收付款单);
            List<ContextMenuController> list = new List<ContextMenuController>();
            if (PaymentType == ReceivePaymentType.收款)
            {
                list.Add(new ContextMenuController("【转为收款单】", true, false, "NewSumDataGridView_转为收付款单"));
            }
            else
            {
                list.Add(new ContextMenuController("【转为付款单】", true, false, "NewSumDataGridView_转为收付款单"));
            }
           // list.Add(new ContextMenuController("【生成对账单】", true, false, "NewSumDataGridView_生成对账单"));
            return list;
        }
        public override void BuildContextMenuController()
        {
            List<EventHandler> ContextClickList = new List<EventHandler>();
            ContextClickList.Add(NewSumDataGridView_转为收付款单);
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
        private async void NewSumDataGridView_转为收付款单(object sender, EventArgs e)
        {
            List<tb_FM_ReceivablePayable> selectlist = GetSelectResult();
            foreach (var item in selectlist)
            {
                //只有审核状态才可以转换为收款单
                bool canConvert = item.ARAPStatus == (long)ARAPStatus.已生效 && item.ApprovalStatus == (int)ApprovalStatus.已审核 && item.ApprovalResults.HasValue && item.ApprovalResults.Value;
                if (canConvert || item.ARAPStatus == (long)ARAPStatus.部分支付)
                {
                    tb_FM_PaymentRecordController<tb_FM_PaymentRecord> paymentController = MainForm.Instance.AppContext.GetRequiredService<tb_FM_PaymentRecordController<tb_FM_PaymentRecord>>();
                    tb_FM_PaymentRecord paymentRecord = await paymentController.CreatePaymentRecord(item, false);
                    MenuPowerHelper menuPowerHelper;
                    menuPowerHelper = Startup.GetFromFac<MenuPowerHelper>();
                    tb_MenuInfo RelatedMenuInfo = MainForm.Instance.MenuList.Where(m => m.IsVisble && m.EntityName == nameof(tb_FM_PaymentRecord) && m.BIBaseForm == "BaseBillEditGeneric`2").FirstOrDefault();
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


        //按客户生成对账单
        private  void NewSumDataGridView_生成对账单(object sender, EventArgs e)
        {
          
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
            base.MasterInvisibleCols.Add(c => c.SourceBill_ID);
            if (PaymentType == ReceivePaymentType.收款)
            {
                //应收款，不需要对方的收款信息。收款才要显示
                base.MasterInvisibleCols.Add(c => c.PayeeInfoID);
                base.MasterInvisibleCols.Add(c => c.PayeeAccountNo);
            }
       
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
            base._UCBillMasterQuery.GridRelated.SetComplexTargetField<tb_FM_ReceivablePayable>(c => c.SourceBizType, c => c.SourceBillNO);
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
                base._UCBillMasterQuery.GridRelated.SetRelatedInfo<tb_FM_ReceivablePayable>(c => c.SourceBillNO, keyNamePair);
            }
            #endregion

        }
    }
}
