using System;
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
using RUINORERP.UI.Common;
using RUINORERP.Global;
using RUINORERP.Business;

namespace RUINORERP.UI.BI
{
    [MenuAttrAssemblyInfo("编号规则编辑", true, UIType.单表数据)]
    public partial class UCsysBillNoRuleEdit : BaseEditGeneric<tb_sys_BillNoRule>
    {
        public UCsysBillNoRuleEdit()
        {
            InitializeComponent();
        }

        tb_sys_BillNoRule entity;

        public override void BindData(BaseEntity baseEntity)
        {
            entity = baseEntity as tb_sys_BillNoRule;
            DataBindingHelper.BindData4TextBox<tb_sys_BillNoRule>(entity, t => t.RuleName, txtRuleName, BindDataType4TextBox.Text, true);
            DataBindingHelper.BindData4CmbByEnum<tb_sys_BillNoRule>(entity, k => k.BizType, typeof(BizType), cmbBizType, false);
            DataBindingHelper.BindData4CmbByEnum<tb_sys_BillNoRule>(entity, k => k.RuleType, typeof(RuleType), cmbRuleType, false);
            DataBindingHelper.BindData4CmbByEnum<tb_sys_BillNoRule>(entity, k => k.DateFormat, typeof(DateFormat), cmbDateFormat, false);
            DataBindingHelper.BindData4CmbByEnum<tb_sys_BillNoRule>(entity, k => k.ResetMode, typeof(ResetMode), cmbResetMode, false);
            DataBindingHelper.BindData4CmbByEnum<tb_sys_BillNoRule>(entity, k => k.StorageType, typeof(StorageType), cmbStorageType, false);
            DataBindingHelper.BindData4TextBox<tb_sys_BillNoRule>(entity, t => t.Prefix, txtPrefix, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4TextBox<tb_sys_BillNoRule>(entity, t => t.SequenceLength, txtSequenceLength, BindDataType4TextBox.Qty, false);
            DataBindingHelper.BindData4CheckBox<tb_sys_BillNoRule>(entity, t => t.UseCheckDigit, chkUseCheckDigit, false);
            DataBindingHelper.BindData4CheckBox<tb_sys_BillNoRule>(entity, t => t.IsActive, chkIsActive, false);
            DataBindingHelper.BindData4TextBox<tb_sys_BillNoRule>(entity, t => t.Description, txtDescription, BindDataType4TextBox.Text, false);

            //如果属性变化 则状态为修改
            entity.PropertyChanged += (sender, s2) =>
            {
                //权限允许
                if (entity.ActionStatus == ActionStatus.修改 || entity.ActionStatus == ActionStatus.新增)
                {
                    //如果是采购入库引入变化则加载明细及相关数据
                    if (s2.PropertyName == entity.GetPropertyName<tb_sys_BillNoRule>(c => c.BizType))
                    {
                        entity.RuleName = $"{(BizType)entity.BizType}编号规则";
                    }
                }



            };

            base.BindData(entity);
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

        private void UCSystemConfigEdit_Load(object sender, EventArgs e)
        {

        }


    }
}
