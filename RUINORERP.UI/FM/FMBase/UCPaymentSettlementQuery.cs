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
using RUINORERP.Business.CommService;
using RUINORERP.Global.Model;
using Org.BouncyCastle.Crypto.Prng;
namespace RUINORERP.UI.FM
{
    /// <summary>
    /// 核销单查询
    /// 需要生成核销记录：资金与业务单据（应收/应付）直接冲抵时。
    /// 不需要生成核销记录：单纯记录资金流动（收付款）且未关联业务单据时。
    /// 设计核心：通过核销记录建立资金与业务的关联，确保财务数据可追溯，同时避免冗余记录。
    /// </summary>
    public partial class UCPaymentSettlementQuery : BaseBillQueryMC<tb_FM_PaymentSettlement, tb_FM_PaymentSettlement>
    {
        public UCPaymentSettlementQuery()
        {
            InitializeComponent();
            //base.RelatedBillEditCol = (c => c.no);
            //标记没有明细子表
            HasChildData = false;
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


            var lambda = Expressionable.Create<tb_FM_PaymentSettlement>()
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


        public override void BuildSummaryCols()
        {
            base.MasterSummaryCols.Add(c => c.SettledForeignAmount);
            base.MasterSummaryCols.Add(c => c.SettledLocalAmount);
        }

        public override void BuildInvisibleCols()
        {
            base.MasterInvisibleCols.Add(c => c.SettlementId);
            base.MasterInvisibleCols.Add(c => c.SourceBillId);
            base.MasterInvisibleCols.Add(c => c.TargetBillId);
            base.MasterInvisibleCols.Add(c => c.ReceivePaymentType);

            //base.ChildInvisibleCols.Add(c => c.SubtotalCostAmount);
        }


        protected override void Delete(List<tb_FM_PaymentSettlement> Datas)
        {
            MessageBox.Show("核销记录不能删除？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
            return;
        }

        private void UCPaymentSettlementQuery_Load(object sender, EventArgs e)
        {
            #region 双击单号后按业务类型查询显示对应业务窗体
            base._UCBillMasterQuery.GridRelated.ComplexType = true;
            //由这个列来决定单号显示哪个的业务窗体
            base._UCBillMasterQuery.GridRelated.SetComplexTargetField<tb_FM_PaymentSettlement>(c => c.SourceBizType, c => c.SourceBillNo);
            base._UCBillMasterQuery.GridRelated.SetComplexTargetField<tb_FM_PaymentSettlement>(c => c.TargetBizType, c => c.TargetBillNo);
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
                base._UCBillMasterQuery.GridRelated.SetRelatedInfo<tb_FM_PaymentSettlement>(c => c.SourceBillNo, keyNamePair);
            }
            #endregion
        }
    }
}
