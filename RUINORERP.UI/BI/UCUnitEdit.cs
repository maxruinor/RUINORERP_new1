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
using RUINORERP.Business;
using RUINORERP.UI.Common;

namespace RUINORERP.UI.BI
{


    [MenuAttrAssemblyInfo("计量单位编辑", true, UIType.单表数据)]
    public partial class UCUnitEdit : BaseEditGeneric<tb_Unit>
    {
        public UCUnitEdit()
        {
            InitializeComponent();
        }

        public override void BindData(BaseEntity entity)
        {
            DataBindingHelper.BindData4TextBox<tb_Unit>(entity, t => t.UnitName, txtUnitName, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4TextBox<tb_Unit>(entity, t => t.Notes, txtDesc, BindDataType4TextBox.Text, false);
            //有默认值
            DataBindingHelper.BindData4RadioGroupTrueFalse<tb_Unit>(entity, t => t.is_measurement_unit, rdbis_enabledYes, rdbis_enabledNo);

            base.errorProviderForAllInput.DataSource = entity;
            base.BindData(entity);
        }

        //public override void BindData(tb_Unit entity)
        //{
        //    DataBindingHelper.BindData4TextBox<tb_Unit>(entity, t => t.UnitName, txtUnitName, BindDataType4TextBox.Text, false);
        //    DataBindingHelper.BindData4TextBox<tb_Unit>(entity, t => t.Notes, txtDesc, BindDataType4TextBox.Text, false);
        //    base.errorProviderForAllInput.DataSource = entity;
        //}


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
