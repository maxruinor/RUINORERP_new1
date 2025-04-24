using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using RUINORERP.Common;
using RUINORERP.UI.UCToolBar;
using RUINORERP.Model;
using Krypton.Toolkit;
using RUINORERP.UI.BaseForm;
using RUINORERP.Business.LogicaService;
using RUINORERP.UI.Common;
using RUINORERP.Business;
using RUINORERP.Global.EnumExt;
using RUINORERP.Business.Processor;
using RUINORERP.Business.Security;
using SqlSugar;

namespace RUINORERP.UI.BI
{
    [MenuAttrAssemblyInfo("收付款账号编辑", true, UIType.单表数据)]
    public partial class UCFMAccountEdit : BaseEditGeneric<tb_FM_Account>
    {
        public UCFMAccountEdit()
        {
            InitializeComponent();
        }


        public override void BindData(BaseEntity entityPara)
        {
            tb_FM_Account entity = entityPara as tb_FM_Account;
            if (entity == null)
            {

                return;
            }
            DataBindingHelper.BindData4Cmb<tb_Department>(entity, k => k.DepartmentID, v => v.DepartmentName, cmbDepartmentID);

            DataBindingHelper.BindData4Cmb<tb_Company>(entity, k => k.ID, v => v.CNName, cmbCompany);

            DataBindingHelper.BindData4Cmb<tb_Currency>(entity, k => k.Currency_ID, v => v.CurrencyCode, cmbCurrency_ID);

            DataBindingHelper.BindData4TextBox<tb_FM_Account>(entity, t => t.Account_name, txtaccount_name, BindDataType4TextBox.Text, false);

            DataBindingHelper.BindData4TextBox<tb_FM_Account>(entity, t => t.Account_No, txtaccount_No, BindDataType4TextBox.Text, false);

            DataBindingHelper.BindData4CmbByEnum<tb_FM_Account>(entity, k => k.Account_type, typeof(AccountType), cmbAccount_type, false);

            DataBindingHelper.BindData4TextBox<tb_FM_Account>(entity, t => t.Bank, txtBank, BindDataType4TextBox.Text, false);

            DataBindingHelper.BindData4TextBox<tb_FM_Account>(entity, t => t.OpeningBalance.ToString(), txtOpeningBalance, BindDataType4TextBox.Money, false);

            DataBindingHelper.BindData4TextBox<tb_FM_Account>(entity, t => t.CurrentBalance.ToString(), txtCurrentBalance, BindDataType4TextBox.Money, false);


            //如果属性变化 则状态为修改
            entity.PropertyChanged += (sender, s2) =>
            {
                if (entity == null)
                {
                    return;
                }

                //如果公司有变化，带出对应部门
                if (entity.ID > 0 && s2.PropertyName == entity.GetPropertyName<tb_FM_Account>(c => c.ID))
                {
                    //创建表达式
                    var lambda = Expressionable.Create<tb_Department>()
                                    .And(t => t.isdeleted == false)
                                    .AndIF(entity.ID > 0, t => t.ID == entity.ID)
                                    .ToExpression();

                    BaseProcessor baseProcessor = Startup.GetFromFacByName<BaseProcessor>(typeof(tb_Department).Name + "Processor");
                    QueryFilter queryFilterC = baseProcessor.GetQueryFilter();
                    queryFilterC.FilterLimitExpressions.Add(lambda);
                    DataBindingHelper.BindData4Cmb<tb_Department>(entity, k => k.DepartmentID, v => v.DepartmentName, cmbDepartmentID, queryFilterC.GetFilterExpression<tb_Department>(), true);
                    DataBindingHelper.InitFilterForControlByExp<tb_Department>(entity, cmbDepartmentID, c => c.DepartmentName, queryFilterC);
                }
            };


            //后面这些依赖于控件绑定的数据源和字段。所以要在绑定后执行。
            if (entity.ActionStatus == ActionStatus.新增 || entity.ActionStatus == ActionStatus.修改)
            {
                base.InitRequiredToControl(MainForm.Instance.AppContext.GetRequiredService<tb_FM_AccountValidator>(), kryptonPanel1.Controls);
                base.InitEditItemToControl(entity, kryptonPanel1.Controls);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            bindingSourceEdit.CancelEdit();
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }




        private void btnOk_Click(object sender, EventArgs e)
        {
            if (base.Validator())
            {
                bindingSourceEdit.EndEdit();
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }



    }
}
