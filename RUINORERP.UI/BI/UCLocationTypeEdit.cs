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

namespace RUINORERP.UI.BI
{


    [MenuAttrAssemblyInfo("库位类型编辑", true, UIType.单表数据)]
    public partial class UCLocationTypeEdit : BaseEditGeneric<tb_LocationType>
    {
        public UCLocationTypeEdit()
        {
            InitializeComponent();
        }



        public override void BindData(BaseEntity entity)
        {

            DataBindingHelper.BindData4TextBox<tb_LocationType>(entity, t => t.TypeName,txtTypeName, BindDataType4TextBox.Text, true);
            DataBindingHelper.BindData4TextBox<tb_LocationType>(entity, t => t.Desc, txtDesc, BindDataType4TextBox.Text, true);
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
            if (base.Validator())
            {
                bindingSourceEdit.EndEdit();
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }
    }
}
