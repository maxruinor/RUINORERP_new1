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
namespace RUINORERP.UI.FM
{
    /// <summary>
    /// 核销单查询
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
            var lambda = Expressionable.Create<tb_FM_PaymentSettlement>()
                             .And(t => t.ReceivePaymentType == (int)PaymentType)
                            .AndIF(AuthorizeController.GetOwnershipControl(MainForm.Instance.AppContext),
                             t => t.Created_by.Value == MainForm.Instance.AppContext.CurUserInfo.UserInfo.Employee_ID)
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
            base.MasterInvisibleCols.Add(c => c.ReceivePaymentType);
            //base.ChildInvisibleCols.Add(c => c.SubtotalCostAmount);
        }


        protected override void Delete(List<tb_FM_PaymentSettlement> Datas)
        {
            MessageBox.Show("核销记录不能删除？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
            return;
        }

    }
}
