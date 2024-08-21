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
using RUINORERP.UI.BaseForm;

namespace RUINORERP.UI.BI
{
    [MenuAttrAssemblyInfo( "出入库类型编辑", true, UIType.单表数据)]
    public partial class UCOutInStockTypeEdit : BaseEditGeneric<tb_OutInStockType>
    {
        public UCOutInStockTypeEdit()
        {
            InitializeComponent();
        }

        public override void BindData(BaseEntity entity)
        {
            DataBindingHelper.BindData4TextBox<tb_OutInStockType>(entity, t => t.TypeName, txtTypeName, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4TextBox<tb_OutInStockType>(entity, t => t.TypeDesc, txtTypeDesc, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4RadioGroupTrueFalse<tb_OutInStockType>(entity, t => t.OutIn, rdbis_in, rdbis_out);
            //有默认值
            DataBindingHelper.BindData4RadioGroupTrueFalse<tb_OutInStockType>(entity, t => t.Is_enabled, rdbis_enabledYes, rdbis_enabledNo);
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
