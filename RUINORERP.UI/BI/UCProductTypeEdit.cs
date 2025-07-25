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


    [MenuAttrAssemblyInfo( "产品类型编辑", true, UIType.单表数据)]
    public partial class UCProductTypeEdit : BaseEditGeneric<tb_ProductType>
    {
        public UCProductTypeEdit()
        {
            InitializeComponent();
        }

   

        public override void BindData(BaseEntity entity)
        {
            DataBindingHelper.BindData4TextBox<tb_ProductType>(entity, t => t.TypeName, txtTypeName, BindDataType4TextBox.Text, true);
            DataBindingHelper.BindData4TextBox<tb_ProductType>(entity, t => t.TypeDesc, txtDesc, BindDataType4TextBox.Text, true);
            DataBindingHelper.BindData4RadioGroupTrueFalse<tb_ProductType>(entity, t => t.ForSale, rdbis_enabledYes, rdbis_enabledNo);
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
