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


    [MenuAttrAssemblyInfo("单位换算编辑", true, UIType.单表数据)]
    public partial class UCUnitConversionEdit : BaseEditGeneric<tb_Unit_Conversion>
    {
        public UCUnitConversionEdit()
        {
            InitializeComponent();
           // DataBindingHelper.InitDataToCmb<tb_Unit>(k => k.Unit_ID, v => v.UnitName, cmbSource_unit_id);
           // DataBindingHelper.InitDataToCmb<tb_Unit>(k => k.Unit_ID, v => v.UnitName, cmbTarget_unit_id);
        }

        public override void BindData(BaseEntity entity)
        {
            DataBindingHelper.BindData4TextBox<tb_Unit_Conversion>(entity, t => t.UnitConversion_Name, txtUnitConversion_Name, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4TextBox<tb_Unit_Conversion>(entity, t => t.Notes, txtNotes, BindDataType4TextBox.Text, false);

            DataBindingHelper.BindData4Cmb<tb_Unit, tb_Unit_Conversion>(entity, k => k.Unit_ID, v => v.UnitName, t => t.Source_unit_id, cmbSource_unit_id, false);
            DataBindingHelper.BindData4Cmb<tb_Unit, tb_Unit_Conversion>(entity, k => k.Unit_ID, v => v.UnitName, t => t.Target_unit_id, cmbTarget_unit_id, false);

            DataBindingHelper.BindData4TextBox<tb_Unit_Conversion>(entity, t => t.Conversion_ratio.ToString(), txtConversion_ratio, BindDataType4TextBox.Money, false);

            base.errorProviderForAllInput.DataSource = entity;
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
            //来源单位，写回基本单位表中为可计量换算？
            if (base.Validator())
            {
                bindingSourceEdit.EndEdit();
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }
    }
}
