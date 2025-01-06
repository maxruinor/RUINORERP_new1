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
using RUINORERP.Business.Processor;
using RUINORERP.Global.EnumExt;
using RUINORERP.Common.Extensions;
using SqlSugar;
using RUINORERP.Business.Security;
using NPOI.SS.Formula.Functions;
using NPOI.SS.Formula;

namespace RUINORERP.UI.BI
{

    [MenuAttrAssemblyInfo("收款账号管理", ModuleMenuDefine.模块定义.基础资料, ModuleMenuDefine.基础资料.财务资料)]
    public partial class UCFMPayeeInfoList : BaseForm.BaseListGeneric<tb_FM_PayeeInfo>
    {
        public UCFMPayeeInfoList()
        {
            InitializeComponent();
            base.EditForm = typeof(UCFMPayeeInfoEdit);
            //固定值也包括枚举值,也可以将没有缓存的提前查询出来给

            System.Linq.Expressions.Expression<Func<tb_FM_PayeeInfo, int?>> exprCheckMode;
            exprCheckMode = (p) => p.Account_type;
            base.ColNameDataDictionary.TryAdd(exprCheckMode.GetMemberInfo().Name, Common.CommonHelper.Instance.GetKeyValuePairs(typeof(AccountType)));

        }


        public override void QueryConditionBuilder()
        {

            BaseProcessor baseProcessor = Startup.GetFromFacByName<BaseProcessor>(typeof(tb_FM_PayeeInfo).Name + "Processor");
            QueryConditionFilter = baseProcessor.GetQueryFilter();

            //创建表达式
            var lambda = Expressionable.Create<tb_FM_PayeeInfo>()
                             //.And(t => t.Is_enabled == true)
                             .AndIF(AuthorizeController.GetOwnershipControl(MainForm.Instance.AppContext), t => t.Employee_ID == MainForm.Instance.AppContext.CurUserInfo.UserInfo.Employee_ID)//限制只看到自己的
                            .ToExpression();//注意 这一句 不能少
            QueryConditionFilter.FilterLimitExpressions.Add(lambda);

            //清空过滤条件，因为这个基本数据 需要显示出来 
            //QueryConditionFilter.FilterLimitExpressions.Clear();

        }

        protected override Task<bool> Delete()
        {
            tb_FM_PayeeInfo payinfo = (tb_FM_PayeeInfo)this.bindingSourceList.Current;
            //指向员工收款信息时。只能自己删除 或超级用户来删除
            if ((payinfo.Employee_ID.HasValue && payinfo.Employee_ID != MainForm.Instance.AppContext.CurUserInfo.UserInfo.Employee_ID) || (MainForm.Instance.AppContext.IsSuperUser))
            {
                //只能删除自己的收款信息。
                MessageBox.Show("只能删除自己的收款信息。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return Task.FromResult(false);
            }
            return base.Delete();
        }

        private void frm_Load(object sender, EventArgs e)
        {

        }
    }
}
