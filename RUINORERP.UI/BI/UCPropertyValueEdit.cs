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


    [MenuAttrAssemblyInfo("属性值编辑", true, UIType.单表数据)]
    public partial class UCPropertyValueEdit : BaseEditGeneric<tb_ProdPropertyValue>
    {
        public UCPropertyValueEdit()
        {
            InitializeComponent();
        }

        public override void BindData(BaseEntity entity)
        {
            DataBindingHelper.BindData4Cmb<tb_ProdProperty>(entity, k => k.Property_ID, v => v.PropertyName, cmbProperty_ID);
            DataBindingHelper.BindData4TextBox<tb_ProdPropertyValue>(entity, t => t.PropertyValueName, txtPropertyValueName, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4TextBox<tb_ProdPropertyValue>(entity, t => t.PropertyValueDesc, txtPropertyValueDesc, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4TextBox<tb_ProdProperty>(entity, t => t.SortOrder.ToString(), txtSortOrder, BindDataType4TextBox.Text, false);
            //后面这些依赖于控件绑定的数据源和字段。所以要在绑定后执行。
            if (entity.ActionStatus == ActionStatus.新增 || entity.ActionStatus == ActionStatus.修改)
            {
                base.InitRequiredToControl(new tb_ProdPropertyValueValidator(), kryptonPanel1.Controls);
                base.InitEditItemToControl(entity, kryptonPanel1.Controls);
            }
        }


        private void btnCancel_Click(object sender, EventArgs e)
        {
            bindingSourceEdit.CancelEdit();
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        Business.LogicaService.UnitController mc = Startup.GetFromFac<UnitController>();


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
