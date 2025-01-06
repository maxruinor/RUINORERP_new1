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
namespace RUINORERP.UI.FM
{
    [MenuAttrAssemblyInfo("付款申请单查询", ModuleMenuDefine.模块定义.财务管理, ModuleMenuDefine.财务管理.收付账款, BizType.付款申请单)]
    public partial class UCPaymentApplicationList : BaseBillQueryMC<tb_FM_PaymentApplication, tb_FM_PaymentApplication>
    {
        public UCPaymentApplicationList()
        {
            InitializeComponent();
            base.RelatedBillEditCol = (c => c.ApplicationNo);
            //标记没有明细子表
            HasChildData = false;
        }

        public override void BuildLimitQueryConditions()
        {
            var lambda = Expressionable.Create<tb_FM_PaymentApplication>()
                              .And(t => t.isdeleted == false)
                            .AndIF(AuthorizeController.GetOwnershipControl(MainForm.Instance.AppContext),
             t => t.Employee_ID == MainForm.Instance.AppContext.CurUserInfo.UserInfo.Employee_ID)
                         .ToExpression();//注意 这一句 不能少

            QueryConditionFilter.FilterLimitExpressions.Add(lambda);

            base.LimitQueryConditions = lambda;
        }

        public override void BuildSummaryCols()
        {
            base.MasterSummaryCols.Add(c => c.TotalAmount);
            base.MasterSummaryCols.Add(c => c.OverpaymentAmount);
        }


        public override void BuildInvisibleCols()
        {
            base.MasterInvisibleCols.Add(c => c.PrePaymentBill_id);
            //base.ChildInvisibleCols.Add(c => c.SubtotalCostAmount);
        }








    }
}
